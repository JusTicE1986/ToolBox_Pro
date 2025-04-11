using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class UserManagementViewModel : ObservableObject
    {
        private readonly UserService _userService;

        [ObservableProperty]
        private AppUser selectedUser;

        public ObservableCollection<AppUser> Benutzerliste { get; set; }
        public ObservableCollection<string> WerkeListe { get; }
        public int BenutzerAnzahl => Benutzerliste?.Count ?? 0;

        public UserManagementViewModel()
        {
            _userService = new UserService();
            _userService.LoadUsers();
            Benutzerliste = new ObservableCollection<AppUser>(_userService.Users);
        }

        [RelayCommand]
        private void Speichern()
        {
            _userService.SaveUsers();
            MessageBox.Show("Benutzer gespeichert.");
        }

        private void RefreshAnzahl()
        {
            OnPropertyChanged(nameof(BenutzerAnzahl));
        }
    }
}