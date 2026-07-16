using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
using DigitalProductionConfigEditor.ViewModels;

namespace DigitalProductionConfigEditor.Views
{
    public class ProductGridRow : INotifyPropertyChanged
    {
        private string _productName = "";
        private Dictionary<string, string> _attributes = new Dictionary<string, string>();

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        public Dictionary<string, string> Attributes
        {
            get => _attributes;
            set => _attributes = value;
        }

        public void SetAttribute(string key, string value)
        {
            _attributes[key] = value;
            OnPropertyChanged(key);
        }

        public string GetAttribute(string key)
        {
            return _attributes.ContainsKey(key) ? _attributes[key] : "";
        }

        public Dictionary<string, string> GetAllAttributes()
        {
            return new Dictionary<string, string>(_attributes);
        }

        // Indexer for dynamic attribute access
        public string this[string attributeName]
        {
            get => GetAttribute(attributeName);
            set => SetAttribute(attributeName, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public partial class BulkAddProductsDialog : Window
    {
        private WizardViewModel _viewModel;
        private ObservableCollection<ProductGridRow> _products;
        private List<string> _attributeKeys;
        private Dictionary<string, List<string>?> _dropdownOptions;

        public string ConfigurationName { get; set; }

        public BulkAddProductsDialog(WizardViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            ConfigurationName = _viewModel.SelectedConfigurationName;
            DataContext = this;

            _products = new ObservableCollection<ProductGridRow>();
            _attributeKeys = new List<string>();
            _dropdownOptions = new Dictionary<string, List<string>?>();

            // Initialize based on existing packages
            InitializeAttributeTemplate();

            // Set up DataGrid
            ProductsDataGrid.ItemsSource = _products;
        }

        private void InitializeAttributeTemplate()
        {
            var existingAttributes = GetAttributesFromExistingPackages();
            bool hasExistingPackages = existingAttributes.Count > 0;

            if (hasExistingPackages)
            {
                // Store attribute keys for later use
                _attributeKeys = existingAttributes.Keys.ToList();
                
                // Store dropdown options for each attribute
                foreach (var key in _attributeKeys)
                {
                    _dropdownOptions[key] = GetDropdownOptionsForAttribute(key);
                }
            }
            else
            {
                // Use default attribute
                _attributeKeys.Add("enable");
                _dropdownOptions["enable"] = new List<string> { "TRUE", "FALSE" };
            }
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
                foreach (XmlAttribute attr in firstPackage.Attributes)
                {
                    if (!attr.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                        attributeTemplate[attr.Name] = attr.Value;
                }
            }

            return attributeTemplate;
        }

        private static XmlNode? FindFirstPackage(XmlNode? configNode)
        {
            if (configNode == null) return null;
            var packages = configNode.SelectNodes("Package");
            return (packages != null && packages.Count > 0) ? packages[0] : null;
        }

        private List<string>? GetDropdownOptionsForAttribute(string attributeName)
        {
            // Generate all valid Sublot values: 1A-1E, 2A-2E, 3A-3E
            var sublotOptions = Enumerable.Range(1, 3)
                .SelectMany(n => new[] { "A", "B", "C", "D", "E" }.Select(l => $"{n}{l}"))
                .ToList();

            // Define default dropdown options for specific attributes
            var dropdownMap = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "enable",      new List<string> { "FALSE", "TRUE" } },
                { "viewer",      new List<string> { "true", "false" } },
                { "mode",        new List<string> { "AUTO", "MANUAL", "SWEEP", "POINT" } },
                { "rule",        new List<string> { "DATETIME", "REV" } },
                { "avg_channel", new List<string> { "ALL", "EACH" } },
                { "Sublot",      sublotOptions }
            };

            // Try to get options from the master XML configuration node
            if (_viewModel.SelectedConfigurationNode != null)
            {
                var parentNode = _viewModel.SelectedConfigurationNode;
                
                // Map attribute names to their option node names in the XML
                var optionNodeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "enable", "EnableOptions" },
                    { "mode", "ModeOptions" },
                    { "rule", "RuleOptions" },
                    { "avg_channel", "AvgChannelOptions" },
                    { "viewer", "ViewerOptions" }
                };

                if (optionNodeMap.ContainsKey(attributeName))
                {
                    var optionNodeName = optionNodeMap[attributeName];
                    var optionNode = parentNode.SelectSingleNode(optionNodeName);
                    
                    if (optionNode?.InnerText != null)
                    {
                        // Parse options from XML (format: "OPTION1 | OPTION2 | OPTION3")
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

            // Return default dropdown options if available, otherwise null (will show TextBox)
            return dropdownMap.ContainsKey(attributeName) ? dropdownMap[attributeName] : null;
        }

        private void GenerateGrid_Click(object sender, RoutedEventArgs e)
        {
            var inputText = ProductNamesTextBox.Text;
            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Please enter at least one product name.", 
                    "No Input", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
                return;
            }

            // Parse product names
            var lines = inputText.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            var productNames = new List<string>();

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                var productName = trimmedLine.ToUpper();
                productNames.Add(productName);
            }

            // Remove duplicates
            productNames = productNames.Distinct().ToList();

            if (productNames.Count == 0)
            {
                MessageBox.Show("No valid product names found.", 
                    "No Products", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
                return;
            }

            // Clear existing grid
            _products.Clear();
            ProductsDataGrid.Columns.Clear();

            // Add Product Name column
            var nameColumn = new DataGridTextColumn
            {
                Header = "Product Name",
                Binding = new Binding("ProductName") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                Width = new DataGridLength(200)
            };
            nameColumn.ElementStyle = new Style(typeof(TextBlock));
            nameColumn.ElementStyle.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
            nameColumn.ElementStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, System.Windows.Media.Brushes.DarkBlue));
            nameColumn.ElementStyle.Setters.Add(new Setter(TextBlock.PaddingProperty, new Thickness(5)));
            ProductsDataGrid.Columns.Add(nameColumn);

