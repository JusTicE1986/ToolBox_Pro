using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class MappingViewModel : ObservableObject
    {
        private readonly ConfigService _configService;
        public ConfigService ConfigService => _configService;
        private readonly MaterialnummerImportService _importService;

        public ObservableCollection<string> Brands { get; set; } = new();
        public ObservableCollection<string> Manufacturers { get; set; } = new();
        public ObservableCollection<string> ProductTypes { get; set; } = new();
        public ObservableCollection<string> DocumentTypes { get; set; } = new();
        public ObservableCollection<string> Layouts { get; set; } = new();
        public ObservableCollection<string> PIMGroups { get; set; } = new();
        public ObservableCollection<string> DocumentContents { get; set; } = new();
        public ObservableCollection<string> Labors { get; set; } = new();
        public ObservableCollection<string> CapitalMarket { get; set; } = new();

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

            SelectedMapping = new DocumentMapping();

            LoadLists();

            // Nach dem Laden der Listen erste Werte setzen
            if (Brands.Any()) SelectedMapping.Brand = Brands[0];
            if (Manufacturers.Any()) SelectedMapping.Manufacturer = Manufacturers[0];
            if (ProductTypes.Any()) SelectedMapping.ProductType = ProductTypes[0];
            if (DocumentTypes.Any()) SelectedMapping.DocumentType = DocumentTypes[0];
            if (Layouts.Any()) SelectedMapping.Layout = Layouts[0];
            if (PIMGroups.Any()) SelectedMapping.PIMGroup = PIMGroups[0];
            if (DocumentContents.Any()) SelectedMapping.DocumentContent = DocumentContents[0];
            if (Labors.Any()) SelectedMapping.Labor = Labors[0];
            if (CapitalMarket.Any()) SelectedMapping.CapitalMarket = CapitalMarket[0];
        }

        [RelayCommand]
        private void ImportMaterialnummern()
        {
            var ofd = new OpenFileDialog
            {
                Title = "Materialnummern-Datei auswählen.",
                Filter = "Excel-Dateien|*.xlsx;*.xls"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var mapping = _importService.ImportiereSpracheMaterialnummern(ofd.FileName);

                if (SelectedMapping.LanguageMapping == null)
                    SelectedMapping.LanguageMapping = new();

                SelectedMapping.LanguageMapping.Clear();

                foreach (var kvp in mapping)
                {
                    SelectedMapping.LanguageMapping[kvp.Key] = kvp.Value;
                }

                OnPropertyChanged(nameof(LanguageMappingMultilinePreview));
            }
        }

        [RelayCommand]
        private void AddCurrentMapping()
        {
            if (SelectedMapping == null) return;
            var errors = new List<string>();
            // 1. Validierung: Hersteller = PGK => Version muss x.x
            if(SelectedMapping.Manufacturer == "PGK")
            {
                var versionPattern = @"^\d+\.\d+$";
                if (string.IsNullOrWhiteSpace(SelectedMapping.Version) ||
                    !System.Text.RegularExpressions.Regex.IsMatch(SelectedMapping.Version, versionPattern))
                {
                    errors.Add("❌ Version muss im Format x.x sein (z. B. 2.3)");
                    return;
                }
            }

            // 2. Validierung: EditionDate muss im Format MM/YYYY sein und MM <= 12
            if (!string.IsNullOrWhiteSpace(SelectedMapping.EditionDate))
            {
                var parts = SelectedMapping.EditionDate.Split("/");
                if(parts.Length != 2 ||
                    !int.TryParse(parts[0], out int month) ||
                    month <1 || month > 12 ||
                    !int.TryParse(parts[1], out int year) ||
                    parts[1].Length != 4 ||
                    year < 2000 || year > DateTime.Now.Year + 1)
                {
                    errors.Add("❌ Ausgabedatum muss im Format MM/YYYY vorliegen (z. B. 06/2024).");
                }
            }
            else
            {
                errors.Add("❌ Ausgabedatum darf nicht leer sein.");
            }

            // 3. Validierung: VK-Materialnummer 10-Stellige Zahl mit 1000 beginnend.
            if (string.IsNullOrWhiteSpace(SelectedMapping.MaterialnumberSellingMachine))
            {
                errors.Add("❌ Die VK-Materialnummer darf nicht leer sein.");
            }
            else
            {
                var matNr = SelectedMapping.MaterialnumberSellingMachine;

                if(matNr.Length != 10 || !matNr.All(char.IsDigit))
                {
                    errors.Add("❌ Die VK-Materialnummer muss genau 10 Ziffern enthalten.");
                }

                // Muss mit 1000 beginnen
                if (!matNr.StartsWith("1000"))
                {
                    errors.Add("❌ Die VK-Materialnummer muss mit '1000' beginnen.");
                }
            }

            // 4. Keines der übrigen Textfelder darf leer sein
            if (string.IsNullOrWhiteSpace(SelectedMapping.Type))
                errors.Add("❌ Das Feld 'Type' darf nicht leer sein.");

            if (string.IsNullOrWhiteSpace(SelectedMapping.Designation))
                errors.Add("❌ Das Feld 'Designation' darf nicht leer sein.");

            if (string.IsNullOrWhiteSpace(SelectedMapping.StandardFilter))
                errors.Add("❌ Das Feld 'Standard Filter' darf nicht leer sein.");


            //SelectedMapping.NodeTitle = $"{SelectedMapping.Type} - {SelectedMapping.Designation} {SelectedMapping.DocumentType}";
            StatusMessage = errors.Any()
                ? string.Join(Environment.NewLine, errors)
                : "✅ Alle Eingaben sind gültig.";
            Debug.WriteLine(StatusMessage);
            Mappings.Add(SelectedMapping);
            SelectedMapping = new DocumentMapping();
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
        private void UpdateSelectedMapping()
        {
            if (SelectedMapping != null)
            {
                var index = Mappings.IndexOf(SelectedMapping);
                if (index >= 0)
                {
                    Mappings[index] = SelectedMapping;
                    OnPropertyChanged(nameof(Mappings));
                }
            }
        }

        [RelayCommand]
        public void ForceUpdateSelectedMapping()
        {
            OnPropertyChanged(nameof(SelectedMapping));
            OnPropertyChanged(nameof(LanguageMappingPreview));
            OnPropertyChanged(nameof(LanguageMappingMultilinePreview));
        }

        //partial void OnSelectedMappingChanged(DocumentMapping value)
        //{
        //    OnPropertyChanged(nameof(LanguageMappingMultilinePreview));
        //    ValidateMapping();
        //}

        public string LanguageMappingPreview =>
        SelectedMapping?.LanguageMapping?.Count > 0
        ? string.Join(" | ", SelectedMapping.LanguageMapping.Select(kvp => $"{kvp.Key}: {kvp.Value}"))
        : "Noch keine Sprachzuordnung vorhanden";

        public string LanguageMappingMultilinePreview =>
        SelectedMapping?.LanguageMapping?.Count > 0
        ? string.Join(Environment.NewLine, SelectedMapping.LanguageMapping.Select(kvp => $"{kvp.Key}: {kvp.Value}"))
        : "Noch keine Sprachzuordnung vorhanden";

        private void LoadLists()
        {
            foreach (var brand in _configService.Config.Brands) 
            {
                Brands.Add(brand);
            }
            foreach (var manufacturer in _configService.Config.Manufacturers)
            {
                Manufacturers.Add(manufacturer);
            }
            foreach (var productType in _configService.Config.ProductTypes)
            {
                ProductTypes.Add(productType);
            }
            foreach (var docType in _configService.Config.DocumentTypes)
            {
                DocumentTypes.Add(docType);
            }
            foreach (var layout in _configService.Config.Layouts)
            {
                Layouts.Add(layout);
            }
            foreach (var pimGroup in _configService.Config.PIMGroups)
            {
                PIMGroups.Add(pimGroup);
            }
            foreach (var docContent in _configService.Config.DocumentContents)
            {
                DocumentContents.Add(docContent);
            }
            foreach (var labor in _configService.Config.Labors)
            {
                Labors.Add(labor);
            }
            foreach (var capitalMarket in _configService.Config.CapitalMarkets)
            {
                CapitalMarket.Add(capitalMarket);
            }
        }

        private void ValidateMapping()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(SelectedMapping.Type))
                errors.Add("❌ Typ darf nicht leer sein.");

            if (SelectedMapping.Manufacturer == "PGK")
            {
                if (string.IsNullOrWhiteSpace(SelectedMapping.Version) || !System.Text.RegularExpressions.Regex.IsMatch(SelectedMapping.Version, @"^\d+\.\d+$"))
                    errors.Add("❌ Version muss im Format x.x vorliegen.");
            }

            if (string.IsNullOrWhiteSpace(SelectedMapping.EditionDate) || !System.Text.RegularExpressions.Regex.IsMatch(SelectedMapping.EditionDate, @"^(0[1-9]|1[0-2])/20\d{2}$"))
                errors.Add("❌ Ausgabedatum muss im Format MM/YYYY vorliegen.");

            if (string.IsNullOrWhiteSpace(SelectedMapping.MaterialnumberSellingMachine) ||
                SelectedMapping.MaterialnumberSellingMachine.Length != 10 ||
                !SelectedMapping.MaterialnumberSellingMachine.StartsWith("1000"))
                errors.Add("❌ VK-Materialnummer muss mit '1000' beginnen und 10 Ziffern lang sein.");

            if (string.IsNullOrWhiteSpace(SelectedMapping.Designation) ||
                string.IsNullOrWhiteSpace(SelectedMapping.Brand) ||
                string.IsNullOrWhiteSpace(SelectedMapping.Manufacturer) ||
                string.IsNullOrWhiteSpace(SelectedMapping.ProductType) ||
                string.IsNullOrWhiteSpace(SelectedMapping.DocumentType) ||
                string.IsNullOrWhiteSpace(SelectedMapping.Layout) ||
                string.IsNullOrWhiteSpace(SelectedMapping.PIMGroup) ||
                string.IsNullOrWhiteSpace(SelectedMapping.DocumentContent) ||
                string.IsNullOrWhiteSpace(SelectedMapping.Labor) ||
                string.IsNullOrWhiteSpace(SelectedMapping.CapitalMarket) ||
                string.IsNullOrWhiteSpace(SelectedMapping.StandardFilter))
                errors.Add("❌ Alle Felder müssen ausgefüllt sein.");

            StatusMessage = errors.Any()
                ? string.Join(Environment.NewLine, errors)
                : "✅ Alle Eingaben sind gültig.";
        }

    }
}
