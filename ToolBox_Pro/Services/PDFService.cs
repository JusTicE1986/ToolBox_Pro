using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using ToolBox_Pro.Models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace ToolBox_Pro.Services
{
    public class PDFService
    {
        public List<OfferModel> ExtractPDFData(string folderPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            List<OfferModel> pdfList = new List<OfferModel>();
            decimal sumOffer = 0;

            foreach (var file in directoryInfo.GetFiles())
            {
                if (file.Extension.ToLower() == ".pdf" && file.Name.StartsWith("attr"))
                {
                    string pdfText = ExtractTextFromPDF(file.FullName);
                    decimal nettoValue = ExtractNettoValue(file.FullName);

                    if (nettoValue > 0)
                    {
                        int PosWN = 0;
                        if (file.Name.Contains("WN"))
                        {
                            PosWN = file.Name.IndexOf("WN");
                        }
                        else if (file.Name.Contains("Wacker_Neuson"))
                        {
                            PosWN = file.Name.IndexOf("Wacker_Neuson");
                        }
                        int posLanguage = file.Name.IndexOf("de-DE");
                        int PosLength = posLanguage - PosWN + 11;

                        OfferModel offer = new OfferModel(file.Name.Substring(PosWN, PosLength), nettoValue);
                        pdfList.Add(offer);

                        sumOffer += nettoValue;
                    }

                }
            }
            return pdfList;
        }

        private string ExtractTextFromPDF(string filePath)
        {
            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                string text = string.Empty;
                foreach (UglyToad.PdfPig.Content.Page page in document.GetPages())
                {
                    text += page.Text;
                }
                return text;
            }
        }

        private decimal ExtractNettoValue(string filePath)
        {
            try
            {
                using (var pdfDocument = PdfiumViewer.PdfDocument.Load(filePath))
                {
                    StringBuilder text = new StringBuilder();
                    for (int i = 0; i < pdfDocument.PageCount; i++)
                    {
                        text.Append(pdfDocument.GetPdfText(i));
                    }

                    // Angebotssumme per RegEx suchen
                    string pattern = @"Netto\s*\(.*?\)\s*([\d.,]+)";
                    var match = Regex.Match(text.ToString(), pattern);

                    if (match.Success)
                    {
                        // Den gefundenen Betrag parsen
                        string amountText = match.Groups[1].Value.Replace(".", "").Replace(",", ".");
                        if (decimal.TryParse(amountText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal result))
                        {
                            return result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Extrahieren des Netto-Wertes: {ex.Message}");
            }
            return 0;
        }
    }
}
