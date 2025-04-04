using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToolBox_Pro.Services
{
    public class ExportXmlService
    {
        public void ExportiereXmlFilterDatei(List<string> ids, string speicherpfad)
        {
            XNamespace ns = "";
            var root = new XElement("filter", new XAttribute("v", "2"));

            void AddCriterion(string id, string name, bool leading = false, string scope = "UPPERTREE")
            {
                var criterion = new XElement("criterion",
                    new XAttribute("type", "XmlDocument"),
                    new XAttribute("id", id),
                    new XAttribute("name", name));

                var selection = new XElement("ontologySelection",
                    new XAttribute("type", "XmlDocument"),
                    new XAttribute("scope", scope),
                    new XAttribute("logicalOperation", "OR"));

                if (leading) selection.SetAttributeValue("leadingTaxonomy", "true");

                criterion.Add(selection);
                root.Add(criterion);
            }

            // Feste Kriterien
            AddCriterion("232510723", "ontlc.SegmenteTaxonomyLink", leading: true);
            AddCriterion("235636995", "ontlc.TextartTaxonomyLink");
            AddCriterion("235642243", "ontlc.DokumentenartTaxonomyLink");
            AddCriterion("249021443", "ontlc.WerkTaxonomyLink");
            AddCriterion("14566926595", "ontlc.PGKComponentsTaxonomyLink", scope: "SUBTREE");

            // ID-Knoten
            var criterionVC = new XElement("criterion",
                new XAttribute("type", "XmlDocument"),
                new XAttribute("id", "5119443843"),
                new XAttribute("name", "ontlc.VariantConfigurationTaxonomyLink"));
            var ontologySelection = new XElement("ontologySelection",
                new XAttribute("scope", "SUBTREE"),
                new XAttribute("logicalOperation", "OR"));
            foreach (var id in ids)
            {
                ontologySelection.Add(new XElement("node", new XAttribute("id", id)));
            }

            criterionVC.Add(ontologySelection);
            root.Add(criterionVC);

            //Schreiben
            var doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), root);
            doc.Save(speicherpfad);
        }
    }
}
