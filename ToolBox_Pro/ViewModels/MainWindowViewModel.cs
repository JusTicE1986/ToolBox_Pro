using System;
using System.Collections.Generic;
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
        private bool _isLogoVisible = true;
        public bool IsLogoVisible
        {
            get => _isLogoVisible;
            set
            {
                _isLogoVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(_isAdmin));
                OnPropertyChanged(nameof(AdminVisibility));
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

        public Visibility AdminVisibility => CurrentUserRole == UserRole.Admin ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PriceListVisibility => CurrentUserRole == UserRole.PriceLists ? Visibility.Visible : Visibility.Collapsed;
        public Visibility NormalUserVisibility => CurrentUserRole == UserRole.NormalUser ? Visibility.Visible : Visibility.Collapsed;


        public ICommand ShowOfferCalculationCommand { get; }
        public ICommand ShowPDFProcessingCommand { get; }
        public ICommand ShowLanguageXMLCommand { get; }
        public ICommand ShowCleanupViewCommand { get; }
        public ICommand ShowWikiUploadCommand { get; }
        public ICommand ShowPreislisteExportCommand { get; }  

        public MainWindowViewModel()
        {
            IsAdmin = CheckIfUserIsAdmin();
            ShowOfferCalculationCommand = new RelayCommands(ShowOfferCalculation);
            ShowPDFProcessingCommand = new RelayCommands(ShowPDFProcessing);
            ShowLanguageXMLCommand = new RelayCommands(ShowLanguageXML);
            ShowCleanupViewCommand = new RelayCommands(ShowCleanupView);
            ShowWikiUploadCommand = new RelayCommands(ShowWikiUploadView);
            ShowPreislisteExportCommand = new RelayCommands(ShowPreislisteExport);
        }

        private bool CheckIfUserIsAdmin()
        {
            string currentUser = Environment.UserName;
            return currentUser.Equals("LNZNEUMA", StringComparison.OrdinalIgnoreCase) || currentUser.Equals("JusTicE1986", StringComparison.OrdinalIgnoreCase);
        }

        private readonly List<string> _priceListUser = new()
        {
            "LNZDRAWM",
            "LNZFISCC",
            "LNZNEUMA",
            "JusTicE86"
        };

        private UserRole GetCurrentUserRole()
        {
            string currentUser = Environment.UserName;

            if (currentUser.Equals("LNZNEUMA", StringComparison.OrdinalIgnoreCase) || currentUser.Equals("JusTicE"))
                return UserRole.Admin;
            if (_priceListUser.Contains(currentUser, StringComparison.OrdinalIgnoreCase))
                return UserRole.PriceLists;
            return UserRole.NormalUser;
        }

        private void ShowOfferCalculation()
        {
            IsLogoVisible = false;
            CurrentView = new OfferCalculation();
        }

        private void ShowPDFProcessing()
        {
            CurrentView = new PDFProcessingView();
        }

        private void ShowLanguageXML()
        {
            CurrentView = new Views.LanguageXML();
        }

        private void ShowCleanupView()
        {
            CurrentView = new CleanupView();
        }

        private void ShowWikiUploadView()
        {
            CurrentView = new WikiUploadView();
        }

        private void ShowPreislisteExport() 
        {
            CurrentView = new PreislsiteExportView()
            {
                DataContext = new PreislisteExportViewModel()
            };
        }

    }
}
