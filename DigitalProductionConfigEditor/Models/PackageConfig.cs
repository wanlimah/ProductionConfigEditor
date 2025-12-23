using System.ComponentModel;

namespace DigitalProductionConfigEditor.Models
{
    public class PackageConfig : INotifyPropertyChanged
    {
        public string NodeName { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string Enable { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public string EnaMode { get; set; } = string.Empty;
        public string EnaAvgMode { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Sampling { get; set; }
        public int Threshold { get; set; }
        public string ProcessName { get; set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

