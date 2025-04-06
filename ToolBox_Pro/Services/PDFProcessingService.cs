using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ToolBox_Pro.Services
{
    public class PDFProcessingService
    {
        #region archiv
        //private readonly PDFService _pdfService;
        //private readonly DimensionService _dimensionService;

        //public PDFProcessingService()
        //{
        //    _pdfService = new PDFService();
        //    _dimensionService = new DimensionService();
        //}

        //public List<Dictionary<string, string>> ProcessPdfs(string directoryPath, IProgress<int> progress)
        //{
        //    var processedData = new List<Dictionary<string, string>>();
        //    var pdfFiles = Directory.GetFiles(directoryPath, "*.pdf");
        //    int totalFiles = pdfFiles.Length;

        //    for (int i = 0; i < totalFiles; i++)
        //    {
        //        var file = pdfFiles[i];
        //        var fileData = ProcessPdfFile(file);

        //        // Füge das verarbeitete PDF in die Liste ein
        //        processedData.Add(fileData);

        //        // Fortschrittsaktualisierung
        //        progress.Report((int)((i + 1) / (float)totalFiles * 100)); // Prozentualer Fortschritt
        //    }

        //    return processedData;
        //}

        //private Dictionary<string, string> ProcessPdfFile(string filePath)
        //{
        //    var fileData = new Dictionary<string, string>();

        //    try
        //    {
        //        // Extrahieren der Seitenzahl
        //        int pageCount = GetPageCount(filePath);
        //        string text = _pdfService.ExtractTextFromPDF(filePath);
        //        // Extrahieren der Materialnummer, Gewicht und Format aus dem PDF (Beispiel)
        //        string materialNumber = ExtractMaterialNumber(text);  // Hier müsste eine Methode kommen, um diese Information aus dem PDF zu extrahieren
        //        string format = _dimensionService.CheckFirstPageDimensions(filePath); // Beispiel, ggf. muss auch das Format aus dem PDF ermittelt werden
        //        double weight = _dimensionService.CalculateWeight(pageCount, format);  // Beispielwert, auch hier muss ggf. eine Logik zum Extrahieren hinzugefügt werden
        //        string editionDate = ExtractEditionDate(text);  // Beispielwert, auch hier muss ggf. eine Logik zum Extrahieren hinzugefügt werden
        //        string vehicleType = ExtractTypeFromText(text);  // Beispielwert, auch hier muss ggf. eine Logik zum Extrahieren hinzugefügt werden
        //        string vehicleModel = ExtractModelFromFileName(Path.GetFileName(filePath).Trim());
        //        string language = ExtractLanguageFromText(text);
        //        string version = ExtractVersionFromFileName(Path.GetFileName(filePath).Trim());

        //        fileData.Add("Materialnummer", materialNumber);
        //        fileData.Add("Format", format);
        //        fileData.Add("Seitenzahl", pageCount.ToString());
        //        fileData.Add("Gewicht in kg", weight.ToString());
        //        fileData.Add("Ausgabedatum", editionDate);
        //        fileData.Add("Fahrzeugtyp", vehicleType);
        //        fileData.Add("Fahrzeugmodell", vehicleModel);
        //        fileData.Add("Language", language);
        //        fileData.Add("Version", version);
        //    }
        //    catch (Exception ex)
        //    {
        //        fileData.Add("Error", $"Fehler beim Verarbeiten von {Path.GetFileName(filePath)}: {ex.Message}");
        //    }

        //    return fileData;
        //}
        //#region extractions
        //private static string ExtractMaterialNumber(string text)
        //{
        //    var match = Regex.Match(text, @"(?<!\d)\d{10}(?!\d)");
        //    return match.Success ? match.Value : "Nicht gefunden";
        //}

        //private static string ExtractEditionDate(string text)
        //{
        //    var match = Regex.Match(text, @"\d{2}\/\d{4}");
        //    return match.Success ? match.Value : "Nicht gefunden";
        //}
        //private static string ExtractModelFromFileName(string text)
        //{
        //    var match = Regex.Match(text, @"(?<=^OM\s)(\S+)");
        //    return match.Success ? match.Value : "Nicht gefunden";
        //}
        //private static string ExtractVersionFromFileName(string text)
        //{
        //    //var version = text.Substring(text.IndexOf(" V")+2, 3);
        //    var startVersion = text.IndexOf(" V");
        //    var endVersion = text.LastIndexOf(" ");
        //    var lengthVersion = endVersion - startVersion - 2;
        //    var version = text.Substring(startVersion + 2, lengthVersion);
        //    return version;
        //}
        //private static string ExtractTypeFromText(string text)
        //{
        //    var match = Regex.Match(text, @"(A\d{2}-\d{2}|T\d{2}-\d{2}|RL\d{2}-\d{2}|RL\d{2}T-\d{2})");
        //    return match.Success ? match.Value : "Nicht gefunden";
        //}
        //private static string ExtractLanguageFromText(string text)
        //{
        //    var match = Regex.Match(text, @"\[\w{2}\]");
        //    return match.Success ? match.Value : "Nicht gefunden";
        //}


        //private static int GetPageCount(string filePath)
        //{
        //    using PdfDocument document = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
        //    return document.PageCount;
        //}
        //#endregion

        //#region Excel Export
        //public void ExportDataToExcel(List<Dictionary<string, string>> processedData, string outputDirectory)
        //{
        //    #region Microsoft Excel
        //    //    var excelApp = new Application
        //    //    {
        //    //        Visible = false // Excel im Hintergrund ausführen
        //    //    };
        //    //    Workbook workBook = excelApp.Workbooks.Add();
        //    //    Worksheet workSheet = (Worksheet)workBook.Sheets[1];

        //    //    // Kopfzeilen in die Excel-Datei einfügen
        //    //    workSheet.Cells[1, 1] = "Materialnummer";
        //    //    workSheet.Cells[1, 2] = "Format";
        //    //    workSheet.Cells[1, 3] = "Seitenzahl";
        //    //    workSheet.Cells[1, 4] = "Gewicht in kg";
        //    //    workSheet.Cells[1, 5] = "Ausgabedatum";
        //    //    workSheet.Cells[1, 6] = "Fahrzeugtyp";
        //    //    workSheet.Cells[1, 7] = "Fahrzeugmodell";
        //    //    workSheet.Cells[1, 8] = "Language";


        //    //    // Daten in die Excel-Datei einfügen
        //    //    for (int i = 0; i < processedData.Count; i++)
        //    //    {
        //    //        var data = processedData[i];
        //    //        workSheet.Cells[i + 2, 1] = data["Materialnummer"];
        //    //        workSheet.Cells[i + 2, 2] = data["Format"];
        //    //        workSheet.Cells[i + 2, 3] = data["Seitenzahl"];
        //    //        workSheet.Cells[i + 2, 4] = data["Gewicht in kg"];
        //    //        workSheet.Cells[i + 2, 5] = data["Ausgabedatum"];
        //    //        workSheet.Cells[i + 2, 6] = data["Fahrzeugtyp"];
        //    //        workSheet.Cells[i + 2, 7] = data["Fahrzeugmodell"];
        //    //        workSheet.Cells[i + 2, 8] = data["Language"];
        //    //    }

        //    //    // Excel-Datei speichern
        //    //    string filePath = Path.Combine(outputDirectory, $"Betriebsanleitungen {DateTime.Now:dd-MM-yyyy}.xlsx");
        //    //    workBook.SaveAs(filePath);
        //    //    workBook.Close(false);
        //    //    excelApp.Quit();
        //    //}
        //    #endregion


        //    // Erstelle eine neue Excel-Datei
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("Betriebsanleitungen");

        //        // Kopfzeilen in die Excel-Datei einfügen
        //        worksheet.Cell(1, 1).Value = "Materialnummer";
        //        worksheet.Cell(1, 2).Value = "Format";
        //        worksheet.Cell(1, 3).Value = "Version";
        //        worksheet.Cell(1, 4).Value = "Ausgabe";
        //        worksheet.Cell(1, 5).Value = "Fahrzeugtyp";
        //        worksheet.Cell(1, 6).Value = "Verkaufsbezeichnung";
        //        worksheet.Cell(1, 7).Value = "Seitenzahl";
        //        worksheet.Cell(1, 8).Value = "Sprache";
        //        worksheet.Cell(1, 9).Value = "Gewicht in kg";

        //        // Daten in die Excel-Datei einfügen
        //        for (int i = 0; i < processedData.Count; i++)
        //        {
        //            var data = processedData[i];
        //            worksheet.Cell(i + 2, 1).Value = data["Materialnummer"];
        //            worksheet.Cell(i + 2, 2).Value = data["Format"];
        //            worksheet.Cell(i + 2, 3).Value = data["Version"];
        //            worksheet.Cell(i + 2, 4).Value = data["Ausgabedatum"];
        //            worksheet.Cell(i + 2, 5).Value = data["Fahrzeugtyp"];
        //            worksheet.Cell(i + 2, 6).Value = data["Fahrzeugmodell"];
        //            worksheet.Cell(i + 2, 7).Value = data["Seitenzahl"];
        //            worksheet.Cell(i + 2, 8).Value = data["Language"];
        //            worksheet.Cell(i + 2, 9).Value = data["Gewicht in kg"];
        //        }

        //        // Kopfzeilen formatieren (fett)
        //        var headerRange = worksheet.Range(1, 1, 1, 9); // Zeile 1, Spalten 1 bis 9
        //        headerRange.Style.Font.Bold = true;

        //        // Rahmenlinien um die gesamte Tabelle hinzufügen
        //        var tableRange = worksheet.Range(1, 1, processedData.Count + 1, 9); // Von Kopfzeilen bis letzte Zeile
        //        tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        //        tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        //        // Spaltenbreite automatisch anpassen
        //        worksheet.Columns().AdjustToContents();

        //        // Excel-Datei speichern
        //        string filePath = Path.Combine(outputDirectory, $"Betriebsanleitungen {DateTime.Now:dd-MM-yyyy}.xlsx");
        //        workbook.SaveAs(filePath);
        //    }
        //    #endregion
        //}
        #endregion
        private readonly PDFService _pdfService = new();
        private readonly DimensionService _dimensionService = new();

        public List<Dictionary<string, string>> ProcessPdfs(string dictionaryPath, IProgress<int>? progress = null, Action<string>? log = null)
        {
            var processedData = new List<Dictionary<string, string>>();
            var pdfFiles = Directory.GetFiles(dictionaryPath, "*.pdf");
            int totalFiles = pdfFiles.Length;

            for (int i = 0; i < totalFiles; i++)
            {
                var file = pdfFiles[i];
                log?.Invoke($"📄 Verarbeite: {Path.GetFileName(file)}");
                var fileData = ProcessPdfFile(file, log);
                processedData.Add(fileData);
                progress?.Report((int)((i + 1) / (float)totalFiles * 100));
            }
            return processedData;
        }

        private Dictionary<string, string> ProcessPdfFile(string filePath, Action<string>? log = null)
        {
            var fileData = new Dictionary<string, string>();

            try
            {
                int pageCount = GetPageCount(filePath);
                string text = _pdfService.ExtractTextFromPDF(filePath);

                string materialNumber = ExtractMaterialNumber(text);
                string format = _dimensionService.CheckFirstPageDimensions(filePath);
                double weight = _dimensionService.CalculateWeight(pageCount, format);
                string editionDate = ExtractEditionDate(text);
                string vehicleType = ExtractTypeFromText(text);
                string vehicleModel = ExtractModelFromFileName(Path.GetFileName(filePath));
                string language = ExtractLanguageFromText(text);
                string version = ExtractVersionFromFileName(Path.GetFileName(filePath));

                fileData.Add("Materialnummer", materialNumber);
                fileData.Add("Format", format);
                fileData.Add("Seitenzahl", pageCount.ToString());
                fileData.Add("Gewicht in kg", weight.ToString("F2"));
                fileData.Add("Ausgabedatum", editionDate);
                fileData.Add("Fahrzeugtyp", vehicleType);
                fileData.Add("Fahrzeugmodell", vehicleModel);
                fileData.Add("Language", language);
                fileData.Add("Version", version);
            }
            catch (Exception ex)
            {
                fileData.Add("Error", $"Fehler bei {Path.GetFileName(filePath)}: {ex.Message}");
                log?.Invoke($"❌ Fehler: {ex.Message}");
            }

            return fileData;
        }
        private static string ExtractMaterialNumber(string text) =>
            Regex.Match(text, @"(?<!\d)\d{10}(?!\d)").ValueOrDefault("Nicht gefunden");

        private static string ExtractEditionDate(string text) =>
            Regex.Match(text, @"\d{2}/\d{4}").ValueOrDefault("Nicht gefunden");

        private static string ExtractModelFromFileName(string fileName) =>
            Regex.Match(fileName, @"(?<=^OM\s)(\S+)").ValueOrDefault("Nicht gefunden");

        private static string ExtractVersionFromFileName(string fileName)
        {
            int start = fileName.IndexOf(" V") + 2;
            int end = fileName.LastIndexOf(" ");
            if (start > 1 && end > start) return fileName.Substring(start, end - start);
            return "Nicht gefunden";
        }

        private static string ExtractTypeFromText(string text) =>
            Regex.Match(text, @"(A\d{2}-\d{2}|T\d{2}-\d{2}|RL\d{2}-\d{2}|RL\d{2}T-\d{2})").ValueOrDefault("Nicht gefunden");

        //private static string ExtractLanguageFromText(string text) =>
        //    Regex.Match(text, @"\[\w{2}\]").ValueOrDefault("Nicht gefunden");

        private static string ExtractLanguageFromText(string text)
        {
            var match = Regex.Match(text, @"\[\w{2}(?:_\w{2})?\]");
            return match.Success ? match.Value.Trim('[', ']') : "Nicht gefunden";
        }


        private static int GetPageCount(string filePath)
        {
            using var doc = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
            return doc.PageCount;
        }

        public void ExportDataToExcel(List<Dictionary<string, string>> data, string outputDirectory)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Betriebsanleitungen");

            var headers = new[] { "Materialnummer", "Format", "Version", "Ausgabedatum", "Fahrzeugtyp", "Fahrzeugmodell", "Seitenzahl", "Language", "Gewicht in kg" };
            for (int i = 0; i < headers.Length; i++) worksheet.Cell(1, i + 1).Value = headers[i];

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < headers.Length; j++)
                    worksheet.Cell(i + 2, j + 1).Value = data[i].GetValueOrDefault(headers[j], "");
            }

            worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.RangeUsed().Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            worksheet.Columns().AdjustToContents();

            string filePath = Path.Combine(outputDirectory, $"Betriebsanleitungen {DateTime.Now:dd-MM-yyyy}.xlsx");
            workbook.SaveAs(filePath);
        }
    }

    public static class RegexExtensions
    {
        public static string ValueOrDefault(this Match match, string fallback) =>
            match.Success ? match.Value : fallback;
    }
}
