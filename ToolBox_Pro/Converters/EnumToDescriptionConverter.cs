using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ToolBox_Pro.Converters
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var field = enumValue.GetType().GetField(enumValue.ToString());
                var description = field?.GetCustomAttribute<DescriptionAttribute>()?.Description;
                return description ?? enumValue.ToString();
            }
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string strValue && targetType.IsEnum)
            {
                foreach (var field in targetType.GetFields())
                {
                    var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description;
                    if (description == strValue) return Enum.Parse(targetType, field.Name);
                }

                return Enum.Parse(targetType, strValue);
            }

            return Binding.DoNothing;

        }
    }
}
