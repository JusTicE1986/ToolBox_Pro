using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class ConfigData
    {
        public List<LanguageMapping> Languages { get; set; } = new();
        public List<string> Brands { get; set; } = new();
        public List<string> Manufacturers { get; set; } = new();
        public List<string> ProductTypes { get; set; } = new();
        public List<string> Layouts { get; set; } = new();
        public List<string> DocumentTypes { get; set; } = new();
        public List<string> PIMGroups { get; set; } = new();
        public List<string> DocumentContents { get; set; } = new();
        public List<string> Labors { get; set; } = new();
        public List<string> CapitalMarkets { get; set; } = new();
    }

}
