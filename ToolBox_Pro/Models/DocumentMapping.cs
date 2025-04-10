using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class DocumentMapping
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // ← Intern zur Identifikation

        public string Type { get; set; }
        public string Designation { get; set; }

        public string Brand { get; set; } // Enum? Begrenzte Anzahl von Marken => Auswahl
        public string Manufacturer { get; set; } // Enum? Begrenzte Anzahl von Manufacturer => Auswahl
        public string ProductType { get; set; } // Enum? Begrenzte Anzahl von Produktgruppen => Auswahl
        public string DocumentType { get; set; } // Enum? Begrenzte Anzahl von Dokumenttypen => Auswahl
        public string Layout { get; set; } // Enum? Begrenzte Anzahl von Layouts => Auswahl
        public string PIMGroup { get; set; } // Enum? Begrenzte Anzahl von PIM-Gruppen => Auswahl

        public string Version { get; set; }
        public string EditionDate { get; set; }

        public string DocumentContent { get; set; } // Enum? Begrenzte Anzahl von Inhalten => Auswahl
        public string Labor { get; set; } // Enum? Begrenzte Anzahl von Laboren => Auswahl
        public string CapitalMarket { get; set; } //Enum? Begrenzte Anzahl von Kapitalmärkten => Auswahl

        public string StandardFilter { get; set; }

        public string NodeTitle => $"{Type} - {Designation} {DocumentType}"; // Setzt sich zusammen aus $"{Type} - {Designation} {DocumentType}

        public string MaterialnumberSellingMachine { get; set; }

        public Dictionary<string, string> LanguageMapping { get; set; } = new();

        public string LanguagesFormatted =>
        LanguageMapping != null && LanguageMapping.Any()
            ? string.Join(" | ", LanguageMapping.Select(x => $"{x.Key}: {x.Value}"))
            : "–";

        public DocumentMapping Clone(bool preserveId = false)
        {
            return new DocumentMapping
            {
                Id = preserveId ? this.Id : Guid.NewGuid(),
                Type = this.Type,
                Designation = this.Designation,
                Brand = this.Brand,
                Manufacturer = this.Manufacturer,
                ProductType = this.ProductType,
                DocumentType = this.DocumentType,
                Layout = this.Layout,
                PIMGroup = this.PIMGroup,
                Version = this.Version,
                EditionDate = this.EditionDate,
                DocumentContent = this.DocumentContent,
                Labor = this.Labor,
                CapitalMarket = this.CapitalMarket,
                StandardFilter = this.StandardFilter,
                MaterialnumberSellingMachine = this.MaterialnumberSellingMachine,
                LanguageMapping = this.LanguageMapping != null
                    ? new Dictionary<string, string>(this.LanguageMapping) // WICHTIG: neue Instanz!
                    : new Dictionary<string, string>()
            };
        }


    }
}
