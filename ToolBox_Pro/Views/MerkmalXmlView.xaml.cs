using System.Windows.Controls;
using ToolBox_Pro.ViewModels;

namespace ToolBox_Pro.Views
{
    /// <summary>
    /// Interaktionslogik für MerkmalXmlView.xaml
    /// </summary>
    public partial class MerkmalXmlView : UserControl
    {
        public MerkmalXmlView()
        {
            InitializeComponent();
            DataContext = new MerkmalVergleichViewModel();
        }
    }
}
