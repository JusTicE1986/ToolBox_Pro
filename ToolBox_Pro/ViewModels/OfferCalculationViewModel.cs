using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ToolBox_Pro.Commands;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class OfferCalculationViewModel : ObservableObject
    {
        private readonly PDFService _pdfService;
        private readonly MailService _mailService;

        [ObservableProperty]
        private string offerDestination;
        [ObservableProperty]
        private ObservableCollection<OfferModel> offers = new();
        [ObservableProperty]
        private decimal totalPrice;
        [ObservableProperty]
        private bool isBusy;
        [ObservableProperty]
        private string statusMessage = "Bereit";

        public OfferCalculationViewModel()
        {
            _pdfService = new PDFService();
            _mailService = new MailService();

            OfferDestination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KERN Angebote");
        }

        [RelayCommand]
        private void SaveFiles()
        {
            string senderMail = "transeng@e-kern.com";

            if (string.IsNullOrWhiteSpace(senderMail) || string.IsNullOrWhiteSpace(OfferDestination))
            {
                System.Windows.MessageBox.Show("Bitte Absender Email und Export-Pfad angeben.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var serviceCaller = new OutlookServiceCaller();
            serviceCaller.ExportAttachments(senderMail, OfferDestination);
        }
        [RelayCommand]
        private async Task AnalyzeOffersAsync()
        {
            if (string.IsNullOrEmpty(OfferDestination))
            {
                System.Windows.MessageBox.Show("Bitte ein Verzeichnis auswählen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsBusy = true;
            StatusMessage = "Angebote werden analysiert...";
            await Task.Run(() => {
                var offerList = _pdfService.ExtractPDFData(OfferDestination);

                App.Current.Dispatcher.Invoke(() =>
                {
                    Offers.Clear();
                    foreach (var offer in offerList)
                    {
                        Offers.Add(offer);
                    }
                    CalculateTotalPrice();
                });
            });
            StatusMessage = "Analyse abgeschlossen.";

            IsBusy = false;
        }
        [RelayCommand]
        private void GenerateOffer()
        {
            if (string.IsNullOrEmpty(OfferDestination))
            {
                System.Windows.MessageBox.Show("Bitte ein Verzeichnis auswählen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            try
            {
                var pdfMergeService = new PdfMergeService(OfferDestination);

                // 1. Übersicht als erste Seite erzeugen
                var mergedPdf = pdfMergeService.CreatePdfWithSummary(Offers.ToList(), TotalPrice.ToString("N2") + " €");

                // 2. Alle Angebots-PDFs aus dem Zielverzeichnis anhängen
                pdfMergeService.MergePdfs(mergedPdf);

                // 3. Gesamtdokument speichern
                var pdfPath = Path.Combine(OfferDestination, $"Gesamtangebot vom {DateTime.Now:yyyy-MM-dd}.pdf");
                mergedPdf.Save(pdfPath);

                StatusMessage = $"Gesamtangebot wurde erstellt und gespeichert unter\n{pdfPath}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Erstellen des Angebots:\n{ex.Message}";
            }
        }

        //[RelayCommand]
        //private void GenerateOffer()
        //{
        //    if (string.IsNullOrEmpty(OfferDestination))
        //    {
        //        System.Windows.MessageBox.Show("Bitte ein Verzeichnis auswählen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Stop);
        //        return;
        //    }

        //    var pdfMergeService = new PdfMergeService(OfferDestination);
        //    var pdfDocument = pdfMergeService.CreatePdfWithSummary(Offers.ToList(), TotalPrice.ToString("N2") + " €");

        //    var pdfPath = Path.Combine(OfferDestination, $"Gesamtangebot vom {DateTime.Now:yyyy-MM-dd}.pdf");
        //    pdfDocument.Save(pdfPath);

        //    System.Windows.MessageBox.Show($"Gesamtangebot wurde erstellt und gespeichert unter {pdfPath}", "Datei gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);

        //}

        private void CalculateTotalPrice()
        {
            TotalPrice = Offers.Sum(x => x.Price);
        }

        [RelayCommand]
        private void PickFolder()
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Speicherort für Angebote wählen",
                SelectedPath = OfferDestination
            };

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                OfferDestination = dialog.SelectedPath;
                StatusMessage = OfferDestination;
            }
            else
            {
                OfferDestination = OfferDestination;
            }
        }
    }
}
