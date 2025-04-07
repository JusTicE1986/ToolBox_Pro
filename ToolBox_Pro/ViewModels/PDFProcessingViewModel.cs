using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ToolBox_Pro.Commands;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class PDFProcessingViewModel : ObservableObject
    {
        #region archiv
        //private string _pdfDirectory;
        //private readonly PDFProcessingService _pdfService;
        //private string _statusMessage;
        //private bool _isProcessing;

        //// Diese Sammlung enthält die verarbeiteten PDF-Daten, die im UserControl angezeigt werden
        //public ObservableCollection<PDFDataModel> ProcessedFiles { get; set; } = new ObservableCollection<PDFDataModel>();


        //public string PDFDirectory
        //{
        //    get => _pdfDirectory;
        //    set
        //    {
        //        if (_pdfDirectory != value)
        //        {
        //            _pdfDirectory = value;
        //            OnPropertyChanged(nameof(PDFDirectory));
        //        }
        //    }
        //}

        //// Das Kommando, das das PDF-Verarbeitungsprozess ausführt
        //public ICommand ProcessPDFsCommand { get; }
        //public ICommand SaveToExcelCommand { get; }

        //// Zeigt eine Statusnachricht (z.B. "Verarbeitung läuft...")
        //public string StatusMessage
        //{
        //    get => _statusMessage;
        //    set
        //    {
        //        if (_statusMessage != value)
        //        {
        //            _statusMessage = value;
        //            OnPropertyChanged(nameof(StatusMessage));
        //        }
        //    }
        //}

        //// Zeigt an, ob die Verarbeitung läuft
        //public bool IsProcessing
        //{
        //    get => _isProcessing;
        //    set
        //    {
        //        if (_isProcessing != value)
        //        {
        //            _isProcessing = value;
        //            OnPropertyChanged(nameof(IsProcessing));
        //        }
        //    }
        //}

        //// Konstruktor
        //public PDFProcessingViewModel()
        //{
        //    _pdfService = new PDFProcessingService();
        //    ProcessPDFsCommand = new RelayCommands(async () => await ProcessPDFsAsync());
        //    SaveToExcelCommand = new RelayCommands(() => SaveToExcel());
        //}

        //// Methode, um die PDFs asynchron zu verarbeiten und den Fortschritt zu überwachen
        //private async Task ProcessPDFsAsync()
        //{
        //    try
        //    {
        //        PDFDirectory = Path.GetFullPath(PDFDirectory.Trim());
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.MessageBox.Show($"Ungültiger Pfad: {ex.Message}");
        //        return;
        //    }
        //    if (!Directory.Exists(PDFDirectory))
        //    {
        //        System.Windows.MessageBox.Show("Das angegebene Verzeichnis existiert nicht.");
        //        return;
        //    }

        //    // Setze die Statusanzeige
        //    IsProcessing = true;
        //    StatusMessage = "Verarbeitung läuft...";

        //    try
        //    {
        //        // Erstelle eine Liste mit den zu verarbeitenden PDFs
        //        var pdfFiles = Directory.GetFiles(PDFDirectory, "*.pdf");

        //        // Fortschrittsanzeige erstellen
        //        var progress = new Progress<int>(percent =>
        //        {
        //            StatusMessage = $"Verarbeite... {percent}%";
        //        });

        //        var processedData = await Task.Run(() => _pdfService.ProcessPdfs(PDFDirectory, progress));

        //        // Die Textbox mit den verarbeiteten Dateien befüllen
        //        ProcessedFiles.Clear();
        //        foreach (var data in processedData)
        //        {
        //            //string fileInfo = $"Materialnummer: {data["Materialnummer"]}, " +
        //            //                  $"Format: {data["Format"]}, " +
        //            //                  $"Seitenzahl: {data["Seitenzahl"]}, " +
        //            //                  $"Gewicht: {data["Gewicht in kg"]} kg";
        //            ProcessedFiles.Add(new PDFDataModel
        //            {
        //                Materialnummer = data["Materialnummer"],
        //                Format = data["Format"],
        //                Seitenzahl = Convert.ToInt32(data["Seitenzahl"]),
        //                Gewicht = Convert.ToDouble(data["Gewicht in kg"]),
        //                AusgabeDatum = data["Ausgabedatum"],
        //                Typ = data["Fahrzeugtyp"],
        //                Model = data["Fahrzeugmodell"],
        //                Language = data["Language"],
        //                Version = data["Version"]
        //            });

        //        }

        //        //// Automatische Erstellung der Excel-Liste
        //        //_pdfService.ExportDataToExcel(processedData, PDFDirectory);
        //        //StatusMessage = "Verarbeitung abgeschlossen und Excel-Datei erstellt.";

        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = $"Fehler: {ex.Message}";
        //    }
        //    finally
        //    {
        //        IsProcessing = false;
        //    }

        //}
        //private void SaveToExcel()
        //{
        //    //System.Windows.MessageBox.Show("Funktion wird gerufen");
        //    try
        //    {
        //        if (ProcessedFiles.Count == 0)
        //        {
        //            StatusMessage = "Keine Daten zum Exportieren.";
        //            return;
        //        }

        //        // Daten für den Export vorbereiten (aus ProcessedFiles)
        //        var processedData = new List<Dictionary<string, string>>();
        //        foreach (var file in ProcessedFiles)
        //        {
        //            processedData.Add(new Dictionary<string, string>
        //            {
        //                { "Materialnummer", file.Materialnummer },
        //                { "Format", file.Format },
        //                { "Seitenzahl", file.Seitenzahl.ToString() },
        //                { "Gewicht in kg", file.Gewicht.ToString() },
        //                { "Ausgabedatum", file.AusgabeDatum },
        //                { "Fahrzeugtyp", file.Typ },
        //                { "Fahrzeugmodell", file.Model },
        //                { "Language", file.Language },
        //                { "Version", file.Version }

        //            });
        //        }

        //        // Excel-Datei exportieren
        //        _pdfService.ExportDataToExcel(processedData, PDFDirectory);
        //        StatusMessage = "Excel-Datei erfolgreich erstellt.";
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = $"Fehler beim Exportieren: {ex.Message}";
        //    }


        //}
        #endregion

        private readonly PDFProcessingService _pdfService = new();

        [ObservableProperty]
        private string pdfDirectory;
        [ObservableProperty]
        private string statusMessage;
        [ObservableProperty]
        private bool isProcessing;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTableVisible))]
        private ObservableCollection<PDFDataModel> processedFiles = new();

        public bool IsTableVisible => ProcessedFiles?.Count > 0;

        [RelayCommand]
        private void WaehleOrdner()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                PdfDirectory = dialog.SelectedPath;
                StatusMessage = $"📂 Ordner gewählt: {PdfDirectory}";
            }
        }

        [RelayCommand]
        private async Task VerarbeitePdfsAsync()
        {
            if (string.IsNullOrWhiteSpace(PdfDirectory) || !Directory.Exists(PdfDirectory))
            {
                StatusMessage = "❌ Ungültiger Ordnerpfad.";
                return;
            }

            IsProcessing = true;
            StatusMessage = "⏳ Verarbeitung gestartet...";
            ProcessedFiles.Clear();

            var progress = new Progress<int>(percent =>
            {
                StatusMessage = $"⏳ Verarbeitung... {percent}%";
            });

            try
            {
                var result = await Task.Run(() => _pdfService.ProcessPdfs(PdfDirectory, progress));

                foreach (var data in result)
                {
                    ProcessedFiles.Add(new PDFDataModel
                    {
                        Materialnummer = data["Materialnummer"],
                        Format = data["Format"],
                        Seitenzahl = Convert.ToInt32(data["Seitenzahl"]),
                        Gewicht = Convert.ToDouble(data["Gewicht in kg"]),
                        AusgabeDatum = data["Ausgabedatum"],
                        Typ = data["Fahrzeugtyp"],
                        Model = data["Fahrzeugmodell"],
                        Language = data["Language"],
                        Version = data["Version"]
                    });
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Fehler: {ex.Message}";
            }
            finally
            {
                IsProcessing = false;
                OnPropertyChanged(nameof(IsTableVisible));

            }
        }

        [RelayCommand]
        private void ExportiereExcel()
        {
            try
            {
                if (ProcessedFiles.Count == 0)
                {
                    StatusMessage = "⚠ Keine Daten zum Exportieren.";
                    return;
                }

                var data = new List<Dictionary<string, string>>();
                foreach (var file in ProcessedFiles)
                {
                    data.Add(new Dictionary<string, string>
                    {
                        { "Materialnummer", file.Materialnummer },
                        { "Format", file.Format },
                        { "Seitenzahl", file.Seitenzahl.ToString() },
                        { "Gewicht in kg", file.Gewicht.ToString("0.000", new CultureInfo("de-DE")) },
                        { "Ausgabedatum", file.AusgabeDatum },
                        { "Fahrzeugtyp", file.Typ },
                        { "Fahrzeugmodell", file.Model },
                        { "Language", file.Language },
                        { "Version", file.Version }
                    });
                }

                string outputDirectory = PdfDirectory;
                _pdfService.ExportDataToExcel(data, outputDirectory);

                StatusMessage = $"✅ Export abgeschlossen: {Path.Combine(outputDirectory, $"Betriebsanleitungen {DateTime.Now:dd-MM-yyyy}.xlsx")}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Fehler beim Exportieren: {ex.Message}";
            }
        }
    }
}
