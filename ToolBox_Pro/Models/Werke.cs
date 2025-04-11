using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public enum Werke
    {
        [Description("000 - Default")]
        Def = 000,
        
        [Description("100 - PGR")]
        PGR = 100,

        [Description("110 - PGP")]
        PGP = 110,

        [Description("120 - PGK")]
        PGK = 120,

        [Description("130 - PAL")]
        PAL = 130,

        [Description("200 - PNA")]
        PNA = 200,

        [Description("300 - PCP")]
        PCP = 300
    }
}
