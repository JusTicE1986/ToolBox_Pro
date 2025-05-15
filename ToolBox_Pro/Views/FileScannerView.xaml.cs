using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToolBox_Pro.Interfaces;
using ToolBox_Pro.Services;
using ToolBox_Pro.ViewModels;

namespace ToolBox_Pro.Views;
/// <summary>
/// Interaktionslogik für FileScannerView.xaml
/// </summary>
public partial class FileScannerView : UserControl
{
    public FileScannerView()
    {
        InitializeComponent();
        DataContext = new FileScannerViewModel(new FileScannerService());
    }
}
