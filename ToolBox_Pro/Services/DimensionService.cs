using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToolBox_Pro.Services
{
    class DimensionService
    {
        public string CheckFirstPageDimensions(string directoryPath)
        {
            // Prüfen, ob der Ordner existiert
            var realPath = Path.GetDirectoryName(directoryPath).Trim();
            

            // Liste aller PDF-Dateien im Ordner
            string[] pdfFiles = Directory.GetFiles(realPath, "*.pdf");

            if (pdfFiles.Length == 0)
            {
                Console.WriteLine("Keine PDF-Dateien im Ordner gefunden.");
                return "0";
            }

            //// Toleranzwerte für Beschnittzugabe (3 mm in Punkten)
            //const double tolerance = 8.5;

            //// DIN A4 und DIN A5 Abmessungen (in Punkten)
            //const double a4Width = 595.0;
            //const double a4Height = 842.0;
            //const double a5Width = 420.0;
            //const double a5Height = 595.0;
            const double converter = 0.3528;

            // Verarbeite jede PDF-Datei
            foreach (var pdfFile in pdfFiles)
            {
                try
                {
                    // PDF öffnen
                    using (PdfDocument document = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import))
                    {
                        Console.WriteLine($"PDF-Datei: {Path.GetFileName(pdfFile)}");

                        if (document.PageCount > 0)
                        {
                            var page = document.Pages[0]; // Nur die erste Seite analysieren
                            double width= page.Width * converter;
                            double height = page.Height * converter;

                            // Überprüfen, ob es DIN A4 oder DIN A5 ist
                            if (height > 297 && width > 210)
                            {
                                return "A4";
                            }
                            else if ((height < 297 && height > 210) && (width < 210 && width > 148))
                            {
                                return "A5";
                            }
                            else
                            {
                                return "0";
                            }
                        }
                        else
                        {
                            return "0";
                        }
                    }
                }
                catch (Exception ex)
                {
                   return $"Error: {ex.Message}";
                }
            }
            return "0";
        }

        public double CalculateWeight(int pageCount, string format)
        {
            // Gewicht pro Seite (in g)
            const double weight = 5.0;
            const double weightCover = 7.5;

            // Seitenzahl durch doppelseitigen Druck halbieren
            double pages = pageCount / 2.0;
            double totalWeight = 0;

            // Berechnung
            if (pageCount > 4 && format == "A4")
            {
                totalWeight = ((2 * weightCover) + ((pages - 4) * weight)) / 1000; // Gewicht in kg
            }
            else if (pageCount > 4 && format == "A5")
            {
                totalWeight = ((2 * weightCover) + ((pages - 4) * weight)) / 2000; // Gewicht in kg
            }
            else
            {
                totalWeight = (2 * weightCover) / 1000; // Gewicht in kg
            }

            return totalWeight;
        }
    }
}
