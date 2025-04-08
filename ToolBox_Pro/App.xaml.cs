using DocumentFormat.OpenXml.Spreadsheet;
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
using ToolBox_Pro.ViewModels;
using ToolBox_Pro.Views;

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
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var splash = new Views.SplashScreen();
            splash.Show();
            Task.Run(() =>
            {
                try
                {
                    
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Bin jetzt fertig!");
                        var mainViewModel = new MainWindowViewModel();
                        var mainWindow = new MainWindow(mainViewModel);
                        mainWindow.Show();
                        splash.Close();
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Fehler beim Start:\n" + ex.Message);
                        splash.Close();
                        Application.Current.Shutdown(); // optional
                    });
                }

            });
        }
    }
}

