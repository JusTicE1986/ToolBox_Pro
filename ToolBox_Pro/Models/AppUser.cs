
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox_Pro.Models
{
    public class AppUser
    {
        public string Username { get; set; } // von Environment.UserName
        public string DisplayName { get; set; } // Optional vom Admin ergänzt
        public UserRole Role { get; set; } = UserRole.NormalUser;
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsConfirmed { get; set; } = false;
    }
}
