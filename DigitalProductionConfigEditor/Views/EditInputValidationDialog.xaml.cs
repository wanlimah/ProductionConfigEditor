using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Xml;

namespace DigitalProductionConfigEditor.Views
{
    // ── Value converter: bool → string label ──────────────────────────────────
    public class BoolToStringConverter : IValueConverter
    {
        public string TrueValue { get; set; } = "True";
        public string FalseValue { get; set; } = "False";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && b ? TrueValue : FalseValue;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    // ── ViewModel for one <TestConfig> entry ──────────────────────────────────
    public class TestConfigViewModel : INotifyPropertyChanged
    {
        private string _name = "";
        private string _tag = "";
        private string _value = "";
        private string _expiry = "";
        private bool _isNew;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(ValueHint));
                OnPropertyChanged(nameof(ValuePlaceholder));
            }
        }

        public string Tag
        {
            get => _tag;
            set { _tag = value; OnPropertyChanged(nameof(Tag)); OnPropertyChanged(nameof(IsGlobal)); OnPropertyChanged(nameof(IsPerPackage)); }
        }

        public string Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }

        public string Expiry
        {
            get => _expiry;
            set { _expiry = value; OnPropertyChanged(nameof(Expiry)); }
        }

        public bool IsNew
        {
            get => _isNew;
            set { _isNew = value; OnPropertyChanged(nameof(IsNew)); }
        }

        public bool IsGlobal => string.IsNullOrWhiteSpace(_tag);
        public bool IsPerPackage => !IsGlobal;

        // Format hint shown below the Value textbox
        public string ValueHint => _name switch
        {
            "ADD_DEVICEID"            => "Format: AFEM-####-AL/ML/AC or ENGR-####-AL/ML/AC  (e.g. AFEM-1234-AL;ENGR-5678-ML)",
            "ADD_LOADBOARDID"         => "Format: LB-RF1-###-### or JP#### or LBSP  (e.g. LB-RF1-001-001;JP1234)",
            "ADD_ADDITIONAL_SUBLOTID" => "Format: [1-3][A-E]  (e.g. 1A;2B;3C)",
            _                         => "Separate multiple values with a semicolon ( ; )"
        };

        // Placeholder text shown inside the empty TextBox
        public string ValuePlaceholder => _name switch
        {
            "ADD_DEVICEID"            => "e.g. AFEM-1234-AL;ENGR-5678-ML;AFEM-9012-AC",
            "ADD_LOADBOARDID"         => "e.g. LB-RF1-001-001;JP1234;LBSP",
            "ADD_ADDITIONAL_SUBLOTID" => "e.g. 1A;2B;3C",
            _                         => "Enter values separated by  ;"
        };

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // ── Dialog code-behind ────────────────────────────────────────────────────
    public partial class EditInputValidationDialog : Window
    {
        private readonly ObservableCollection<TestConfigViewModel> _entries = new();
        private XmlNode? _inputValidationNode;

        // Hardcoded validation regex per config name.
        // Each token in the semicolon-separated Value must match at least one of these patterns.
        private static readonly Dictionary<string, Regex[]> _tokenPatterns = new()
        {
            ["ADD_DEVICEID"] = new[]
            {
                new Regex(@"^(AFEM|ENGR)-\d{4}-(AL|ML|AC)$")
            },
            ["ADD_LOADBOARDID"] = new[]
            {
                new Regex(@"^LB-?(RF1|RF2|NF|NS)?-\d{3,4}-\d{3}$", RegexOptions.IgnoreCase),
                new Regex(@"^JP\d{4}$",                              RegexOptions.IgnoreCase),
                new Regex(@"^LBSP",                                   RegexOptions.IgnoreCase),
            },
            ["ADD_ADDITIONAL_SUBLOTID"] = new[]
            {
                new Regex(@"^[1-3][A-E]$")
            },
        };

        private static readonly Dictionary<string, string> _formatDescriptions = new()
        {
            ["ADD_DEVICEID"]            = "AFEM-####-AL  or  ENGR-####-AL\nAFEM-####-ML  or  ENGR-####-ML\nAFEM-####-AC  or  ENGR-####-AC\n  (4 digits, suffix must be AL / ML / AC)\n  e.g. AFEM-1234-AL",
            ["ADD_LOADBOARDID"]         = "LB-RF1-###-###  or  JP####  or  LBSP...\n  e.g. LB-RF1-001-001",
            ["ADD_ADDITIONAL_SUBLOTID"] = "[1-3][A-E]  (digit 1–3 + uppercase letter A–E)\n  e.g. 1A",
        };

        public EditInputValidationDialog(XmlNode? inputValidationNode)
        {
            InitializeComponent();
            _inputValidationNode = inputValidationNode;
            LoadEntries();
            EntriesItemsControl.ItemsSource = _entries;
        }

        private void LoadEntries()
        {
            _entries.Clear();
            if (_inputValidationNode == null) return;

            var nodes = _inputValidationNode.SelectNodes("TestConfig");
            if (nodes == null) return;

            foreach (XmlNode node in nodes)
            {
                _entries.Add(new TestConfigViewModel
                {
                    Name   = node.Attributes?["name"]?.Value   ?? "",
                    Tag    = node.Attributes?["tag"]?.Value    ?? "",
                    Value  = node.Attributes?["value"]?.Value  ?? "",
                    Expiry = node.Attributes?["expiry"]?.Value ?? "",
                    IsNew  = false
                });
            }
        }

        private void OnAddDeviceIdClick(object sender, RoutedEventArgs e)
        {
            _entries.Add(new TestConfigViewModel
            {
                Name  = "ADD_DEVICEID",
                Tag   = "",
                Value = "",
                IsNew = true
            });
        }

        private void OnAddLoadboardClick(object sender, RoutedEventArgs e)
        {
            _entries.Add(new TestConfigViewModel
            {
                Name  = "ADD_LOADBOARDID",
                Tag   = "",
                Value = "",
                IsNew = true
            });
        }

        private void OnAddPerPackageClick(object sender, RoutedEventArgs e)
        {
            _entries.Add(new TestConfigViewModel
            {
                Name   = "ADD_ADDITIONAL_SUBLOTID",
                Tag    = "AFEM-XXXX-AP1-RF2",   // placeholder — user must replace with real package tag
                Value  = "",
                Expiry = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"),
                IsNew  = true
            });
        }

        private void OnDeleteEntryClick(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is TestConfigViewModel entry)
            {
                var result = MessageBox.Show(
                    $"Delete the entry '{entry.Name}'" +
                    (entry.IsGlobal ? " (global)?" : $" for tag '{entry.Tag}'?"),
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                    _entries.Remove(entry);
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            foreach (var entry in _entries)
            {
                if (string.IsNullOrWhiteSpace(entry.Name))
                {
                    MessageBox.Show("Each entry must have a Config Name.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (entry.IsPerPackage && entry.Tag.Contains("XXXX"))
                {
                    MessageBox.Show(
                        $"Entry '{entry.Name}' has a placeholder Package Tag \"{entry.Tag}\".\n\nPlease replace it with the real package tag (e.g. AFEM-8282-AP1-RF2).",
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(entry.Value))
                {
                    MessageBox.Show($"Entry '{entry.Name}' must have a Value.\n\n" +
                                    $"Expected format:\n{GetFormatDescription(entry.Name)}",
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validate each semicolon-separated token against the hardcoded pattern
                if (_tokenPatterns.TryGetValue(entry.Name, out var patterns))
                {
                    var tokens = entry.Value.Split(';');
                    foreach (var raw in tokens)
                    {
                        var token = raw.Trim();
                        if (string.IsNullOrEmpty(token)) continue;

                        bool matched = false;
                        foreach (var rx in patterns)
                        {
                            if (rx.IsMatch(token)) { matched = true; break; }
                        }

                        if (!matched)
                        {
                            MessageBox.Show(
                                $"Invalid value \"{token}\" in entry '{entry.Name}'.\n\n" +
                                $"Expected format:\n{GetFormatDescription(entry.Name)}\n\n" +
                                $"Separate multiple values with a semicolon ( ; ).",
                                "Invalid Value", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                }

                // Validate expiry date format if provided
                if (!string.IsNullOrWhiteSpace(entry.Expiry) &&
                    !DateTime.TryParseExact(entry.Expiry, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    MessageBox.Show($"Entry '{entry.Name}' has an invalid expiry date '{entry.Expiry}'.\nFormat must be YYYY-MM-DD.",
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Per-package entries with empty tag are treated as global
                if (entry.IsPerPackage && string.IsNullOrWhiteSpace(entry.Tag))
                    entry.Expiry = "";
            }

            DialogResult = true;
            Close();
        }

        private static string GetFormatDescription(string name)
            => _formatDescriptions.TryGetValue(name, out var desc) ? desc : "—";

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Replaces all existing TestConfig children of <paramref name="inputValidationNode"/>
        /// with the current state of the entries list.
        /// </summary>
        public void ApplyChangesToXml(XmlDocument xmlDocument, XmlNode inputValidationNode)
        {
            var existing = inputValidationNode.SelectNodes("TestConfig");
            if (existing != null)
            {
                foreach (XmlNode old in existing)
                    inputValidationNode.RemoveChild(old);
            }

            foreach (var entry in _entries)
            {
                XmlElement elem = xmlDocument.CreateElement("TestConfig");

                if (!string.IsNullOrWhiteSpace(entry.Tag))
                    elem.SetAttribute("tag", entry.Tag);

                elem.SetAttribute("name", entry.Name);
                elem.SetAttribute("value", entry.Value);

                if (!string.IsNullOrWhiteSpace(entry.Expiry))
                    elem.SetAttribute("expiry", entry.Expiry);

                inputValidationNode.AppendChild(elem);
            }
        }
    }
}
