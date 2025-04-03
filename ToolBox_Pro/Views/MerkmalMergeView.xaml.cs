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
using ToolBox_Pro.ViewModels;

namespace ToolBox_Pro.Views
{
    /// <summary>
    /// Interaktionslogik für MerkmalMergeView.xaml
    /// </summary>
    public partial class MerkmalMergeView : UserControl
    {
        public MerkmalMergeView()
        {
            InitializeComponent();
            DataContext = new MerkmalMergeViewModel();
        }
    }
}
