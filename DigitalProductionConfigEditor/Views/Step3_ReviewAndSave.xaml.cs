using System;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using DigitalProductionConfigEditor.ViewModels;

namespace DigitalProductionConfigEditor.Views
{
    /// <summary>
    /// Interaction logic for Step3_ReviewAndSave.xaml
    /// </summary>
    public partial class Step3_ReviewAndSave : UserControl
    {
        public Step3_ReviewAndSave()
        {
            InitializeComponent();
            Loaded += Step3_ReviewAndSave_Loaded;
        }

        private void Step3_ReviewAndSave_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Load and display the complete XML when the view is loaded
            LoadXmlPreview();
        }

        private void LoadXmlPreview()
        {
            try
            {
                var viewModel = DataContext as WizardViewModel;
                if (viewModel?.NewXmlDocument != null)
                {
                    // Format the XML with indentation for better readability
                    var xmlString = FormatXml(viewModel.NewXmlDocument);
                    XmlPreviewTextBox.Text = xmlString;
                }
                else
                {
                    XmlPreviewTextBox.Text = "<!-- No XML data available -->";
                }
            }
            catch (Exception ex)
            {
                XmlPreviewTextBox.Text = $"<!-- Error loading XML preview: {ex.Message} -->";
            }
        }

        private string FormatXml(XmlDocument xmlDoc)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\n",
                    NewLineHandling = NewLineHandling.Replace,
                    OmitXmlDeclaration = false,
                    Encoding = Encoding.UTF8
                };

                using (var writer = XmlWriter.Create(stringBuilder, settings))
                {
                    xmlDoc.Save(writer);
                }

                return stringBuilder.ToString();
            }
            catch (Exception)
            {
                // If formatting fails, return the raw XML
                return xmlDoc.OuterXml;
            }
        }
    }
}
