using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;
using ToolBox_Pro.Views;

namespace ToolBox_Pro.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        #region Neues ViewModel mit NavigationItems
        private bool _isFlyoutExpanded;
        public bool IsFlyoutExpanded
        {
            get => _isFlyoutExpanded;
            set
            {
                SetProperty(ref _isFlyoutExpanded, value);
            }
        }

        private NavigationItem _selectedNavigationItem;
        public NavigationItem SelectedNavigationItem
        {
            get => _selectedNavigationItem;
            set
            {
                if (SetProperty(ref _selectedNavigationItem, value))
                {
                    SwitchView(value);
                }
            }
        }
        private UserRole _currentUserRole;
        public UserRole CurrentUserRole
        {
            get => _currentUserRole;
            set
            {
                SetProperty(ref _currentUserRole, value);
                OnPropertyChanged(nameof(FilteredNavigationItems));
            }
        }


        private readonly UserService _userService = new UserService();

        public string Begruessungstext => $"Willkommen {CurrentUserDisplayNameOrUsername} bei der ToolBox Pro 👋";

        [ObservableProperty]
        private object currentView;

        [ObservableProperty]
        private string footerText = "ToolBox Pro © 2025 – Version 1.0.0 by Andreas Neumann";

        public bool IstStartAktiv => CurrentView == null;

        public ObservableCollection<NavigationItem> AllNavigationItems { get; } = new()
{
    new NavigationItem("Projekte und Typen", "FileTreeOutline", new ProjektTypenView()),
    new NavigationItem("Projektfilter erstellen", "FilterVariant", new MerkmalsImportView()),
    new NavigationItem("Blaue Bücher Liste", "FormTextbox", new MappingView()),
    new NavigationItem("KERN Angebote", "Folder", new OfferCalculation()),
    new NavigationItem("Seitenzahlen & Gewicht", "Scale", new PDFProcessingView()),
    new NavigationItem("Preisliste exportieren", "FileExport", new PreislsiteExportView(), UserRole.PriceLists),
    new NavigationItem("Ordner bereinigen", "DeleteSweep", new CleanupView(), UserRole.Admin),
    new NavigationItem("Language_XML", "Translate", new Views.LanguageXML(), UserRole.Admin),
    new NavigationItem("User Settings", "AccountMultiple", new UserManagementView())
};

        public IEnumerable<NavigationItem> FilteredNavigationItems =>
            AllNavigationItems.Where(item =>
                item.RequiredRole == null || item.RequiredRole == CurrentUserRole || CurrentUserRole == UserRole.Admin);


        partial void OnCurrentViewChanged(object value)
        {
            OnPropertyChanged(nameof(IstStartAktiv));
        }

        public MainWindowViewModel()
        {
            CurrentUserRole = GetCurrentUserRole();
        }

        private void SwitchView(NavigationItem item)
        {
            if (item == null) return;
            if (item.RequiredRole == null || item.RequiredRole == CurrentUserRole || CurrentUserRole == UserRole.Admin)
            {
                CurrentView = item.View;
            }
        }

        public Visibility AdminVisibility =>
            CurrentUserRole == UserRole.Admin ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PriceListVisibility =>
            (CurrentUserRole == UserRole.PriceLists || CurrentUserRole == UserRole.Admin) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility NormalUserVisibility =>
            CurrentUserRole == UserRole.NormalUser ? Visibility.Visible : Visibility.Collapsed;

        private UserRole GetCurrentUserRole()
        {
            _userService.LoadUsers();
            var user = _userService.GetOrCreateUser(Environment.UserName);

            // Optional: Admin-Hinweis bei unbestätigten Usern
            if (user.Role == UserRole.Admin)
            {
                var neueUser = _userService.GetUnconfirmedUsers();
                if (neueUser.Any())
                {
                    MessageBox.Show($"Neue Benutzer erkannt:\n{string.Join("\n", neueUser.Select(u => u.Username))}");
                }
            }

            return user.Role;
        }
        public string CurrentUserDisplayNameOrUsername
        {
            get
            {
                var user = _userService.GetOrCreateUser(Environment.UserName);
                return string.IsNullOrWhiteSpace(user.DisplayName)
                    ? user.Username
                    : user.DisplayName;
            }
        }
    }
    public class NavigationItem
    {
        public string Title { get; }
        public string Icon { get; } // Emoji als Icon (alternativ: FontAwesome/SVG möglich)
        public object View { get; }
        public UserRole? RequiredRole { get; }

        public NavigationItem(string title, string icon, object view, UserRole? requiredRole = null)
        {
            Title = title;
            Icon = icon;
            View = view;
            RequiredRole = requiredRole;
        }
    }

    #endregion
}
