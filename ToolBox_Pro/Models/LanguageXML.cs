using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class LanguageXML
    {
        public string Path { get; set; }
        public string TargetFile { get; set; }
        public List<string> ListOfKeys { get; set; }
        public List<List<string>> ListOfValues { get; set; }
        public List<string> ListOfLanguages { get; set; }
        public Dictionary<string, List<string>> DictKeys { get; set; }

        public LanguageXML()
        {
            ListOfKeys = new List<string>();
            ListOfValues = new List<List<string>>();
            ListOfLanguages = new List<string>();
            DictKeys = new Dictionary<string, List<string>>();
        }
    }
}
