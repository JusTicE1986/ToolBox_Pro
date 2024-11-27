using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using PdfSharp.Pdf.IO;
using ToolBox_Pro.Models;
using ToolBox_Pro.ViewModels;

namespace ToolBox_Pro.Services
{
    public class PdfMergeService
    {
        private readonly string _offerDestination;

        public PdfMergeService(string offerDestination)
        {
            _offerDestination = offerDestination;
        }

        // Erstellt eine PDF mit einer Übersicht und der Gesamtsumme
        public PdfDocument CreatePdfWithSummary(List<OfferModel> offers, string totalPrice)
        {
            var document = new PdfDocument();

            // Die erste Seite: Übersicht über alle Angebote und Gesamtsumme
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Position und Schriftart definieren
            var font = new XFont("Arial", 12);
            var boldFont = new XFont("Arial", 12, XFontStyleEx.Bold);

            // Tabelle Header
            double xPos = 20;
            double yPos = 20;
            gfx.DrawString("Angebotsdatei", boldFont, XBrushes.Black, new XPoint(xPos, yPos));
            gfx.DrawString("Summe", boldFont, XBrushes.Black, new XPoint(xPos + 400, yPos));

            yPos += 20;

            // Tabelle mit den Angeboten
            foreach (var offer in offers)
            {
                gfx.DrawString(offer.FileName, font, XBrushes.Black, new XPoint(xPos, yPos));
                gfx.DrawString(offer.Price.ToString("N2") + " €", font, XBrushes.Black, new XPoint(xPos + 400, yPos));
                yPos += 20;

                // Wenn der Text zu lang wird, gehe zur nächsten Seite
                if (yPos > page.Height - 50)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPos = 20; // Reset der y-Position
                }
            }

            // Gesamtsumme unter der Tabelle
            gfx.DrawString($"Gesamtsumme: {totalPrice}", boldFont, XBrushes.Black, new XPoint(xPos, yPos + 20));

            return document;
        }

        // Fügt eine bestehende PDF in das zusammengeführte PDF ein
        public void MergePdfs(PdfDocument mergedPdf)
        {
            var files = Directory.GetFiles(_offerDestination, "*.pdf");

            foreach (var file in files)
            {
                AddPdfToMergedPdf(mergedPdf, file);
            }
        }

        // Fügt eine PDF-Seite zum mergedPdf hinzu
        private void AddPdfToMergedPdf(PdfDocument mergedPdf, string filePath)
        {
            using (var reader = PdfReader.Open(filePath, PdfDocumentOpenMode.Import))
            {
                for (int i = 0; i < reader.PageCount; i++)
                {
                    var page = reader.Pages[i];
                    mergedPdf.AddPage(page);
                }
            }
        }

        //TODO Noch Funktion zum Zusammenführen der PDF Erstellen.
    }
}
