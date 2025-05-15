using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolBox_Pro.Interfaces;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.ViewModels;
public partial class FileScannerViewModel : ObservableObject
{
    private readonly IFileScannerService _fileScannerService;

    public FileScannerViewModel(IFileScannerService fileScannerService)

    {
        _fileScannerService = fileScannerService;
    }

    [ObservableProperty]
    private ObservableCollection<FileDateiModel> largeFiles = new();

    [ObservableProperty]
    private bool scanRecursively = false;
    [ObservableProperty]
    private bool excludePdfFiles = false;

    [RelayCommand]
    private void PickFolderAndScan()
    {
        using (var dialog = new FolderBrowserDialog())
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var files = _fileScannerService.GetLargeFiles(dialog.SelectedPath, 5 * 1024 * 1024, ScanRecursively, ExcludePdfFiles);
                LargeFiles = new ObservableCollection<FileDateiModel>(files);
            }
        }

    }
}
