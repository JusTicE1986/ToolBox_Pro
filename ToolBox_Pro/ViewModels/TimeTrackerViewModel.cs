using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using Timetracker.Helper;
using ToolBox_Pro.Models;
using ToolBoxPro.Helper;
using ToolBoxPro.Models;

namespace ToolBox_Pro.ViewModels
{
    public partial class TimeTrackerViewModel : ObservableObject
    {
        private readonly string dateipfad = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ToolBoxPro", "timetracker.json");

        [ObservableProperty]
        private DateTime datum = DateTime.Today;

        [ObservableProperty]
        private TimeSpan start = new(6, 0, 0);

        [ObservableProperty]
        private TimeSpan ende = new(14, 0, 0);

        [ObservableProperty]
        private TimeSpan pause;

        [ObservableProperty]
        private string notiz = string.Empty;

        [ObservableProperty]
        private ObservableCollection<ArbeitszeitTag> wochenDaten = new();

        [ObservableProperty]
        private ArbeitszeitTag? ausgewaehlterTag;

        [ObservableProperty]
        private TimeSpan wochenSumme;

        [ObservableProperty]
        private MonatsInfo aktuelleMonatsInfo = new();
        [ObservableProperty]
        private int aktuelleKalenderwoche;

        [ObservableProperty]
        private int aktuellesJahr;

        public string KalenderwochenAnzeige =>
            $"KW {KulturHelper.GetKalenderwoche(Datum)} / {Datum.Year}";

        public TimeTrackerViewModel()
        {
            Datum = DateTime.Today;
            AktuelleKalenderwoche = KulturHelper.GetKalenderwoche(Datum);
            AktuellesJahr = Datum.Year;
            BerechnePause();
            LadeWoche();
            BerechneMonatsInfo(); // <- wichtig!
        }

        partial void OnStartChanged(TimeSpan value) => BerechnePause();
        partial void OnEndeChanged(TimeSpan value) => BerechnePause();
        partial void OnAusgewaehlterTagChanged(ArbeitszeitTag? value)
        {
            if (value is null)
                return;

            // Werte aus ausgewähltem Tag übernehmen
            Datum = value.Datum;
            Start = value.Start;
            Ende = value.Ende;
            Pause = value.Pause;
            Notiz = value.Notiz;
        }
        partial void OnAktuelleKalenderwocheChanged(int value)
        {
            OnPropertyChanged(nameof(KalenderwochenAnzeige));
        }
        partial void OnAktuellesJahrChanged(int value)
        {
            OnPropertyChanged(nameof(KalenderwochenAnzeige));
        }
        partial void OnDatumChanged(DateTime value)
        {
            OnPropertyChanged(nameof(KalenderwochenAnzeige));
            AktuelleKalenderwoche = KulturHelper.GetKalenderwoche(value);
            AktuellesJahr = value.Year;
        }

        [RelayCommand]
        private void Speichern()
        {
            var tag = new ArbeitszeitTag
            {
                Datum = Datum,
                Start = Start,
                Ende = Ende,
                Pause = Pause,
                Notiz = Notiz
            };

            var daten = File.Exists(dateipfad)
                ? JsonSerializer.Deserialize<List<ArbeitszeitTag>>(File.ReadAllText(dateipfad)) ?? new()
                : new();

            daten.RemoveAll(t => t.Datum.Date == tag.Datum.Date);
            daten.Add(tag);

            Directory.CreateDirectory(Path.GetDirectoryName(dateipfad)!);
            File.WriteAllText(dateipfad, JsonSerializer.Serialize(daten, new JsonSerializerOptions { WriteIndented = true }));

            LadeWoche();
            BerechneMonatsInfo();
        }

        [RelayCommand]
        private void Laden()
        {
            if (!File.Exists(dateipfad)) return;

            var daten = JsonSerializer.Deserialize<List<ArbeitszeitTag>>(File.ReadAllText(dateipfad));
            var heute = daten?.FirstOrDefault(t => t.Datum.Date == Datum.Date);

            if (heute is null) return;

            Start = heute.Start;
            Ende = heute.Ende;
            Pause = heute.Pause;
            Notiz = heute.Notiz;
            BerechneMonatsInfo();
        }

        [RelayCommand]
        private void WocheVor()
        {
            var start = KulturHelper.GetStartDerKalenderwoche(AktuellesJahr, AktuelleKalenderwoche).AddDays(+7);
            AktuelleKalenderwoche = KulturHelper.GetKalenderwoche(start);
            AktuellesJahr = start.Year;
            Datum = start;
            LadeWoche();
            BerechneMonatsInfo();
        }

