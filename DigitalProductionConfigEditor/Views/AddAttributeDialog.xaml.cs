using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DigitalProductionConfigEditor.ViewModels;

namespace DigitalProductionConfigEditor.Views
{
    public partial class AddAttributeDialog : Window
    {
        private WizardViewModel _viewModel;

        public string AttributeName { get; private set; } = "";
        public string AttributeValue { get; private set; } = "";

        public List<string> CommonAttributes { get; set; }

        public AddAttributeDialog(WizardViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            // Get common attributes from existing packages in the configuration
            CommonAttributes = _viewModel.GetAvailableAttributeNames();

            DataContext = this;

            // Set default to "enable" if available
            if (CommonAttributes.Contains("enable"))
            {
                AttributeNameComboBox.SelectedItem = "enable";
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Validate attribute name
            var attrName = AttributeNameComboBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(attrName))
            {
                MessageBox.Show("Attribute Name is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                AttributeNameComboBox.Focus();
                return;
            }

            // Don't allow "name" as it's handled separately
            if (attrName.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The 'name' attribute is automatically added as the Package Name.\nPlease use a different attribute name.",
                    "Invalid Attribute",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AttributeName = attrName;
            AttributeValue = AttributeValueTextBox.Text.Trim();

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

