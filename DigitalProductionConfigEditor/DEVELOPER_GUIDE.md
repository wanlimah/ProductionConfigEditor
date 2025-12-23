# Developer Guide - Dynamic Configuration Editor

## Overview
This guide explains how to extend and maintain the dynamic configuration editor.

## Architecture

### Key Components

#### 1. AttributeViewModel
**Location:** `ViewModels/WizardViewModel.cs`

```csharp
public class AttributeViewModel : INotifyPropertyChanged
{
    public string Name { get; set; }           // Attribute name (e.g., "enable", "count")
    public string Value { get; set; }          // Current value
    public List<string>? Options { get; set; } // Predefined options (null if free-form)
    public bool HasOptions { get; }            // True if dropdown, false if textbox
}
```

**Purpose:** Represents a single XML attribute with its metadata for UI binding.

#### 2. WizardViewModel
**Location:** `ViewModels/WizardViewModel.cs`

**Key Properties:**
- `XmlDocument` - The loaded XML configuration
- `PackageNodes` - All `<Package>` elements in the XML
- `SelectedPackageNode` - Currently selected package
- `Attributes` - Observable collection of attributes for the selected package

**Key Methods:**
- `LoadAttributesFromSelectedNode()` - Reads attributes from XML node
- `GetOptionsForAttribute()` - Determines if an attribute has predefined options
- `SaveAttributesToNode()` - Writes attribute changes back to XML node

## Adding Support for New Attribute Types

### Scenario 1: Add Support for a New Options Node in XML

**Example:** You want to add support for `<StatusOptions>` in the XML:

```xml
<SOME_CONFIG>
  <StatusOptions> ACTIVE | INACTIVE | PENDING </StatusOptions>
  <Package name="TEST" enable="TRUE" status="ACTIVE" />
</SOME_CONFIG>
```

**Solution:**
1. Open `ViewModels/WizardViewModel.cs`
2. Find the `GetOptionsForAttribute()` method
3. Add your mapping to the `optionNodeMap` dictionary:

```csharp
var optionNodeMap = new Dictionary<string, string>
{
    { "enable", "EnableOptions" },
    { "mode", "ModeOptions" },
    { "rule", "RuleOptions" },
    { "avg_channel", "AvgChannelOptions" },
    { "status", "StatusOptions" }  // ← Add this line
};
```

That's it! The UI will automatically show a dropdown for the `status` attribute.

### Scenario 2: Add Default Options for Common Attributes

**Example:** You want `viewer` attribute to always show true/false dropdown:

**Solution:**
1. Open `ViewModels/WizardViewModel.cs`
2. Find the `GetOptionsForAttribute()` method
3. Add to the `defaultOptions` dictionary:

```csharp
var defaultOptions = new Dictionary<string, List<string>>
{
    { "enable", new List<string> { "TRUE", "FALSE" } },
    { "viewer", new List<string> { "true", "false" } },
    { "active", new List<string> { "YES", "NO" } }  // ← Add this
};
```

### Scenario 3: Add Custom Validation

**Example:** Validate that `count` is a positive integer:

**Solution:**
1. Add a validation method to `WizardViewModel`:

```csharp
public bool ValidateAttributes(out string errorMessage)
{
    foreach (var attr in Attributes)
    {
        if (attr.Name == "count")
        {
            if (!int.TryParse(attr.Value, out int count) || count <= 0)
            {
                errorMessage = "Count must be a positive integer";
                return false;
            }
        }
    }
    errorMessage = "";
    return true;
}
```

2. Call it in `MainWindow.xaml.cs` before saving:

```csharp
private void OnSaveXmlClick(object sender, RoutedEventArgs e)
{
    if (!viewModel.ValidateAttributes(out string error))
    {
        MessageBox.Show(error, "Validation Error");
        return;
    }
    
    viewModel.SaveAttributesToNode();
    // ... rest of save logic
}
```

## Customizing the UI

### Changing Field Widths

Edit `Views/Step2_EditAttributes.xaml`:

```xml
<Grid.ColumnDefinitions>
    <ColumnDefinition Width="150"/>  <!-- Label column -->
    <ColumnDefinition Width="250"/>  <!-- Input column -->
</Grid.ColumnDefinitions>
```

### Adding Icons or Styling

You can customize the attribute display:

```xml
<TextBlock Grid.Column="0" 
           Text="{Binding Name, StringFormat='{}{0}:'}" 
           VerticalAlignment="Center"
           FontWeight="SemiBold"
           Foreground="DarkBlue"/>  <!-- Add custom styling -->
```

### Adding Tooltips

```xml
<TextBox Text="{Binding Value, Mode=TwoWay}" 
         MinWidth="200"
         ToolTip="{Binding Name, StringFormat='Enter value for {0}'}"/>
```

## Debugging Tips

### Check What Attributes Were Loaded

Add this to `LoadAttributesFromSelectedNode()`:

