using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using ToolBox_Pro.Commands;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.ViewModels
{
    public class FileModuleViewModel : BaseViewModel
    {
        public ObservableCollection<FileModel> Files { get; set; } = new ObservableCollection<FileModel>();

        public ICommand LoadFilesCommand { get; }
        public ICommand OpenFileCommand { get; }

        public FileModuleViewModel()
        {
            LoadFilesCommand = new RelayCommands(LoadFiles);
            OpenFileCommand = new RelayCommand<FileModel>(OpenFile, CanOpenFile);
        }

        private void LoadFiles()
        {
            Files.Clear();
            var directory = @"C:\TEMP\Neuer Ordner";
            if (Directory.Exists(directory))
            {
                foreach (var file in Directory.GetFiles(directory))
                {
                    Files.Add(new FileModel { Name = Path.GetFileName(file), Path = file });
                }
            }
        }
        private void OpenFile(FileModel file)
        {
            if (file != null && File.Exists(file.Path))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = file.Path,
                    UseShellExecute = true
                });
            }
        }
        private bool CanOpenFile(FileModel file)
        {
            return file != null && File.Exists(file.Path);
        }


    }
}
