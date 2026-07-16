using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace DigitalProductionConfigEditor.Views
{
    public class PinViewModel : INotifyPropertyChanged
    {
        private string _name = "";
        private string _testMode = "FORCEV";
        private string _vSet = "";
        private string _vSetHi = "";
        private string _vSetLo = "";
        private string _iSetOrSource = "";
        private string _iLevel = "";
        private string _highLimit = "";
        private string _lowLimit = "";

        public string Name          { get => _name;          set { _name = value;          OnPC(nameof(Name)); } }
        public string TestMode      { get => _testMode;      set { _testMode = value;      OnPC(nameof(TestMode)); } }
        public string VSet          { get => _vSet;          set { _vSet = value;          OnPC(nameof(VSet)); } }
        public string VSetHi        { get => _vSetHi;        set { _vSetHi = value;        OnPC(nameof(VSetHi)); } }
        public string VSetLo        { get => _vSetLo;        set { _vSetLo = value;        OnPC(nameof(VSetLo)); } }
        public string ISetOrSource  { get => _iSetOrSource;  set { _iSetOrSource = value;  OnPC(nameof(ISetOrSource)); } }
        public string ILevel        { get => _iLevel;        set { _iLevel = value;        OnPC(nameof(ILevel)); } }
        public string HighLimit     { get => _highLimit;     set { _highLimit = value;     OnPC(nameof(HighLimit)); } }
        public string LowLimit      { get => _lowLimit;      set { _lowLimit = value;      OnPC(nameof(LowLimit)); } }

        public List<string> TestModeOptions { get; } = new() { "FORCEV", "FORCEI" };

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPC(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public partial class EditDcContactModeDialog : Window
    {
        private readonly ObservableCollection<PinViewModel> _pins = new();

        public EditDcContactModeDialog(XmlNode dcNode)
        {
            InitializeComponent();
            LoadFromNode(dcNode);
            PinItemsControl.ItemsSource = _pins;
        }

        private void LoadFromNode(XmlNode dcNode)
        {
            // Load Delayms
            var delayNode = dcNode.SelectSingleNode("Delayms");
            DelaymsBox.Text = delayNode?.Attributes?["Value"]?.Value ?? "10";

            // Load Pin entries
            var pinNodes = dcNode.SelectNodes("Pin");
            if (pinNodes == null) return;

            foreach (XmlNode pin in pinNodes)
            {
                var vm = new PinViewModel
                {
                    Name         = pin.Attributes?["Name"]?.Value        ?? "",
                    TestMode     = pin.Attributes?["TestMode"]?.Value     ?? "FORCEV",
                    VSet         = pin.Attributes?["VSet"]?.Value         ?? "",
                    VSetHi       = pin.Attributes?["VSetHi"]?.Value       ?? "",
                    VSetLo       = pin.Attributes?["VSetLo"]?.Value       ?? "",
                    ISetOrSource = pin.Attributes?["ISet"]?.Value         ?? pin.Attributes?["ISource"]?.Value ?? "",
                    ILevel       = pin.Attributes?["ILevel"]?.Value       ?? "",
                    HighLimit    = pin.Attributes?["HighLimit"]?.Value    ?? "",
                    LowLimit     = pin.Attributes?["LowLimit"]?.Value     ?? "",
                };
                _pins.Add(vm);
            }
        }

        private void OnAddPinClick(object sender, RoutedEventArgs e)
        {
            _pins.Add(new PinViewModel());
        }

        private void OnDeletePinClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is PinViewModel pin)
            {
                var result = MessageBox.Show(
                    $"Delete pin entry '{pin.Name}'?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) _pins.Remove(pin);
            }
        }

        /// <summary>
        /// Replaces all Pin and Delayms children with the current dialog state.
        /// </summary>
        public void ApplyChangesToXml(XmlDocument xmlDoc, XmlNode dcNode)
        {
            // Remove existing Pin and Delayms children
            var toRemove = new List<XmlNode>();
            foreach (XmlNode child in dcNode.ChildNodes)
            {
                if (child.Name == "Pin" || child.Name == "Delayms")
                    toRemove.Add(child);
            }
            foreach (var n in toRemove) dcNode.RemoveChild(n);

            // Re-add Pin entries
            foreach (var vm in _pins)
            {
                XmlElement pinElem = xmlDoc.CreateElement("Pin");
                Set(pinElem, "Name", vm.Name);
                Set(pinElem, "TestMode", vm.TestMode);

                if (vm.TestMode == "FORCEV")
                {
                    SetIfNotEmpty(pinElem, "VSet", vm.VSet);
                    SetIfNotEmpty(pinElem, "ISet", vm.ISetOrSource);
                }
                else // FORCEI
                {
                    SetIfNotEmpty(pinElem, "VSetHi", vm.VSetHi);
                    SetIfNotEmpty(pinElem, "VSetLo", vm.VSetLo);
                    SetIfNotEmpty(pinElem, "ISource", vm.ISetOrSource);
                    SetIfNotEmpty(pinElem, "ILevel",  vm.ILevel);
                    SetIfNotEmpty(pinElem, "LowLimit", vm.LowLimit);
                }
                SetIfNotEmpty(pinElem, "HighLimit", vm.HighLimit);

                dcNode.AppendChild(pinElem);
            }

            // Re-add Delayms
            XmlElement delayElem = xmlDoc.CreateElement("Delayms");
            Set(delayElem, "Value", DelaymsBox.Text.Trim());
            dcNode.AppendChild(delayElem);
        }

        private static void Set(XmlElement elem, string attr, string value)
        {
            elem.SetAttribute(attr, value);
        }

        private static void SetIfNotEmpty(XmlElement elem, string attr, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                elem.SetAttribute(attr, value);
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            foreach (var pin in _pins)
            {
                if (string.IsNullOrWhiteSpace(pin.Name))
                {
                    MessageBox.Show("Every Pin entry must have a Name.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
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
    }
}
