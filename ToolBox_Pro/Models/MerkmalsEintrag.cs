using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class MerkmalsEintrag
    {
        public string Type { get; set; }
        public string Model { get; set; }
        public string Merkmalname { get; set; }
        public string Merkmalwert { get; set; }
        public string MerkmalNameUndWert => $"{Merkmalname}_{Merkmalwert}";
        public string VC_Aktion { get; set; }
        public string ModellBez { get; set; }
        public string MerkmalBezeichnung { get; set; }
        public string BezeichMerkmalwert { get; set; }
        public bool VerwendetInXml { get; set; }
        public string Id { get; set; }

    }
}
