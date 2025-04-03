using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

public partial class MerkmalVergleichViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<MerkmalGruppeUndWert> xmlEintraege = new();

    [ObservableProperty]
    private ObservableCollection<ExcelMerkmalEintrag> excelEintraege = new();

    // Excel-Datei laden
    [RelayCommand]
    private void LadeExcel()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Excel-Dateien (*.xlsx)|*.xlsx",
            Title = "Excel-Datei auswählen"
        };

        if (dialog.ShowDialog() == true)
        {
            var daten = ExcelMerkmalService.LadeAusExcel(dialog.FileName);
            ExcelEintraege = new ObservableCollection<ExcelMerkmalEintrag>(daten);
            MessageBox.Show($"Excel geladen ({daten.Count} Einträge)", "Erfolg");
        }
    }

    // XML-Datei laden
    [RelayCommand]
    private void LadeXml()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "XML-Dateien (*.xml)|*.xml",
            Title = "XML-Datei auswählen"
        };

        if (dialog.ShowDialog() == true)
        {
            var daten = XmlMerkmalImportService.LadeMerkmaleAusXml(dialog.FileName);
            XmlEintraege = new ObservableCollection<MerkmalGruppeUndWert>(daten);
            MessageBox.Show($"XML geladen ({daten.Count} Einträge)", "Erfolg");
        }
    }

    // Vergleich markieren (setzt TrefferInXml)
    [RelayCommand]
    private void Vergleiche()
    {
        var xmlWerte = XmlEintraege
            .Select(x => x.Wert_DE?.Split(' ').FirstOrDefault())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToHashSet();

        foreach (var eintrag in ExcelEintraege)
        {
            eintrag.TrefferInXML = xmlWerte.Contains(eintrag.Merkmalsbezeichnung);
        }

        MessageBox.Show("Vergleich durchgeführt.", "Vergleich");
    }

    // Export mit Markierung
    [RelayCommand]
    private void Exportiere()
    {
        var dialog = new SaveFileDialog
        {
            Filter = "Excel-Datei (*.xlsx)|*.xlsx",
            FileName = "Merkmal_Vergleich"
        };

        if (dialog.ShowDialog() == true)
        {
            ExcelExportVergleichService.ExportiereMitVergleich(
                ExcelEintraege.ToList(),
                XmlEintraege.ToList(),
                dialog.FileName);

            MessageBox.Show("Export abgeschlossen!", "Export");
        }
    }

    // Alles zurücksetzen
    [RelayCommand]
    private void Reset()
    {
        XmlEintraege.Clear();
        ExcelEintraege.Clear();
        MessageBox.Show("Listen wurden geleert.", "Zurückgesetzt");
    }
}
