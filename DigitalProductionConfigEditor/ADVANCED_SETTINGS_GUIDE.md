# Advanced Settings Feature - User Guide

## 🎯 Overview

The **Advanced Settings** feature allows you to view and edit non-standard XML configurations that don't follow the typical `<Package>` structure. This feature automatically detects and displays any custom XML sections in your configuration.

---

## 🚀 How to Access

1. Open or create an XML configuration file
2. Click the **"⚙️ Advanced Settings"** button in the main window
3. A dialog will open showing all detected non-standard configurations

---

## 📋 What Appears in Advanced Settings?

The system automatically detects XML configurations that:

### ✅ **Will Appear:**
- Sections that **do NOT use `<Package>` elements**
- Custom XML structures with unique element names
- Configuration nodes with non-standard child elements

### ❌ **Will NOT Appear:**
- Standard configurations with `<Package>` children (handled by main wizard)
- Empty nodes without attributes or child elements

---

## 🌳 Example: DC_CONTACT_MODE_SETTING_OVERWRITE

### Original Structure (User's Version):
```xml
<DC_CONTACT_MODE_SETTING_OVERWRITE>
  <Pin Name="Vdd" VSet="0.1" ISet="10e-6" HighLimit="9.5e-6" TestMode="FORCEV"/>
  <Pin Name="Vio1" VSetHi="1.2" VSetLo="-0.3" ISource="1.5e-6" ILevel="2e-6" HighLimit="1.14" LowLimit="50e-3" TestMode="FORCEI"/>
  <Delayms Value="80"/>
</DC_CONTACT_MODE_SETTING_OVERWRITE>
```

**Status:** ✅ **Will appear in Advanced Settings**
- Uses custom `<Pin>` and `<Delayms>` elements (not `<Package>`)
- Automatically detected and displayed in tree view

### Converted to Package Format (Alternative):
```xml
<DC_CONTACT_MODE_SETTING_OVERWRITE>
  <Package name="Pin_Vdd" VSet="0.1" ISet="10e-6" HighLimit="9.5e-6" TestMode="FORCEV" />
  <Package name="Pin_Vio1" VSetHi="1.2" VSetLo="-0.3" ISource="1.5e-6" ILevel="2e-6" HighLimit="1.14" LowLimit="50e-3" TestMode="FORCEI" />
  <Package name="Delay" Value="80" unit="ms" />
</DC_CONTACT_MODE_SETTING_OVERWRITE>
```

**Status:** ❌ **Will appear in main wizard** (not Advanced Settings)
- Uses standard `<Package>` elements
- Handled by the regular wizard interface

---

## 🎨 User Interface

### Tree View (Left Panel)
```
📁 DC_CONTACT_MODE_SETTING_OVERWRITE
   ├─ 📄 Pin [Name="Vdd"]              [3 attrs]
   ├─ 📄 Pin [Name="Vio1"]             [8 attrs]
   └─ 📄 Delayms                       [1 attr]
```

- **📁** = Node with children
- **📄** = Leaf node (no children)
- **[Name="..."]** = Key identifier attribute
- **[X attrs]** = Number of editable attributes

### Attribute Editor (Right Panel)

When you select a node, its attributes appear on the right:

```
┌─ Edit: Pin [Name="Vdd"] ───────────────┐
│                                         │
│ VSet:        [0.1]        ← Numeric    │
│ ISet:        [10e-6]      ← Scientific │
│ HighLimit:   [9.5e-6]     ← Scientific │
│ TestMode:    [FORCEV ▼]   ← Dropdown   │
│                                         │
│            [💾 Save Changes]            │
└─────────────────────────────────────────┘
```

---

## 🔍 Attribute Types & Validation

### 1. **Numeric Fields**
- Automatically detected from value format
- Only accepts: digits, decimal point (.), minus sign (-), scientific notation (e/E)
- Examples: `0.1`, `10e-6`, `-0.3`, `1.5e-6`

**Validation:**
```
✅ Valid:   0.1, 10e-6, -0.3, 1.14, 50e-3
❌ Invalid: abc, 10x6, 1.2.3, e-6 (missing number)
```

### 2. **Dropdown Fields (Options)**
- Defined in XML using `<AttributeNameOptions>` tags
- Restricts input to predefined values

**Example:**
```xml
<DC_CONTACT_MODE_SETTING_OVERWRITE>
  <TestModeOptions>FORCEV | FORCEI | MEASURE</TestModeOptions>
  <Pin Name="Vdd" TestMode="FORCEV" />
</DC_CONTACT_MODE_SETTING_OVERWRITE>
```

**Result:** `TestMode` shows dropdown: **FORCEV | FORCEI | MEASURE**

### 3. **Free Text Fields**
- Any attribute without options or non-numeric value
- No validation restrictions
- Examples: names, URLs, descriptions

---

## 📝 Adding Dropdown Options to Your XML

To add validation dropdowns for custom attributes:

### Step 1: Identify the attribute name
```xml
<Pin Name="Vdd" TestMode="FORCEV" />
        ↑             ↑
    Attribute     Attribute
```

### Step 2: Add `<AttributeNameOptions>` node
```xml
<DC_CONTACT_MODE_SETTING_OVERWRITE>
  <!-- Add this line to create dropdown for TestMode -->
  <TestModeOptions>FORCEV | FORCEI | MEASURE</TestModeOptions>
  
  <Pin Name="Vdd" TestMode="FORCEV" />
</DC_CONTACT_MODE_SETTING_OVERWRITE>
```

