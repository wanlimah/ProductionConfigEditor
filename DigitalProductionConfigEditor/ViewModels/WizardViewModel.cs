using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace DigitalProductionConfigEditor.ViewModels
{
    public class AttributeViewModel : INotifyPropertyChanged
    {
        private string _value = "";
        private bool _isEditable = true;
        private string _name = "";

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    // Mark parent ViewModel as dirty if possible (requires reference, or simple logic here)
                    // Ideally AttributeViewModel should notify parent. For simplicity, we'll assume dirty management happens at higher level
                    // or we just rely on basic dirty flag in WizardViewModel being set manually when needed.
                }

                // If value is null or empty, just set it
                if (string.IsNullOrEmpty(value))
                {
                    _value = value ?? "";
                    OnPropertyChanged(nameof(Value));
                    return;
                }

                // Non-negative integer fields: reject decimals, minus sign, and letters (no digit-stripping)
                if (!string.IsNullOrEmpty(_name) && PackageNumericAttributes.IsNonNegativeInteger(_name))
                {
                    if (!Regex.IsMatch(value, "^[0-9]+$"))
                    {
                        OnPropertyChanged(nameof(Value));
                        return;
                    }
                    _value = value;
                }
                // Sublot field: auto-uppercase; dropdown limits to [1-3][A-E]
                else if (_name.Equals("Sublot", StringComparison.OrdinalIgnoreCase))
                {
                    _value = value.ToUpperInvariant();
                }
                else
                {
                    _value = value;
                }

                OnPropertyChanged(nameof(Value));
            }
        }

        private List<string>? _options;
        public List<string>? Options
        {
            get => _options;
            set
            {
                _options = value;
                OnPropertyChanged(nameof(Options));
                OnPropertyChanged(nameof(HasOptions));
            }
        }
        public bool HasOptions => _options != null && _options.Count > 0;
        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChanged(nameof(IsEditable));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class WizardViewModel : INotifyPropertyChanged
    {
        private XmlDocument _masterXmlDocument = new(); // Source/template XML
        private XmlDocument _newXmlDocument = new(); // User's new XML being built
        private XmlNode? _selectedPackageNode;
        private XmlNode? _selectedConfigurationNode;
        private ObservableCollection<AttributeViewModel> _attributes = new();
        private string _newXmlPath = "";
        private string _masterXmlPath = "";
        private string _masterXmlHash = "";  // To detect if Master XML was modified

        private bool _isDirty = false;
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                _isDirty = value;
                // Just track it, no UI notification needed usually
            }
        }

        // Master XML Path (public property)
        public string MasterXmlPath
        {
            get => _masterXmlPath;
            set
            {
                _masterXmlPath = value;
                OnPropertyChanged(nameof(MasterXmlPath));
            }
        }

        // Flag to track if we are in "Edit Master" mode
        private bool _isEditingMaster = false;
        public bool IsEditingMaster
        {
            get => _isEditingMaster;
            set
            {
                _isEditingMaster = value;
                OnPropertyChanged(nameof(IsEditingMaster));
            }
        }

        // Flag to track if Developer Mode is Unlocked (Persists for session)
        private bool _isDeveloperModeUnlocked = false;
        public bool IsDeveloperModeUnlocked
        {
            get => _isDeveloperModeUnlocked;
            set
            {
                _isDeveloperModeUnlocked = value;
                OnPropertyChanged(nameof(IsDeveloperModeUnlocked));
            }
        }

        // Master XML (Source/Template - Read Only)
        public XmlDocument MasterXmlDocument
        {
            get => _masterXmlDocument;
            set
            {
                _masterXmlDocument = value;
                OnPropertyChanged(nameof(MasterXmlDocument));
                OnPropertyChanged(nameof(MasterConfigurationNodes));
            }
        }

        // New XML (User's editable document)
        public XmlDocument NewXmlDocument
        {
            get => _newXmlDocument;
            set
            {
                _newXmlDocument = value;
                OnPropertyChanged(nameof(NewXmlDocument));
                OnPropertyChanged(nameof(NewConfigurationNodes));
            }
        }

        // Legacy property for backward compatibility
        public XmlDocument XmlDocument
        {
            get => _newXmlDocument;
            set
            {
                _newXmlDocument = value;
                OnPropertyChanged(nameof(XmlDocument));
                OnPropertyChanged(nameof(PackageNodes));
                OnPropertyChanged(nameof(ConfigurationNodes));
                OnPropertyChanged(nameof(NewConfigurationNodes));
            }
        }

        public XmlNodeList? PackageNodes => NewXmlDocument?.SelectNodes("//Package");

        public XmlNodeList? ConfigurationNodes => NewXmlDocument?.SelectNodes("//ProductionUserConfigs/*[Package]");

        // Master XML nodes (read-only source)
        public XmlNodeList? MasterConfigurationNodes => MasterXmlDocument?.SelectNodes("//ProductionUserConfigs/*[Package]");

        // New XML nodes (editable destination)
        public XmlNodeList? NewConfigurationNodes => NewXmlDocument?.SelectNodes("//ProductionUserConfigs/*[Package]");

        // DeveloperValidationConfig nodes
        public XmlNodeList? MasterDeveloperValidationNodes => MasterXmlDocument?.SelectNodes("//DeveloperValidationConfig/*[Package]");
        public XmlNodeList? NewDeveloperValidationNodes => NewXmlDocument?.SelectNodes("//DeveloperValidationConfig/*[Package]");

        // PcbFormatConfig nodes
        public XmlNode? MasterPcbFormatConfig => MasterXmlDocument?.SelectSingleNode("//PcbFormatConfig");
        public XmlNode? NewPcbFormatConfig => NewXmlDocument?.SelectSingleNode("//PcbFormatConfig");

        // InputValidationConfigs nodes
        public XmlNode? MasterInputValidationConfigs => MasterXmlDocument?.SelectSingleNode("//InputValidationConfigs");
        public XmlNode? NewInputValidationConfigs => NewXmlDocument?.SelectSingleNode("//InputValidationConfigs");

        // DC_CONTACT_MODE_SETTING_OVERWRITE nodes
        public XmlNode? MasterDcContactModeSettingOverwrite => MasterXmlDocument?.SelectSingleNode("//DC_CONTACT_MODE_SETTING_OVERWRITE");
        public XmlNode? NewDcContactModeSettingOverwrite => NewXmlDocument?.SelectSingleNode("//DC_CONTACT_MODE_SETTING_OVERWRITE");

        // MailConfig nodes
        public XmlNode? MasterMailConfig => MasterXmlDocument?.SelectSingleNode("//MailConfig");
        public XmlNode? NewMailConfig => NewXmlDocument?.SelectSingleNode("//MailConfig");

        // Combined list of all editable configuration nodes (Production + Developer Validation)
        public ObservableCollection<XmlNode> AllEditableConfigurationNodes
        {
            get
            {
                var nodes = new ObservableCollection<XmlNode>();

                // Add Production User Configs
                if (NewConfigurationNodes != null)
                {
                    foreach (XmlNode node in NewConfigurationNodes)
                    {
                        nodes.Add(node);
                    }
                }

                // Add Developer Validation Configs
                if (NewDeveloperValidationNodes != null)
                {
                    foreach (XmlNode node in NewDeveloperValidationNodes)
                    {
                        nodes.Add(node);
                    }
                }

                return nodes;
            }
        }

        // Configuration node items with checkbox selection support for Step 1
        private ObservableCollection<ConfigNodeItemViewModel> _masterConfigNodeItems = new();
        private ObservableCollection<ConfigNodeItemViewModel> _masterDevValidationNodeItems = new();

        public ObservableCollection<ConfigNodeItemViewModel> MasterConfigNodeItems
        {
            get => _masterConfigNodeItems;
            set
            {
                _masterConfigNodeItems = value;
                OnPropertyChanged(nameof(MasterConfigNodeItems));
            }
        }

        public ObservableCollection<ConfigNodeItemViewModel> MasterDevValidationNodeItems
        {
            get => _masterDevValidationNodeItems;
            set
            {
                _masterDevValidationNodeItems = value;
                OnPropertyChanged(nameof(MasterDevValidationNodeItems));
            }
        }

        // Refresh configuration node items for Step 1
        public void RefreshMasterConfigNodeItems()
        {
            MasterConfigNodeItems.Clear();
            if (MasterConfigurationNodes != null)
            {
                foreach (XmlNode node in MasterConfigurationNodes)
                {
                    MasterConfigNodeItems.Add(new ConfigNodeItemViewModel(node));
                }
            }
        }

        public void RefreshMasterDevValidationNodeItems()
        {
            MasterDevValidationNodeItems.Clear();
            if (MasterDeveloperValidationNodes != null)
            {
                foreach (XmlNode node in MasterDeveloperValidationNodes)
                {
                    MasterDevValidationNodeItems.Add(new ConfigNodeItemViewModel(node));
                }
            }
        }

        // Get selected master configuration nodes
        public List<XmlNode> GetSelectedMasterConfigNodes()
        {
            return MasterConfigNodeItems
                .Where(n => n.IsSelected)
                .Select(n => n.ConfigNode)
                .ToList();
        }

        // Get selected master developer validation nodes
        public List<XmlNode> GetSelectedMasterDevValidationNodes()
        {
            return MasterDevValidationNodeItems
                .Where(n => n.IsSelected)
                .Select(n => n.ConfigNode)
                .ToList();
        }

        // Select all master configuration nodes
        public void SelectAllMasterConfigNodes()
        {
            foreach (var node in MasterConfigNodeItems)
            {
                node.IsSelected = true;
            }
        }

        // Deselect all master configuration nodes
        public void DeselectAllMasterConfigNodes()
        {
            foreach (var node in MasterConfigNodeItems)
            {
                node.IsSelected = false;
            }
        }

        // Select all master developer validation nodes
        public void SelectAllMasterDevValidationNodes()
        {
            foreach (var node in MasterDevValidationNodeItems)
            {
                node.IsSelected = true;
            }
        }

        // Deselect all master developer validation nodes
        public void DeselectAllMasterDevValidationNodes()
        {
            foreach (var node in MasterDevValidationNodeItems)
            {
                node.IsSelected = false;
            }
        }

        // Add multiple selected configuration nodes from master
        public void AddSelectedMasterConfigNodes()
        {
            var selectedNodes = GetSelectedMasterConfigNodes();

            if (selectedNodes.Count == 0)
            {
                StatusMessage = "No configuration nodes selected";
                return;
            }

            int addedCount = 0;
            int skippedCount = 0;

            foreach (var node in selectedNodes)
            {
                // Check if this configuration already exists in NewXml
                var nodeName = node.Name;
                var existingNode = NewXmlDocument.SelectSingleNode($"//ProductionUserConfigs/{nodeName}");

                if (existingNode != null)
                {
                    skippedCount++;
                    continue;
                }

                // Find the ProductionUserConfigs parent node
                var configsNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfigs");
                if (configsNode == null)
                {
                    StatusMessage = "Error: ProductionUserConfigs node not found";
                    return;
                }

                // Import the node (deep copy) into the new document
                XmlNode importedNode = NewXmlDocument.ImportNode(node, true);

                // Strip editor-only metadata so they never reach Clotho
                StripEditorMetadata(importedNode);

                // 🔒 SAFETY: Set all package enable attributes to FALSE for new configurations
                var packages = importedNode.SelectNodes("Package");
                if (packages != null)
                {
                    foreach (XmlNode package in packages)
                    {
                        var enableAttr = package.Attributes?["enable"];
                        if (enableAttr != null)
                        {
                            enableAttr.Value = "FALSE";  // Default to FALSE for safety
                        }
                    }
                }

                configsNode.AppendChild(importedNode);
                addedCount++;
            }

            // Deselect all after adding
            DeselectAllMasterConfigNodes();

            // Refresh the view
            OnPropertyChanged(nameof(NewConfigurationNodes));
            OnPropertyChanged(nameof(ConfigurationNodes));
            OnPropertyChanged(nameof(AllEditableConfigurationNodes));

            if (addedCount > 0 && skippedCount > 0)
            {
                StatusMessage = $"Added {addedCount} configuration(s) with enable=FALSE for safety, skipped {skippedCount} (already exists)";
            }
            else if (addedCount > 0)
            {
                StatusMessage = $"Added {addedCount} configuration(s) with enable=FALSE for safety";
            }
            else
            {
                StatusMessage = $"All {skippedCount} selected configuration(s) already exist in your XML";
            }
        }

        // Add multiple selected developer validation nodes from master
        public void AddSelectedMasterDevValidationNodes()
        {
            var selectedNodes = GetSelectedMasterDevValidationNodes();

            if (selectedNodes.Count == 0)
            {
                StatusMessage = "No developer validation nodes selected";
                return;
            }

            int addedCount = 0;
            int skippedCount = 0;

            // Find or create the DeveloperValidationConfig parent node
            var configsNode = NewXmlDocument.SelectSingleNode("//DeveloperValidationConfig");
            if (configsNode == null)
            {
                // Create DeveloperValidationConfig section if it doesn't exist
                var rootNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfig");
                if (rootNode == null)
                {
                    StatusMessage = "Error: ProductionUserConfig root node not found";
                    return;
                }

                configsNode = NewXmlDocument.CreateElement("DeveloperValidationConfig");
                rootNode.AppendChild(configsNode);
            }

            foreach (var node in selectedNodes)
            {
                // Check if this configuration already exists in NewXml
                var nodeName = node.Name;
                var existingNode = NewXmlDocument.SelectSingleNode($"//DeveloperValidationConfig/{nodeName}");

                if (existingNode != null)
                {
                    skippedCount++;
                    continue;
                }

                // Import the node (deep copy) into the new document
                XmlNode importedNode = NewXmlDocument.ImportNode(node, true);

                // Strip editor-only metadata so they never reach Clotho
                StripEditorMetadata(importedNode);

                // 🔒 SAFETY: Set all package enable attributes to FALSE for new Developer Validation configs
                var packages = importedNode.SelectNodes("Package");
                if (packages != null)
                {
                    foreach (XmlNode package in packages)
                    {
                        var enableAttr = package.Attributes?["enable"];
                        if (enableAttr != null)
                        {
                            enableAttr.Value = "FALSE";  // Default to FALSE for safety
                        }
                    }
                }

                configsNode.AppendChild(importedNode);
                addedCount++;
            }

            // Deselect all after adding
            DeselectAllMasterDevValidationNodes();

            // Refresh the view
            OnPropertyChanged(nameof(NewDeveloperValidationNodes));
            OnPropertyChanged(nameof(AllEditableConfigurationNodes));

            if (addedCount > 0 && skippedCount > 0)
            {
                StatusMessage = $"Added {addedCount} developer validation config(s) with enable=FALSE for safety, skipped {skippedCount} (already exists)";
            }
            else if (addedCount > 0)
            {
                StatusMessage = $"Added {addedCount} developer validation config(s) with enable=FALSE for safety";
            }
            else
            {
                StatusMessage = $"All {skippedCount} selected developer validation config(s) already exist in your XML";
            }
        }

        public XmlNode? SelectedConfigurationNode
        {
            get => _selectedConfigurationNode;
            set
            {
                _selectedConfigurationNode = value;
                OnPropertyChanged(nameof(SelectedConfigurationNode));
                OnPropertyChanged(nameof(SelectedConfigurationName));
                OnPropertyChanged(nameof(PackagesInSelectedConfiguration));
            }
        }

        public string SelectedConfigurationName => SelectedConfigurationNode?.Name ?? "None";

        public ObservableCollection<XmlNode> PackagesInSelectedConfiguration
        {
            get
            {
                var packages = new ObservableCollection<XmlNode>();
                if (SelectedConfigurationNode != null)
                {
                    var packageNodes = SelectedConfigurationNode.SelectNodes("Package");
                    if (packageNodes != null)
                    {
                        foreach (XmlNode node in packageNodes)
                        {
                            packages.Add(node);
                        }
                    }
                }
                return packages;
            }
        }

        // Package items with checkbox selection support
        private ObservableCollection<PackageItemViewModel> _packageItems = new();

        public ObservableCollection<PackageItemViewModel> PackageItems
        {
            get => _packageItems;
            set
            {
                _packageItems = value;
                OnPropertyChanged(nameof(PackageItems));
            }
        }

        // Refresh package items when configuration changes
        public void RefreshPackageItems()
        {
            PackageItems.Clear();
            if (SelectedConfigurationNode != null)
            {
                var packageNodes = SelectedConfigurationNode.SelectNodes("Package");
                if (packageNodes != null)
                {
                    foreach (XmlNode node in packageNodes)
                    {
                        PackageItems.Add(new PackageItemViewModel(node));
                    }
                }
            }
        }

        // Get all selected packages
        public List<XmlNode> GetSelectedPackages()
        {
            return PackageItems
                .Where(p => p.IsSelected)
                .Select(p => p.PackageNode)
                .ToList();
        }

        // Select all packages
        public void SelectAllPackages()
        {
            foreach (var package in PackageItems)
            {
                package.IsSelected = true;
            }
        }

        // Deselect all packages
        public void DeselectAllPackages()
        {
            foreach (var package in PackageItems)
            {
                package.IsSelected = false;
            }
        }

        // Delete multiple packages
        public void DeleteSelectedPackages()
        {
            var selectedPackages = GetSelectedPackages().ToList();

            if (selectedPackages.Count == 0)
            {
                StatusMessage = "No packages selected for deletion";
                return;
            }

            try
            {
                foreach (var packageNode in selectedPackages)
                {
                    var parentNode = packageNode.ParentNode;
                    parentNode?.RemoveChild(packageNode);

                    // Clear selection if the deleted node was selected
                    if (SelectedPackageNode == packageNode)
                    {
                        SelectedPackageNode = null;
                    }
                }

                // Refresh the views
                RefreshPackageItems();
                OnPropertyChanged(nameof(PackagesInSelectedConfiguration));
                OnPropertyChanged(nameof(PackageNodes));

                StatusMessage = $"Deleted {selectedPackages.Count} package(s) successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to delete packages: {ex.Message}";
            }
        }

        public ObservableCollection<AttributeViewModel> Attributes
        {
            get => _attributes;
            set
            {
                _attributes = value;
                OnPropertyChanged(nameof(Attributes));
            }
        }

        public XmlNode? SelectedPackageNode
        {
            get => _selectedPackageNode;
            set
            {
                _selectedPackageNode = value;
                LoadAttributesFromSelectedNode();
                OnPropertyChanged(nameof(SelectedPackageNode));
                OnPropertyChanged(nameof(StatusMessage));
                OnPropertyChanged(nameof(SelectedNodeName));
                OnPropertyChanged(nameof(SelectedPackageName));
                OnPropertyChanged(nameof(SelectedEnable));
                OnPropertyChanged(nameof(SelectedMode));
                OnPropertyChanged(nameof(SelectedCount));
                OnPropertyChanged(nameof(SelectedSampling));
                OnPropertyChanged(nameof(SelectedThreshold));
                OnPropertyChanged(nameof(SelectedAvgChannel));
            }
        }

        private void LoadAttributesFromSelectedNode()
        {
            Attributes.Clear();

            if (SelectedPackageNode?.Attributes == null) return;

            var parentNode = SelectedPackageNode.ParentNode;

            // Collect all attribute names already in the package
            var loadedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (XmlAttribute attr in SelectedPackageNode.Attributes)
            {
                if (attr.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                    continue;

                var attrViewModel = new AttributeViewModel
                {
                    Name  = attr.Name,
                    Value = attr.Value
                };

                var options = GetOptionsForAttribute(attr.Name, parentNode);
                if (options != null && options.Count > 0)
                    attrViewModel.Options = options;

                Attributes.Add(attrViewModel);
                loadedNames.Add(attr.Name);
            }

            // Merge any attributes that exist in the master's equivalent package but are missing here.
            // This ensures attributes added to the master later (e.g. Sublot) always appear for editing.
            if (parentNode != null)
            {
                var masterConfig = MasterXmlDocument?.SelectSingleNode($"//{parentNode.Name}");
                var masterFirstPackage = masterConfig?.SelectSingleNode("Package");
                if (masterFirstPackage?.Attributes != null)
                {
                    foreach (XmlAttribute masterAttr in masterFirstPackage.Attributes)
                    {
                        if (masterAttr.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                            continue;
                        if (loadedNames.Contains(masterAttr.Name))
                            continue;

                        // Attribute exists in master but not in user's package — add with empty value
                        var attrViewModel = new AttributeViewModel
                        {
                            Name  = masterAttr.Name,
                            Value = ""
                        };

                        var options = GetOptionsForAttribute(masterAttr.Name, parentNode);
                        if (options != null && options.Count > 0)
                            attrViewModel.Options = options;

                        Attributes.Add(attrViewModel);
                    }
                }
            }
        }

        private List<string>? GetOptionsForAttribute(string attributeName, XmlNode? parentNode)
        {
            // Generate all valid Sublot values: 1A-1E, 2A-2E, 3A-3E
            var sublotOptions = Enumerable.Range(1, 3)
                .SelectMany(n => new[] { "A", "B", "C", "D", "E" }.Select(l => $"{n}{l}"))
                .ToList();

            // Default options for common attributes
            var defaultOptions = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "enable", new List<string> { "TRUE", "FALSE" } },
                { "viewer", new List<string> { "true", "false" } },
                { "Sublot", sublotOptions }
            };

            // Map attribute names to their option node names in XML
            var optionNodeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "enable", "EnableOptions" },
                { "mode", "ModeOptions" },
                { "rule", "RuleOptions" },
                { "avg_channel", "AvgChannelOptions" }
            };

            // Try to get options from XML first
            if (parentNode != null && optionNodeMap.ContainsKey(attributeName))
            {
                var optionNodeName = optionNodeMap[attributeName];
                var optionNode = parentNode.SelectSingleNode(optionNodeName);

                if (optionNode?.InnerText != null)
                {
                    // Parse options (format: "OPTION1 | OPTION2 | OPTION3")
                    var options = optionNode.InnerText
                        .Split('|')
                        .Select(o => o.Trim())
                        .Where(o => !string.IsNullOrWhiteSpace(o))
                        .ToList();

                    if (options.Count > 0) return options;
                }
            }

            // Fall back to default options if available
            if (defaultOptions.ContainsKey(attributeName))
            {
                return defaultOptions[attributeName];
            }

            return null;
        }

        public void SaveAttributesToNode()
        {
            if (SelectedPackageNode?.Attributes == null) return;

            foreach (var attr in Attributes)
            {
                var existing = SelectedPackageNode.Attributes[attr.Name];
                if (existing != null)
                {
                    // Update existing attribute
                    existing.Value = attr.Value;
                }
                else
                {
                    // Create new attribute (e.g. Sublot added from master merge)
                    var xmlDoc = SelectedPackageNode.OwnerDocument;
                    if (xmlDoc != null)
                    {
                        var newAttr = xmlDoc.CreateAttribute(attr.Name);
                        newAttr.Value = attr.Value;
                        SelectedPackageNode.Attributes.Append(newAttr);
                    }
                }
            }
        }

        // Properties for binding to selected package attributes
        public string SelectedNodeName => SelectedPackageNode?.ParentNode?.Name ?? "No Selection";

        public string SelectedPackageName
        {
            get => SelectedPackageNode?.Attributes?["name"]?.Value ?? "";
            set
            {
                if (SelectedPackageNode?.Attributes?["name"] != null)
                {
                    SelectedPackageNode.Attributes["name"]!.Value = value;
                    OnPropertyChanged(nameof(SelectedPackageName));
                }
            }
        }

        public string SelectedEnable
        {
            get => SelectedPackageNode?.Attributes?["enable"]?.Value ?? "";
            set
            {
                if (SelectedPackageNode?.Attributes?["enable"] != null)
                {
                    SelectedPackageNode.Attributes["enable"]!.Value = value;
                    OnPropertyChanged(nameof(SelectedEnable));
                }
            }
        }

        public string SelectedMode
        {
            get => SelectedPackageNode?.Attributes?["mode"]?.Value ?? "";
            set
            {
                if (SelectedPackageNode?.Attributes?["mode"] != null)
                {
                    SelectedPackageNode.Attributes["mode"]!.Value = value;
                    OnPropertyChanged(nameof(SelectedMode));
                }
            }
        }

        public string SelectedCount
        {
            get => SelectedPackageNode?.Attributes?["count"]?.Value ?? "";
            set
            {
                if (SelectedPackageNode?.Attributes?["count"] != null)
                {
                    SelectedPackageNode.Attributes["count"]!.Value = value;
                    OnPropertyChanged(nameof(SelectedCount));
                }
            }
        }

        public string SelectedSampling
        {
            get => SelectedPackageNode?.Attributes?["sampling"]?.Value ?? "";
            set
            {
                if (SelectedPackageNode?.Attributes?["sampling"] != null)
                {
                    SelectedPackageNode.Attributes["sampling"]!.Value = value;
                    OnPropertyChanged(nameof(SelectedSampling));
                }
            }
        }

        public string SelectedThreshold
        {
            get => SelectedPackageNode?.Attributes?["threshold"]?.Value ?? "";
            set
            {
                if (SelectedPackageNode?.Attributes?["threshold"] != null)
                {
                    SelectedPackageNode.Attributes["threshold"]!.Value = value;
                    OnPropertyChanged(nameof(SelectedThreshold));
                }
            }
        }

        public string SelectedAvgChannel
        {
            get => SelectedPackageNode?.Attributes?["avg_channel"]?.Value ?? "";
            set
            {
                if (SelectedPackageNode?.Attributes?["avg_channel"] != null)
                {
                    SelectedPackageNode.Attributes["avg_channel"]!.Value = value;
                    OnPropertyChanged(nameof(SelectedAvgChannel));
                }
            }
        }

        private string _statusMessage = "No package selected";
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public int CurrentStep { get; set; } = 1;

        // Options for ComboBoxes
        public List<string> EnableOptions => new() { "TRUE", "FALSE" };
        public List<string> ModeOptions => new() { "AUTO", "MANUAL" };
        public List<string> AvgChannelOptions => new() { "ALL", "EACH" };

        private string CalculateFileHash(string filePath)
        {
            try
            {
                using var sha256 = System.Security.Cryptography.SHA256.Create();
                using var stream = System.IO.File.OpenRead(filePath);
                var hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "");
            }
            catch
            {
                return "";
            }
        }

        public bool VerifyMasterXmlIntegrity()
        {
            if (string.IsNullOrEmpty(_masterXmlPath) || string.IsNullOrEmpty(_masterXmlHash))
                return true; // Can't verify, assume OK

            try
            {
                var currentHash = CalculateFileHash(_masterXmlPath);
                if (currentHash != _masterXmlHash)
                {
                    // Master XML was modified!
                    var result = MessageBox.Show(
                        "⚠️ WARNING: Master XML Template Has Been Modified!\n\n" +
                        "The Master_ProductionUserConfig.xml file has been changed outside the application.\n\n" +
                        "This could cause unexpected behavior or errors.\n\n" +
                        "Recommended actions:\n" +
                        "1. Restore from backup: Master_ProductionUserConfig.BACKUP.xml\n" +
                        "2. Reload the application to use the modified version\n\n" +
                        "Would you like to reload the Master XML now?",
                        "Master XML Modified",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        LoadMasterXml(_masterXmlPath);
                    }
                    return false;
                }
                return true;
            }
            catch
            {
                return true; // Can't verify, assume OK
            }
        }

        public void SetMasterReadOnly(bool isReadOnly)
        {
            if (string.IsNullOrEmpty(_masterXmlPath)) return;
            try
            {
                var fileInfo = new System.IO.FileInfo(_masterXmlPath);
                if (fileInfo.Exists)
                {
                    fileInfo.IsReadOnly = isReadOnly;
                    System.Diagnostics.Debug.WriteLine($"Set Master XML read-only status to {isReadOnly}: {_masterXmlPath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Warning: Could not set Master XML read-only status: {ex.Message}");
            }
        }

        public void LoadMasterXml(string path)
        {
            try
            {
                // Store path for monitoring
                _masterXmlPath = path;
                MasterXmlPath = path; // Also set public property for Advanced Settings

                MasterXmlDocument = new XmlDocument();
                MasterXmlDocument.Load(path);

                // 🔒 PROTECTION 1: Calculate hash to detect unauthorized modifications
                _masterXmlHash = CalculateFileHash(path);
                System.Diagnostics.Debug.WriteLine($"Master XML hash: {_masterXmlHash}");

                // 🔒 PROTECTION 2: Set Master XML file to read-only to prevent accidental modification
                try
                {
                    // Skip locking if we are running in Debug mode/folder to avoid build issues
                    if (path.Contains("bin\\Debug") || path.Contains("bin/Debug"))
                    {
                        System.Diagnostics.Debug.WriteLine($"Skipping Read-Only lock for Debug build: {path}");
                    }
                    else
                    {
                        var fileInfo = new System.IO.FileInfo(path);
                        if (!fileInfo.IsReadOnly)
                        {
                            fileInfo.IsReadOnly = true;
                            System.Diagnostics.Debug.WriteLine($"Set Master XML to read-only: {path}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Could not set Master XML to read-only: {ex.Message}");
                    // Don't fail if we can't set read-only - just continue
                }

                // Initialize master config node items for checkbox selection
                RefreshMasterConfigNodeItems();
                RefreshMasterDevValidationNodeItems();

                // Notify developer if any elements are missing their Google Sheet link
                ValidateMasterDocRanges();

                StatusMessage = $"Loaded Master XML with {MasterConfigurationNodes?.Count} configuration nodes available (read-only, protected).";

                // Debug information
                System.Diagnostics.Debug.WriteLine($"Master XML loaded successfully. Found {MasterConfigurationNodes?.Count} configuration nodes.");
                if (MasterConfigurationNodes != null)
                {
                    foreach (XmlNode node in MasterConfigurationNodes)
                    {
                        var packageCount = node.SelectNodes("Package")?.Count ?? 0;
                        System.Diagnostics.Debug.WriteLine($"Master Configuration: {node.Name} - {packageCount} packages");
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to load Master XML: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Master XML loading error: {ex}");
                throw;
            }
        }

        public void CreateNewConfigurationNode(string nodeName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
            {
                StatusMessage = "Configuration name cannot be empty";
                return;
            }

            // Clean up name (remove spaces, special chars)
            nodeName = System.Text.RegularExpressions.Regex.Replace(nodeName, @"[^a-zA-Z0-9_]", "_").ToUpper();

            try
            {
                // Check if exists
                var existingNode = NewXmlDocument.SelectSingleNode($"//ProductionUserConfigs/{nodeName}");
                if (existingNode != null)
                {
                    StatusMessage = $"Configuration '{nodeName}' already exists";
                    return;
                }

                var configsNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfigs");
                if (configsNode == null)
                {
                    StatusMessage = "Error: ProductionUserConfigs node not found";
                    return;
                }

                // Create the new node structure
                XmlElement newNode = NewXmlDocument.CreateElement(nodeName);

                // Add a default Package to make it usable
                XmlElement defaultPackage = NewXmlDocument.CreateElement("Package");
                defaultPackage.SetAttribute("name", "SUSER");
                defaultPackage.SetAttribute("enable", "FALSE");

                newNode.AppendChild(defaultPackage);
                configsNode.AppendChild(newNode);

                // Select it and refresh view
                RefreshMasterConfigNodeItems();
                OnPropertyChanged(nameof(NewConfigurationNodes));
                OnPropertyChanged(nameof(ConfigurationNodes));
                OnPropertyChanged(nameof(AllEditableConfigurationNodes));

                // Auto-select
                SelectedConfigurationNode = newNode;

                StatusMessage = $"Created new configuration '{nodeName}'";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to create configuration: {ex.Message}";
            }
        }

        public void CreateNewBlankXml()
        {
            try
            {
                IsEditingMaster = false; // Reset Edit Master flag
                NewXmlDocument = new XmlDocument();

                // Add XML declaration
                XmlDeclaration xmlDeclaration = NewXmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
                NewXmlDocument.AppendChild(xmlDeclaration);

                // Create root elements
                XmlElement rootElement = NewXmlDocument.CreateElement("ProductionUserConfig");
                NewXmlDocument.AppendChild(rootElement);

                XmlElement configsElement = NewXmlDocument.CreateElement("ProductionUserConfigs");
                XmlAttribute viewerAttr = NewXmlDocument.CreateAttribute("viewer");
                viewerAttr.Value = "false";
                configsElement.Attributes.Append(viewerAttr);
                rootElement.AppendChild(configsElement);

                // Add comment
                XmlComment comment = NewXmlDocument.CreateComment(@" 
				Common Instructions:			
				1. 'name' is the package name. 
					- 'SUSER' refers to a user mode where Clotho is executed by the user in SUSER mode.
				2. 'enable' determines whether the package is active (""TRUE"" for active, ""FALSE"" for inactive).
	            		3. The default value for enable is FALSE for all items, except for MQTT_ENABLE.
			");
                configsElement.AppendChild(comment);

                OnPropertyChanged(nameof(NewXmlDocument));
                OnPropertyChanged(nameof(NewConfigurationNodes));
                OnPropertyChanged(nameof(ConfigurationNodes));
                OnPropertyChanged(nameof(NewDeveloperValidationNodes));
                OnPropertyChanged(nameof(NewPcbFormatConfig));
                OnPropertyChanged(nameof(NewInputValidationConfigs));
                OnPropertyChanged(nameof(NewDcContactModeSettingOverwrite));
                OnPropertyChanged(nameof(NewMailConfig));
                OnPropertyChanged(nameof(AllEditableConfigurationNodes));

                StatusMessage = "Created new blank XML configuration. Add configuration nodes from the Master list.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to create blank XML: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Blank XML creation error: {ex}");
            }
        }

        public void LoadNewXml(string path)
        {
            try
            {
                IsEditingMaster = false; // Reset flag by default (override if needed by caller)
                NewXmlDocument = new XmlDocument();
                NewXmlDocument.Load(path);
                _newXmlPath = path;

                // Initialize with first configuration node
                var firstConfigNode = AllEditableConfigurationNodes.FirstOrDefault();
                SelectedConfigurationNode = firstConfigNode;

                OnPropertyChanged(nameof(AllEditableConfigurationNodes));

                StatusMessage = $"Loaded XML with {NewConfigurationNodes?.Count} production config(s) and {NewDeveloperValidationNodes?.Count} developer validation config(s).";

                // Debug information
                System.Diagnostics.Debug.WriteLine($"New XML loaded successfully. Found {NewConfigurationNodes?.Count} configuration nodes.");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to load XML: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"XML loading error: {ex}");
                throw;
            }
        }

        // Legacy method for backward compatibility
        public void LoadXml(string path)
        {
            LoadNewXml(path);
        }

        // Copy a configuration node from Master XML to New XML
        public void CopyConfigurationNodeFromMaster(XmlNode masterNode)
        {
            // 🔒 PROTECTION: Verify Master XML hasn't been tampered with
            if (!VerifyMasterXmlIntegrity())
            {
                StatusMessage = "Master XML integrity check failed - please review the warning";
                return;
            }

            if (masterNode == null)
            {
                StatusMessage = "No configuration node selected to copy";
                return;
            }

            try
            {
                // Check if this configuration already exists in NewXml
                var nodeName = masterNode.Name;
                var existingNode = NewXmlDocument.SelectSingleNode($"//ProductionUserConfigs/{nodeName}");

                if (existingNode != null)
                {
                    StatusMessage = $"Configuration '{nodeName}' already exists in your new XML";
                    return;
                }

                // Find the ProductionUserConfigs parent node
                var configsNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfigs");
                if (configsNode == null)
                {
                    StatusMessage = "Error: ProductionUserConfigs node not found";
                    return;
                }

                // Import the node (deep copy) into the new document
                XmlNode importedNode = NewXmlDocument.ImportNode(masterNode, true);

                // Strip editor-only metadata so they never reach Clotho
                StripEditorMetadata(importedNode);

                // SAFETY: Set all package enable attributes to FALSE for new configurations
                var packages = importedNode.SelectNodes("Package");
                if (packages != null)
                {
                    foreach (XmlNode package in packages)
                    {
                        var enableAttr = package.Attributes?["enable"];
                        if (enableAttr != null)
                        {
                            enableAttr.Value = "FALSE";  // Default to FALSE for safety
                        }
                    }
                }

                configsNode.AppendChild(importedNode);

                // Automatically select the newly added configuration
                SelectedConfigurationNode = importedNode;

                // Refresh the view
                OnPropertyChanged(nameof(NewConfigurationNodes));
                OnPropertyChanged(nameof(ConfigurationNodes));
                OnPropertyChanged(nameof(AllEditableConfigurationNodes));

                var packageCount = importedNode.SelectNodes("Package")?.Count ?? 0;
                StatusMessage = $"Added configuration '{nodeName}' with {packageCount} package(s) to your new XML (all enable=FALSE for safety)";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to copy configuration: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Copy configuration error: {ex}");
            }
        }

        // ── Google Sheet documentation validation ──────────────────────────────

        private string _missingDocRangeWarning = "";

        /// <summary>
        /// Non-empty when the root docUrl is configured but one or more elements
        /// are missing a docRange — prompts the developer to update the Google Sheet.
        /// </summary>
        public string MissingDocRangeWarning
        {
            get => _missingDocRangeWarning;
            private set
            {
                _missingDocRangeWarning = value;
                OnPropertyChanged(nameof(MissingDocRangeWarning));
                OnPropertyChanged(nameof(HasMissingDocRangeWarning));
            }
        }

        public bool HasMissingDocRangeWarning => !string.IsNullOrEmpty(_missingDocRangeWarning);

        /// <summary>
        /// Scans every element in the master XML.  If docUrl is set on the root but any
        /// element is missing docRange, builds a warning message listing the offenders.
        /// Called automatically after the master XML loads.
        /// </summary>
        private void ValidateMasterDocRanges()
        {
            var rootDocUrl = MasterXmlDocument?.DocumentElement?.Attributes?["docUrl"]?.Value ?? "";
            if (string.IsNullOrWhiteSpace(rootDocUrl))
            {
                MissingDocRangeWarning = "";
                return;
            }

            var missing = new List<string>();

            // Only Production User Config elements need Google Sheet documentation.
            // Developer Validation Config is TDG-internal and requires no guide.
            if (MasterConfigurationNodes != null)
                foreach (XmlNode node in MasterConfigurationNodes)
                    if (string.IsNullOrWhiteSpace(node.Attributes?["docRange"]?.Value))
                        missing.Add(node.Name);

            MissingDocRangeWarning = missing.Count > 0
                ? $"⚠️ {missing.Count} element(s) have no Google Sheet link:\n{string.Join(", ", missing)}\n\nAction required: add the row to the Google Sheet, create a Named Range using the element name, then set docRange in Master_ProductionUserConfig.xml."
                : "";
        }

        // ───────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Removes editor-only metadata attributes (supportedTests, docRange) from a node
        /// before it is written to the user's output XML, so Clotho never sees them.
        /// </summary>
        private static void StripEditorMetadata(XmlNode node)
        {
            if (node?.Attributes == null) return;
            foreach (var name in new[] { "supportedTests", "docRange" })
            {
                var attr = node.Attributes[name];
                if (attr != null) node.Attributes.Remove(attr);
            }
        }

        // Delete a configuration node from New XML
        public void DeleteConfigurationNode(XmlNode configNode)
        {
            if (configNode == null)
            {
                StatusMessage = "No configuration node selected to delete";
                return;
            }

            try
            {
                var nodeName = configNode.Name;
                var parentNode = configNode.ParentNode;

                parentNode?.RemoveChild(configNode);

                // Clear selection if the deleted node was selected
                if (SelectedConfigurationNode == configNode)
                {
                    SelectedConfigurationNode = null;
                }

                // Refresh the view
                OnPropertyChanged(nameof(NewConfigurationNodes));
                OnPropertyChanged(nameof(ConfigurationNodes));
                OnPropertyChanged(nameof(NewDeveloperValidationNodes));
                OnPropertyChanged(nameof(AllEditableConfigurationNodes));

                StatusMessage = $"Deleted configuration '{nodeName}' from your XML";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to delete configuration: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Delete configuration error: {ex}");
            }
        }

        // Copy a DeveloperValidationConfig node from Master XML to New XML
        public void CopyDeveloperValidationNodeFromMaster(XmlNode masterNode)
        {
            if (masterNode == null)
            {
                StatusMessage = "No configuration node selected to copy";
                return;
            }

            try
            {
                // Check if this configuration already exists in NewXml
                var nodeName = masterNode.Name;
                var existingNode = NewXmlDocument.SelectSingleNode($"//DeveloperValidationConfig/{nodeName}");

                if (existingNode != null)
                {
                    StatusMessage = $"Configuration '{nodeName}' already exists in your Developer Validation Config";
                    return;
                }

                // Find or create the DeveloperValidationConfig parent node
                var configsNode = NewXmlDocument.SelectSingleNode("//DeveloperValidationConfig");
                if (configsNode == null)
                {
                    // Create DeveloperValidationConfig section if it doesn't exist
                    var rootNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfig");
                    if (rootNode == null)
                    {
                        StatusMessage = "Error: ProductionUserConfig root node not found";
                        return;
                    }

                    configsNode = NewXmlDocument.CreateElement("DeveloperValidationConfig");
                    rootNode.AppendChild(configsNode);
                }

                // Import the node (deep copy) into the new document
                XmlNode importedNode = NewXmlDocument.ImportNode(masterNode, true);

                // Strip editor-only metadata so they never reach Clotho
                StripEditorMetadata(importedNode);

                // 🔒 SAFETY: Set all package enable attributes to FALSE for new Developer Validation configs
                var packages = importedNode.SelectNodes("Package");
                if (packages != null)
                {
                    foreach (XmlNode package in packages)
                    {
                        var enableAttr = package.Attributes?["enable"];
                        if (enableAttr != null)
                        {
                            enableAttr.Value = "FALSE";  // Default to FALSE for safety
                        }
                    }
                }

                configsNode.AppendChild(importedNode);

                // Automatically select the newly added configuration
                SelectedConfigurationNode = importedNode;

                // Refresh the view
                OnPropertyChanged(nameof(NewDeveloperValidationNodes));
                OnPropertyChanged(nameof(AllEditableConfigurationNodes));

                var packageCount = importedNode.SelectNodes("Package")?.Count ?? 0;
                StatusMessage = $"Added Developer Validation config '{nodeName}' with {packageCount} package(s) to your new XML (all enable=FALSE for safety)";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to copy Developer Validation configuration: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Copy Developer Validation configuration error: {ex}");
            }
        }

        // Copy entire PcbFormatConfig from Master XML to New XML
        public void CopyPcbFormatConfigFromMaster()
        {
            if (MasterPcbFormatConfig == null)
            {
                StatusMessage = "No PcbFormatConfig found in Master XML";
                return;
            }

            try
            {
                // Check if PcbFormatConfig already exists in NewXml
                var existingNode = NewXmlDocument.SelectSingleNode("//PcbFormatConfig");

                if (existingNode != null)
                {
                    var result = MessageBox.Show(
                        "PcbFormatConfig already exists in your XML. Do you want to replace it?",
                        "Replace Existing Configuration",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.No)
                    {
                        StatusMessage = "PcbFormatConfig copy cancelled";
                        return;
                    }

                    // Remove existing node
                    existingNode.ParentNode?.RemoveChild(existingNode);
                }

                // Find the root node
                var rootNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfig");
                if (rootNode == null)
                {
                    StatusMessage = "Error: ProductionUserConfig root node not found";
                    return;
                }

                // Import the PcbFormatConfig node (deep copy)
                XmlNode importedNode = NewXmlDocument.ImportNode(MasterPcbFormatConfig, true);

                // Strip editor-only metadata so they never reach Clotho
                StripEditorMetadata(importedNode);

                // Insert after ProductionUserConfigs if it exists
                var productionConfigsNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfigs");
                if (productionConfigsNode != null)
                {
                    rootNode.InsertAfter(importedNode, productionConfigsNode);
                }
                else
                {
                    rootNode.AppendChild(importedNode);
                }

                // Refresh the view
                OnPropertyChanged(nameof(NewPcbFormatConfig));

                var islandCount = importedNode.SelectNodes("Island")?.Count ?? 0;
                StatusMessage = $"Added PcbFormatConfig with {islandCount} island(s) to your new XML";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to copy PcbFormatConfig: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Copy PcbFormatConfig error: {ex}");
            }
        }

        // Delete PcbFormatConfig from New XML
        public void DeletePcbFormatConfig()
        {
            if (NewPcbFormatConfig == null)
            {
                StatusMessage = "No PcbFormatConfig found to delete";
                return;
            }

            try
            {
                var parentNode = NewPcbFormatConfig.ParentNode;
                parentNode?.RemoveChild(NewPcbFormatConfig);

                OnPropertyChanged(nameof(NewPcbFormatConfig));
                StatusMessage = "Deleted PcbFormatConfig from your XML";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to delete PcbFormatConfig: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Delete PcbFormatConfig error: {ex}");
            }
        }

        // Edit PcbFormatConfig
        public void EditPcbFormatConfig()
        {
            if (NewPcbFormatConfig == null)
            {
                StatusMessage = "No PcbFormatConfig found to edit. Please add it first.";
                return;
            }

            try
            {
                var dialog = new Views.EditPcbFormatDialog(NewPcbFormatConfig);
                var result = dialog.ShowDialog();

                if (result == true)
                {
                    // Apply changes from dialog to XML
                    dialog.ApplyChangesToXml(NewXmlDocument, NewPcbFormatConfig);

                    OnPropertyChanged(nameof(NewPcbFormatConfig));
                    StatusMessage = "PcbFormatConfig updated successfully";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to edit PcbFormatConfig: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Edit PcbFormatConfig error: {ex}");
            }
        }

        // ── InputValidationConfigs ─────────────────────────────────────────────

        /// <summary>
        /// Copies the entire InputValidationConfigs section from the Master XML into the
        /// user's new XML document.  If a section already exists the user is asked whether
        /// to replace it.
        /// </summary>
        public void CopyInputValidationConfigsFromMaster()
        {
            if (MasterInputValidationConfigs == null)
            {
                StatusMessage = "No InputValidationConfigs section found in Master XML";
                return;
            }

            try
            {
                var existingNode = NewXmlDocument.SelectSingleNode("//InputValidationConfigs");

                if (existingNode != null)
                {
                    var result = MessageBox.Show(
                        "InputValidationConfigs already exists in your XML. Do you want to replace it?",
                        "Replace Existing Configuration",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.No)
                    {
                        StatusMessage = "InputValidationConfigs copy cancelled";
                        return;
                    }

                    existingNode.ParentNode?.RemoveChild(existingNode);
                }

                var rootNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfig");
                if (rootNode == null)
                {
                    StatusMessage = "Error: ProductionUserConfig root node not found";
                    return;
                }

                XmlNode importedNode = NewXmlDocument.ImportNode(MasterInputValidationConfigs, true);
                StripEditorMetadata(importedNode);

                // Insert before DeveloperValidationConfig if it exists, otherwise append
                var devNode = NewXmlDocument.SelectSingleNode("//DeveloperValidationConfig");
                if (devNode != null)
                    rootNode.InsertBefore(importedNode, devNode);
                else
                    rootNode.AppendChild(importedNode);

                OnPropertyChanged(nameof(NewInputValidationConfigs));

                var entryCount = importedNode.SelectNodes("TestConfig")?.Count ?? 0;
                StatusMessage = $"Added InputValidationConfigs with {entryCount} TestConfig entry(ies) to your XML";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to copy InputValidationConfigs: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Copy InputValidationConfigs error: {ex}");
            }
        }

        /// <summary>
        /// Removes the InputValidationConfigs section from the user's new XML document.
        /// </summary>
        public void DeleteInputValidationConfigs()
        {
            if (NewInputValidationConfigs == null)
            {
                StatusMessage = "No InputValidationConfigs section found to delete";
                return;
            }

            try
            {
                NewInputValidationConfigs.ParentNode?.RemoveChild(NewInputValidationConfigs);
                OnPropertyChanged(nameof(NewInputValidationConfigs));
                StatusMessage = "Deleted InputValidationConfigs from your XML";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to delete InputValidationConfigs: {ex.Message}";
            }
        }

        /// <summary>
        /// Opens the EditInputValidationDialog so the user can manage TestConfig entries.
        /// </summary>
        public void EditInputValidationConfigs()
        {
            if (NewInputValidationConfigs == null)
            {
                StatusMessage = "No InputValidationConfigs found. Please add it first.";
                return;
            }

            try
            {
                var dialog = new Views.EditInputValidationDialog(NewInputValidationConfigs);
                var result = dialog.ShowDialog();

                if (result == true)
                {
                    dialog.ApplyChangesToXml(NewXmlDocument, NewInputValidationConfigs);
                    OnPropertyChanged(nameof(NewInputValidationConfigs));
                    StatusMessage = "InputValidationConfigs updated successfully";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to edit InputValidationConfigs: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Edit InputValidationConfigs error: {ex}");
            }
        }

        // ── DC_CONTACT_MODE_SETTING_OVERWRITE ──────────────────────────────────

        public void CopyDcContactModeSettingOverwriteFromMaster()
        {
            if (MasterDcContactModeSettingOverwrite == null)
            {
                StatusMessage = "No DC_CONTACT_MODE_SETTING_OVERWRITE found in Master XML";
                return;
            }

            try
            {
                var existingNode = NewXmlDocument.SelectSingleNode("//DC_CONTACT_MODE_SETTING_OVERWRITE");
                if (existingNode != null)
                {
                    var result = MessageBox.Show(
                        "DC_CONTACT_MODE_SETTING_OVERWRITE already exists in your XML. Replace it?",
                        "Replace Existing Configuration",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.No) { StatusMessage = "DC_CONTACT_MODE_SETTING_OVERWRITE copy cancelled"; return; }
                    existingNode.ParentNode?.RemoveChild(existingNode);
                }

                var rootNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfig");
                if (rootNode == null) { StatusMessage = "Error: ProductionUserConfig root node not found"; return; }

                XmlNode importedNode = NewXmlDocument.ImportNode(MasterDcContactModeSettingOverwrite, true);
                StripEditorMetadata(importedNode);

                var pcbNode = NewXmlDocument.SelectSingleNode("//PcbFormatConfig");
                if (pcbNode != null)
                    rootNode.InsertBefore(importedNode, pcbNode);
                else
                    rootNode.AppendChild(importedNode);

                OnPropertyChanged(nameof(NewDcContactModeSettingOverwrite));
                var pinCount = importedNode.SelectNodes("Pin")?.Count ?? 0;
                StatusMessage = $"Added DC_CONTACT_MODE_SETTING_OVERWRITE with {pinCount} Pin entry(ies) to your XML";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to copy DC_CONTACT_MODE_SETTING_OVERWRITE: {ex.Message}";
            }
        }

        public void DeleteDcContactModeSettingOverwrite()
        {
            if (NewDcContactModeSettingOverwrite == null) { StatusMessage = "No DC_CONTACT_MODE_SETTING_OVERWRITE found to delete"; return; }
            try
            {
                NewDcContactModeSettingOverwrite.ParentNode?.RemoveChild(NewDcContactModeSettingOverwrite);
                OnPropertyChanged(nameof(NewDcContactModeSettingOverwrite));
                StatusMessage = "Deleted DC_CONTACT_MODE_SETTING_OVERWRITE from your XML";
            }
            catch (Exception ex) { StatusMessage = $"Failed to delete DC_CONTACT_MODE_SETTING_OVERWRITE: {ex.Message}"; }
        }

        public void EditDcContactModeSettingOverwrite()
        {
            if (NewDcContactModeSettingOverwrite == null) { StatusMessage = "No DC_CONTACT_MODE_SETTING_OVERWRITE found. Please add it first."; return; }
            try
            {
                var dialog = new Views.EditDcContactModeDialog(NewDcContactModeSettingOverwrite);
                if (dialog.ShowDialog() == true)
                {
                    dialog.ApplyChangesToXml(NewXmlDocument, NewDcContactModeSettingOverwrite);
                    OnPropertyChanged(nameof(NewDcContactModeSettingOverwrite));
                    StatusMessage = "DC_CONTACT_MODE_SETTING_OVERWRITE updated successfully";
                }
            }
            catch (Exception ex) { StatusMessage = $"Failed to edit DC_CONTACT_MODE_SETTING_OVERWRITE: {ex.Message}"; }
        }

        // ── MailConfig ─────────────────────────────────────────────────────────

        public void CopyMailConfigFromMaster()
        {
            if (MasterMailConfig == null) { StatusMessage = "No MailConfig found in Master XML"; return; }
            try
            {
                var existingNode = NewXmlDocument.SelectSingleNode("//MailConfig");
                if (existingNode != null)
                {
                    var result = MessageBox.Show(
                        "MailConfig already exists in your XML. Replace it?",
                        "Replace Existing Configuration",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.No) { StatusMessage = "MailConfig copy cancelled"; return; }
                    existingNode.ParentNode?.RemoveChild(existingNode);
                }

                var rootNode = NewXmlDocument.SelectSingleNode("//ProductionUserConfig");
                if (rootNode == null) { StatusMessage = "Error: ProductionUserConfig root node not found"; return; }

                XmlNode importedNode = NewXmlDocument.ImportNode(MasterMailConfig, true);
                StripEditorMetadata(importedNode);
                rootNode.AppendChild(importedNode);

                OnPropertyChanged(nameof(NewMailConfig));
                StatusMessage = "Added MailConfig to your XML";
            }
            catch (Exception ex) { StatusMessage = $"Failed to copy MailConfig: {ex.Message}"; }
        }

        public void DeleteMailConfig()
        {
            if (NewMailConfig == null) { StatusMessage = "No MailConfig found to delete"; return; }
            try
            {
                NewMailConfig.ParentNode?.RemoveChild(NewMailConfig);
                OnPropertyChanged(nameof(NewMailConfig));
                StatusMessage = "Deleted MailConfig from your XML";
            }
            catch (Exception ex) { StatusMessage = $"Failed to delete MailConfig: {ex.Message}"; }
        }

        public void EditMailConfig()
        {
            if (NewMailConfig == null) { StatusMessage = "No MailConfig found. Please add it first."; return; }
            try
            {
                var dialog = new Views.EditMailConfigDialog(NewMailConfig);
                if (dialog.ShowDialog() == true)
                {
                    dialog.ApplyChangesToXml(NewMailConfig);
                    OnPropertyChanged(nameof(NewMailConfig));
                    StatusMessage = "MailConfig updated successfully";
                }
            }
            catch (Exception ex) { StatusMessage = $"Failed to edit MailConfig: {ex.Message}"; }
        }

        // ───────────────────────────────────────────────────────────────────────

        public void SaveXml(string path)
        {
            try
            {
                // Ensure file is not Read-Only before writing
                if (System.IO.File.Exists(path))
                {
                    var fileInfo = new System.IO.FileInfo(path);
                    if (fileInfo.IsReadOnly)
                    {
                        fileInfo.IsReadOnly = false;
                    }
                }

                // Create XmlWriterSettings for formatted output
                var settings = new System.Xml.XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = System.Xml.NewLineHandling.Replace,
                    Encoding = System.Text.Encoding.UTF8
                };

                // Save with formatting
                using (var writer = System.Xml.XmlWriter.Create(path, settings))
                {
                    NewXmlDocument.Save(writer);
                }

                _newXmlPath = path;

                // 🔒 SAFETY: If we saved to a path that is different from the Master path,
                // we are no longer editing the master. Turn off the flag to prevent accidental syncs.
                if (IsEditingMaster && !string.IsNullOrEmpty(MasterXmlPath))
                {
                    try
                    {
                        string normalizedNew = System.IO.Path.GetFullPath(path);
                        string normalizedMaster = System.IO.Path.GetFullPath(MasterXmlPath);

                        if (!normalizedNew.Equals(normalizedMaster, StringComparison.OrdinalIgnoreCase))
                        {
                            IsEditingMaster = false;
                            System.Diagnostics.Debug.WriteLine("Save path differs from Master path. 'Edit Master' mode disabled.");
                        }
                    }
                    catch
                    {
                        // If path checking fails, err on the side of safety and disable the flag
                        IsEditingMaster = false;
                    }
                }

                // Debug: Check if PcbFormatConfig exists before save
                var pcbNode = NewXmlDocument.SelectSingleNode("//PcbFormatConfig");
                var devNode = NewXmlDocument.SelectSingleNode("//DeveloperValidationConfig");
                var islandCount = pcbNode?.SelectNodes("Island")?.Count ?? 0;

                System.Diagnostics.Debug.WriteLine($"Saved XML - PcbFormatConfig: {(pcbNode != null ? $"YES ({islandCount} islands)" : "NO")}");
                System.Diagnostics.Debug.WriteLine($"Saved XML - DeveloperValidationConfig: {(devNode != null ? "YES" : "NO")}");

                StatusMessage = $"Configuration saved to {path}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to save XML: {ex.Message}";
            }
        }

        public string GetNewXmlPath()
        {
            return _newXmlPath;
        }

        // Add a new Package node to the selected Configuration Node
        public void AddPackageToConfiguration(string packageName, Dictionary<string, string> attributes)
        {
            if (SelectedConfigurationNode == null)
            {
                StatusMessage = "Please select a configuration node first";
                return;
            }

            try
            {
                // Create new Package element
                XmlElement newPackage = XmlDocument.CreateElement("Package");

                // Add name attribute
                XmlAttribute nameAttr = XmlDocument.CreateAttribute("name");
                nameAttr.Value = packageName;
                newPackage.Attributes.Append(nameAttr);

                // Add other attributes
                foreach (var attr in attributes)
                {
                    XmlAttribute newAttr = XmlDocument.CreateAttribute(attr.Key);
                    newAttr.Value = attr.Value;
                    newPackage.Attributes.Append(newAttr);
                }

                // Append to configuration node
                SelectedConfigurationNode.AppendChild(newPackage);

                // Refresh the view
                RefreshPackageItems();
                OnPropertyChanged(nameof(PackagesInSelectedConfiguration));
                OnPropertyChanged(nameof(PackageNodes));

                StatusMessage = $"Added package '{packageName}' to {SelectedConfigurationName}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to add package: {ex.Message}";
            }
        }

        // Delete the selected Package node
        public void DeletePackage(XmlNode packageNode)
        {
            if (packageNode == null)
            {
                StatusMessage = "No package selected to delete";
                return;
            }

            try
            {
                var parentNode = packageNode.ParentNode;
                var packageName = packageNode.Attributes?["name"]?.Value ?? "Unknown";

                parentNode?.RemoveChild(packageNode);

                // Clear selection if the deleted node was selected
                if (SelectedPackageNode == packageNode)
                {
                    SelectedPackageNode = null;
                }

                // Refresh the view
                RefreshPackageItems();
                OnPropertyChanged(nameof(PackagesInSelectedConfiguration));
                OnPropertyChanged(nameof(PackageNodes));

                StatusMessage = $"Deleted package '{packageName}' from {parentNode?.Name}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to delete package: {ex.Message}";
            }
        }

        // Get available attribute names from existing packages in the same configuration node
        public List<string> GetAvailableAttributeNames()
        {
            var attributeNames = new HashSet<string>();

            if (SelectedConfigurationNode != null)
            {
                var packages = SelectedConfigurationNode.SelectNodes("Package");
                if (packages != null)
                {
                    foreach (XmlNode package in packages)
                    {
                        if (package.Attributes != null)
                        {
                            foreach (XmlAttribute attr in package.Attributes)
                            {
                                attributeNames.Add(attr.Name);
                            }
                        }
                    }
                }
            }

            // Always include common attributes
            attributeNames.Add("name");
            attributeNames.Add("enable");

            return attributeNames.OrderBy(n => n).ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
