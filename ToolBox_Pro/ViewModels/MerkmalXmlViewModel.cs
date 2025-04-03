using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class MerkmalXmlViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<MerkmalGruppeUndWert> eintraege = new();
        [ObservableProperty]
        private ObservableCollection<ExcelMerkmalEintrag> excelEintraege = new();

        [ObservableProperty]
        private ObservableCollection<MerkmalGruppeUndWert> xmlEintraege = new();


        [RelayCommand]
        private void ExportExcel()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel-Datei (*.xlsx)|*.xlsx",
                FileName = "Merkmale_Export"
            };

            if (dialog.ShowDialog() == true)
            {
                ExcelExportService.ExportiereXmlMerkmaleAlsExcel(Eintraege.ToList(), dialog.FileName);
                MessageBox.Show("Export abgeschlossen!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        [RelayCommand]
        private void ExportVergleich()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel-Datei (*.xlsx)|*.xlsx",
                FileName = "Merkmal_Vergleich"
            };

            if (dialog.ShowDialog() == true)
            {
                ExcelExportVergleichService.ExportiereMitVergleich(
                    ExcelEintraege.ToList(),    // deine geladene Excel-Liste
                    XmlEintraege.ToList(),      // deine geladene XML-Liste
                    dialog.FileName);

                MessageBox.Show("Vergleich erfolgreich exportiert!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public MerkmalXmlViewModel()
        {
            string pfad = @"\\wnad.net\global\all\lnz\Global_ALL_SCHEMA_ST4\00_Admin\Modellierung\SAP Merkmale.xml";
            var daten = XmlMerkmalImportService.LadeMerkmaleAusXml(pfad);
            
            Eintraege = new ObservableCollection<MerkmalGruppeUndWert>(daten);
        }
    }

}
