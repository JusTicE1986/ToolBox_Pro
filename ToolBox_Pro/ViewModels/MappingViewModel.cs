using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class MappingViewModel : ObservableObject
    {
        private readonly ConfigService _configService;
        private readonly MaterialnummerImportService _importService;

        public ConfigService ConfigService => _configService;

        public ObservableCollection<string> Brands { get; } = new();
        public ObservableCollection<string> Manufacturers { get; } = new();
        public ObservableCollection<string> ProductTypes { get; } = new();
        public ObservableCollection<string> DocumentTypes { get; } = new();
        public ObservableCollection<string> Layouts { get; } = new();
        public ObservableCollection<string> PIMGroups { get; } = new();
        public ObservableCollection<string> DocumentContents { get; } = new();
        public ObservableCollection<string> Labors { get; } = new();
        public ObservableCollection<string> CapitalMarket { get; } = new();

        [ObservableProperty]
        private DocumentMapping selectedMapping;

        [ObservableProperty]
        private ObservableCollection<DocumentMapping> mappings = new();

        [ObservableProperty]
        private string statusMessage = "ℹ️ Noch keine Eingabe geprüft.";

        public MappingViewModel()
        {
            _configService = new ConfigService();
            _configService.Load();
            _importService = new MaterialnummerImportService(_configService);

            LoadLists();
            SelectedMapping = new DocumentMapping();
            ClearSelectedMapping();
        }

        [RelayCommand]
        private void ImportMaterialnummern()
        {
            // Schritt 1: Eingabevalidierung
            var errors = ValidateSelectedMapping();
            if (errors.Any())
            {
                StatusMessage = "⚠️ Eingabe ungültig:\n" + string.Join(Environment.NewLine, errors);
                return;
            }
            var ofd = new OpenFileDialog
            {
                Title = "Materialnummern-Datei auswählen.",
                Filter = "Excel-Dateien|*.xlsx;*.xls"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            // Schritt 3: Prüfen ob Datei verwendbar ist
            if (!IsFileReadable(ofd.FileName))
            {
                StatusMessage = $"❌ Datei '{System.IO.Path.GetFileName(ofd.FileName)}' ist gesperrt oder wird von einem anderen Programm verwendet.";
                return;
            }

            try
            {
                var mapping = _importService.ImportiereSpracheMaterialnummern(ofd.FileName);

                SelectedMapping.LanguageMapping ??= new();
                SelectedMapping.LanguageMapping.Clear();

                foreach (var kvp in mapping)
                    SelectedMapping.LanguageMapping[kvp.Key] = kvp.Value;

                OnPropertyChanged(nameof(LanguageMappingMultilinePreview));
                StatusMessage = $"✅ {mapping.Count} Einträge erfolgreich importiert.";

                AddCurrentMappingCommand.NotifyCanExecuteChanged();
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Fehler beim Import: {ex.Message}";
            }
        }

        [RelayCommand(CanExecute =nameof(CanAddMapping))]
        private void AddCurrentMapping()
        {
            if (SelectedMapping == null)
                return;

            var errors = ValidateSelectedMapping();

            StatusMessage = errors.Any()
                ? string.Join(Environment.NewLine, errors)
                : "✅ Alle Eingaben sind gültig.";

            if (errors.Any())
                return;

            var clone = SelectedMapping.Clone();
            clone.Id = Guid.NewGuid(); // explizit neue ID vergeben
            Mappings.Add(clone);
            StatusMessage += Environment.NewLine + "➕ Neuer Eintrag hinzugefügt.";

            ClearSelectedMapping(); // leert das Formular, bereitet neuen Typ vor
        }

        [RelayCommand]
        private void ExportMappings()
        {
            var sfd = new SaveFileDialog
            {
                Title = "Exportieren als Excel-Datei",
                Filter = "Excel-Datei|*.xlsx",
                FileName = $"{DateTime.Now:yyyy-MM-dd_HH_mm_ss} PGK Materialnumber import OMs.xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var exporter = new ExcelExportService(_configService);
                exporter.Export(Mappings.ToList(), sfd.FileName);
            }
        }

        [RelayCommand]
        private void DeleteSelectedMapping()
        {
            if (SelectedMapping != null && Mappings.Contains(SelectedMapping))
                Mappings.Remove(SelectedMapping);
        }
        [RelayCommand]
        private void DeleteAndReset()
        {
            if (SelectedMapping == null)
                return;

            // Eintrag entfernen
            var existing = Mappings.FirstOrDefault(m => m.Id == SelectedMapping.Id);
            if (existing != null)
            {
                Mappings.Remove(existing);
                StatusMessage = "🗑️ Eintrag gelöscht. Neues Formular bereit.";
            }
            else
            {
                StatusMessage = "⚠️ Kein passender Eintrag gefunden.";
            }

            // Formular leeren
            ClearSelectedMapping();
        }


        [RelayCommand]
        private void ResetMapping()
        {
            ClearSelectedMapping();
            StatusMessage = "ℹ️ Formular wurde zurückgesetzt.";
        }

        private void LoadLists()
        {
            Brands.AddRange(_configService.Config.Brands);
            Manufacturers.AddRange(_configService.Config.Manufacturers);
            ProductTypes.AddRange(_configService.Config.ProductTypes);
            DocumentTypes.AddRange(_configService.Config.DocumentTypes);
            Layouts.AddRange(_configService.Config.Layouts);
            PIMGroups.AddRange(_configService.Config.PIMGroups);
            DocumentContents.AddRange(_configService.Config.DocumentContents);
            Labors.AddRange(_configService.Config.Labors);
            CapitalMarket.AddRange(_configService.Config.CapitalMarkets);
        }

        private List<string> ValidateSelectedMapping()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(SelectedMapping.Type))
                errors.Add("❌ Typ darf nicht leer sein.");

            if (string.IsNullOrWhiteSpace(SelectedMapping.Designation))
                errors.Add("❌ Bezeichnung darf nicht leer sein.");

            if (string.IsNullOrWhiteSpace(SelectedMapping.Version) || !System.Text.RegularExpressions.Regex.IsMatch(SelectedMapping.Version, @"^\d+\.\d+$"))
                errors.Add("❌ Version muss im Format x.x vorliegen.");

            if (string.IsNullOrWhiteSpace(SelectedMapping.EditionDate))
                errors.Add("❌ Ausgabedatum darf nicht leer sein.");
            else
            {
                var parts = SelectedMapping.EditionDate.Split('/');
                if (parts.Length != 2 || !int.TryParse(parts[0], out int month) || month is < 1 or > 12 || !int.TryParse(parts[1], out int year) || year < 2000)
                    errors.Add("❌ Ausgabedatum muss im Format MM/YYYY vorliegen.");
            }

            if (string.IsNullOrWhiteSpace(SelectedMapping.MaterialnumberSellingMachine) ||
                !SelectedMapping.MaterialnumberSellingMachine.StartsWith("1000") ||
                SelectedMapping.MaterialnumberSellingMachine.Length != 10)
            {
                errors.Add("❌ Materialnummer muss mit 1000 beginnen und genau 10-stellig sein.");
            }

            return errors;
        }

        private void ClearSelectedMapping()
        {
            if (SelectedMapping == null)
                SelectedMapping = new DocumentMapping();

            SelectedMapping.Type = string.Empty;
            SelectedMapping.Designation = string.Empty;
            SelectedMapping.Brand = Brands.FirstOrDefault();
            SelectedMapping.Manufacturer = Manufacturers.FirstOrDefault();
            SelectedMapping.ProductType = ProductTypes.FirstOrDefault();
            SelectedMapping.DocumentType = DocumentTypes.FirstOrDefault();
            SelectedMapping.Layout = Layouts.FirstOrDefault();
            SelectedMapping.PIMGroup = PIMGroups.FirstOrDefault();
            SelectedMapping.Version = string.Empty;
            SelectedMapping.EditionDate = string.Empty;
            SelectedMapping.DocumentContent = DocumentContents.FirstOrDefault();
            SelectedMapping.Labor = Labors.FirstOrDefault();
            SelectedMapping.CapitalMarket = CapitalMarket.FirstOrDefault();
            SelectedMapping.StandardFilter = string.Empty;
            SelectedMapping.MaterialnumberSellingMachine = string.Empty;
            SelectedMapping.LanguageMapping = new Dictionary<string, string>();

            OnPropertyChanged(nameof(SelectedMapping));
            OnPropertyChanged(nameof(LanguageMappingMultilinePreview));
            AddCurrentMappingCommand.NotifyCanExecuteChanged();

        }

        public string LanguageMappingMultilinePreview =>
            SelectedMapping?.LanguageMapping?.Count > 0
                ? string.Join(Environment.NewLine, SelectedMapping.LanguageMapping.Select(kvp => $"{kvp.Key}: {kvp.Value}"))
                : "Noch keine Sprachzuordnung vorhanden";
        partial void OnSelectedMappingChanged(DocumentMapping value)
        {
            OnPropertyChanged(nameof(LanguageMappingMultilinePreview));
            StatusMessage = "ℹ️ Auswahl geändert. Eingaben ggf. prüfen.";
            OnPropertyChanged(nameof(StatusMessage));
            AddCurrentMappingCommand.NotifyCanExecuteChanged();
        }
        private bool CanAddMapping()
        {
            return SelectedMapping != null &&
                   !ValidateSelectedMapping().Any() &&
                   SelectedMapping.LanguageMapping is { Count: > 0 };
        }
        private bool IsFileReadable(string filePath)
        {
            try
            {
                using var stream = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                return true;
            }
            catch
            {
                return false;
            }
        }



    }


    // Hilfs-Extension für Collection-Auffüllen
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }
    }
}
