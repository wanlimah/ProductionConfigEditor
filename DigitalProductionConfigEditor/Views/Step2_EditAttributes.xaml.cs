using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using DigitalProductionConfigEditor.ViewModels;

namespace DigitalProductionConfigEditor.Views
{
    /// <summary>
    /// Interaction logic for Step2_EditAttributes.xaml
    /// </summary>
    public partial class Step2_EditAttributes : UserControl
    {
        public Step2_EditAttributes()
        {
            InitializeComponent();
        }

        private void Configuration_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Save any pending changes before switching to a different configuration
            var viewModel = DataContext as WizardViewModel;
            if (viewModel != null && e.RemovedItems.Count > 0)
            {
                // Save the changes from the previously selected configuration
                viewModel.SaveAttributesToNode();
            }
            
            // Refresh package items for the new configuration
            if (viewModel != null)
            {
                viewModel.RefreshPackageItems();
            }
        }

        private void EditPackage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is XmlNode packageNode)
            {
                var viewModel = DataContext as WizardViewModel;
                if (viewModel != null)
                {
                    // IMPORTANT: Save current package's attributes before switching to another package
                    if (viewModel.SelectedPackageNode != null && viewModel.SelectedPackageNode != packageNode)
                    {
                        viewModel.SaveAttributesToNode();
                    }
                    
                    // Now load the new package
                    viewModel.SelectedPackageNode = packageNode;
                    viewModel.StatusMessage = $"Editing package: {packageNode.Attributes?["name"]?.Value}";
                }
            }
        }

        private void AddPackage_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel?.SelectedConfigurationNode == null)
            {
                MessageBox.Show("Please select a Configuration Node first.", "No Configuration Selected", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Open Add Package dialog
            var dialog = new AddPackageDialog(viewModel);
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
        }

        private void BulkAddProducts_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel?.SelectedConfigurationNode == null)
            {
                MessageBox.Show("Please select a Configuration Node first.", "No Configuration Selected", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Open Bulk Add Products dialog
            var dialog = new BulkAddProductsDialog(viewModel);
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
        }

        private void DeletePackage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is XmlNode packageNode)
            {
                var viewModel = DataContext as WizardViewModel;
                if (viewModel != null)
                {
                    var packageName = packageNode.Attributes?["name"]?.Value ?? "Unknown";
                    var result = MessageBox.Show(
                        $"Are you sure you want to delete the package '{packageName}'?",
                        "Confirm Deletion",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        viewModel.DeletePackage(packageNode);
                    }
                }
            }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.SelectAllPackages();
        }

        private void DeselectAll_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.DeselectAllPackages();
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel == null) return;

            var selectedPackages = viewModel.GetSelectedPackages();
            if (selectedPackages.Count == 0)
            {
                MessageBox.Show(
                    "No packages selected. Please select at least one package using the checkboxes.",
                    "No Selection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var packageNames = string.Join("\n• ", selectedPackages.Select(p => p.Attributes?["name"]?.Value ?? "Unknown"));
            var result = MessageBox.Show(
                $"Are you sure you want to delete {selectedPackages.Count} package(s)?\n\nPackages to be deleted:\n• {packageNames}",
                "Confirm Bulk Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                viewModel.DeleteSelectedPackages();
            }
        }

        private void NumericAttribute_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (sender is TextBox textBox)
                {
                    // Get the attribute name from the Tag
                    string? attributeName = null;
                    
                    if (textBox.Tag is string tagString)
                    {
                        attributeName = tagString;
                    }
                    else if (textBox.DataContext is AttributeViewModel attrVM)
                    {
                        attributeName = attrVM.Name;
                    }
                    
                    if (!string.IsNullOrEmpty(attributeName))
                    {
                        // List of attributes that should only accept integers
                        var numericFields = new[] { "count", "sampling", "threshold", "x", "y" };
                        
                        if (numericFields.Contains(attributeName.ToLower()))
                        {
                            // Only allow digits (integers only)
                            Regex regex = new Regex("[^0-9]+");
                            e.Handled = regex.IsMatch(e.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Silently catch any exceptions to prevent crashes
                System.Diagnostics.Debug.WriteLine($"Validation error: {ex.Message}");
            }
        }

        private void ValidateInputs()
        {
            var vm = DataContext as WizardViewModel;
            
            if (vm?.SelectedPackageNode == null) 
            {
                if (vm != null)
                    vm.StatusMessage = "No package selected for validation.";
                return;
            }

            // Parse numeric values for validation
            if (int.TryParse(vm.SelectedCount, out int count) &&
                int.TryParse(vm.SelectedSampling, out int sampling) &&
                int.TryParse(vm.SelectedThreshold, out int threshold))
            {
                if (count <= 0 || sampling <= 0 || threshold <= 0)
                {
                    vm.StatusMessage = "⚠ Please enter valid positive values for Count, Sampling, and Threshold.";
                }
                else
                {
                    vm.StatusMessage = "✓ All values are valid.";
                }
            }
            else
            {
                vm.StatusMessage = "⚠ Please enter numeric values for Count, Sampling, and Threshold.";
            }
        }
    }
}
