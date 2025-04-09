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

        private readonly UserService _userService = new UserService();

        public string Begruessungstext => $"Willkommen {CurrentUserDisplayNameOrUsername} bei der ToolBox Pro 👋";

        [ObservableProperty]
        private object currentView;

        [ObservableProperty]
        private string footerText = "ToolBox Pro © 2025 – Version 1.0.0 by Andreas Neumann";

        //private AppUser _currentUser;
        //public AppUser CurrentUser
        //{
        //    get => _currentUser;
        //    set
        //    {
        //        _currentUser = value;
        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(CurrentUserDisplayNameOrUsername));
        //        OnPropertyChanged(nameof(AdminVisibility));
        //        OnPropertyChanged(nameof(PriceListVisibility));
        //        OnPropertyChanged(nameof(NormalUserVisibility));
        //    }
        //}

        //public string CurrentUserDisplayNameOrUsername =>
        //    string.IsNullOrWhiteSpace(CurrentUser?.DisplayName)
        //        ? CurrentUser?.Username
        //        : CurrentUser?.DisplayName;

        //public bool HasRole(UserRole role) => CurrentUser?.Role.Contains(role) == true;

        public bool IstStartAktiv => CurrentView == null;

        partial void OnCurrentViewChanged(object value)
        {
            OnPropertyChanged(nameof(IstStartAktiv));
        }

        public ObservableCollection<NavigationItem> NavigationItems { get; } = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("Projektfilter erstellen", "FilterVariant", new MerkmalsImportView()),
                new NavigationItem("KERN Angebote", "Folder", new OfferCalculation()),
                new NavigationItem("Seitenzahlen & Gewicht", "Scale", new PDFProcessingView()),
                new NavigationItem("Preisliste exportieren", "FileExport", new PreislsiteExportView(), UserRole.PriceLists),
                new NavigationItem("Ordner bereinigen", "DeleteSweep", new CleanupView(), UserRole.Admin),
                new NavigationItem("Language_XML", "Translate", new Views.LanguageXML(), UserRole.Admin),
                new NavigationItem("User Settings", "AccountMultiple", new UserManagementView(), UserRole.Admin),
                new NavigationItem("Blaue Bücher Liste", "FormTextbox", new MappingView())
            };

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


        private UserRole _currentUserRole;
        public UserRole CurrentUserRole
        {
            get => _currentUserRole;
            set
            {
                _currentUserRole = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AdminVisibility));
                OnPropertyChanged(nameof(PriceListVisibility));
                OnPropertyChanged(nameof(NormalUserVisibility));
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
    #region kaputt
    //    private readonly UserService _userService = new();

    //    [ObservableProperty]
    //    private object currentView;

    //    [ObservableProperty]
    //    private string footerText = "ToolBox Pro © 2025 – Version 1.0.0 by Andreas Neumann";

    //    [ObservableProperty]
    //    private bool isFlyoutExpanded;

    //    public bool IstStartAktiv => CurrentView == null;

    //    public ObservableCollection<NavigationItem> NavigationItems { get; } = new()
    //    {
    //        new NavigationItem("Projektfilter erstellen", "FilterVariant", new MerkmalsImportView()),
    //        new NavigationItem("KERN Angebote", "Folder", new OfferCalculation()),
    //        new NavigationItem("Seitenzahlen & Gewicht", "Scale", new PDFProcessingView()),
    //        new NavigationItem("Preisliste exportieren", "FileExport", new PreislsiteExportView(), UserRole.PriceLists, UserRole.Admin),
    //        new NavigationItem("Ordner bereinigen", "DeleteSweep", new CleanupView(), UserRole.Admin),
    //        new NavigationItem("Language_XML", "Translate", new Views.LanguageXML(), UserRole.Admin),
    //        new NavigationItem("User Settings", "AccountMultiple", new UserManagementView())
    //    };

    //    private NavigationItem _selectedNavigationItem;
    //    public NavigationItem SelectedNavigationItem
    //    {
    //        get => _selectedNavigationItem;
    //        set
    //        {
    //            if (SetProperty(ref _selectedNavigationItem, value))
    //            {
    //                SwitchView(value);
    //            }
    //        }
    //    }

    //    private AppUser _currentUser;
    //    public AppUser CurrentUser
    //    {
    //        get => _currentUser;
    //        set
    //        {
    //            SetProperty(ref _currentUser, value);
    //            OnPropertyChanged(nameof(CurrentUserDisplayNameOrUsername));
    //            OnPropertyChanged(nameof(AdminVisibility));
    //            OnPropertyChanged(nameof(PriceListVisibility));
    //            OnPropertyChanged(nameof(NormalUserVisibility));
    //        }
    //    }

    //    public string CurrentUserDisplayNameOrUsername =>
    //        string.IsNullOrWhiteSpace(CurrentUser?.DisplayName)
    //            ? CurrentUser?.Username
    //            : CurrentUser.DisplayName;

    //    public string Begruessungstext =>
    //        $"Willkommen {CurrentUserDisplayNameOrUsername} bei der ToolBox Pro 👋";

    //    public Visibility AdminVisibility => HasRole(UserRole.Admin) ? Visibility.Visible : Visibility.Collapsed;
    //    public Visibility PriceListVisibility => HasRole(UserRole.PriceLists) ? Visibility.Visible : Visibility.Collapsed;
    //    public Visibility NormalUserVisibility => HasRole(UserRole.NormalUser) ? Visibility.Visible : Visibility.Collapsed;

    //    public MainWindowViewModel()
    //    {
    //        Initialize();
    //    }

    //    private void Initialize()
    //    {
    //        _userService.LoadUsers();
    //        var user = _userService.GetOrCreateUser(Environment.UserName);
    //        CurrentUser = user;

    //        if (HasRole(UserRole.Admin))
    //        {
    //            var neueUser = _userService.GetUnconfirmedUsers();
    //            if (neueUser.Any())
    //            {
    //                MessageBox.Show($"Neue Benutzer erkannt:\n{string.Join("\n", neueUser.Select(u => u.Username))}");
    //            }
    //        }
    //    }

    //    private void SwitchView(NavigationItem item)
    //    {
    //        if (item == null) return;
    //        if (item.RequiredRoles == null || !item.RequiredRoles.Any() || item.RequiredRoles.Any(HasRole))
    //        {
    //            CurrentView = item.View;
    //        }
    //    }

    //    public bool HasRole(UserRole role) =>
    //        CurrentUser?.Roles?.Contains(role) == true;
    //}

    //public class NavigationItem
    //{
    //    public string Title { get; }
    //    public string Icon { get; }
    //    public object View { get; }
    //    public List<UserRole> RequiredRoles { get; }

    //    public NavigationItem(string title, string icon, object view, params UserRole[] requiredRoles)
    //    {
    //        Title = title;
    //        Icon = icon;
    //        View = view;
    //        RequiredRoles = requiredRoles?.ToList() ?? new();
    //    }
    //}
    #endregion
}
