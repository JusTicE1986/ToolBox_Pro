using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services
{
    public class MerkmalsImportService
    {
        public List<MerkmalsEintrag> LadeUndFiltereEintraege(string dateipfad)
        {
            var result = new List<MerkmalsEintrag>();
            
                using var workbook = new XLWorkbook(dateipfad);
                var worksheet = workbook.Worksheets.First();
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                foreach (var row in rows)
                {
                    string vcAktion = row.Cell(5).GetString();
                    string bezeichMerkmalwert = row.Cell(8).GetString();


                    //if (vcAktion == "I")
                    if (vcAktion == "I" && !string.Equals(bezeichMerkmalwert, "Nein", System.StringComparison.OrdinalIgnoreCase))
                    {
                        result.Add(new MerkmalsEintrag
                        {
                            Type = row.Cell(1).GetString(),
                            Model = row.Cell(2).GetString(),
                            Merkmalname = row.Cell(3).GetString(),
                            Merkmalwert = row.Cell(4).GetString(),
                            VC_Aktion = row.Cell(5).GetString(),
                            ModellBez = row.Cell(6).GetString(),
                            MerkmalBezeichnung = row.Cell(7).GetString(),
                            BezeichMerkmalwert = bezeichMerkmalwert,
                        });
                    }
                }
            return result;
        }

        public List<XmlMerkmal> LadeUntersteXmlMerkmale(string xmlPfad)
        {
            var result = new List<XmlMerkmal>();

            XDocument doc = XDocument.Load(xmlPfad);
            XNamespace ns = "http://www.schema.de/2004/ST4/XmlImportExport/Node";

            var valueNodes = doc.Descendants(ns + "Data-OntologyTitle")
                .SelectMany(e => e.Elements(ns + "Value")
                .Where(v => (string)v.Attribute(ns + "Aspect") == "de"));

            foreach (var val in valueNodes)
            {
                var text = val.Value?.Trim();
                if(!string.IsNullOrWhiteSpace(text) && text.Contains("_"))
                {
                    var node = val.Ancestors(ns + "VariantConfigurationTaxonomyNode").FirstOrDefault();
                    if(node != null)
                    {
                        bool hatKinder = node.Elements(ns + "VariantConfigurationTaxonomyNode").Any();
                        if (!hatKinder)
                        {
                            var idRaw = node.Attribute(ns + "Id")?.Value;
                            if (!string.IsNullOrEmpty(idRaw) && idRaw.Contains("#"))
                            {
                                result.Add(new XmlMerkmal
                                {
                                    Bezeichnung = text,
                                    Id = idRaw.Split('#')[0]
                                });
                            }
                        }
                    }
                }
            }


            return result;
        }
    }
}
