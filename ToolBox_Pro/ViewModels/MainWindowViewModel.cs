using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using ToolBox_Pro.Commands;
using ToolBox_Pro.Models;
using ToolBox_Pro.Views;

namespace ToolBox_Pro.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        //    #region Backup ToolBox_Pro
        //    private object _currentView;
        //    public object CurrentView
        //    {
        //        get => _currentView;
        //        set
        //        {
        //            _currentView = value;
        //            OnPropertyChanged(nameof(CurrentView));
        //        }
        //    }

        //    private bool _isLogoVisible = true;
        //    public bool IsLogoVisible
        //    {
        //        get => _isLogoVisible;
        //        set
        //        {
        //            _isLogoVisible = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    private bool _isAdmin;
        //    public bool IsAdmin
        //    {
        //        get => _isAdmin;
        //        set
        //        {
        //            _isAdmin = value;
        //            OnPropertyChanged(nameof(_isAdmin));
        //            OnPropertyChanged(nameof(AdminVisibility));
        //        }
        //    }

        //    private UserRole _currentUserRole;
        //    public UserRole CurrentUserRole
        //    {
        //        get => _currentUserRole;
        //        set
        //        {
        //            _currentUserRole = value;
        //            OnPropertyChanged();
        //            OnPropertyChanged(nameof(AdminVisibility));
        //            OnPropertyChanged(nameof(PriceListVisibility));
        //            OnPropertyChanged(nameof(NormalUserVisibility));
        //        }
        //    }

        //    public Visibility AdminVisibility => 
        //        CurrentUserRole == UserRole.Admin ? Visibility.Visible : Visibility.Collapsed;
        //    public Visibility PriceListVisibility => 
        //        (CurrentUserRole == UserRole.PriceLists || CurrentUserRole == UserRole.Admin) ? Visibility.Visible : Visibility.Collapsed;
        //    public Visibility NormalUserVisibility => 
        //        CurrentUserRole == UserRole.NormalUser ? Visibility.Visible : Visibility.Collapsed;


        //public ICommand ShowOfferCalculationCommand { get; }
        //public ICommand ShowPDFProcessingCommand { get; }
        //public ICommand ShowLanguageXMLCommand { get; }
        //public ICommand ShowCleanupViewCommand { get; }
        //public ICommand ShowWikiUploadCommand { get; }
        //public ICommand ShowPreislisteExportCommand { get; }

        //public MainWindowViewModel()
        //{
        //    CurrentUserRole = GetCurrentUserRole();
        //    ShowOfferCalculationCommand = new RelayCommands(ShowOfferCalculation);
        //    ShowPDFProcessingCommand = new RelayCommands(ShowPDFProcessing);
        //    ShowLanguageXMLCommand = new RelayCommands(ShowLanguageXML);
        //    ShowCleanupViewCommand = new RelayCommands(ShowCleanupView);
        //    ShowWikiUploadCommand = new RelayCommands(ShowWikiUploadView);
        //    ShowPreislisteExportCommand = new RelayCommands(ShowPreislisteExport);
        //}

        //    private bool CheckIfUserIsAdmin()
        //    {
        //        string currentUser = Environment.UserName;
        //        return currentUser.Equals("LNZNEUMA", StringComparison.OrdinalIgnoreCase) || currentUser.Equals("JusTicE1986", StringComparison.OrdinalIgnoreCase);
        //    }

        //    private readonly List<string> _priceListUser = new()
        //    {
        //        "LNZDRAWM",
        //        "LNZFISCC",
        //        "LNZNEUMA",
        //        "LNZLAABJ",
        //        "JusTicE86"
        //    };

        //    private UserRole GetCurrentUserRole()
        //    {
        //        string currentUser = Environment.UserName.ToLower();

        //        if (currentUser.Equals("LNZNEUMA", StringComparison.OrdinalIgnoreCase) || currentUser.Equals("JusTicE86"))
        //            return UserRole.Admin;
        //        if (_priceListUser.Any(u => u.Equals(currentUser, StringComparison.OrdinalIgnoreCase)))
        //            return UserRole.PriceLists;
        //        return UserRole.NormalUser;
        //    }

        //private void ShowOfferCalculation()
        //{
        //    IsLogoVisible = false;
        //    CurrentView = new OfferCalculation();
        //}

        //private void ShowPDFProcessing()
        //{
        //    CurrentView = new PDFProcessingView();
        //}

        //private void ShowLanguageXML()
        //{
        //    CurrentView = new Views.LanguageXML();
        //}

        //private void ShowCleanupView()
        //{
        //    CurrentView = new CleanupView();
        //}

        //private void ShowWikiUploadView()
        //{
        //    CurrentView = new WikiUploadView();
        //}

        //private void ShowPreislisteExport()
        //{
        //    CurrentView = new PreislsiteExportView()
        //    {
        //        DataContext = new PreislisteExportViewModel()
        //    };
        //}

        //}
        //#endregion

        #region Neues ViewModel mit NavigationItems
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

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

        public ObservableCollection<NavigationItem> NavigationItems { get; } = new ObservableCollection<NavigationItem>
{
    //new NavigationItem("Startseite", "🏠", new HomeView()),
    new NavigationItem("KERN Angebote", "📂", new OfferCalculation()),
    new NavigationItem("Seitenzahlen & Gewicht", "📏", new PDFProcessingView()),
    new NavigationItem("Ordner bereinigen", "🧹", new CleanupView()),
    new NavigationItem("Sprachdatei XML", "🗣️", new Views.LanguageXML()),
    new NavigationItem("Wiki Upload", "🌐", new WikiUploadView(), UserRole.Admin),
    new NavigationItem("Preisliste Export", "💾", new PreislsiteExportView(), UserRole.PriceLists),
    new NavigationItem("Projektfilter erstellen", "🧩", new MerkmalsImportView(), UserRole.Admin)
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
            string currentUser = Environment.UserName.ToLower();

            if (currentUser.Equals("lnzneuma", StringComparison.OrdinalIgnoreCase) || currentUser.Equals("justice86"))
                return UserRole.Admin;

            if (new[] { "lnzdrawm", "lnzfiscc", "lnzneuma", "lnzlaabj", "justice86" }
                .Any(u => u.Equals(currentUser, StringComparison.OrdinalIgnoreCase)))
                return UserRole.PriceLists;

            return UserRole.NormalUser;
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
