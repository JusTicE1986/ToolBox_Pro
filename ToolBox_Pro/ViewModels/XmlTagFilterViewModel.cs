using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;
using System.Windows.Forms;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;

namespace ToolBox_Pro.ViewModels;
public partial class XmlTagFilterViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<XmlTagMatch> matches = new();
    [ObservableProperty]
    private string selectedFilePath = string.Empty;

    [RelayCommand]
    private void BrowseFile()
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "CSV-Dateien|*.csv"
        };
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            SelectedFilePath = dialog.FileName;
            Debug.WriteLine($"Datei gewählt: {SelectedFilePath}");
        }
    }

    [RelayCommand]
    private void LoadMatches()
    {
        Console.WriteLine("LoadMatches wurde aufgerufen!");
        if (File.Exists(SelectedFilePath))
        {
            var result = XmlTagFilterService.FilteringMatchingLines(SelectedFilePath);
            Matches = new ObservableCollection<XmlTagMatch>(result);
        }
    }

    [RelayCommand]
    private void ExportMatches()
    {
        var dialog = new SaveFileDialog
        {
            Filter = "CSV-Dateien|*.csv"
        };
        if(dialog.ShowDialog() == DialogResult.OK)
        {
            XmlTagFilterService.SaveMatchesToFile(Matches, dialog.FileName);
        }
    }
}
