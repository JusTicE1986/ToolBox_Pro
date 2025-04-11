using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class ProjektTypEntry
    {
        public string Typ { get; set; }
        public string BezWM { get; set; }
        public string BezWN { get; set; }
        public string BezKramerBau { get; set; }
        public string BezKramerLand { get; set; }
        public DateTime? SOPNullserie { get; set; }
        public DateTime? SODNullserie { get; set; }
        public DateTime? SOPSerie { get; set; }
        public DateTime? SODSerie { get; set; }
        public bool IstAktiv { get; set; }
        public bool IsEditable { get; set; } = true;

        public bool IstSOPIn2Monaten 
        { get 
            {
                var cutoff = DateTime.Today.AddMonths(2);
                return (SOPNullserie.HasValue && SOPNullserie.Value <= cutoff) ||
                    (SOPSerie.HasValue && SOPSerie.Value <= cutoff);
            } 
        }
        // public bool IstSOP => true wenn SOPNullserie < DateTime.Today + 2 Monate || SOPSerie < DateTime.Today + 2 Monate
    }
}