        [RelayCommand]
        private void WocheZurueck()
        {
            var start = KulturHelper.GetStartDerKalenderwoche(AktuellesJahr, AktuelleKalenderwoche).AddDays(-7);
            AktuelleKalenderwoche = KulturHelper.GetKalenderwoche(start);
            AktuellesJahr = start.Year;
            Datum = start;
            LadeWoche();
            BerechneMonatsInfo();
        }

        [RelayCommand]
        private void Zuruecksetzen()
        {
            Datum = DateTime.Today;
            AktuelleKalenderwoche = KulturHelper.GetKalenderwoche(Datum);
            AktuellesJahr = Datum.Year;
            LadeWoche();
            BerechneMonatsInfo();
        }


        private void LadeWoche()
        {
            WochenDaten.Clear();
            WochenSumme = TimeSpan.Zero;

            if (!File.Exists(dateipfad))
            {
                Debug.WriteLine("Datei nicht gefunden: " + dateipfad);
                return;
            }

            var json = File.ReadAllText(dateipfad);
            var daten = JsonSerializer.Deserialize<List<ArbeitszeitTag>>(json);

            if (daten == null)
            {
                Debug.WriteLine("Keine Daten geladen!");
                return;
            }

            Debug.WriteLine($"Daten geladen: {daten.Count} Einträge");

            var startDerWoche = KulturHelper.GetStartDerKalenderwoche(Datum.Year, KulturHelper.GetKalenderwoche(Datum));

            for (int i = 0; i < 7; i++)
            {
                var datum = startDerWoche.AddDays(i);
                var eintrag = daten.FirstOrDefault(t => t.Datum.Date == datum.Date)
                              ?? new ArbeitszeitTag { Datum = datum };

                Debug.WriteLine($"{datum:dd.MM.yyyy}: {eintrag.Start}-{eintrag.Ende}");

                WochenDaten.Add(eintrag);
                WochenSumme += eintrag.BerechneteGearbeiteteZeit;
            }
        }

        private void BerechnePause()
        {
            var arbeitszeitBrutto = Ende - Start;

            if (arbeitszeitBrutto <= TimeSpan.FromHours(6))
            {
                Pause = TimeSpan.Zero;
            }
            else if (arbeitszeitBrutto > TimeSpan.FromHours(6) && arbeitszeitBrutto <= TimeSpan.FromHours(6.5))
            {
                Pause = arbeitszeitBrutto - TimeSpan.FromHours(6);
            }
            else if (arbeitszeitBrutto > TimeSpan.FromHours(6.5) && arbeitszeitBrutto <= TimeSpan.FromHours(9.25))
            {
                Pause = TimeSpan.FromMinutes(30);
            }
            else if (arbeitszeitBrutto > TimeSpan.FromHours(9.25) && arbeitszeitBrutto <= TimeSpan.FromHours(9.5))
            {
                Pause = arbeitszeitBrutto - TimeSpan.FromHours(9);
            }
            else
            {
                Pause = TimeSpan.FromMinutes(45);
            }
        }

        private void BerechneMonatsInfo()
        {
            var monat = Datum.Month;
            var jahr = Datum.Year;
            var sollzeitProTag = TimeSpan.FromMinutes(456); // 7,6h

            var alleDaten = File.Exists(dateipfad)
                ? JsonSerializer.Deserialize<List<ArbeitszeitTag>>(File.ReadAllText(dateipfad)) ?? new()
                : new();

            var tageImMonat = alleDaten
                .Where(t => t.Datum.Month == monat && t.Datum.Year == jahr)
                .ToList();

            var gearbeiteteZeit = tageImMonat
                .Select(t => t.BerechneteGearbeiteteZeit)
                .Aggregate(TimeSpan.Zero, (summe, zeit) => summe + zeit);

            // Feiertage im aktuellen Monat
            var feiertage = FeiertagsHelper.GetFeiertage(jahr)
                .Where(f => f.Key.Month == monat)
                .ToList();

            // Alle Kalendertage des Monats durchgehen
            var solltage = Enumerable.Range(1, DateTime.DaysInMonth(jahr, monat))
                .Select(tag => new DateTime(jahr, monat, tag))
                .Count(d =>
                    (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday) // regulärer Werktag
                    || feiertage.Any(f => f.Key.Date == d.Date && f.Key.DayOfWeek != DayOfWeek.Saturday && f.Key.DayOfWeek != DayOfWeek.Sunday)
                );

            var sollzeit = TimeSpan.FromTicks(sollzeitProTag.Ticks * solltage);

            AktuelleMonatsInfo = new MonatsInfo
            {
                MonatJahr = new DateTime(jahr, monat, 1).ToString("MMMM yyyy", new CultureInfo("de-DE")),
                MonatlicheSollzeit = sollzeit,
                MonatlichGearbeitet = gearbeiteteZeit
            };
        }


    }
}
