using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public enum UserRole
    {
        [Description("Standarduser")]
        NormalUser,

        [Description("Administrator")]
        Admin,

        [Description("Preislisten")]
        PriceLists,
        
        [Description("Abteilungsleiter TED")]
        HeadOfTED,


    }
}
