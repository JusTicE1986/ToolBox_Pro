using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class MonatsInfo
    {
        public string MonatJahr { get; set; } = string.Empty;
        public TimeSpan MonatlicheSollzeit { get; set; }
        public TimeSpan MonatlichGearbeitet { get; set; }
        public TimeSpan MonatlicheAbweichung => MonatlichGearbeitet - MonatlicheSollzeit;
    }

}
