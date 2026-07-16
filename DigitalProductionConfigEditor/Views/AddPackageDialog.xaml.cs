using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DigitalProductionConfigEditor.ViewModels;

namespace DigitalProductionConfigEditor.Views
{
    public class AttributeEntry : INotifyPropertyChanged
    {
        private string _key = "";
        private string _value = "";
        private List<string>? _dropdownOptions;

        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged(nameof(Key));
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public List<string>? DropdownOptions
        {
            get => _dropdownOptions;
            set
            {
                _dropdownOptions = value;
                OnPropertyChanged(nameof(DropdownOptions));
                OnPropertyChanged(nameof(HasDropdown));
            }
        }

        public bool HasDropdown => DropdownOptions != null && DropdownOptions.Count > 0;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public partial class AddPackageDialog : Window
    {
        private WizardViewModel _viewModel;
        private ObservableCollection<AttributeEntry> _attributes;

        public string ConfigurationName { get; set; }

        public AddPackageDialog(WizardViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            ConfigurationName = _viewModel.SelectedConfigurationName;
            DataContext = this;

            // Initialize with attributes from existing packages in this configuration
            _attributes = new ObservableCollection<AttributeEntry>();
            
            // Get all unique attributes from existing packages in the selected configuration
            var existingAttributes = GetAttributesFromExistingPackages();
            
            if (existingAttributes.Count > 0)
            {
                // Use attributes from existing packages as template
                foreach (var attr in existingAttributes)
                {
                    // ALWAYS force enable to FALSE for new packages (safety requirement)
                    string attributeValue = attr.Value;
                    if (attr.Key.Equals("enable", StringComparison.OrdinalIgnoreCase))
                    {
                        attributeValue = "FALSE";  // ALWAYS FALSE for new packages
                    }
                    
                    // Create entry with DropdownOptions FIRST, then set Value
                    var entry = new AttributeEntry();
                    entry.Key = attr.Key;
                    entry.DropdownOptions = GetDropdownOptionsForAttribute(attr.Key);
                    entry.Value = attributeValue;
                    _attributes.Add(entry);
                }
            }
            else
            {
                // Fallback: if no existing packages, just add enable with dropdown
                var entry = new AttributeEntry();
                entry.Key = "enable";
                entry.DropdownOptions = new List<string> { "FALSE", "TRUE" };
                entry.Value = "FALSE";
                _attributes.Add(entry);
            }

            AttributesItemsControl.ItemsSource = _attributes;
            
            // Setup event handlers for real-time preview
            PackageNameTextBox.TextChanged += (s, e) => UpdatePackageNamePreview();
        }

        private Dictionary<string, string> GetAttributesFromExistingPackages()
        {
            var attributeTemplate = new Dictionary<string, string>();

            // 1. Try the user's new XML node first
            var firstPackage = FindFirstPackage(_viewModel.SelectedConfigurationNode);

            // 2. If no packages exist yet, fall back to the master XML's equivalent node
            if (firstPackage == null && _viewModel.SelectedConfigurationName != "None")
            {
                var masterNode = _viewModel.MasterXmlDocument?
                    .SelectSingleNode($"//ProductionUserConfigs/{_viewModel.SelectedConfigurationName}");
                firstPackage = FindFirstPackage(masterNode);
            }

            if (firstPackage?.Attributes != null)
            {
                foreach (System.Xml.XmlAttribute attr in firstPackage.Attributes)
                {
                    if (!attr.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                        attributeTemplate[attr.Name] = attr.Value;
                }
            }

            return attributeTemplate;
        }

        private static System.Xml.XmlNode? FindFirstPackage(System.Xml.XmlNode? configNode)
        {
            if (configNode == null) return null;
            var packages = configNode.SelectNodes("Package");
            return (packages != null && packages.Count > 0) ? packages[0] : null;
        }

        // Add/Remove attribute methods removed - attributes are now fixed based on template

        private List<string>? GetDropdownOptionsForAttribute(string attributeName)
        {
            // Generate all valid Sublot values: 1A-1E, 2A-2E, 3A-3E
            var sublotOptions = Enumerable.Range(1, 3)
                .SelectMany(n => new[] { "A", "B", "C", "D", "E" }.Select(l => $"{n}{l}"))
                .ToList();

            // Define dropdown options for specific attributes
            var dropdownMap = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "enable",  new List<string> { "FALSE", "TRUE" } },  // FALSE FIRST = default for new packages!
                { "viewer",  new List<string> { "true", "false" } },
                { "mode",    new List<string> { "AUTO", "MANUAL", "SWEEP", "POINT" } },
                { "rule",    new List<string> { "DATETIME", "REV" } },
                { "avg_channel", new List<string> { "ALL", "EACH" } },
                { "Sublot",  sublotOptions }
            };

            // Try to get options from the master XML configuration node
            if (_viewModel.SelectedConfigurationNode != null)
            {
                var parentNode = _viewModel.SelectedConfigurationNode;
                
                // Map attribute names to their option node names
                var optionNodeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "enable", "EnableOptions" },
                    { "mode", "ModeOptions" },
                    { "rule", "RuleOptions" },
                    { "avg_channel", "AvgChannelOptions" }
                };

