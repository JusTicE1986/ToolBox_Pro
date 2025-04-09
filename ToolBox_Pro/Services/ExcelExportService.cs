using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services
{
    public class ExcelExportService
    {
        private readonly ConfigService _configService;

        public ExcelExportService(ConfigService configService)
        {
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
        }

        public void Export(List<DocumentMapping> mappings, string filePath)
        {
            if (mappings == null || mappings.Count == 0)
                throw new ArgumentException("Keine Mappings zum Exportieren vorhanden.");

            var fixedHeaders = new[]
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
                "Standard filter (meta.BB.Filter)"
            };

            var languageColumns = _configService.Config.Languages
                .Select(l => l.Column)
                .ToList();

            var allHeaders = fixedHeaders
                .Concat(languageColumns)
                .Concat(new[] { "Node Title" })
                .ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Document Mappings");

            // Header schreiben
            for (int i = 0; i < allHeaders.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = allHeaders[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }

            // Inhalte schreiben
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

                // Dynamische Sprachspalten
                for (int i = 0; i < languageColumns.Count; i++)
                {
                    var colName = languageColumns[i];
                    string matnr = string.Empty;
                    m.LanguageMapping?.TryGetValue(colName, out matnr);
                    worksheet.Cell(r, 16 + i).Value = matnr;
                }

                // Node Title
                var nodeTitle = $"{m.Type} - {m.Designation} {m.DocumentType}";
                worksheet.Cell(r, 16 + languageColumns.Count).Value = nodeTitle;
            }

            // Speichern
            workbook.SaveAs(filePath);
        }
    }
}
