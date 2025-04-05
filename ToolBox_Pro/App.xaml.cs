using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;


//            // Seed-Daten für User
//            var userService = new UserService();
//            userService.LoadUsers();

//            var seedUsers = new List<AppUser>
//{
//    new AppUser { Username = "andre", DisplayName = "Andreas", Role = UserRole.Admin, IsConfirmed = true },
//    new AppUser { Username = "lnzneuma", DisplayName = "Andreas", Role = UserRole.Admin, IsConfirmed = true },
//    new AppUser { Username = "lnzfiscc", DisplayName = "Christina", Role = UserRole.PriceLists, IsConfirmed = true },
//    new AppUser { Username = "lnzpohlj", DisplayName = "Jürgen", Role = UserRole.NormalUser, IsConfirmed = true },
//};

//            foreach (var user in seedUsers)
//            {
//                if (!userService.Users.Any(u => u.Username == user.Username))
//                    userService.AddUser(user);
//            }

//            userService.SaveUsers();

        }
    }
}
