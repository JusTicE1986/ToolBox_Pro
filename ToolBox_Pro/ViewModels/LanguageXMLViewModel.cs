using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ToolBox_Pro.Commands;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public partial class LanguageXMLViewModel : ObservableObject
    {
        #region archiv
        //private string _statusMessage;

        //public LanguageXML LanguageData { get; set; }

        //public ICommand ProcessDataCommand { get; }

        //public string StatusMessage
        //{
        //    get => _statusMessage;
        //    set
        //    {
        //        if (_statusMessage != value)
        //        {
        //            _statusMessage = value;
        //            OnPropertyChanged(nameof(StatusMessage));
        //        }
        //    }
        //}

        //public LanguageXMLViewModel()
        //{
        //    LanguageData = new LanguageXML();
        //    ProcessDataCommand = new RelayCommands(ProcessLanguageData);
        //}

        //private void ProcessLanguageData()
        //{
        //    try
        //    {
        //        var _LanguageService = new LanguageXMLService();
        //        LanguageData.Path = @"S:\00_Admin\Language_xml\Source xlsm\Language XML Data 2.1.xlsm";
        //        LanguageData.TargetFile = @$"S:\00_Admin\Language_xml\{DateTime.Now:yyyy-MM-dd}_Language_import.sti";
        //        _LanguageService.LoadDataFromExcel(LanguageData);
        //        MessageBox.Show("Aufgabe ausgeführt.");
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = StatusMessage = $"Fehler beim Verarbeiten: {ex.Message}"; 
        //        throw;
        //    }
        //}
        #endregion

        private readonly LanguageXMLService _languageService = new();

        [ObservableProperty]
        private LanguageXML languageData = new();

        [ObservableProperty]
        private ObservableCollection<string> verarbeiteteDateien = new();

        [ObservableProperty]
        private string statusMessage;

        [ObservableProperty]
        private bool isLoading;

        [RelayCommand]
        private void Dateiauswahl()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Excel-Dateien (*.xlsm)|*.xlsm",
                Title = "Language Excel-Datei auswählen"
            };

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LanguageData.Path = openFileDialog.FileName;
                StatusMessage = $"Datei ausgewählt: {languageData.Path}";
            }
        }

        [RelayCommand]
        private async Task ProcessDataAsync()
        {
            if (string.IsNullOrWhiteSpace(LanguageData.Path))
            {
                StatusMessage = "Bitte zuerst eine Datei auswählen.";
                return;
            }
            try
            {
                IsLoading = true;
                StatusMessage = "Verarbeitung gestartet...";
                VerarbeiteteDateien.Clear();

                languageData.TargetFile = $@"S:\\00_Admin\\Language_xml\\{DateTime.Now:yyyy-MM-dd}_Language_import.sti";

                await Task.Run(() => _languageService.LoadDataFromExcel(languageData, log =>
                {
                    App.Current.Dispatcher.Invoke(() => VerarbeiteteDateien.Add(log));
                }));

                StatusMessage = $"Verarbeitung abgeschlossen";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
