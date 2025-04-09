using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services
{
    public class ExcelExportService
    {
        public void Export(List<DocumentMapping> mappings, string filePath)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Export");

            // 📌 Exakter Header
            var headers = new[]
            {
                "Type Designation (meta.BB.TypeVariant)",
                "Selling Designation (meta.BB.ProductName)",
                "Brand (meta.BB.Brand)",
                "Manufacturer (meta.BB.Manufacturer)",
                "ProductType -LE/CE/CE-PGP (meta.BB.ProductType)",
                "Document type (meta.BB.DocumentType)",
                "Layout (meta.BB.PrintFormat)",
                "Product type (meta.BB.PIMProductGroup)",
                "Version (meta.BB.ReleaseVersion)",
                "Edition Date (meta.BB.ReleaseDate)",
                "DocumentContent (meta.BB.DocumentContent)",
                "Labor (meta.BB.Labor)",
                "Materialnumber of selling machine (meta.BB.ObjectLink_MAT)",
                "Capital Market (meta.BB.CapitalMarket)",
                "Standard filter (meta.BB.Filter)",
                "arabic",
"bulgarian",
"czech",
"danish",
"german",
"greek",
"english",
"english-US",
"americas",
"spanish",
"mexican",
"estonian",
"finnish",
"french",
"canadian",
"croatian",
"hungarian",
"icelandic",
"italian",
"japanese",
"korean",
"lithuanian",
"latvian",
"dutch",
"norwegian",
"polnish",
"portuguese",
"brazilian",
"romanian",
"russian",
"slovakian",
"slovenian",
"serbian",
"swedish",
"turkish",
"ukrainian",
"chinese",

                "Node Title"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }

            // 📝 Inhalte schreiben
            for (int row = 0; row < mappings.Count; row++)
            {
                var m = mappings[row];
                int r = row + 2;

                worksheet.Cell(r, 1).Value = m.Type;
                worksheet.Cell(r, 2).Value = m.Designation;
                worksheet.Cell(r, 3).Value = m.Brand;
                worksheet.Cell(r, 4).Value = m.Manufacturer;
                worksheet.Cell(r, 5).Value = m.ProductType;
                worksheet.Cell(r, 6).Value = m.DocumentType;
                worksheet.Cell(r, 7).Value = m.Layout;
                worksheet.Cell(r, 8).Value = m.PIMGroup;
                worksheet.Cell(r, 9).Value = m.Version;
                worksheet.Cell(r, 10).Value = m.EditionDate;
                worksheet.Cell(r, 11).Value = m.DocumentContent;
                worksheet.Cell(r, 12).Value = m.Labor;
                worksheet.Cell(r, 13).Value = m.MaterialnumberSellingMachine;
                worksheet.Cell(r, 14).Value = m.CapitalMarket;
                worksheet.Cell(r, 15).Value = m.StandardFilter;

                // 📦 Sprach-Mapping basierend auf Spaltentiteln
                worksheet.Cell(r, 16).Value = m.LanguageMapping?.GetValueOrDefault("arabic");
                worksheet.Cell(r, 17).Value = m.LanguageMapping?.GetValueOrDefault("bulgarian");
                worksheet.Cell(r, 18).Value = m.LanguageMapping?.GetValueOrDefault("czech");
                worksheet.Cell(r, 19).Value = m.LanguageMapping?.GetValueOrDefault("danish");
                worksheet.Cell(r, 20).Value = m.LanguageMapping?.GetValueOrDefault("german");
                worksheet.Cell(r, 21).Value = m.LanguageMapping?.GetValueOrDefault("greek");
                worksheet.Cell(r, 22).Value = m.LanguageMapping?.GetValueOrDefault("english");
                worksheet.Cell(r, 23).Value = m.LanguageMapping?.GetValueOrDefault("english-US");
                worksheet.Cell(r, 24).Value = m.LanguageMapping?.GetValueOrDefault("americas");
                worksheet.Cell(r, 25).Value = m.LanguageMapping?.GetValueOrDefault("spanish");
                worksheet.Cell(r, 26).Value = m.LanguageMapping?.GetValueOrDefault("mexican");
                worksheet.Cell(r, 27).Value = m.LanguageMapping?.GetValueOrDefault("estonian");
                worksheet.Cell(r, 28).Value = m.LanguageMapping?.GetValueOrDefault("finnish");
                worksheet.Cell(r, 29).Value = m.LanguageMapping?.GetValueOrDefault("french");
                worksheet.Cell(r, 30).Value = m.LanguageMapping?.GetValueOrDefault("canadian");
                worksheet.Cell(r, 31).Value = m.LanguageMapping?.GetValueOrDefault("croatian");
                worksheet.Cell(r, 32).Value = m.LanguageMapping?.GetValueOrDefault("hungarian");
                worksheet.Cell(r, 33).Value = m.LanguageMapping?.GetValueOrDefault("icelandic");
                worksheet.Cell(r, 34).Value = m.LanguageMapping?.GetValueOrDefault("italian");
                worksheet.Cell(r, 35).Value = m.LanguageMapping?.GetValueOrDefault("japanese");
                worksheet.Cell(r, 36).Value = m.LanguageMapping?.GetValueOrDefault("korean");
                worksheet.Cell(r, 37).Value = m.LanguageMapping?.GetValueOrDefault("lithuanian");
                worksheet.Cell(r, 38).Value = m.LanguageMapping?.GetValueOrDefault("latvian");
                worksheet.Cell(r, 39).Value = m.LanguageMapping?.GetValueOrDefault("dutch");
                worksheet.Cell(r, 40).Value = m.LanguageMapping?.GetValueOrDefault("norwegian");
                worksheet.Cell(r, 41).Value = m.LanguageMapping?.GetValueOrDefault("polnish");
                worksheet.Cell(r, 42).Value = m.LanguageMapping?.GetValueOrDefault("portuguese");
                worksheet.Cell(r, 43).Value = m.LanguageMapping?.GetValueOrDefault("brazilian");
                worksheet.Cell(r, 44).Value = m.LanguageMapping?.GetValueOrDefault("romanian");
                worksheet.Cell(r, 45).Value = m.LanguageMapping?.GetValueOrDefault("russian");
                worksheet.Cell(r, 46).Value = m.LanguageMapping?.GetValueOrDefault("slovakian");
                worksheet.Cell(r, 47).Value = m.LanguageMapping?.GetValueOrDefault("slovenian");
                worksheet.Cell(r, 48).Value = m.LanguageMapping?.GetValueOrDefault("serbian");
                worksheet.Cell(r, 49).Value = m.LanguageMapping?.GetValueOrDefault("swedish");
                worksheet.Cell(r, 50).Value = m.LanguageMapping?.GetValueOrDefault("turkish");
                worksheet.Cell(r, 51).Value = m.LanguageMapping?.GetValueOrDefault("ukrainian");
                worksheet.Cell(r, 52).Value = m.LanguageMapping?.GetValueOrDefault("chinese");

                worksheet.Cell(r, 53).Value = $"{m.Type} - {m.Designation} {m.DocumentType}";
            }

            workbook.SaveAs(filePath);
        }
    }
}

