using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Xml;

namespace DigitalProductionConfigEditor.ViewModels
{
    /// <summary>
    /// Represents a single XML attribute that can be edited
    /// </summary>
    public class XmlAttributeItem : INotifyPropertyChanged
    {
        private string _value = "";
        
        public string Name { get; set; } = "";
        
        public string Value 
        { 
            get => _value;
            set 
            {
                if (_value == value) return;
                
                // Validate based on detected type
                if (IsNumeric && !string.IsNullOrEmpty(value))
                {
                    // Allow numbers, decimal point, minus sign, and scientific notation (e/E)
                    if (!IsValidNumericInput(value))
                    {
                        return; // Reject invalid input
                    }
                }
                
                _value = value;
                OnPropertyChanged(nameof(Value));
                IsDirty = true;
            }
        }
        
        public bool IsNumeric { get; set; }
        public bool HasOptions => Options != null && Options.Count > 0;
        public ObservableCollection<string>? Options { get; set; }
        public bool IsDirty { get; set; }
        
        public XmlAttribute? SourceAttribute { get; set; } // Reference to actual XML attribute
        
        private bool IsValidNumericInput(string input)
        {
            // Try parsing as double - this validates proper scientific notation
            // Valid: 10, 0.1, -0.3, 10e-6, 1.5e-6, -1.2e3
            // Invalid: 10ee-6, e-6, 1.2.3, --5
            if (double.TryParse(input, System.Globalization.NumberStyles.Any, 
                System.Globalization.CultureInfo.InvariantCulture, out _))
            {
                return true;
            }
            
            // Allow empty or partial input during typing (for better UX)
            // Allow: "1", "1.", "1.2", "1e", "1e-", "-", "-1", etc.
            if (string.IsNullOrEmpty(input) || input == "-" || input == "." || input == "-.")
            {
                return true;
            }
            
            // Check if it's a valid partial scientific notation being typed
            // Examples: "1e", "1e-", "1.5e", "1.5e-"
            if (input.EndsWith("e", StringComparison.OrdinalIgnoreCase) || 
                input.EndsWith("e-", StringComparison.OrdinalIgnoreCase) ||
                input.EndsWith("e+", StringComparison.OrdinalIgnoreCase))
            {
                // Check if the part before 'e' is a valid number
                var parts = input.Split(new[] { 'e', 'E' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    return double.TryParse(parts[0], System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out _);
                }
            }
            
            return false;
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
    /// <summary>
    /// Represents an XML element node in the tree
    /// </summary>
    public class XmlNodeItem : INotifyPropertyChanged
    {
        private bool _isExpanded = true;
        private bool _isSelected;
        
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public ObservableCollection<XmlAttributeItem> Attributes { get; set; } = new();
        public ObservableCollection<XmlNodeItem> Children { get; set; } = new();
        public XmlNode? SourceNode { get; set; } // Reference to actual XML node
        
        public bool IsExpanded 
        { 
            get => _isExpanded;
            set 
            {
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }
        
        public bool IsSelected 
        { 
            get => _isSelected;
            set 
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        
        public bool HasAttributes => Attributes.Count > 0;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
    /// <summary>
    /// ViewModel for Advanced Settings - handles all non-Package XML configurations
    /// </summary>
    public class AdvancedSettingsViewModel : INotifyPropertyChanged
    {
        private XmlDocument? _xmlDocument;
        private ObservableCollection<XmlNodeItem> _rootNodes = new();
        private XmlNodeItem? _selectedNode;
        
        public ObservableCollection<XmlNodeItem> RootNodes 
        { 
            get => _rootNodes;
            set 
            {
                _rootNodes = value;
                OnPropertyChanged(nameof(RootNodes));
            }
        }
        
        public XmlNodeItem? SelectedNode 
        { 
            get => _selectedNode;
            set 
            {
                _selectedNode = value;
                OnPropertyChanged(nameof(SelectedNode));
                OnPropertyChanged(nameof(SelectedAttributes));
                OnPropertyChanged(nameof(HasSelection));
                
                // Debug
                System.Diagnostics.Debug.WriteLine($"SelectedNode changed to: {value?.DisplayName ?? "null"}");
                System.Diagnostics.Debug.WriteLine($"  HasAttributes: {value?.HasAttributes}");
                System.Diagnostics.Debug.WriteLine($"  Attributes.Count: {value?.Attributes.Count}");
            }
        }
        
        public ObservableCollection<XmlAttributeItem> SelectedAttributes => 
            SelectedNode?.Attributes ?? new ObservableCollection<XmlAttributeItem>();
        
        public bool HasSelection => SelectedNode != null && SelectedNode.HasAttributes;
        
        /// <summary>
        /// Merge missing settings from a Master document into the current document
        /// </summary>
        public void MergeMissingSettingsFromMaster(XmlDocument masterDoc)
        {
            if (_xmlDocument?.DocumentElement == null || masterDoc?.DocumentElement == null) return;
            
            var masterNodes = FindNonPackageNodes(masterDoc.DocumentElement);
            var currentNodes = FindNonPackageNodes(_xmlDocument.DocumentElement);
            
            bool changesMade = false;
            
            foreach (var masterNode in masterNodes)
            {
                // Check if this node exists in current document (by Name)
                // We use Name as the unique identifier for top-level config sections
                bool exists = currentNodes.Any(n => n.Name == masterNode.Name);
                
                if (!exists)
                {
                    try
                    {
                        // Import node from Master to Current
                        XmlNode importedNode = _xmlDocument.ImportNode(masterNode, true);
                        
                        // Determine where to add it
                        // If it was inside ProductionUserConfigs in Master, try to put it there in Current
                        if (masterNode.ParentNode?.Name == "ProductionUserConfigs")
                        {
                            var configsNode = _xmlDocument.SelectSingleNode("//ProductionUserConfigs");
                            if (configsNode != null)
                            {
                                configsNode.AppendChild(importedNode);
                            }
                            else
                            {
                                _xmlDocument.DocumentElement.AppendChild(importedNode);
                            }
                        }
                        else
                        {
                            // Otherwise add to root
                            _xmlDocument.DocumentElement.AppendChild(importedNode);
                        }
                        
                        // Add to TreeView
                        var treeNode = BuildTreeNode(importedNode);
                        if (treeNode != null)
                        {
                            RootNodes.Add(treeNode);
                            changesMade = true;
                            System.Diagnostics.Debug.WriteLine($"Merged missing setting from Master: {masterNode.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to merge setting {masterNode.Name}: {ex.Message}");
                        MessageBox.Show($"Failed to merge setting '{masterNode.Name}': {ex.Message}", "Merge Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            
            if (changesMade)
            {
                // Notify user that new settings were found and added
                MessageBox.Show(
                    "🆕 New settings detected from Master Template!\n\n" +
                    "The following settings were missing from your configuration and have been automatically added:\n" +
                    string.Join("\n", RootNodes.Where(n => !currentNodes.Any(c => c.Name == n.Name)).Select(n => "• " + n.DisplayName)),
                    "Settings Merged",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Remove settings from current document that are NO LONGER in the Master document
        /// (Used only in Edit Master mode to sync external deletions)
        /// </summary>
        public void PruneOrphanedSettings(XmlDocument masterDoc)
        {
            if (_xmlDocument?.DocumentElement == null || masterDoc?.DocumentElement == null) return;
            
            var masterNodes = FindNonPackageNodes(masterDoc.DocumentElement);
            var currentNodes = FindNonPackageNodes(_xmlDocument.DocumentElement);
            
            var orphanedNodes = new System.Collections.Generic.List<XmlNode>();
            var orphanedTreeNodes = new System.Collections.Generic.List<XmlNodeItem>();
            
            foreach (var currentNode in currentNodes)
            {
                // Check if this node exists in Master (by Name)
                bool existsInMaster = masterNodes.Any(n => n.Name == currentNode.Name);
                
                if (!existsInMaster)
                {
                    orphanedNodes.Add(currentNode);
                    var treeNode = RootNodes.FirstOrDefault(n => n.Name == currentNode.Name);
                    if (treeNode != null)
                    {
                        orphanedTreeNodes.Add(treeNode);
                    }
                }
            }
            
            if (orphanedNodes.Count > 0)
            {
                var result = MessageBox.Show(
                    $"⚠️ Sync with Master File\n\n" +
                    $"Found {orphanedNodes.Count} setting(s) that are NOT in the Master file on disk.\n" +
                    $"This usually happens if you deleted them externally (e.g. Notepad++).\n\n" +
                    $"Do you want to remove them from your current session to match the file?",
                    "Sync Deletions",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                    
                if (result == MessageBoxResult.Yes)
                {
                    // Remove from XML Document
                    foreach (var node in orphanedNodes)
                    {
                        node.ParentNode?.RemoveChild(node);
                    }
                    
                    // Remove from Tree View
                    foreach (var treeNode in orphanedTreeNodes)
                    {
                        RootNodes.Remove(treeNode);
                    }
                    
                    MessageBox.Show("Settings removed to match Master file.", "Synced", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Load non-Package configurations from XML document
        /// </summary>
        public void LoadFromXmlDocument(XmlDocument xmlDoc)
        {
            _xmlDocument = xmlDoc;
            RootNodes.Clear();
            
            if (xmlDoc?.DocumentElement == null) return;
            
            // Find all non-Package configuration sections
            var nonPackageNodes = FindNonPackageNodes(xmlDoc.DocumentElement);
            
            // Debug: Log found nodes
            System.Diagnostics.Debug.WriteLine($"Found {nonPackageNodes.Count} non-Package nodes:");
            foreach (var node in nonPackageNodes)
            {
                System.Diagnostics.Debug.WriteLine($"  - {node.Name}");
            }
            
            foreach (var node in nonPackageNodes)
            {
                var treeNode = BuildTreeNode(node);
                if (treeNode != null)
                {
                    RootNodes.Add(treeNode);
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"Added {RootNodes.Count} nodes to tree");
        }
        
        /// <summary>
        /// Find all XML nodes that are NOT Package-based configurations
        /// </summary>
        private System.Collections.Generic.List<XmlNode> FindNonPackageNodes(XmlNode root)
        {
            var nonPackageNodes = new System.Collections.Generic.List<XmlNode>();
            
            // Look for common configuration sections
            var sectionsToCheck = new[] 
            { 
                "//ProductionUserConfig/PcbFormatConfig",
                "//ProductionUserConfig/DeveloperValidationConfig",
                "//ProductionUserConfig/*[not(self::ProductionUserConfigs) and not(self::PcbFormatConfig) and not(self::DeveloperValidationConfig)]"
            };
            
            // Add any direct children of ProductionUserConfig that are not Package-based
            if (root.Name == "ProductionUserConfig")
            {
                foreach (XmlNode child in root.ChildNodes)
                {
                    if (child.NodeType != XmlNodeType.Element) continue;
                    
                    // Skip ProductionUserConfigs (handled by main UI)
                    if (child.Name == "ProductionUserConfigs")
                    {
                        // But check inside for non-Package nodes
                        // System.Diagnostics.Debug.WriteLine("Checking inside ProductionUserConfigs...");
                        foreach (XmlNode configChild in child.ChildNodes)
                        {
                            if (configChild.NodeType != XmlNodeType.Element) continue;
                            
                            // Skip option nodes inside ProductionUserConfigs too
                            if (configChild.Name.EndsWith("Options", StringComparison.OrdinalIgnoreCase))
                                continue;
                            
                            // System.Diagnostics.Debug.WriteLine($"  Checking config: {configChild.Name}");
                            
                            // If this config node does NOT contain <Package> children, it's non-standard
                            bool hasPackageChildren = false;
                            foreach (XmlNode grandchild in configChild.ChildNodes)
                            {
                                if (grandchild.Name == "Package")
                                {
                                    hasPackageChildren = true;
                                    break;
                                }
                            }
                            
                            System.Diagnostics.Debug.WriteLine($"    Has <Package> children: {hasPackageChildren}");
                            System.Diagnostics.Debug.WriteLine($"    Has editable content: {HasEditableContent(configChild)}");
                            
                            if (!hasPackageChildren && HasEditableContent(configChild))
                            {
                                System.Diagnostics.Debug.WriteLine($"    ✓ Added {configChild.Name} as non-Package config");
                                nonPackageNodes.Add(configChild);
                            }
                        }
                    }
                    else
                    {
                        // Other top-level nodes (custom nodes)
                        // Skip nodes that contain <Package> elements anywhere in their hierarchy (e.g., DeveloperValidationConfig)
                        // Skip well-known configurations that are handled elsewhere (e.g., PcbFormatConfig, DeveloperValidationConfig)
                        
                        // List of nodes to exclude from Advanced Settings (handled by main UI or other features)
                        var excludedNodes = new[] { "PcbFormatConfig", "DeveloperValidationConfig", "InputValidationConfigs", "DC_CONTACT_MODE_SETTING_OVERWRITE", "MailConfig" };
                        
                        if (excludedNodes.Contains(child.Name))
                        {
                            System.Diagnostics.Debug.WriteLine($"  Top-level node: {child.Name} - EXCLUDED (handled by main UI)");
                            continue;
                        }
                        
                        bool hasPackageChildren = ContainsPackageElements(child);
                        
                        System.Diagnostics.Debug.WriteLine($"  Top-level node: {child.Name}, Has <Package> children: {hasPackageChildren}");
                        
                        if (!hasPackageChildren && HasEditableContent(child))
                        {
                            System.Diagnostics.Debug.WriteLine($"    ✓ Added {child.Name} as non-Package top-level node");
                            nonPackageNodes.Add(child);
                        }
                    }
                }
            }
            
            return nonPackageNodes;
        }
        
        /// <summary>
        /// Recursively check if a node contains any <Package> elements in its hierarchy
        /// </summary>
        private bool ContainsPackageElements(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;
                
                // Direct Package child found
                if (child.Name == "Package")
                    return true;
                
                // Recursively check children
                if (ContainsPackageElements(child))
                    return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Check if a node has editable content (child elements or attributes)
        /// </summary>
        private bool HasEditableContent(XmlNode node)
        {
            // Has attributes to edit?
            if (node.Attributes != null && node.Attributes.Count > 0)
                return true;
            
            // Has child elements (not just text)?
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                    return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Build a tree node from an XML node (recursive)
        /// </summary>
        private XmlNodeItem? BuildTreeNode(XmlNode xmlNode)
        {
            if (xmlNode.NodeType != XmlNodeType.Element) return null;
            
            var treeNode = new XmlNodeItem
            {
                Name = xmlNode.Name,
                DisplayName = GetDisplayName(xmlNode),
                SourceNode = xmlNode
            };
            
            // Add attributes
            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute attr in xmlNode.Attributes)
                {
                    var attrItem = CreateAttributeItem(attr, xmlNode);
                    treeNode.Attributes.Add(attrItem);
                }
            }
            
            // Add child elements recursively
            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    // Skip option nodes (they define dropdowns, not editable content)
                    if (child.Name.EndsWith("Options", StringComparison.OrdinalIgnoreCase))
                        continue;
                    
                    var childNode = BuildTreeNode(child);
                    if (childNode != null)
                    {
                        treeNode.Children.Add(childNode);
                    }
                }
            }
            
            return treeNode;
        }
        
        /// <summary>
        /// Create a display name for a node (includes key attributes like name, id, type)
        /// </summary>
        private string GetDisplayName(XmlNode node)
        {
            var name = node.Name;
            
            // Add key identifier attributes to display name
            var keyAttrs = new[] { "name", "Name", "id", "type", "Type" };
            foreach (var attrName in keyAttrs)
            {
                var attr = node.Attributes?[attrName];
                if (attr != null)
                {
                    return $"{name} [{attrName}=\"{attr.Value}\"]";
                }
            }
            
            return name;
        }
        
        /// <summary>
        /// Create an attribute item with proper validation and options
        /// </summary>
        private XmlAttributeItem CreateAttributeItem(XmlAttribute attr, XmlNode parentNode)
        {
            var attrItem = new XmlAttributeItem
            {
                Name = attr.Name,
                Value = attr.Value,
                SourceAttribute = attr
            };
            
            // Detect if numeric
            attrItem.IsNumeric = IsNumericValue(attr.Value);
            
            // Check for predefined options
            var options = GetOptionsForAttribute(attr.Name, parentNode);
            if (options != null && options.Count > 0)
            {
                attrItem.Options = new ObservableCollection<string>(options);
            }
            
            return attrItem;
        }
        
        /// <summary>
        /// Detect if a value is numeric (including scientific notation)
        /// </summary>
        private bool IsNumericValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            
            // Try parsing as double (handles scientific notation like 10e-6)
            return double.TryParse(value, System.Globalization.NumberStyles.Any, 
                System.Globalization.CultureInfo.InvariantCulture, out _);
        }
        
        /// <summary>
        /// Get predefined options for an attribute (from XML or defaults)
        /// </summary>
        private System.Collections.Generic.List<string>? GetOptionsForAttribute(string attrName, XmlNode parentNode)
        {
            // Default options for common attributes
            var defaultOptions = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>
            {
                { "enable", new System.Collections.Generic.List<string> { "TRUE", "FALSE" } },
                { "Enable", new System.Collections.Generic.List<string> { "TRUE", "FALSE" } },
                { "default", new System.Collections.Generic.List<string> { "true", "false" } }
            };
            
            // Check for options defined in XML (e.g., <TestModeOptions>FORCEV | FORCEI</TestModeOptions>)
            var optionsNodeName = $"{attrName}Options";
            var optionsNode = parentNode.ParentNode?.SelectSingleNode(optionsNodeName);
            
            if (optionsNode?.InnerText != null)
            {
                var options = optionsNode.InnerText
                    .Split('|')
                    .Select(o => o.Trim())
                    .Where(o => !string.IsNullOrWhiteSpace(o))
                    .ToList();
                
                if (options.Count > 0) return options;
            }
            
            // Check for capitalized version
            var capitalizedOptionsNodeName = $"{char.ToUpper(attrName[0])}{attrName.Substring(1)}Options";
            optionsNode = parentNode.ParentNode?.SelectSingleNode(capitalizedOptionsNodeName);
            
            if (optionsNode?.InnerText != null)
            {
                var options = optionsNode.InnerText
                    .Split('|')
                    .Select(o => o.Trim())
                    .Where(o => !string.IsNullOrWhiteSpace(o))
                    .ToList();
                
                if (options.Count > 0) return options;
            }
            
            // Fall back to default options
            var lowerAttrName = attrName.ToLower();
            if (defaultOptions.ContainsKey(lowerAttrName))
            {
                return defaultOptions[lowerAttrName];
            }
            
            if (defaultOptions.ContainsKey(attrName))
            {
                return defaultOptions[attrName];
            }
            
            return null;
        }
        
        /// <summary>
        /// Save all changes back to XML
        /// </summary>
        public bool SaveChanges()
        {
            try
            {
                SaveNodeChanges(RootNodes);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Save Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        
        /// <summary>
        /// Recursively save changes from tree nodes back to XML
        /// </summary>
        private void SaveNodeChanges(ObservableCollection<XmlNodeItem> nodes)
        {
            foreach (var node in nodes)
            {
                // Save attributes
                foreach (var attr in node.Attributes)
                {
                    if (attr.IsDirty && attr.SourceAttribute != null)
                    {
                        attr.SourceAttribute.Value = attr.Value;
                        attr.IsDirty = false;
                    }
                }
                
                // Save children recursively
                if (node.Children.Count > 0)
                {
                    SaveNodeChanges(node.Children);
                }
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

