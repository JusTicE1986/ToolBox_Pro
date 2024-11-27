using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ToolBox_Pro.Commands;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public class OfferCalculationViewModel : BaseViewModel
    {
        // Eigenschaften
        // Private Felder
        private string _offerDestination;
        private ObservableCollection<OfferModel> _offers;
        private decimal _totalPrice;

        // Public Properties
        public string OfferDestination
        {
            get => _offerDestination;
            set
            {
                if (_offerDestination != value)
                {
                    _offerDestination = value;
                    OnPropertyChanged(nameof(OfferDestination));  // PropertyChanged benachrichtigen
                }
            }
        }

        public ObservableCollection<OfferModel> Offers
        {
            get => _offers;
            set
            {
                if (_offers != value)
                {
                    _offers = value;
                    OnPropertyChanged(nameof(Offers));  // PropertyChanged benachrichtigen
                }
            }
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    OnPropertyChanged(nameof(TotalPrice));  // PropertyChanged benachrichtigen
                }
            }
        }


        private readonly PDFService _pdfService;
        private readonly MailService _mailService;


        // Commands
        public ICommand SaveFilesCommand { get; }
        public ICommand AnalyzeOffersCommand { get; }
        public ICommand GenerateOfferCommand { get; }

        public OfferCalculationViewModel()
        {
            
            _pdfService = new PDFService();
            _mailService = new MailService();
            OfferDestination = @"C:\TEMP\Neuer Ordner";
            Offers = new ObservableCollection<OfferModel>();
            SaveFilesCommand = new RelayCommands(SaveFiles);
            AnalyzeOffersCommand = new RelayCommands(AnalyzeOffers);
            GenerateOfferCommand = new RelayCommands(GenerateOffer);
        }

        private void SaveFiles()
        {
            if (string.IsNullOrEmpty(OfferDestination))
            {
                MessageBox.Show("Bitte wählen Sie ein Zielverzeichnis aus.");
                return;
            }

            // Prüfen, ob der Zielordner existiert, wenn nicht, erstelle ihn
            if (!System.IO.Directory.Exists(OfferDestination))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(OfferDestination);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Erstellen des Verzeichnisses: {ex.Message}");
                    return;
                }
            }
            else
            {
                // Zielordner leeren: Nur E-Mail-Anhänge löschen, die bereits existieren
                try
                {
                    var files = System.IO.Directory.GetFiles(OfferDestination, "*.pdf"); // Nur PDF-Dateien löschen
                    foreach (var file in files)
                    {
                        System.IO.File.Delete(file);  // Löschen der Anhänge
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Leeren des Verzeichnisses: {ex.Message}");
                    return;
                }
            }

            // E-Mails mit Anhängen extrahieren und speichern
            try
            {
                string senderEmail = "andreas.neumann86@googlemail.com";  // Absender-E-Mail
                var savedFiles = _mailService.ExtractPDFsFromMails(senderEmail, OfferDestination);
                MessageBox.Show($"Es wurden {savedFiles.Count} PDF-Dateien gespeichert.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Extrahieren der Anhänge: {ex.Message}");
            }
        }

        private void AnalyzeOffers()
        {
            if (string.IsNullOrEmpty(OfferDestination))
            {
                MessageBox.Show("Bitte wählen Sie ein Verzeichnis aus.");
                return;
            }

            var offerList = _pdfService.ExtractPDFData(OfferDestination);
            Offers.Clear();
            foreach (var offer in offerList)
            {
                Offers.Add(offer);
            }
            CalculateTotalPrice();
        }
        private void GenerateOffer()
        {
            if (string.IsNullOrEmpty(OfferDestination))
            {
                MessageBox.Show("Bitte wählen Sie ein Verzeichnis aus.");
                return;
            }

            var pdfMergeService = new PdfMergeService(OfferDestination);
            var pdfDocument = pdfMergeService.CreatePdfWithSummary(Offers.ToList(), TotalPrice.ToString("N2") + " €");

            // PDF speichern
            var pdfPath = System.IO.Path.Combine(OfferDestination, $"Gesamtangebot vom {DateTime.Now.ToString("yyyy-MM-dd")}.pdf");
            pdfDocument.Save(pdfPath);

            MessageBox.Show($"Gesamtangebot wurde erstellt und gespeichert unter {pdfPath}");
        }
        private void CalculateTotalPrice()
        {
            TotalPrice = Offers.Sum(x => x.Price);
        }
    }
}
