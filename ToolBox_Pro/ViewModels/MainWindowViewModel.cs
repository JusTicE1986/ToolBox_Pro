using System;
using System.Windows;
using System.Windows.Input;
using ToolBox_Pro.Commands;
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
        public Visibility AdminVisibility => IsAdmin ? Visibility.Visible : Visibility.Collapsed;

        public ICommand ShowOfferCalculationCommand { get; }
        public ICommand ShowPDFProcessingCommand { get; }
        public ICommand ShowLanguageXMLCommand { get; }
        public ICommand ShowCleanupViewCommand { get; }
        public ICommand ShowWikiUploadCommand { get; }

        public MainWindowViewModel()
        {
            IsAdmin = CheckIfUserIsAdmin();
            ShowOfferCalculationCommand = new RelayCommands(ShowOfferCalculation);
            ShowPDFProcessingCommand = new RelayCommands(ShowPDFProcessing);
            ShowLanguageXMLCommand = new RelayCommands(ShowLanguageXML);
            ShowCleanupViewCommand = new RelayCommands(ShowCleanupView);
            ShowWikiUploadCommand = new RelayCommands(ShowWikiUploadView);
        }

        private bool CheckIfUserIsAdmin()
        {
            string currentUser = Environment.UserName;
            return currentUser.Equals("LNZNEUMA", StringComparison.OrdinalIgnoreCase) || currentUser.Equals("JusTicE1986", StringComparison.OrdinalIgnoreCase);
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
            CurrentView = new LanguageXML();
        }

        private void ShowCleanupView()
        {
            CurrentView = new CleanupView();
        }

        private void ShowWikiUploadView()
        {
            CurrentView = new WikiUploadView();
        }
    }
}
