using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DigitalProductionConfigEditor.ViewModels;

namespace DigitalProductionConfigEditor.Views
{
    public partial class AdvancedSettingsView : UserControl
    {
        public AdvancedSettingsView()
        {
            InitializeComponent();
        }
        
        private void ConfigTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is AdvancedSettingsViewModel viewModel && e.NewValue is XmlNodeItem selectedNode)
            {
                viewModel.SelectedNode = selectedNode;
            }
        }
        
        private void TreeViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is XmlNodeItem selectedNode)
            {
                if (DataContext is AdvancedSettingsViewModel viewModel)
                {
                    viewModel.SelectedNode = selectedNode;
                    e.Handled = false; // Allow tree expansion/collapse to work
                }
            }
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdvancedSettingsViewModel viewModel)
            {
                if (viewModel.SaveChanges())
                {
                    MessageBox.Show("Changes saved successfully!", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            string helpMessage =
                "⚙️ ADVANCED SETTINGS GUIDE\n\n" +

                "📌 WHAT IS THIS?\n" +
                "Advanced Settings allows you to edit special XML configurations that don't follow the standard Package structure. " +
                "These global settings (applying to all products) are automatically detected from the Master template.\n\n" +

                "🔍 WHAT YOU CAN DO\n" +
                "• View and edit global configurations (e.g., hardware settings, pin definitions)\n" +
                "• Modify custom XML structures without editing raw files\n\n" +

                "📝 HOW TO USE\n" +
                "1. Select a Node: Click an item in the left tree. (📁 = Group, 📄 = Setting)\n" +
                "2. Edit Attributes: The right panel displays editable fields for the selected item.\n" +
                "   • Text, Numbers (supports scientific notation like 10e-6), and Dropdowns.\n" +
                "3. Save Changes: Click the \"💾 Save Changes\" button in this dialog to apply edits.\n\n" +

                "💾 IMPORTANT SAVING INSTRUCTIONS\n" +
                "• \"Save Changes\" here only updates the application's memory.\n" +
                "• You MUST click the main \"💾 Save\" button in the main window to write your changes to the XML file.\n\n" +

                "❓ FREQUENTLY ASKED QUESTIONS\n" +
                "Q: Why does it say \"No advanced settings found\"?\n" +
                "A: Your Master template doesn't contain any non-standard global configuration sections. This is normal.\n\n" +
                "Q: Where are my Package settings?\n" +
                "A: Standard Package settings are handled in the main wizard (Step 1 & 2). Advanced Settings is only for special global items.";

            MessageBox.Show(helpMessage,
                "⚙️ Advanced Settings - User Guide",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}

