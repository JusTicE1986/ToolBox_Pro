using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services
{
    public static class XmlMerkmalImportService
    {
        public static List<MerkmalGruppeUndWert> LadeMerkmaleAusXml(string dateipfad)
        {
            var doc = XDocument.Load(dateipfad);
            

            // Namespace für n: (wo VariantConfigurationTaxonomyNode liegt)
            XNamespace ns = "http://www.schema.de/2004/ST4/XmlImportExport/Node";

            var result = new List<MerkmalGruppeUndWert>();

            void VerarbeiteNode(XElement node)
            {
                var kinder = node.Elements(ns + "VariantConfigurationTaxonomyNode");

                if (kinder.Any())
                {
                    // Es gibt Unterknoten → das hier ist eine Gruppe → ignoriere
                    foreach (var kind in kinder)
                        VerarbeiteNode(kind); // rekursiv weiter prüfen
                }
                else
                {
                    // Unterster Knoten → prüfen, ob es sich um ein Merkmal handelt
                    var titelElement = node.Element(ns + "Data-OntologyTitle");
                    if (titelElement != null)
                    {
                        var de = titelElement.Elements(ns + "Value")
                            .FirstOrDefault(v => (string)v.Attribute(ns + "Aspect") == "de")?.Value;

                        var en = titelElement.Elements(ns + "Value")
                            .FirstOrDefault(v => (string)v.Attribute(ns + "Aspect") == "en-US")?.Value;

                        if (!string.IsNullOrWhiteSpace(de) && IstMerkmalstitel(de))
                        {
                            result.Add(new MerkmalGruppeUndWert
                            {
                                Gruppe = "", // leer, da keine Gruppierung mehr
                                Wert_DE = de,
                                Wert_EN = en ?? ""
                            });
                        }
                    }
                }
            }



            // Einstiegspunkt: alle <n:SystemFolder>
            var systemFolderNodes = doc.Descendants(ns + "SystemFolder").ToList();
            foreach (var folder in systemFolderNodes)
            {
                foreach (var variantNode in folder.Descendants(ns + "VariantConfigurationTaxonomyNode"))
                {
                    var debugTitle = variantNode.Descendants(ns + "Data-OntologyTitle");
                    if (debugTitle != null)
                    {
                        var debugValue = debugTitle.Descendants(ns + "Value")
                            .FirstOrDefault(v => (string)v.Attribute(ns + "Aspect") == "de")?.Value;

                    }
                    VerarbeiteNode(variantNode);
                }
            }

            bool IstMerkmalstitel(string titel)
            {
                var firstPart = titel.Split(' ').FirstOrDefault(); // z. B. "CWS_BACKUPW_001"
                if (firstPart == null) return false;

                var suffix = firstPart.Split('_').LastOrDefault(); // → "001"
                return suffix != null && suffix.Length == 3 && suffix.All(char.IsDigit);
            }

            return result;
        }

        
    }
}
