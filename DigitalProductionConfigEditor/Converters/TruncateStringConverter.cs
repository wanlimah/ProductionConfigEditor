using System;
using System.Globalization;
using System.Windows.Data;

namespace DigitalProductionConfigEditor.Converters
{
    public class TruncateStringConverter : IValueConverter
    {
        public int MaxLength { get; set; } = 30;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                if (text.Length <= MaxLength)
                {
                    return text;
                }
                return text.Substring(0, MaxLength) + "...";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}




