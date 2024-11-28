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

    public ICommand ShowOfferCalculationCommand { get; }
        public ICommand ShowPDFProcessingCommand { get; }

        public MainWindowViewModel()
        {
            ShowOfferCalculationCommand = new RelayCommands(ShowOfferCalculation);
            ShowPDFProcessingCommand = new RelayCommands(ShowPDFProcessing);

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
    }
}
