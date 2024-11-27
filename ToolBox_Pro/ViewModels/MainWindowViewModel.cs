using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ToolBox_Pro.Commands;
using ToolBox_Pro.ViewModels;
using ToolBox_Pro.Views;

namespace ToolBox_Pro.ViewModels
{
    public class MainWindowViewModel : ObservableObject
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

        public MainWindowViewModel()
        {
            ShowOfferCalculationCommand = new RelayCommands(ShowOfferCalculation);

        }

        private void ShowOfferCalculation()
        {
            IsLogoVisible = false;
            CurrentView = new OfferCalculation();
        }
    }
}
