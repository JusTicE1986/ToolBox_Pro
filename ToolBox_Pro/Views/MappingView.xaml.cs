using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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
using ToolBox_Pro.Converters;
using ToolBox_Pro.Models;
using ToolBox_Pro.Services;
using ToolBox_Pro.ViewModels;

namespace ToolBox_Pro.Views
{
    /// <summary>
    /// Interaktionslogik für MappingView.xaml
    /// </summary>
    public partial class MappingView : UserControl
    {
        public MappingView()
        {
            InitializeComponent();
            //this.Loaded += MappingViewModel_Loaded;
            //DataContext = new MappingViewModel();
        }

        

        //private void MappingViewModel_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if(DataContext is MappingViewModel vm)
        //    {
        //        GenerateLanguageColumns(vm.ConfigService.Config.Languages);
        //    }
        //}
        //private void GenerateLanguageColumns(List<LanguageMapping> languages)
        //{
        //    LanguageGrid.Columns.Clear();

        //    LanguageGrid.Columns.Add(new DataGridTextColumn{
        //        Header = "Type",
        //            Binding = new Binding ("Type")
        //    });
        //    LanguageGrid.Columns.Add(new DataGridTextColumn{
        //        Header = "Designation",
        //            Binding = new Binding("Designation")
        //    });

        //    // Dynamisch: eine Spalte pro Sprache
        //    foreach (var lang in languages)
        //    {
        //        var binding = new MultiBinding
        //        {
        //            Converter = new LanguageDictionaryConverter(),
        //            ConverterParameter = lang.Code
        //        };
        //        binding.Bindings.Add(new Binding("LanguageMapping"));

        //        LanguageGrid.Columns.Add(new DataGridTextColumn
        //        {
        //            Header = lang.Code,
        //            Binding = binding
        //        });
        //    }
        //}
    }
}
