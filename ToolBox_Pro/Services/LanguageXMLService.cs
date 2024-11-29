using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox_Pro.Models;
using ClosedXML.Excel;
using System.Xml;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using System.Xml.Serialization;

namespace ToolBox_Pro.Services
{
    public class LanguageXMLService
    {
        public void LoadDataFromExcel(LanguageXML languageData)
        {
            if (string.IsNullOrWhiteSpace(languageData.Path) || !File.Exists(languageData.Path))
            {
                throw new FileNotFoundException("Die angegebene Excel-Datei wurde nicht gefunden.");
            }

            using (var workbook = new XLWorkbook(languageData.Path))
            {
                var sheet = workbook.Worksheet(1);

                foreach (var cell in sheet.Row(1).Cells())

                {
                    if (!string.IsNullOrWhiteSpace(cell.GetString()))
                        languageData.ListOfLanguages.Add(cell.GetString());
                }

                for (int i = 2; i <= sheet.LastRowUsed().RowNumber(); i++)
                {
                    var row = sheet.Row(i);
                    var keyCell = row.Cell(1).GetString();

                    if (!string.IsNullOrWhiteSpace(keyCell) && keyCell.Contains("gentext"))
                    {
                        var key = ExtractKeyBetweenQuotes(keyCell);
                        languageData.ListOfKeys.Add(key);

                        var values = new List<string>();
                        for (int j = 1; j <= languageData.ListOfLanguages.Count; j++)
                        {
                            var cellValue = row.Cell(j).GetString();
                            if (!string.IsNullOrWhiteSpace(cellValue) && cellValue.Contains("gentext"))
                            {
                                values.Add(ExtractValueBetweenQuotes(cellValue));
                            }
                            else
                            {
                                values.Add("-");
                            }
                        }
                        languageData.ListOfValues.Add(values);
                    }
                }
                for (int i = 0; i < languageData.ListOfKeys.Count; i++)
                {
                    languageData.DictKeys[languageData.ListOfKeys[i]] = languageData.ListOfValues[i];
                }
            }
            GenerateLanguageXMLFiles(languageData);
            GenerateSTIXML(languageData);
        }

        public void GenerateLanguageXMLFiles(LanguageXML languageData)
        {
            if (languageData.ListOfLanguages.Count == 0 || languageData.ListOfKeys.Count == 0)
            {
                throw new InvalidOperationException("Es sind keine Daten zum Generieren der XML verfügbar.");
            }

            foreach (var language in languageData.ListOfLanguages)
            {
                var filePath = Path.Combine(@"S:\00_Admin\Language_xml\Language", $"Language_{language}.xml");

                // Erstelle XmlWriterSettings mit Indentation und Encoding
                var xmlWriterSettings = new XmlWriterSettings { Indent = true, Encoding = System.Text.Encoding.UTF8 };

                using (var writer = XmlWriter.Create(filePath, xmlWriterSettings))
                {
                    writer.WriteStartDocument();

                    // Setze den Namensraum für 'l' korrekt mit Prefix "l" in l:langdata
                    writer.WriteStartElement("l", "langdata", "http://www.schema.de/XSL/ST4DocuManagerlang");

                    // Schreibe das <l:langblock>-Element mit dem xml:lang Attribut
                    writer.WriteStartElement("l", "langblock", "http://www.schema.de/XSL/ST4DocuManagerlang");
                    writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", language);

                    // Iteriere über alle Schlüssel und Werte und schreibe <gentext>-Elemente
                    for (int i = 0; i < languageData.ListOfKeys.Count; i++)
                    {
                        var key = languageData.ListOfKeys[i];
                        var valueIndex = languageData.ListOfLanguages.IndexOf(language);
                        var value = languageData.DictKeys[key][valueIndex];

                        writer.WriteStartElement("l", "gentext", "http://www.schema.de/XSL/ST4DocuManagerlang");
                        writer.WriteAttributeString("key", key);
                        writer.WriteAttributeString("value", value);
                        writer.WriteEndElement();
                    }

                    // Schließe <langblock> und <langdata>
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                }
            }
        }

        public void GenerateSTIXML(LanguageXML languageData)
        {
            if (languageData.ListOfLanguages.Count == 0) throw new InvalidOperationException("Es sind keine Sprachdaten zum Erstellen vorhanden");

            using (var writer = new StreamWriter(languageData.TargetFile, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("<stimport>");
                writer.WriteLine("\t<node id=\"86781835\" class=\"/class::BaseNodeClass/DocuManagerBaseClass/Resource/DocumentResource\" parent=\"46764043\">");


                foreach (var language in languageData.ListOfLanguages)
                {
                    writer.WriteLine($"\t\t<attribute name=\"Resource\" type=\"resource\" aspect=\"{language}\">Language\\Language_{language}.xml</attribute>");
                    writer.WriteLine($"\t\t<attribute name=\"Title\" type=\"string\" aspect=\"{language}\">Language\\Language_{language}.xml</attribute>");
                    writer.WriteLine($"\t\t<attribute name=\"trans.SourceLanguage\" type=\"string\" aspect=\"{language}\">134150</attribute>");
                    writer.WriteLine($"\t\t<attribute name=\"trans.EditLanguage\" type=\"string\">134150</attribute>");
                    writer.WriteLine($"\t\t<attribute name=\"trans.Status\" type=\"xml\" aspect=\"{language}\">");
                    writer.WriteLine("\t\t\t<key name=\"Content\">Translated</key>");
                    writer.WriteLine("\t\t\t<key name=\"Title\">Translated</key>");
                    writer.WriteLine("\t\t</attribute>");
                }

                writer.WriteLine("\t</node>");
                writer.WriteLine("\t</stimport>");
            }
        }

        public string ExtractValueBetweenQuotes(string input)
        {
            var match = System.Text.RegularExpressions.Regex.Match(input, @"(?<=value\s*=\s*\"")[^\""]+");
            return match.Success ? match.Groups[0].Value : string.Empty;
        }
        public string ExtractKeyBetweenQuotes(string input)
        {
            var match = System.Text.RegularExpressions.Regex.Match(input, @"key=""([^""]+)""");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}
