using System.Windows;
using System.Windows.Controls;
using ToolBox_Pro.ViewModels; // Den Namespace anpassen, falls erforderlich

namespace ToolBox_Pro.Views
{
    public partial class OfferCalculation : UserControl
    {
        public OfferCalculation()
        {
            InitializeComponent();
            // Falls du das DataContext hier manuell setzen möchtest:
            //DataContext = new OfferCalculationViewModel();
        }

        
    }
}
