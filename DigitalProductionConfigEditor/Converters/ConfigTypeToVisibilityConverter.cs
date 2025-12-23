using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DigitalProductionConfigEditor.Converters
{
    public class ConfigTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Collapsed;

            string parentName = value.ToString() ?? "";
            string targetType_str = parameter.ToString() ?? "";

            // Show badge if parent name matches the parameter
            return parentName.Equals(targetType_str, StringComparison.OrdinalIgnoreCase) 
                ? Visibility.Visible 
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

















