```csharp
System.Diagnostics.Debug.WriteLine($"Loading attributes for {SelectedPackageNode?.ParentNode?.Name}");
foreach (var attr in Attributes)
{
    System.Diagnostics.Debug.WriteLine($"  {attr.Name} = {attr.Value} (HasOptions: {attr.HasOptions})");
}
```

### Verify Options Detection

Add this to `GetOptionsForAttribute()`:

```csharp
if (options.Count > 0)
{
    System.Diagnostics.Debug.WriteLine($"Found options for {attributeName}: {string.Join(", ", options)}");
}
```

## Common Issues and Solutions

### Issue 1: Dropdown Not Showing for an Attribute

**Symptoms:** An attribute that should have options shows as a textbox instead.

**Solutions:**
1. Check if the options node exists in XML (e.g., `<ModeOptions>`)
2. Verify the mapping in `optionNodeMap` is correct
3. Check the XML format: options should be "OPTION1 | OPTION2" (separated by pipes)
4. Add the attribute to `defaultOptions` if it should always have options

### Issue 2: Changes Not Being Saved

**Symptoms:** You edit values but they don't persist in the saved XML.

**Solutions:**
1. Ensure `SaveAttributesToNode()` is called before `SaveXml()`
2. Check if the attribute exists in the XML node (not dynamically added)
3. Verify two-way binding in XAML: `Mode=TwoWay`

### Issue 3: UI Not Updating When Package Selected

**Symptoms:** Selecting a new package doesn't refresh the attribute list.

**Solutions:**
1. Verify `LoadAttributesFromSelectedNode()` is called in the `SelectedPackageNode` setter
2. Check that `Attributes` collection is using `ObservableCollection<T>`
3. Ensure `OnPropertyChanged(nameof(Attributes))` is called

## Performance Considerations

### Large XML Files

If you have many packages (100+), consider:

1. **Lazy Loading:** Only load attributes when a package is selected (already implemented)
2. **Virtualization:** Use `VirtualizingStackPanel` for the package list
3. **Caching:** Cache options lookups

```csharp
private Dictionary<string, List<string>> _optionsCache = new();

private List<string>? GetOptionsForAttribute(string attributeName, XmlNode? parentNode)
{
    string cacheKey = $"{parentNode?.Name}_{attributeName}";
    if (_optionsCache.ContainsKey(cacheKey))
        return _optionsCache[cacheKey];
        
    var options = /* ... existing logic ... */;
    _optionsCache[cacheKey] = options;
    return options;
}
```

## Testing

### Manual Testing Checklist

- [ ] Select different packages and verify correct attributes shown
- [ ] Edit text fields and verify changes persist
- [ ] Select dropdown values and verify they update
- [ ] Navigate through all 3 steps
- [ ] Save XML and verify file contents are correct
- [ ] Reload XML and verify changes were saved
- [ ] Test packages with no attributes
- [ ] Test packages with many attributes (10+)
- [ ] Test attributes with and without options

### Test Packages to Use

Create test packages in your XML:

```xml
<!-- Minimal package -->
<TEST_MINIMAL>
  <Package name="TEST1" enable="TRUE" />
</TEST_MINIMAL>

<!-- Complex package -->
<TEST_COMPLEX>
  <ModeOptions> MODE1 | MODE2 | MODE3 </ModeOptions>
  <Package name="TEST2" enable="FALSE" mode="MODE1" count="10" sampling="5" threshold="99" />
</TEST_COMPLEX>

<!-- No attributes package -->
<TEST_EMPTY>
  <Package />
</TEST_EMPTY>
```

## Best Practices

1. **Always use two-way binding** for editable fields: `Mode=TwoWay`
2. **Use ObservableCollection** for collections that update the UI
3. **Call OnPropertyChanged** whenever a property changes
4. **Validate user input** before saving
5. **Handle null values** gracefully (use null-conditional operators)
6. **Add helpful tooltips** for complex attributes
7. **Document XML options** in the XML file itself using comments

## Future Enhancements

### Ideas for Improvement

1. **Search/Filter Packages:** Add a search box to filter the package list
2. **Undo/Redo:** Implement undo/redo functionality
3. **Attribute Descriptions:** Show tooltips from XML comments
4. **Batch Editing:** Edit multiple packages at once
5. **Export/Import:** Export specific configurations
6. **Validation Rules:** Read validation rules from XML
7. **History Tracking:** Show change history for each attribute

### Code Structure for Validation Rules

```xml
<SOME_CONFIG>
  <ValidationRules>
    <Rule attribute="count" type="integer" min="1" max="1000" />
    <Rule attribute="sampling" type="integer" min="1" max="100" />
    <Rule attribute="url" type="url" required="true" />
  </ValidationRules>
  <Package name="TEST" count="10" sampling="5" url="http://example.com" />
</SOME_CONFIG>
```

This would allow validation rules to be defined in XML and automatically enforced by the editor!

## Support

For questions or issues, review:
1. This guide
2. `DYNAMIC_IMPROVEMENTS.md` for feature overview
3. `COMPARISON.md` for before/after examples
4. Code comments in `WizardViewModel.cs`































































