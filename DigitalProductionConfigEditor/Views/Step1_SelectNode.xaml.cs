using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Step1_SelectNode.xaml
    /// </summary>
    public partial class Step1_SelectNode : UserControl
    {
        public Step1_SelectNode()
        {
            InitializeComponent();
            this.Loaded += Step1_SelectNode_Loaded;
        }

        private void Step1_SelectNode_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAdvancedSettingsList();
        }

        private void LoadAdvancedSettingsList()
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel == null || viewModel.MasterXmlDocument == null) return;

            try
            {
                // Create temporary AdvancedSettingsViewModel to detect non-standard settings
                var advancedViewModel = new AdvancedSettingsViewModel();
                advancedViewModel.LoadFromXmlDocument(viewModel.MasterXmlDocument);

                // Get list of setting names
                var settingNames = new List<string>();
                if (advancedViewModel.RootNodes != null)
                {
                    foreach (var node in advancedViewModel.RootNodes)
                    {
                        settingNames.Add(node.DisplayName);
                    }
                }

                // Bind to ItemsControl
                AdvancedSettingsList.ItemsSource = settingNames;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading Advanced Settings list: {ex.Message}");
            }
        }

        private void OnAddConfigurationClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var node = button?.Tag as XmlNode;
            var viewModel = DataContext as WizardViewModel;

            if (node != null && viewModel != null)
            {
                viewModel.CopyConfigurationNodeFromMaster(node);
            }
        }

        private void OnEditConfigurationClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var node = button?.Tag as XmlNode;
            var viewModel = DataContext as WizardViewModel;

            if (node != null && viewModel != null)
            {
                // Select this configuration node
                viewModel.SelectedConfigurationNode = node;
                
                // Navigate to Step 2 to edit packages
                viewModel.CurrentStep = 2;
                
                // Trigger UI update by refreshing the parent window
                var window = Window.GetWindow(this);
                if (window is MainWindow mainWindow)
                {
                    mainWindow.NavigateToStep2();
                }
            }
        }

        private void OnDeleteConfigurationClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var node = button?.Tag as XmlNode;
            var viewModel = DataContext as WizardViewModel;

            if (node != null && viewModel != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the configuration '{node.Name}'?\n\nThis will remove all packages within it.",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    viewModel.DeleteConfigurationNode(node);
                }
            }
        }

        private void OnAddDeveloperValidationClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var node = button?.Tag as XmlNode;
            var viewModel = DataContext as WizardViewModel;

            if (node != null && viewModel != null)
            {
                viewModel.CopyDeveloperValidationNodeFromMaster(node);
            }
        }

        private void OnEditDeveloperValidationClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var node = button?.Tag as XmlNode;
            var viewModel = DataContext as WizardViewModel;

            if (node != null && viewModel != null)
            {
                // Select this configuration node
                viewModel.SelectedConfigurationNode = node;
                
                // Navigate to Step 2 to edit packages
                viewModel.CurrentStep = 2;
                
                // Trigger UI update by refreshing the parent window
                var window = Window.GetWindow(this);
                if (window is MainWindow mainWindow)
                {
                    mainWindow.NavigateToStep2();
                }
            }
        }

        private void OnDeleteDeveloperValidationClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var node = button?.Tag as XmlNode;
            var viewModel = DataContext as WizardViewModel;

            if (node != null && viewModel != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the Developer Validation config '{node.Name}'?\n\nThis will remove all packages within it.",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    viewModel.DeleteConfigurationNode(node);
                }
            }
        }

        private void OnAddPcbFormatConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;

            if (viewModel != null)
            {
                viewModel.CopyPcbFormatConfigFromMaster();
            }
        }

        private void OnEditPcbFormatConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;

            if (viewModel != null)
            {
                viewModel.EditPcbFormatConfig();
            }
        }

        private void OnDeletePcbFormatConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;

            if (viewModel != null)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete the PcbFormatConfig?\n\nThis will remove all island configurations.",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    viewModel.DeletePcbFormatConfig();
                }
            }
        }

        // Bulk selection handlers for Production User Configs
        private void OnSelectAllConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.SelectAllMasterConfigNodes();
        }

        private void OnDeselectAllConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.DeselectAllMasterConfigNodes();
        }

        private void OnAddSelectedConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel == null) return;

            var selectedNodes = viewModel.GetSelectedMasterConfigNodes();
            if (selectedNodes.Count == 0)
            {
                MessageBox.Show(
                    "No configuration nodes selected. Please select at least one configuration node using the checkboxes.",
                    "No Selection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var nodeNames = string.Join("\n• ", selectedNodes.Select(n => n.Name));
            var result = MessageBox.Show(
                $"Add {selectedNodes.Count} configuration node(s) to your new XML?\n\nNodes to be added:\n• {nodeNames}",
                "Confirm Bulk Add",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                viewModel.AddSelectedMasterConfigNodes();
            }
        }

        // Bulk selection handlers for Developer Validation Configs
        private void OnSelectAllDevValidationClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.SelectAllMasterDevValidationNodes();
        }

        private void OnDeselectAllDevValidationClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.DeselectAllMasterDevValidationNodes();
        }

        private void OnAddSelectedDevValidationClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel == null) return;

            var selectedNodes = viewModel.GetSelectedMasterDevValidationNodes();
            if (selectedNodes.Count == 0)
            {
                MessageBox.Show(
                    "No developer validation nodes selected. Please select at least one node using the checkboxes.",
                    "No Selection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var nodeNames = string.Join("\n• ", selectedNodes.Select(n => n.Name));
            var result = MessageBox.Show(
                $"Add {selectedNodes.Count} developer validation node(s) to your new XML?\n\nNodes to be added:\n• {nodeNames}",
                "Confirm Bulk Add",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                viewModel.AddSelectedMasterDevValidationNodes();
            }
        }

        private void OnCreateNewConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel == null) return;

            // Prompt for new configuration name
            var dialog = new InputDialog("Enter New Configuration Name:", "Create New Configuration", "NEW_FUNCTION_NAME");
            if (dialog.ShowDialog() == true)
            {
                string configName = dialog.InputText;
                viewModel.CreateNewConfigurationNode(configName);
            }
        }

        private void OnAdvancedSettingsClick(object sender, RoutedEventArgs e)
        {
            // Get the MainWindow and call its OnAdvancedSettingsClick handler
            var window = Window.GetWindow(this);
            if (window is MainWindow mainWindow)
            {
                // Use reflection to call the private method or trigger it via event
                var method = typeof(MainWindow).GetMethod("OnAdvancedSettingsClick", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method?.Invoke(mainWindow, new object[] { sender, e });
            }
        }
    }
}
