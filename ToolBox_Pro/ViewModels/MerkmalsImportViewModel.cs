using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class MerkmalsImportViewModel : ObservableObject
    {
        private readonly MerkmalsImportService _importService = new();

        [ObservableProperty]
        private ObservableCollection<MerkmalsEintrag> eintraege = new();
        [ObservableProperty]
        private ObservableCollection<XmlMerkmal> xmlMerkmale = new();
        [ObservableProperty]
        private ObservableCollection<string> typen = new();
        [ObservableProperty]
        private string ausgewaehlterTyp;
        [ObservableProperty]
        private bool istBeschaeftigt;
        [ObservableProperty]
        private ICollectionView gefilterteEintraege;
        [ObservableProperty]
        private ObservableCollection<MerkmalModelEintrag> merkmalModelle = new();
        [ObservableProperty]
        MerkmalModelEintrag ausgewaehltesMerkmalModell = new();
        [ObservableProperty]
        private string statusmeldung;
        [ObservableProperty]
        private bool istListeGeladen;
        [ObservableProperty]
        private bool istXmlGeladen;
        [ObservableProperty]
        private bool istVerglichen;
        [ObservableProperty]
        private bool istDataGridSichtbar;
        [ObservableProperty]
        public int anzahlGefiltert;

        [RelayCommand]
        private async Task ExcelLadenAsync()
        {
            IstBeschaeftigt = true;

            // Dialog nur im UI-Thread
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Excel Dateien (*.xlsx)|*.xlsx",
                Title = "Excel-Datei mit Merkmalen auswählen"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                await Task.Delay(100); // Spinner zeigen

                // Dateipfad übergeben – Zugriff im Hintergrundthread erlaubt
                var daten = await Task.Run(() => _importService.LadeUndFiltereEintraege(openFileDialog.FileName));

                Eintraege = new ObservableCollection<MerkmalsEintrag>(daten);


                var patternText = File.ReadAllText("Config/typPattern.txt"); // z. B. "^[ATWED]"
                var typPattern = new Regex(patternText);
                //var typPattern = new Regex("^[ATW]");
                Typen = new ObservableCollection<string>(
                    Eintraege
                    .Select(e => e.Type)
                    .Where(t => !string.IsNullOrWhiteSpace(t) && typPattern.IsMatch(t))
                    .Distinct()
                    .OrderBy(t => t));

                GefilterteEintraege = CollectionViewSource.GetDefaultView(Eintraege);
                GefilterteEintraege.Filter = FilterEintraege;
                GefilterteEintraege?.Refresh();
                IstListeGeladen = daten.Any();
                IstVerglichen = false;
                IstXmlGeladen = false;

                Statusmeldung = $"✔ {Eintraege.Count} aus Excel geladen.";
            }
            else
            {
                Statusmeldung = "❌ Ladevorgang abgebrochen.";
            }

                IstBeschaeftigt = false;
            }
            [RelayCommand]
            private void LadeXmlMerkmale()
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "XML Merkmalsdatei (*.xml)|*.xml"
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var liste = _importService.LadeUntersteXmlMerkmale(openFileDialog.FileName);
                    XmlMerkmale = new ObservableCollection<XmlMerkmal>(liste);

                    IstXmlGeladen = liste.Any();
                    IstVerglichen = false;

                    Statusmeldung = $"✔ {liste.Count} Merkmale aus XML geladen.";
                }
            }
            [RelayCommand]
            private async Task AbgleichIdsMitXmlAsync()
            {

                if (XmlMerkmale == null || XmlMerkmale.Count == 0 || Eintraege == null)
                {
                    Statusmeldung = "⚠ Bitte zuerst XML- und Excel-Liste laden.";
                    return;
                }
                IstBeschaeftigt = true;
                await Task.Delay(100);
                int zugewiesen = 0;
                await Task.Run(() =>
                {
                    foreach (var eintrag in Eintraege)
                    {
                        var match = XmlMerkmale.FirstOrDefault(x => x.Bezeichnung.StartsWith(eintrag.MerkmalNameUndWert));
                        if (match != null)
                        {
                            eintrag.Id = match.Id;
                            zugewiesen++;
                        }
                    }
                });
                Statusmeldung = $"✔ {zugewiesen} IDs zugewiesen.";
                OnPropertyChanged(nameof(Eintraege));
                GefilterteEintraege?.Refresh();
                IstVerglichen = true;
                IstBeschaeftigt = false;
            }
            [RelayCommand]
            private void ExportiereXml()
            {
                var exportService = new ExportXmlService();

                //Nur Einträge mit zugewiesener ID
                var idListe = GefilterteEintraege
                    .Cast<MerkmalsEintrag>()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Id))
                    .Select(x => x.Id)
                    .Distinct()
                    .ToList();

                if (idListe.Count == 0) return;

                SaveFileDialog sfd = new()
                {
                    Filter = "XML Dateien (*.xml)|*.xml",
                    FileName = $"{DateTime.Today.ToString("yyyy-MM-dd")} {AusgewaehlterTyp}_{AusgewaehltesMerkmalModell.Model} Projektfilter",
                    Title = "Projektfilter-XML speichern"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    exportService.ExportiereXmlFilterDatei(idListe, sfd.FileName);
                }
            }
            [RelayCommand]
            private void FilterXmlAktualisieren()
            {
                MessageBox.Show("Mit der Ausführung dieses Befehls wird der ursprüngliche Filter aktualisiert und überschrieben.", "Achtung!", MessageBoxButtons.OKCancel);
                var dialog = new OpenFileDialog
                {
                    Filter = "XML Dateien (*.xml)|*.xml",
                    Title = "Filter-XML auswählen"
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var ids = GefilterteEintraege
                        .Cast<MerkmalsEintrag>()
                        .Where(x => !string.IsNullOrWhiteSpace(x.Id))
                        .Select(x => x.Id)
                        .Distinct()
                        .ToList();


                    var service = new ExportXmlService();
                    service.AktualisiereFilterXmlDatei(dialog.FileName, ids);
                }
            }

        private bool FilterEintraege(object obj)
        {
            if (obj is not MerkmalsEintrag eintrag) return false;

            bool typOk = string.IsNullOrWhiteSpace(AusgewaehlterTyp)
                || eintrag.Type == AusgewaehlterTyp;

            string modellFilter = AusgewaehltesMerkmalModell?.Model;

            bool modellOk = string.IsNullOrWhiteSpace(modellFilter)
                || eintrag.Model == modellFilter
                || eintrag.Model == "*";

            return typOk && modellOk;
        }

        partial void OnAusgewaehlterTypChanged(string value)
        {
            var neueModelle = Eintraege
    .Where(e => e.Type == value && e.Model != "*") // 👈 "*" ignorieren
    .Select(e => new MerkmalModelEintrag
    {
        Model = e.Model,
        ModellBez = e.ModellBez
    })
    .Where(m => !string.IsNullOrWhiteSpace(m.Model) && !string.IsNullOrWhiteSpace(m.ModellBez))
    .DistinctBy(m => m.Model)
    .OrderBy(m => m.ModellBez ?? string.Empty)
    .ToList();
            MerkmalModelle = new ObservableCollection<MerkmalModelEintrag>(neueModelle);
            AusgewaehltesMerkmalModell = null;
            GefilterteEintraege?.Refresh();
            AnzahlGefiltert = GefilterteEintraege.Cast<object>().Count();
        }

        partial void OnAusgewaehltesMerkmalModellChanged(MerkmalModelEintrag value)

        {
            GefilterteEintraege?.Refresh();
        }
    }
}
