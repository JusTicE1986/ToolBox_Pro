using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services
{
    public static class ExcelMerkmalService
    {
        public static List<ExcelMerkmalEintrag> LadeAusExcel(string pfad)
        {
            var result = new List<ExcelMerkmalEintrag>();

            using var wb = new XLWorkbook(pfad);
            var ws = wb.Worksheets.Worksheet("Merkmalswerte");

            var rows = ws.RangeUsed().RowsUsed().Skip(1);

            var raw = rows
                .Select(r => new
                {
                    Nummer = r.Cell(1).GetString(),
                    Wert = r.Cell(2).GetString(),
                    Bezeichnung = r.Cell(3).GetString(),
                    Sprache = r.Cell(4).GetString()
                })
                .Where(x => x.Sprache is "DE" or "EN")
                .GroupBy(x => new { x.Nummer, x.Wert });

            foreach (var gruppe in raw)
            {
                var de = gruppe.FirstOrDefault(x => x.Sprache == "DE")?.Bezeichnung ?? "";
                var en = gruppe.FirstOrDefault(x => x.Sprache == "EN")?.Bezeichnung ?? "";

                result.Add(new ExcelMerkmalEintrag
                {
                    Merkmalsbezeichnung = $"{gruppe.Key.Nummer}_{gruppe.Key.Wert}",
                    Bezeichnung_DE = de,
                    Bezeichnung_EN = en
                });
            }

            return result;
        }
    }
}
