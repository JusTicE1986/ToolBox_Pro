using System;
using System.Windows;
using System.Windows.Media.Animation;
using ToolBox_Pro.ViewModels;

namespace ToolBox_Pro.Views
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            //MessageBox.Show($"Username ist: {Environment.UserName}");
        }
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FlyoutOpenAnimation");
            storyboard.Begin();
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("FlyoutCloseAnimation");
            storyboard.Begin();
        }
        private void FlyoutBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.IsFlyoutExpanded = true;
            AnimateFlyoutBorder(320); // Breite bei MouseOver
        }

        private void FlyoutBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ViewModel.IsFlyoutExpanded = false;
            AnimateFlyoutBorder(60); // Collapsed-Zustand
        }

        private void AnimateFlyoutBorder(double targetWidth)
        {
            var animation = new DoubleAnimation
            {
                To = targetWidth,
                Duration = TimeSpan.FromMilliseconds(300),
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.7
            };

            FlyoutBorder.BeginAnimation(WidthProperty, animation);
        }
    }
}
