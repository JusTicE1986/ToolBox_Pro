using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToolBox_Pro.Commands;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels
{
    public class LanguageXMLViewModel : BaseViewModel
    {
        private string _statusMessage;

        public LanguageXML LanguageData { get; set; }

        public ICommand ProcessDataCommand { get; }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged(nameof(StatusMessage));
                }
            }
        }

        public LanguageXMLViewModel()
        {
            LanguageData = new LanguageXML();
            ProcessDataCommand = new RelayCommands(ProcessLanguageData);
        }

        private void ProcessLanguageData()
        {
            try
            {
                var _LanguageService = new LanguageXMLService();
                LanguageData.Path = @"S:\00_Admin\Language_xml\Source xlsm\Language XML Data 2.1.xlsm";
                LanguageData.TargetFile = @$"S:\00_Admin\Language_xml\{DateTime.Now:yyyy-MM-dd}_Language_import.sti";
                _LanguageService.LoadDataFromExcel(LanguageData);
                MessageBox.Show("Aufgabe ausgeführt.");
            }
            catch (Exception ex)
            {
                StatusMessage = StatusMessage = $"Fehler beim Verarbeiten: {ex.Message}"; 
                throw;
            }
        }
    }
}
