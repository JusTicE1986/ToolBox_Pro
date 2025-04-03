using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class ExcelMerkmalEintrag
    {
        public string Merkmalsbezeichnung { get; set; }
        public string Bezeichnung_DE { get; set; }
        public string Bezeichnung_EN { get; set; }
        public bool TrefferInXML { get; set; }
    }
}