            // Get default attribute values
            var defaultAttributes = GetAttributesFromExistingPackages();

            // Add attribute columns
            foreach (var attrKey in _attributeKeys)
            {
                var dropdownOptions = _dropdownOptions.ContainsKey(attrKey) ? _dropdownOptions[attrKey] : null;

                if (dropdownOptions != null && dropdownOptions.Count > 0)
                {
                    // ComboBox column for dropdown attributes with visual indicator
                    var comboColumn = new DataGridComboBoxColumn
                    {
                        Header = $"{attrKey} ▼",  // Add dropdown indicator
                        SelectedItemBinding = new Binding($"[{attrKey}]") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                        ItemsSource = dropdownOptions,
                        Width = new DataGridLength(120),
                        IsReadOnly = false
                    };
                    
                    // Style the header to indicate it's a dropdown (light blue background)
                    var headerStyle = new Style(typeof(DataGridColumnHeader));
                    headerStyle.Setters.Add(new Setter(DataGridColumnHeader.BackgroundProperty, System.Windows.Media.Brushes.LightBlue));
                    headerStyle.Setters.Add(new Setter(DataGridColumnHeader.FontWeightProperty, FontWeights.Bold));
                    headerStyle.Setters.Add(new Setter(DataGridColumnHeader.PaddingProperty, new Thickness(5)));
                    comboColumn.HeaderStyle = headerStyle;
                    
                    ProductsDataGrid.Columns.Add(comboColumn);
                }
                else
                {
                    // TextBox column for regular attributes
                    var textColumn = new DataGridTextColumn
                    {
                        Header = attrKey,
                        Binding = new Binding($"[{attrKey}]") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                        Width = new DataGridLength(100),
                        IsReadOnly = false
                    };

                    if (PackageNumericAttributes.IsNonNegativeInteger(attrKey))
                    {
                        var editingStyle = new Style(typeof(TextBox));
                        editingStyle.Setters.Add(new EventSetter(TextBox.PreviewTextInputEvent,
                            new TextCompositionEventHandler(NumericAttributeCell_PreviewTextInput)));
                        editingStyle.Setters.Add(new EventSetter(DataObject.PastingEvent,
                            new DataObjectPastingEventHandler(NumericAttributeCell_Pasting)));
                        textColumn.EditingElementStyle = editingStyle;
                    }
                    
                    // Style regular column headers
                    var headerStyle = new Style(typeof(DataGridColumnHeader));
                    headerStyle.Setters.Add(new Setter(DataGridColumnHeader.FontWeightProperty, FontWeights.SemiBold));
                    headerStyle.Setters.Add(new Setter(DataGridColumnHeader.PaddingProperty, new Thickness(5)));
                    textColumn.HeaderStyle = headerStyle;
                    
                    ProductsDataGrid.Columns.Add(textColumn);
                }
            }

