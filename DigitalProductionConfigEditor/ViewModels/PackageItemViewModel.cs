using System.ComponentModel;
using System.Xml;

namespace DigitalProductionConfigEditor.ViewModels
{
    /// <summary>
    /// View model for a package item with checkbox selection support
    /// </summary>
    public class PackageItemViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        
        public XmlNode PackageNode { get; set; }
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        
        public string PackageName => PackageNode?.Attributes?["name"]?.Value ?? "Unknown";
        
        public string PackageXml => PackageNode?.OuterXml ?? "";
        
        public PackageItemViewModel(XmlNode packageNode)
        {
            PackageNode = packageNode;
            IsSelected = false;
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}




