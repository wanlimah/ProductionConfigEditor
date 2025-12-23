using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace DigitalProductionConfigEditor.Views
{
    public class IslandViewModel : INotifyPropertyChanged
    {
        private string _id = "";
        private string _stripUnitX = "";
        private string _stripUnitY = "";
        private string _panelStripX = "";
        private string _panelStripY = "";
        private bool _isNew = false;

        public string Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string StripUnitX
        {
            get => _stripUnitX;
            set { _stripUnitX = value; OnPropertyChanged(nameof(StripUnitX)); }
        }

        public string StripUnitY
        {
            get => _stripUnitY;
            set { _stripUnitY = value; OnPropertyChanged(nameof(StripUnitY)); }
        }

        public string PanelStripX
        {
            get => _panelStripX;
            set { _panelStripX = value; OnPropertyChanged(nameof(PanelStripX)); }
        }

        public string PanelStripY
        {
            get => _panelStripY;
            set { _panelStripY = value; OnPropertyChanged(nameof(PanelStripY)); }
        }

        public bool IsNew
        {
            get => _isNew;
            set { _isNew = value; OnPropertyChanged(nameof(IsNew)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public partial class EditPcbFormatDialog : Window
    {
        private ObservableCollection<IslandViewModel> _islands = new();
        private XmlNode? _pcbFormatConfigNode;

        public EditPcbFormatDialog(XmlNode? pcbFormatConfigNode)
        {
            InitializeComponent();
            _pcbFormatConfigNode = pcbFormatConfigNode;
            
            LoadIslands();
            IslandsItemsControl.ItemsSource = _islands;
        }

        private void LoadIslands()
        {
            _islands.Clear();

            if (_pcbFormatConfigNode == null) return;

            var islandNodes = _pcbFormatConfigNode.SelectNodes("Island");
            if (islandNodes == null) return;

            foreach (XmlNode islandNode in islandNodes)
            {
                var island = new IslandViewModel();
                
                // Get Island ID
                island.Id = islandNode.Attributes?["id"]?.Value ?? "";

                // Get StripUnitCount
                var stripUnitNode = islandNode.SelectSingleNode("StripUnitCount");
                if (stripUnitNode != null)
                {
                    island.StripUnitX = stripUnitNode.Attributes?["x"]?.Value ?? "";
                    island.StripUnitY = stripUnitNode.Attributes?["y"]?.Value ?? "";
                }

                // Get PanelStripCount
                var panelStripNode = islandNode.SelectSingleNode("PanelStripCount");
                if (panelStripNode != null)
                {
                    island.PanelStripX = panelStripNode.Attributes?["x"]?.Value ?? "";
                    island.PanelStripY = panelStripNode.Attributes?["y"]?.Value ?? "";
                }

                _islands.Add(island);
            }
        }

        private void OnAddIslandClick(object sender, RoutedEventArgs e)
        {
            // Find the next available island ID
            var maxId = 0;
            foreach (var island in _islands)
            {
                if (int.TryParse(island.Id, out int id) && id > maxId)
                {
                    maxId = id;
                }
            }

            var newIsland = new IslandViewModel
            {
                Id = (maxId + 1).ToString(),
                StripUnitX = "50",
                StripUnitY = "17",
                PanelStripX = "2",
                PanelStripY = "4",
                IsNew = true  // Mark as newly added
            };

            _islands.Add(newIsland);
            
            // Show message to indicate it was added
            MessageBox.Show($"Island {newIsland.Id} added successfully!\n\nNew islands are highlighted in green.", 
                "Island Added", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);
        }

        private void OnDeleteIslandClick(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is IslandViewModel island)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete Island {island.Id}?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _islands.Remove(island);
                }
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            // Validate all islands
            foreach (var island in _islands)
            {
                if (string.IsNullOrWhiteSpace(island.Id))
                {
                    MessageBox.Show("All islands must have an ID.", "Validation Error", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(island.StripUnitX) || 
                    string.IsNullOrWhiteSpace(island.StripUnitY) ||
                    string.IsNullOrWhiteSpace(island.PanelStripX) || 
                    string.IsNullOrWhiteSpace(island.PanelStripY))
                {
                    MessageBox.Show($"Island {island.Id} has incomplete data. All X and Y values must be filled.", 
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validate numeric values
                if (!int.TryParse(island.StripUnitX, out _) || 
                    !int.TryParse(island.StripUnitY, out _) ||
                    !int.TryParse(island.PanelStripX, out _) || 
                    !int.TryParse(island.PanelStripY, out _))
                {
                    MessageBox.Show($"Island {island.Id} has invalid numeric values. All X and Y values must be numbers.", 
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            DialogResult = true;
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public void ApplyChangesToXml(XmlDocument xmlDocument, XmlNode pcbFormatConfigNode)
        {
            // Clear existing islands
            var existingIslands = pcbFormatConfigNode.SelectNodes("Island");
            if (existingIslands != null)
            {
                foreach (XmlNode island in existingIslands)
                {
                    pcbFormatConfigNode.RemoveChild(island);
                }
            }

            // Add islands from the view model
            foreach (var island in _islands)
            {
                XmlElement islandElement = xmlDocument.CreateElement("Island");
                
                // Set ID attribute
                XmlAttribute idAttr = xmlDocument.CreateAttribute("id");
                idAttr.Value = island.Id;
                islandElement.Attributes.Append(idAttr);

                // Create StripUnitCount element
                XmlElement stripUnitElement = xmlDocument.CreateElement("StripUnitCount");
                XmlAttribute stripXAttr = xmlDocument.CreateAttribute("x");
                stripXAttr.Value = island.StripUnitX;
                stripUnitElement.Attributes.Append(stripXAttr);
                XmlAttribute stripYAttr = xmlDocument.CreateAttribute("y");
                stripYAttr.Value = island.StripUnitY;
                stripUnitElement.Attributes.Append(stripYAttr);
                islandElement.AppendChild(stripUnitElement);

                // Create PanelStripCount element
                XmlElement panelStripElement = xmlDocument.CreateElement("PanelStripCount");
                XmlAttribute panelXAttr = xmlDocument.CreateAttribute("x");
                panelXAttr.Value = island.PanelStripX;
                panelStripElement.Attributes.Append(panelXAttr);
                XmlAttribute panelYAttr = xmlDocument.CreateAttribute("y");
                panelYAttr.Value = island.PanelStripY;
                panelStripElement.Attributes.Append(panelYAttr);
                islandElement.AppendChild(panelStripElement);

                pcbFormatConfigNode.AppendChild(islandElement);
            }
        }
    }
}












