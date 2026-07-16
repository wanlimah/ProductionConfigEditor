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
            // Restore developer mode state from ViewModel
            var viewModel = DataContext as WizardViewModel;
            if (viewModel != null && viewModel.IsDeveloperModeUnlocked)
            {
                DeveloperConfigPanel.Visibility = Visibility.Visible;
                DeveloperPasswordPanel.Visibility = Visibility.Collapsed;
                UnlockDeveloperButton.Visibility = Visibility.Collapsed;
            }
        }

        private void OnAddConfigurationClick(object sender, RoutedEventArgs e)        {
            var button = sender as Button;
            var node = button?.Tag as XmlNode;
            var viewModel = DataContext as WizardViewModel;

            if (node != null && viewModel != null)
            {
                viewModel.CopyConfigurationNodeFromMaster(node);
            }
        }

        private void OnOpenDocClick(object sender, RoutedEventArgs e)
        {
            var url = (sender as Button)?.Tag as string;
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open documentation link:\n{ex.Message}",
                        "Open Doc", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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

        private void OnAddInputValidationConfigsClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.CopyInputValidationConfigsFromMaster();
        }

        private void OnEditInputValidationConfigsClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.EditInputValidationConfigs();
        }

        private void OnDeleteInputValidationConfigsClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel == null) return;

            var result = MessageBox.Show(
                "Are you sure you want to delete the InputValidationConfigs section?\n\nThis will remove all TestConfig entries.",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                viewModel.DeleteInputValidationConfigs();
            }
        }

        private void OnAddDcContactModeClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.CopyDcContactModeSettingOverwriteFromMaster();
        }

        private void OnEditDcContactModeClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.EditDcContactModeSettingOverwrite();
        }

        private void OnDeleteDcContactModeClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel == null) return;

            var result = MessageBox.Show(
                "Are you sure you want to delete the DC_CONTACT_MODE_SETTING_OVERWRITE section?\n\nThis will remove all Pin entries.",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
                viewModel.DeleteDcContactModeSettingOverwrite();
        }

        private void OnAddMailConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.CopyMailConfigFromMaster();
        }

        private void OnEditMailConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            viewModel?.EditMailConfig();
        }

        private void OnDeleteMailConfigClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WizardViewModel;
            if (viewModel == null) return;

            var result = MessageBox.Show(
                "Are you sure you want to delete the MailConfig section?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
                viewModel.DeleteMailConfig();
        }

        private void OnUnlockDeveloperClick(object sender, RoutedEventArgs e)
        {
            if (DeveloperPasswordPanel.Visibility == Visibility.Visible)
            {
                DeveloperPasswordPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeveloperPasswordPanel.Visibility = Visibility.Visible;
                DeveloperPasskeyError.Visibility = Visibility.Collapsed;
                DeveloperPasskeyBox.Password = "";
                DeveloperPasskeyBox.Focus();
            }
        }

        private void OnVerifyDeveloperPasskeyClick(object sender, RoutedEventArgs e)
        {
            // Hardcoded passkey as requested - can be changed here
            if (DeveloperPasskeyBox.Password == "admin" || DeveloperPasskeyBox.Password == "Dev2024!")
            {
                DeveloperConfigPanel.Visibility = Visibility.Visible;
                DeveloperPasswordPanel.Visibility = Visibility.Collapsed;
                UnlockDeveloperButton.Visibility = Visibility.Collapsed;

                // Persist state in ViewModel
                var viewModel = DataContext as WizardViewModel;
                if (viewModel != null)
                {
                    viewModel.IsDeveloperModeUnlocked = true;
                }
            }
            else
            {
                DeveloperPasskeyError.Visibility = Visibility.Visible;
                DeveloperPasskeyBox.SelectAll();
                DeveloperPasskeyBox.Focus();
            }
        }
    }
}
