using DocumentFormat.OpenXml.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToolBox_Pro.ViewModels
{
    class WikiUploadViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public WikiUploadViewModel()
        {
            StartJavaProcessCommand = new RelayCommand(() => StartJavaProcess());
        }

        public ICommand StartJavaProcessCommand { get; }

        private void StartJavaProcess()
        {
            string folderpath = "\\\\wnad.net\\local\\PGK\\DATA\\Engineering\\FB_tech_Doku\\Betriebsanleitungen\\Schema ST4\\Wiki Upload automization\\Script und jar\\wiki-content-upload-automatization.jar";
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-ExecutionPolicy Bypass -NoExit -File {folderpath}",
                    //RedirectStandardOutput = false,
                    //RedirectStandardError = false,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();
                    //string output = process.StandardOutput.ReadToEnd();
                    //string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fehler beim Staten des Prozesses {ex.Message}");
            }
        }
    }

}
