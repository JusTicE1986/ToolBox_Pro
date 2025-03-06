using ClosedXML.Excel;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ToolBox_Pro.Services
{
    public class MerkmalCsvExportService
    {
        public void ExportiereMerkmale(string excelPfad, string ZielOrdner, string sheetName, string vcTyp)
        {
            using (var stream = new FileStream(excelPfad, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            { 
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(sheetName);

            //Datenzeilen nach Kriterium filtern
            var merkmale = worksheet.RowsUsed().Skip(1)
                .Where(row =>
                row.Cell("J").GetString().Trim().Equals("JA", StringComparison.OrdinalIgnoreCase) &&
                row.Cell("M").GetString().Trim().Equals(vcTyp, StringComparison.OrdinalIgnoreCase))
                .Select(row => row.Cell("C").GetString().Trim())
                .Distinct() //doppelte Merkmale vermeiden
                .ToList();

            // CSV vorbereiten
            string datumHeute = DateTime.Today.ToString("yyyy-MM-dd");
            string csvFilename = $"{datumHeute}_Merkmale_{vcTyp.Replace(" ", "_")}.csv";
            string csvFullPath = Path.Combine(ZielOrdner, csvFilename);

            // Kopfzeile erstellen
            var kopfzeile = new List<string> { "Art-Nr", "Typ", "Gesamtpreis" };
            kopfzeile.AddRange(merkmale);
            // CSV erzeugen


            // CSV-Datei speichern, Merkmale in einer Zeile
            File.WriteAllText(csvFullPath, string.Join(";", kopfzeile), Encoding.UTF8);
            }
        }

        public void ExportiereMerkmaleMitBezeichnung(string excelPfad, string zielOrdner, string sheetName)
        {
            using (var stream = new FileStream(excelPfad, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(sheetName);

                var merkmale = worksheet.RowsUsed().Skip(1)
                    .Where(row => row.Cell("J").GetString().Trim().Equals("JA", StringComparison.OrdinalIgnoreCase))
                    .Select(row => new
                    {
                        Merkmal = row.Cell("C").GetString().Trim(),
                        Bezeichnung = row.Cell("F").GetString().Trim(),
                    })
                    .Distinct()
                    .OrderBy(m => m.Merkmal)
                    .ToList();


                // CSV-Dateiname erzeugen
                string datumHeute = DateTime.Today.ToString("yyyy-MM-dd");
                string csvFilename = $"{datumHeute}_Merkmale_mit_Bezeichnung.csv";
                string csvFullPath = Path.Combine(zielOrdner, csvFilename);

                // CSV schreiben

                using var writer = new StreamWriter(csvFullPath, false, Encoding.UTF8);

                // Kopfzeile
                writer.WriteLine("Merkmal;Bezeichnung");

                //Datenzeilen
                foreach (var item in merkmale)
                {
                    string merkmal = item.Merkmal.Replace(";", ",");
                    string bezeichnung = item.Bezeichnung.Replace(";", ",");
                    writer.WriteLine($"{item.Merkmal};{item.Bezeichnung}");
                }
            }
        }

        public void ErstellePreislisteNachArtNr(string excelPfad, string zielOrdner, string sheetName, string vcTyp)
        {
            using (var stream = new FileStream(excelPfad, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(sheetName);

                //Merkmale filtern und Kopfzeile vorbereiten
                var merkmale = worksheet.RowsUsed().Skip(1)
                    .Where(row => row.Cell("J").GetString().Trim().Equals("JA") &&
                                    row.Cell("M").GetString().Trim().Equals(vcTyp, StringComparison.OrdinalIgnoreCase) &&
                                    row.Cell("C").GetString().Trim() != "Gesamtpreis")
                    .Select(row => row.Cell("C").GetString().Trim())
                    .Distinct()
                    .OrderBy(m => m)
                    .ToList();


                merkmale.Insert(0, "Gesamtpreis");

                // Gruppierung eindeutig nach Art-Nr.
                var artikelGruppen = worksheet.RowsUsed().Skip(1)
                    .Where(row => row.Cell("J").GetString().Trim().Equals("JA") &&
                    row.Cell("M").GetString().Trim().Equals(vcTyp, StringComparison.OrdinalIgnoreCase))
                    .GroupBy(row => new
                    {
                        ArtNr = row.Cell("A").GetString().Trim(),
                        FzTyp = row.Cell("B").GetString().Trim()
                    });
                //.ToList();

                // csv erstellen
                string datumHeute = DateTime.Today.ToString("yyyy-MM-dd");
                string csvFileName = $"{datumHeute}_Preisliste_{vcTyp.Replace(" ", "_")}.csv";
                string csvFullPath = Path.Combine(zielOrdner, csvFileName);

                using var writer = new StreamWriter(csvFullPath, false, Encoding.UTF8);

                // Header schreiben
                var header = new List<string> { "Art-Nr", "Typ" };
                header.AddRange(merkmale);
                writer.WriteLine(string.Join(";", header));

                //Datenzeilen schreiben (eine pro Art-Nr.
                foreach (var gruppe in artikelGruppen)
                {
                    var zeile = new List<string>
                {
                    gruppe.Key.ArtNr,
                    gruppe.Key.FzTyp
                };

                    foreach (var merkmal in merkmale)
                    {
                        var preisZelle = gruppe.FirstOrDefault(row => row.Cell("C").GetString().Trim() == merkmal);
                        string preis = preisZelle != null ? preisZelle.Cell("I").GetString().Trim() : "";

                        zeile.Add(preis);
                    }
                    writer.WriteLine(string.Join(";", zeile));
                }
            }
        }
    }
}
