using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Converters;
public class RankToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AppUser user)
        {
            if (user.FeatureCounter >= 150)
                return Brushes.MediumPurple; // Diamant (edles Lila)
            if (user.FeatureCounter >= 100)
                return Brushes.DeepSkyBlue; // Platin (blau glänzend)
            if (user.FeatureCounter >= 75)
                return Brushes.Gold; // Gold
            if (user.FeatureCounter >= 50)
                return Brushes.Silver; // Silber
            if (user.FeatureCounter >= 25)
                return Brushes.Peru; // Bronze (erdig)
            return new SolidColorBrush(Color.FromRgb(139, 69, 19)); // Rostig Braun für Eisen
        }
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
