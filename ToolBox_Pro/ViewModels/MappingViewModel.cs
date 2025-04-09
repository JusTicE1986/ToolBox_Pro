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

        public MappingViewModel()
        {
            _configService = new ConfigService();
            _configService.Load();


            _importService = new MaterialnummerImportService(_configService);

            SelectedMapping = new DocumentMapping();

            LoadLists();
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

                OnPropertyChanged(nameof(LanguageMappingPreview));
            }
        }

        [RelayCommand]
        private void AddCurrentMapping()
        {
            if (SelectedMapping == null) return;

            //SelectedMapping.NodeTitle = $"{SelectedMapping.Type} - {SelectedMapping.Designation} {SelectedMapping.DocumentType}";
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
                FileName = $"{DateTime.Now} PGK Materialnumber import OMs.xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var exporter = new ExcelExportService();
                exporter.Export(Mappings.ToList(), sfd.FileName);
            }
        }

        partial void OnSelectedMappingChanged(DocumentMapping value)
        {
            OnPropertyChanged(nameof(LanguageMappingPreview));
        }

        public string LanguageMappingPreview =>
    SelectedMapping?.LanguageMapping?.Count > 0
        ? string.Join(" | ", SelectedMapping.LanguageMapping.Select(kvp => $"{kvp.Key}: {kvp.Value}"))
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
    }
}
