using System.ComponentModel;
using System.Xml;

namespace DigitalProductionConfigEditor.ViewModels
{
    /// <summary>
    /// View model for a configuration node item with checkbox selection support
    /// </summary>
    public class ConfigNodeItemViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        
        public XmlNode ConfigNode { get; set; }
        
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
        
        public string NodeName => ConfigNode?.Name ?? "Unknown";
        
        public int ItemCount => ConfigNode?.ChildNodes.Count ?? 0;
        
        public ConfigNodeItemViewModel(XmlNode configNode)
        {
            ConfigNode = configNode;
            IsSelected = false;
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}




