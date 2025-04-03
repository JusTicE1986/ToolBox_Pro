using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services
{
    public static class ExcelExportVergleichService
    {
        public static void ExportiereMitVergleich(
            List<ExcelMerkmalEintrag> excelDaten,
            List<MerkmalGruppeUndWert> xmlDaten,
            string pfad)
        {
            var xmlVergleichswerte = xmlDaten
                .Select(x => x.Wert_DE?.Split(' ').FirstOrDefault())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToHashSet();

            foreach (var eintrag in excelDaten)
            {
                eintrag.TrefferInXML = xmlVergleichswerte.Contains(eintrag.Merkmalsbezeichnung);
            }

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Vergleich");

            ws.Cell(1, 1).Value = "Merkmalsbezeichnung";
            ws.Cell(1, 2).Value = "Bezeichnung (DE)";
            ws.Cell(1, 3).Value = "Bezeichnung (EN)";
            ws.Cell(1, 4).Value = "In XML vorhanden";

            for (int i = 0; i < excelDaten.Count; i++)
            {
                var row = i + 2;
                var e = excelDaten[i];

                ws.Cell(row, 1).Value = e.Merkmalsbezeichnung;
                ws.Cell(row, 2).Value = e.Bezeichnung_DE;
                ws.Cell(row, 3).Value = e.Bezeichnung_EN;
                ws.Cell(row, 4).Value = e.TrefferInXML ? "✔ Ja" : "✘ Nein";

                if (!e.TrefferInXML)
                {
                    ws.Range(row, 1, row, 4).Style.Fill.BackgroundColor = XLColor.LightPink;
                }
            }

            wb.SaveAs(pfad);
        }
    }
}
