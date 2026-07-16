using System.Windows;
using System.Xml;

namespace DigitalProductionConfigEditor.Views
{
    public partial class EditMailConfigDialog : Window
    {
        public EditMailConfigDialog(XmlNode mailConfigNode)
        {
            InitializeComponent();
            ApiUrlBox.Text        = mailConfigNode.Attributes?["APIURL"]?.Value         ?? "";
            RecipientEmailBox.Text = mailConfigNode.Attributes?["RecipientEmail"]?.Value ?? "";
            ReplyToBox.Text       = mailConfigNode.Attributes?["ReplyTo"]?.Value         ?? "";
        }

        /// <summary>
        /// Writes the dialog values back into the live XML node's attributes.
        /// </summary>
        public void ApplyChangesToXml(XmlNode mailConfigNode)
        {
            SetOrCreate(mailConfigNode, "APIURL",          ApiUrlBox.Text.Trim());
            SetOrCreate(mailConfigNode, "RecipientEmail",  RecipientEmailBox.Text.Trim());
            SetOrCreate(mailConfigNode, "ReplyTo",         ReplyToBox.Text.Trim());
        }

        private static void SetOrCreate(XmlNode node, string attrName, string value)
        {
            if (node.Attributes == null) return;
            var attr = node.Attributes[attrName];
            if (attr != null)
            {
                attr.Value = value;
            }
            else
            {
                var newAttr = node.OwnerDocument!.CreateAttribute(attrName);
                newAttr.Value = value;
                node.Attributes.Append(newAttr);
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ApiUrlBox.Text))
            {
                MessageBox.Show("API URL cannot be empty.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(RecipientEmailBox.Text))
            {
                MessageBox.Show("Recipient Email cannot be empty.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
