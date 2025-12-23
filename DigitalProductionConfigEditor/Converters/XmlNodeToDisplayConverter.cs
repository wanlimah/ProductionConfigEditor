using System;
using System.Globalization;
using System.Windows.Data;
using System.Xml;

namespace DigitalProductionConfigEditor.Converters
{
    public class XmlNodeToDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return "Not Added";
                }
                
                if (value is XmlNode node)
                {
                    // For nodes with attributes (like Package nodes)
                    if (node.Attributes != null && node.Attributes.Count > 0)
                    {
                        string packageName = node.Attributes["name"]?.Value ?? "Unknown";
                        return $"📦 Package: {packageName}";
                    }
                    
                    // For other nodes (like PcbFormatConfig)
                    var childCount = node.ChildNodes?.Count ?? 0;
                    return $"Added ({childCount} items)";
                }
            }
            catch (Exception)
            {
                // Return fallback if any error occurs
                return "Error";
            }
            return "No Selection";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
