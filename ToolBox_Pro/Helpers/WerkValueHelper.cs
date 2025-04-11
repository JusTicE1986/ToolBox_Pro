using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Helpers
{
    public static class WerkValueHelper
    {
        public static IEnumerable<Werke> All => Enum.GetValues(typeof(Werke)).Cast<Werke>();
    }
}