                if (optionNodeMap.ContainsKey(attributeName))
                {
                    var optionNodeName = optionNodeMap[attributeName];
                    var optionNode = parentNode.SelectSingleNode(optionNodeName);
                    
                    if (optionNode?.InnerText != null)
                    {
                        // Parse options (format: "OPTION1 | OPTION2 | OPTION3")
                        var options = optionNode.InnerText
                            .Split('|')
                            .Select(o => o.Trim())
                            .Where(o => !string.IsNullOrWhiteSpace(o))
                            .ToList();

                        if (options.Count > 0)
                        {
                            return options;
                        }
                    }
                }
            }

            // Return dropdown options if available, otherwise null (will show TextBox)
            return dropdownMap.ContainsKey(attributeName) ? dropdownMap[attributeName] : null;
        }

        private void UpdatePackageNamePreview()
        {
            var baseName = PackageNameTextBox.Text.Trim();
            var finalName = GetFinalPackageName();
            
            if (string.IsNullOrWhiteSpace(baseName))
            {
                PreviewNameTextBlock.Text = "(enter package name above)";
                PreviewNameTextBlock.Foreground = System.Windows.Media.Brushes.Gray;
            }
            else
            {
                PreviewNameTextBlock.Text = finalName;
                PreviewNameTextBlock.Foreground = System.Windows.Media.Brushes.DarkBlue;
            }
        }

        private string GetFinalPackageName()
        {
            var baseName = PackageNameTextBox.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(baseName))
            {
                return "";
            }

            return baseName;
        }

        private void CreatePackage_Click(object sender, RoutedEventArgs e)
        {
            // Validate package name
            var baseName = PackageNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(baseName))
            {
                MessageBox.Show("Package Name is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                PackageNameTextBox.Focus();
                return;
            }

            // Get final package name (with suffix if checked)
            var packageName = GetFinalPackageName();

            // Check for duplicate package name in the same configuration
            var existingPackages = _viewModel.PackagesInSelectedConfiguration;
            var duplicate = existingPackages.FirstOrDefault(p =>
                p.Attributes?["name"]?.Value?.Equals(packageName, StringComparison.OrdinalIgnoreCase) == true);

            if (duplicate != null)
            {
                MessageBox.Show(
                    $"A package with the name '{packageName}' already exists in this configuration.\n\nDuplicate packages are not allowed. Please use a different name or edit the existing package.",
                    "Duplicate Package Name",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                PackageNameTextBox.Focus();
                PackageNameTextBox.SelectAll();
                return;
            }

            // Build attributes dictionary
            var attributes = new Dictionary<string, string>();
            foreach (var attr in _attributes)
            {
                if (!string.IsNullOrWhiteSpace(attr.Key))
                {
                    attributes[attr.Key] = attr.Value;
                }
            }

            // Add package to configuration
            _viewModel.AddPackageToConfiguration(packageName, attributes);

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // FORCE enable ComboBox to select FALSE when it loads
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox combo && combo.Tag is string key)
            {
                if (key.Equals("enable", StringComparison.OrdinalIgnoreCase))
                {
                    // FORCE select "FALSE" for enable attribute
                    if (combo.Items.Contains("FALSE"))
                    {
                        combo.SelectedItem = "FALSE";
                    }
                }
            }
        }

        // Numeric validation for integer-only package attributes
        private void NumericField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is string attributeName)
            {
                if (PackageNumericAttributes.IsNonNegativeInteger(attributeName))
                {
                    Regex regex = new Regex("[^0-9]+");
                    e.Handled = regex.IsMatch(e.Text);
                }
            }
        }

        private void NumericField_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is not TextBox textBox || textBox.Tag is not string attributeName)
                return;
            if (!PackageNumericAttributes.IsNonNegativeInteger(attributeName))
                return;
            if (!e.DataObject.GetDataPresent(DataFormats.Text))
                return;
            var text = e.DataObject.GetData(DataFormats.Text) as string ?? "";
            if (!Regex.IsMatch(text, "^[0-9]*$"))
                e.CancelCommand();
        }
    }
}

