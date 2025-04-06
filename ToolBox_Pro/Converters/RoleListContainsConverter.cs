using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Converters
{
    public class RoleListContainsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<UserRole> roles && parameter is UserRole role)
            {
                return roles.Contains(role);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;
            var role = (UserRole)parameter;
            return (isChecked, role); // Muss über CheckBox-Event behandelt werden
        }
    }

}
