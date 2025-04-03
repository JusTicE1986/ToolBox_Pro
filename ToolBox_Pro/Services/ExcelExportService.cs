using System.Collections.Generic;
using ClosedXML.Excel;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services
{
    public static class ExcelExportService
    {
        public static void ExportiereXmlMerkmaleAlsExcel(List<MerkmalGruppeUndWert> daten, string pfad)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Merkmale");

            // Header
            ws.Cell(1, 1).Value = "Gruppe";
            ws.Cell(1, 2).Value = "Wert (DE)";
            ws.Cell(1, 3).Value = "Wert (EN)";

            for (int i = 0; i < daten.Count; i++)
            {
                var zeile = i + 2;
                ws.Cell(zeile, 1).Value = daten[i].Gruppe;
                ws.Cell(zeile, 2).Value = daten[i].Wert_DE;
                ws.Cell(zeile, 3).Value = daten[i].Wert_EN;
            }

            wb.SaveAs(pfad);
        }
    }
}
