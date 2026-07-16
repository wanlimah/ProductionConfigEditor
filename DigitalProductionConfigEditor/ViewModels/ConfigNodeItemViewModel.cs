using System.ComponentModel;
using System.Windows.Media;
using System.Xml;

namespace DigitalProductionConfigEditor.ViewModels
{
    /// <summary>
    /// View model for a configuration node item with checkbox selection support
    /// </summary>
    public class ConfigNodeItemViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        
        public XmlNode ConfigNode { get; set; }
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        
        public string NodeName => ConfigNode?.Name ?? "Unknown";
        
        public int ItemCount => ConfigNode?.ChildNodes.Count ?? 0;

        /// <summary>
        /// The test category this element supports: ALL, RF1, RF2, NFR, TDG, or empty string.
        /// Reads from the element's own supportedTests attribute first; if absent, inherits
        /// from the parent element (e.g. DeveloperValidationConfig children inherit TDG).
        /// </summary>
        public string SupportedTests
        {
            get
            {
                var own = ConfigNode?.Attributes?["supportedTests"]?.Value ?? "";
                if (!string.IsNullOrEmpty(own)) return own;
                return ConfigNode?.ParentNode?.Attributes?["supportedTests"]?.Value ?? "";
            }
        }

        public bool HasSupportedTests => !string.IsNullOrEmpty(SupportedTests);

        /// <summary>
        /// Full documentation URL.  The base URL comes from the docUrl attribute on the root
        /// &lt;ProductionUserConfig&gt; element (one place for the whole team). The optional docRange
        /// attribute on this element (e.g. "C7") appends a Google Sheets range anchor so the link
        /// jumps straight to the relevant row.
        /// </summary>
        /// <summary>
        /// Raw docRange value from XML.  "NO_DATA" is a sentinel meaning the element
        /// exists in the master but has no Google Sheet entry yet.
        /// </summary>
        private string DocRange => ConfigNode?.Attributes?["docRange"]?.Value ?? "";

        public string DocUrl
        {
            get
            {
                var baseUrl = ConfigNode?.OwnerDocument?.DocumentElement?.Attributes?["docUrl"]?.Value ?? "";
                if (string.IsNullOrWhiteSpace(baseUrl)) return "";

                var range = DocRange;
                // NO_DATA sentinel → no working link yet
                if (string.IsNullOrWhiteSpace(range) || range == "NO_DATA") return "";

                var separator = baseUrl.Contains('#') ? "&" : "#";
                return $"{baseUrl}{separator}range={range}";
            }
        }

        public bool HasDocUrl => !string.IsNullOrEmpty(DocUrl);

        /// <summary>
        /// True when docUrl is set on the root but this element has no docRange at all —
        /// the developer must add it to the sheet and set docRange.
        /// </summary>
        public bool IsDocRangeMissing
        {
            get
            {
                var baseUrl = ConfigNode?.OwnerDocument?.DocumentElement?.Attributes?["docUrl"]?.Value ?? "";
                if (string.IsNullOrWhiteSpace(baseUrl)) return false;
                return string.IsNullOrWhiteSpace(DocRange); // "NO_DATA" is non-empty → not missing
            }
        }

        /// <summary>
        /// True when docRange is explicitly set to "NO_DATA" — the element is known but
        /// its Google Sheet entry has not been created yet.  Shows a gray "No Data" badge.
        /// </summary>
        public bool IsDocRangeNoData => DocRange == "NO_DATA";

        /// <summary>
        /// Badge background color keyed to the supported test category.
        /// </summary>
        public Brush SupportedTestsBadgeColor => SupportedTests switch
        {
            "ALL"  => new SolidColorBrush(Color.FromRgb(0, 188, 212)),   // cyan
            "RF1"  => new SolidColorBrush(Color.FromRgb(255, 235, 59)),  // yellow
            "RF2"  => new SolidColorBrush(Color.FromRgb(255, 152, 0)),   // orange
            "NFR"  => new SolidColorBrush(Color.FromRgb(240, 98, 146)),  // pink
            "TDG"  => new SolidColorBrush(Color.FromRgb(100, 181, 246)), // light blue
            _      => new SolidColorBrush(Color.FromRgb(200, 200, 200))  // gray fallback
        };

        /// <summary>
        /// Badge text color (dark for light badges, white for dark badges).
        /// </summary>
        public Brush SupportedTestsTextColor => SupportedTests switch
        {
            "NFR"  => Brushes.White,
            _      => Brushes.Black
        };
        
        public ConfigNodeItemViewModel(XmlNode configNode)
        {
            ConfigNode = configNode;
            IsSelected = false;
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
