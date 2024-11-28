using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToolBox_Pro.Commands;
using ToolBoxPro.Services;

namespace ToolBox_Pro.ViewModels
{
    public class PDFProcessingViewModel : BaseViewModel
    {
        private string _pdfDirectory;
        private readonly PDFProcessingService _pdfService;
        private string _statusMessage;
        private bool _isProcessing;

        // Diese Sammlung enthält die verarbeiteten PDF-Daten, die im UserControl angezeigt werden
        public ObservableCollection<string> ProcessedFiles { get; set; }

        public string PDFDirectory
        {
            get => _pdfDirectory;
            set
            {
                if (_pdfDirectory != value)
                {
                    _pdfDirectory = value;
                    OnPropertyChanged(nameof(PDFDirectory));
                }
            }
        }

        // Das Kommando, das das PDF-Verarbeitungsprozess ausführt
        public ICommand ProcessPDFsCommand { get; }

        // Zeigt eine Statusnachricht (z.B. "Verarbeitung läuft...")
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged(nameof(StatusMessage));
                }
            }
        }

        // Zeigt an, ob die Verarbeitung läuft
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                if (_isProcessing != value)
                {
                    _isProcessing = value;
                    OnPropertyChanged(nameof(IsProcessing));
                }
            }
        }

        // Konstruktor
        public PDFProcessingViewModel()
        {
            ProcessedFiles = new ObservableCollection<string>();
            _pdfService = new PDFProcessingService();
            ProcessPDFsCommand = new RelayCommands(async () => await ProcessPDFsAsync());
        }

        // Methode, um die PDFs asynchron zu verarbeiten und den Fortschritt zu überwachen
        private async Task ProcessPDFsAsync()
        {
            if (string.IsNullOrEmpty(PDFDirectory) || !Directory.Exists(PDFDirectory))
            {
                MessageBox.Show("Bitte geben Sie ein gültiges Verzeichnis ein.");
                return;
            }

            // Setze die Statusanzeige
            IsProcessing = true;
            StatusMessage = "Verarbeitung läuft...";

            try
            {
                // Erstelle eine Liste mit den zu verarbeitenden PDFs
                var pdfFiles = Directory.GetFiles(PDFDirectory, "*.pdf");

                // Fortschrittsanzeige erstellen
                var progress = new Progress<int>(percent =>
                {
                    StatusMessage = $"Verarbeite... {percent}%";
                });

                var processedData = await Task.Run(() => _pdfService.ProcessPdfs(PDFDirectory, progress));

                // Die Textbox mit den verarbeiteten Dateien befüllen
                ProcessedFiles.Clear();
                foreach (var data in processedData)
                {
                    string fileInfo = $"Materialnummer: {data["Materialnummer"]}, " +
                                      $"Format: {data["Format"]}, " +
                                      $"Seitenzahl: {data["Seitenzahl"]}, " +
                                      $"Gewicht: {data["Gewicht in kg"]} kg";
                    ProcessedFiles.Add(fileInfo);
                }

                // Automatische Erstellung der Excel-Liste
                _pdfService.ExportDataToExcel(processedData, PDFDirectory);
                StatusMessage = "Verarbeitung abgeschlossen und Excel-Datei erstellt.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler: {ex.Message}";
            }
            finally
            {
                IsProcessing = false;
            }
        }
    }
}