            // Create product rows with default attribute values
            foreach (var productName in productNames)
            {
                // Check if product already exists in grid
                var existing = _products.FirstOrDefault(p => p.ProductName == productName);
                if (existing == null)
                {
                    var row = new ProductGridRow
                    {
                        ProductName = productName
                    };

                    // Set default attribute values
                    foreach (var attr in _attributeKeys)
                    {
                        // Always default enable to FALSE, copy other attributes from existing packages
                        string value;
                        if (attr.Equals("enable", StringComparison.OrdinalIgnoreCase))
                        {
                            value = "FALSE";  // Always FALSE for enable
                            var originalValue = defaultAttributes.ContainsKey(attr) ? defaultAttributes[attr] : "N/A";
                            System.Diagnostics.Debug.WriteLine($"DEBUG BULK: Forcing enable to FALSE for {productName} (template had: {originalValue})");
                        }
                        else
                        {
                            value = defaultAttributes.ContainsKey(attr) ? defaultAttributes[attr] : "";
                        }
                        row.SetAttribute(attr, value);
                        System.Diagnostics.Debug.WriteLine($"DEBUG BULK: Set {productName}.{attr} = {value}");
                    }

                    _products.Add(row);
                }
            }

            // Update count
            ProductCountTextBlock.Text = $" - {_products.Count} products";

            MessageBox.Show($"Grid generated with {_products.Count} product(s).\n\n✓ Dropdown columns (light blue): {string.Join(", ", _attributeKeys.Where(k => _dropdownOptions.ContainsKey(k) && _dropdownOptions[k] != null).Select(k => k))}\n✓ Text columns: {string.Join(", ", _attributeKeys.Where(k => !_dropdownOptions.ContainsKey(k) || _dropdownOptions[k] == null).Select(k => k))}\n\nYou can now edit each product's attributes individually.\nDropdown columns have a ▼ indicator in the header.", 
                "Grid Generated", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
        }

        private void AddAllProducts_Click(object sender, RoutedEventArgs e)
        {
            if (_products.Count == 0)
            {
                MessageBox.Show("Please generate the grid first by clicking 'Generate Grid'.", 
                    "No Products", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
                return;
            }

            // Confirm action
            var result = MessageBox.Show(
                $"Are you sure you want to add {_products.Count} product(s) to '{ConfigurationName}'?",
                "Confirm Bulk Add",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                int addedCount = 0;
                int skippedCount = 0;
                var skippedNames = new List<string>();

                foreach (var product in _products)
                {
                    // Check if package already exists
                    var existingPackages = _viewModel.PackagesInSelectedConfiguration;
                    var duplicate = existingPackages.FirstOrDefault(p =>
                        p.Attributes?["name"]?.Value?.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase) == true);

                    if (duplicate != null)
                    {
                        skippedCount++;
                        skippedNames.Add(product.ProductName);
                        continue;
                    }

                    // Get attributes for this product
                    var attributes = product.GetAllAttributes();

                    // Add the package
                    _viewModel.AddPackageToConfiguration(product.ProductName, attributes);
                    addedCount++;
                }

                // Show summary
                string message = $"Successfully added {addedCount} product(s) to '{ConfigurationName}'.";
                if (skippedCount > 0)
                {
                    message += $"\n\n⚠️ Skipped {skippedCount} duplicate package(s):\n" + string.Join(", ", skippedNames);
                    message += "\n\nDuplicate packages are not allowed. These products already exist in this configuration.";
                }

                MessageBox.Show(message,
                    "Bulk Add Complete",
                    MessageBoxButton.OK,
                    skippedCount > 0 ? MessageBoxImage.Warning : MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding products: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private static void NumericAttributeCell_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private static void NumericAttributeCell_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.DataObject.GetDataPresent(DataFormats.Text))
                return;

            var text = e.DataObject.GetData(DataFormats.Text) as string ?? "";
            if (!Regex.IsMatch(text, "^[0-9]*$"))
                e.CancelCommand();
        }
    }
}
