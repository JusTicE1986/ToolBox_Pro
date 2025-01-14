using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolBox_Pro.Commands;

namespace ToolBox_Pro.ViewModels
{
    public class CleanupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CleanupViewModel() 
        {
            CleanupST4PdfCommand = new RelayCommand(() => ExecuteBatchCommand(@"\\lnzdfs1.wnad.net\SAP-Shares\KMR\ST4\ST4_pdf_Automated_Production"));
            CleanupDIRSuccessCommand = new RelayCommand(() => ExecuteBatchCommand(@"\\lnzdfs1.wnad.net\SAP-Shares\KMR\ST4\DIR_success"));
            CleanupDIRFailedCommand = new RelayCommand(() => ExecuteBatchCommand(@"\\lnzdfs1.wnad.net\SAP-Shares\KMR\ST4\DIR_failed"));
        }

        public ICommand CleanupST4PdfCommand { get; }
        public ICommand CleanupDIRSuccessCommand { get; }
        public ICommand CleanupDIRFailedCommand { get; }

        private void ExecuteBatchCommand(string folderPath)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c del /q \"{folderPath}\\*.*\" & for /D %a in (\"{folderPath}\\*.*\") do rd /q /s \"%a\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    Debug.WriteLine($"Fehler: {error}");
                }
                else
                {
                    Debug.WriteLine($"Erfolg: {output}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exeption: {ex.Message}");
            }
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
