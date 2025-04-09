using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Services
{
    public class MaterialnummerImportService
    {
        private readonly Dictionary<string, string> _codeToColumnMap;

        public MaterialnummerImportService(ConfigService configService)
        {
            if (configService?.Config?.Languages == null)
            {
                _codeToColumnMap = new Dictionary<string, string>();
                Debug.WriteLine("⚠️ Sprachmapping ist leer – config.json wurde evtl. nicht korrekt geladen.");
                return;
            }

            _codeToColumnMap = configService.Config.Languages
    .Where(x => !string.IsNullOrWhiteSpace(x.Code) && !string.IsNullOrWhiteSpace(x.Column))
    .ToDictionary(
        x => x.Code,                         // Key bleibt original
        x => x.Column,
        StringComparer.OrdinalIgnoreCase);  // 🔥 das ist der Gamechanger
            foreach (var entry in _codeToColumnMap)
            {
                Debug.WriteLine($"📦 Mapping-Eintrag: {entry.Key} → {entry.Value}");
            }

        }

        public Dictionary<string, string> ImportiereSpracheMaterialnummern(string filePath)
        {
            var result = new Dictionary<string, string>();

            using var workbook = new XLWorkbook(filePath);
            var sheet = workbook.Worksheets.First();

            // Annahme: Spalten wie bei dir (1-basiert!)
            const int komponentenNummerCol = 6;  // z. B. "1000517823"
            const int objektKurztextCol = 7;     // z. B. "Betriebsanleitung 3060 de"


            foreach (var row in sheet.RowsUsed().Skip(1))
            {
                var mat = row.Cell(komponentenNummerCol).GetString();
                var text = row.Cell(objektKurztextCol).GetString();

                Debug.WriteLine($"Material: '{mat}' | Kurztext: '{text}'");
            }



            foreach (var row in sheet.RowsUsed().Skip(1)) // erste Zeile ist Header
            {
                try
                {
                    var materialnummer = row.Cell(komponentenNummerCol).GetString().Trim();
                    var objektkurztext = row.Cell(objektKurztextCol).GetString().Trim();


                    if (string.IsNullOrWhiteSpace(materialnummer) || string.IsNullOrWhiteSpace(objektkurztext))
                        continue;

                    var parts = objektkurztext.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0)
                        continue;

                    var langCode = parts.LastOrDefault()?.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(langCode))
                        continue;

                    Debug.WriteLine($"🔎 langCode = '{langCode}' | Mapping Keys: {string.Join(", ", _codeToColumnMap.Keys)}");

                    if (_codeToColumnMap.TryGetValue(langCode, out var columnName) &&
                        !string.IsNullOrWhiteSpace(columnName))
                    {
                        result[columnName] = materialnummer;
                    }
                    else
                    {
                        Debug.WriteLine($"⚠️ Sprachcode '{langCode}' nicht im Mapping enthalten.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"❌ Fehler beim Verarbeiten einer Zeile: {ex.Message}");
                    continue;
                }
            }

            return result;
        }

    }
}