### Format:
```
<AttributeNameOptions>OPTION1 | OPTION2 | OPTION3</AttributeNameOptions>
```

- **Naming:** `{AttributeName}Options` (case-sensitive)
- **Separator:** Use `|` (pipe) to separate options
- **Placement:** Inside parent configuration node
- **Effect:** Creates dropdown in UI automatically

---

## 💾 Saving Changes

### Individual Attribute Changes:
1. Edit attributes in the right panel
2. Click **"💾 Save Changes"** button
3. Changes are saved to XML in memory

### Saving to File:
1. Close the Advanced Settings dialog
2. Dialog asks: "Do you want to save changes?"
   - **Yes** → Apply changes to XML document
   - **No** → Discard changes
   - **Cancel** → Return to editing
3. Click main window **"💾 Save"** or **"💾 Save As..."** to write to file

---

## 🎯 Common Use Cases

### Use Case 1: Contact Mode Pin Settings
```xml
<DC_CONTACT_MODE_SETTING_OVERWRITE>
  <Pin Name="Vdd" VSet="0.1" ISet="10e-6" HighLimit="9.5e-6" TestMode="FORCEV"/>
  <Pin Name="Vio1" VSetHi="1.2" VSetLo="-0.3" TestMode="FORCEI"/>
  <Delayms Value="80"/>
</DC_CONTACT_MODE_SETTING_OVERWRITE>
```

**Appears as:**
- 3 nodes in tree view
- Each node shows its attributes
- Numeric validation for voltage/current values

### Use Case 2: PCB Format Configuration
```xml
<PcbFormatConfig>
  <Island id="1">
    <StripUnitCount x="50" y="17" />
    <PanelStripCount x="2" y="4" />
  </Island>
</PcbFormatConfig>
```

**Appears as:**
- Nested tree structure
- Numeric validation for x, y coordinates
- Expandable/collapsible nodes

### Use Case 3: Custom Calibration Parameters
```xml
<CUSTOM_CALIBRATION_PARAMS>
  <Instrument Type="ENA" Timeout="30" />
  <Instrument Type="PSA" Timeout="20" />
</CUSTOM_CALIBRATION_PARAMS>
```

**Appears as:**
- 2 Instrument nodes
- Each with Type and Timeout attributes
- No code changes needed to support this

---

## ⚠️ Important Notes

### 1. **Auto-Detection**
- System automatically finds non-Package configurations
- No manual configuration needed
- New custom sections appear automatically

### 2. **No Code Changes Required**
- Add any new XML structure
- It will automatically appear in Advanced Settings
- Just ensure it's not using `<Package>` elements

### 3. **Validation is Optional**
- Numeric fields: auto-validated
- Dropdowns: add `<AttributeNameOptions>` to XML
- Free text: no validation

### 4. **Changes Must Be Saved**
- Edit changes only apply to in-memory XML
- Must click "💾 Save" in main window to persist
- Changes are NOT auto-saved

### 5. **Master XML vs Custom XML**
- Master XML: read-only template
- Custom XML: your editable configuration
- Advanced Settings edits your custom XML (or Master if no custom loaded)

---

## 🔧 Troubleshooting

### Problem: "No Advanced Settings Available"
**Cause:** Your XML only contains standard `<Package>` configurations

**Solution:**
- All your configurations use `<Package>` elements
- They are edited in the main wizard instead
- This is normal and expected

### Problem: Attribute not showing dropdown
**Cause:** Missing `<AttributeNameOptions>` definition

**Solution:**
```xml
<!-- Add this to enable dropdown -->
<TestModeOptions>FORCEV | FORCEI</TestModeOptions>
<Pin Name="Vdd" TestMode="FORCEV" />
```

### Problem: Can't enter decimal numbers
**Cause:** Field detected as non-numeric

**Solution:**
- Check if original value is text (e.g., "N/A")
- Edit XML directly to use numeric format
- Reload the file

### Problem: Changes not saved
**Cause:** Didn't click Save in main window

**Solution:**
1. Click "💾 Save Changes" in Advanced Settings dialog
2. Click "Yes" when closing dialog
3. Click "💾 Save" in main window toolbar

---

## 📚 Summary

| Feature | Behavior |
|---------|----------|
| **Auto-Detection** | ✅ Automatic - no configuration needed |
| **Custom Structures** | ✅ Supported - any XML structure works |
| **Numeric Validation** | ✅ Automatic - based on value format |
| **Dropdown Options** | ⚙️ Optional - add `<XxxOptions>` to XML |
| **Code Changes** | ❌ Not required - fully generic |
| **Safety** | ✅ Validation prevents invalid numeric input |

---

## 🎉 Benefits

1. ✅ **No code modifications** needed for new custom settings
2. ✅ **Automatic detection** of non-standard configurations
3. ✅ **Type-safe editing** with numeric validation
4. ✅ **Optional dropdown constraints** for controlled values
5. ✅ **Tree view** for easy navigation of nested structures
6. ✅ **Familiar UI pattern** consistent with main wizard

---

**Need help?** Contact support or check the main help guide (❓ Help button).





