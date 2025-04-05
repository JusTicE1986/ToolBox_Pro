using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Helpers
{
    public static class UserRoleValues
    {
        public static Array All => Enum.GetValues(typeof(UserRole));
    }
}
