using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Excel;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ToolBoxPro.Services
{
    public class PDFProcessingService
    {
        public List<Dictionary<string, string>> ProcessPdfs(string directoryPath, IProgress<int> progress)
        {
            var processedData = new List<Dictionary<string, string>>();
            var pdfFiles = Directory.GetFiles(directoryPath, "*.pdf");
            int totalFiles = pdfFiles.Length;

            for (int i = 0; i < totalFiles; i++)
            {
                var file = pdfFiles[i];
                var fileData = ProcessPdfFile(file);

                // Füge das verarbeitete PDF in die Liste ein
                processedData.Add(fileData);

                // Fortschrittsaktualisierung
                progress.Report((int)((i + 1) / (float)totalFiles * 100)); // Prozentualer Fortschritt
            }

            return processedData;
        }

        private Dictionary<string, string> ProcessPdfFile(string filePath)
        {
            var fileData = new Dictionary<string, string>();

            try
            {
                // Extrahieren der Seitenzahl
                int pageCount = GetPageCount(filePath);

                // Extrahieren der Materialnummer, Gewicht und Format aus dem PDF (Beispiel)
                string materialNumber = "Nicht gefunden";  // Hier müsste eine Methode kommen, um diese Information aus dem PDF zu extrahieren
                string format = "DIN A4";  // Beispiel, ggf. muss auch das Format aus dem PDF ermittelt werden
                string weight = "0.05";  // Beispielwert, auch hier muss ggf. eine Logik zum Extrahieren hinzugefügt werden

                fileData.Add("Materialnummer", materialNumber);
                fileData.Add("Format", format);
                fileData.Add("Seitenzahl", pageCount.ToString());
                fileData.Add("Gewicht in kg", weight);
            }
            catch (Exception ex)
            {
                fileData.Add("Error", $"Fehler beim Verarbeiten von {Path.GetFileName(filePath)}: {ex.Message}");
            }

            return fileData;
        }

        private int GetPageCount(string filePath)
        {
            using (PdfDocument document = PdfReader.Open(filePath, PdfDocumentOpenMode.ReadOnly))
            {
                return document.PageCount;
            }
        }

        public void ExportDataToExcel(List<Dictionary<string, string>> processedData, string outputDirectory)
        {
            var excelApp = new Application();
            excelApp.Visible = false; // Excel im Hintergrund ausführen
            Workbook workBook = excelApp.Workbooks.Add();
            Worksheet workSheet = (Worksheet)workBook.Sheets[1];

            // Kopfzeilen in die Excel-Datei einfügen
            workSheet.Cells[1, 1] = "Materialnummer";
            workSheet.Cells[1, 2] = "Format";
            workSheet.Cells[1, 3] = "Seitenzahl";
            workSheet.Cells[1, 4] = "Gewicht in kg";

            // Daten in die Excel-Datei einfügen
            for (int i = 0; i < processedData.Count; i++)
            {
                var data = processedData[i];
                workSheet.Cells[i + 2, 1] = data["Materialnummer"];
                workSheet.Cells[i + 2, 2] = data["Format"];
                workSheet.Cells[i + 2, 3] = data["Seitenzahl"];
                workSheet.Cells[i + 2, 4] = data["Gewicht in kg"];
            }

            // Excel-Datei speichern
            string filePath = Path.Combine(outputDirectory, "ProcessedPDFs.xlsx");
            workBook.SaveAs(filePath);
            workBook.Close(false);
            excelApp.Quit();
        }
    }
}
