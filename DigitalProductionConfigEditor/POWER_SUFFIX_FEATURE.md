# Power Suffix Feature for Package Names

## Overview

The "Add Package" dialog now supports an **optional power suffix** for product part numbers. Some products have a `-RF1-POWER` variant, and this feature makes it easy to add them.

---

## Feature Details

### Product Part Number Structure

Products can be:
1. **Base product**: `AFEM-8266-AP1-RF1-QA`
2. **With power suffix**: `AFEM-8266-AP1-RF1-QA-RF1-POWER`

The suffix pattern is: **`-RF1-POWER`**

---

## How to Use

### Adding a Package Without Power Suffix

1. Click "Add New Package"
2. Enter **Package Name**: `AFEM-8266-AP1-RF1-QA`
3. Leave "Add Power Suffix" **unchecked**
4. See preview: `AFEM-8266-AP1-RF1-QA`
5. Click "Create Package"

**Result**:
```xml
<Package name="AFEM-8266-AP1-RF1-QA" enable="FALSE" />
```

---

### Adding a Package WITH Power Suffix

1. Click "Add New Package"
2. Enter **Package Name**: `AFEM-8266-AP1-RF1-QA`
3. **Check** "Add Power Suffix" ✅
4. See preview update to: `AFEM-8266-AP1-RF1-QA-RF1-POWER`
5. Click "Create Package"

**Result**:
```xml
<Package name="AFEM-8266-AP1-RF1-QA-RF1-POWER" enable="FALSE" />
```

---

## UI Design

### Add Package Dialog

```
┌───────────────────────────────────────────────────────┐
│ Add New Package                                       │
├───────────────────────────────────────────────────────┤
│ Adding to: INLINE_SORTER_QUEEN_ENABLE                │
│                                                        │
│ Package Name*: [AFEM-8266-AP1-RF1-QA            ]    │
│                                                        │
│ ☑ Add Power Suffix    Result: AFEM-8266-AP1-RF1-QA-RF1-POWER
│                                                        │
│ Attributes:                                           │
│ enable: [FALSE ▼]  [✖]                               │
│ mode:   [AUTO  ▼]  [✖]                               │
│                                                        │
│ [➕ Add Attribute]                                    │
│                                                        │
│ 💡 Tips:                                              │
│ • Package Name is required (product part number)     │
│ • Check "Add Power Suffix" to append "-RF1-POWER"    │
│ • Example: AFEM-8266-AP1-RF1-QA becomes              │
│   AFEM-8266-AP1-RF1-QA-RF1-POWER                     │
│ • Attributes are pre-filled based on existing pkgs   │
│                                                        │
│              [Create Package]  [Cancel]               │
└───────────────────────────────────────────────────────┘
```

---

## Real-Time Preview

The dialog shows a **real-time preview** of the final package name:

### Example 1: Without Suffix
- **Enter**: `WW-PROD`
- **Checkbox**: ☐ Unchecked
- **Preview**: `WW-PROD`

### Example 2: With Suffix
- **Enter**: `AFEM-8266-AP1-RF1-QA`
- **Checkbox**: ☑ Checked
- **Preview**: `AFEM-8266-AP1-RF1-QA-RF1-POWER`

### Example 3: As You Type
- **Enter**: `AFEM-8`
- **Checkbox**: ☑ Checked
- **Preview**: `AFEM-8-RF1-POWER` (updates in real-time)

---

## Use Cases

### Use Case 1: Standard Product
```
Configuration: GU_ENGINEERING_MODE_ENABLE
Package: SUSER
No power suffix needed ☐
```

### Use Case 2: Power Variant Product
```
Configuration: INLINE_SORTER_QUEEN_ENABLE
Package: AFEM-8266-AP1-RF1-QA-RF1-POWER
Power suffix needed ☑
```

### Use Case 3: Mixed Configuration
A configuration can have both:
```xml
<INLINE_SORTER_QUEEN_ENABLE>
  <Package name="SUSER" enable="FALSE" mode="AUTO" />
  <Package name="AFEM-8266-AP1-RF1-QA" enable="FALSE" mode="AUTO" />
  <Package name="AFEM-8266-AP1-RF1-QA-RF1-POWER" enable="FALSE" mode="AUTO" />
</INLINE_SORTER_QUEEN_ENABLE>
```

---

## Technical Implementation

### Code Logic

```csharp
private string GetFinalPackageName()
{
    var baseName = PackageNameTextBox.Text.Trim();
    
    if (string.IsNullOrWhiteSpace(baseName))
    {
        return "";
    }

    // Add power suffix if checkbox is checked
    if (AddPowerSuffixCheckBox.IsChecked == true)
    {
        return $"{baseName}-RF1-POWER";
    }
    
    return baseName;
}
```

### Real-Time Preview

```csharp
private void UpdatePackageNamePreview()
{
    var baseName = PackageNameTextBox.Text.Trim();
    var finalName = GetFinalPackageName();
    
    if (string.IsNullOrWhiteSpace(baseName))
    {
        PreviewNameTextBlock.Text = "(enter package name above)";
        PreviewNameTextBlock.Foreground = Brushes.Gray;
    }
    else
    {
        PreviewNameTextBlock.Text = finalName;
        PreviewNameTextBlock.Foreground = Brushes.DarkBlue;
    }
}
```

### Event Handling

- **TextChanged**: Updates preview as user types
- **Checked/Unchecked**: Updates preview when checkbox changes
- **Create Button**: Uses final name with suffix if checked

---

## Benefits

### ✅ User-Friendly
- No need to manually type the full suffix
- Real-time preview shows exactly what will be created
- One checkbox to toggle the suffix

### ✅ Error-Free
- Consistent suffix format: `-RF1-POWER`
- No typos in the suffix
- Validation still checks for duplicates

### ✅ Flexible
- Works with any base product name
- Can add suffix to any configuration
- User decides if suffix is needed

### ✅ Maintains Naming Convention
- Follows existing naming patterns in XML
- Example from XML: `AFEM-8266-AP1-RF1-QA-RF1-POWER`
- Suffix format is standardized

---

## Example Workflows

### Workflow 1: Adding Power Variant

**Goal**: Add `AFEM-8266-AP1-RF1-QA-RF1-POWER` to `INLINE_SORTER_QUEEN_ENABLE`

**Steps**:
1. Step 1: Select `INLINE_SORTER_QUEEN_ENABLE` → Next
2. Step 2: Click "Add New Package"
3. Enter Package Name: `AFEM-8266-AP1-RF1-QA`
4. ☑ Check "Add Power Suffix"
5. Verify preview: `AFEM-8266-AP1-RF1-QA-RF1-POWER`
6. Modify attributes if needed
7. Click "Create Package"
8. Click "Save XML"

**Result**:
```xml
<INLINE_SORTER_QUEEN_ENABLE>
  <ModeOptions> AUTO | MANUAL </ModeOptions>
  <Package name="SUSER" enable="FALSE" mode="AUTO" />
  <Package name="AFEM-8266-AP1-RF1-QA" enable="FALSE" mode="AUTO" />
  <Package name="AFEM-8266-AP1-RF1-QA-RF1-POWER" enable="FALSE" mode="AUTO" />
</INLINE_SORTER_QUEEN_ENABLE>
```

---

### Workflow 2: Adding Base Product (No Suffix)

**Goal**: Add `NEW-PRODUCT-2024` without suffix

**Steps**:
1. Enter Package Name: `NEW-PRODUCT-2024`
2. ☐ Leave "Add Power Suffix" **unchecked**
3. Verify preview: `NEW-PRODUCT-2024`
4. Click "Create Package"

**Result**:
```xml
<Package name="NEW-PRODUCT-2024" enable="FALSE" />
```

---

## Summary

The Power Suffix feature makes it easy to create product variants:

| Feature | Description |
|---------|-------------|
| **Checkbox** | "Add Power Suffix" toggle |
| **Suffix Format** | `-RF1-POWER` |
| **Preview** | Real-time update as you type |
| **Use Case** | Products like AFEM-8266-AP1-RF1-QA-RF1-POWER |
| **Validation** | Still checks for duplicate names |

**Examples**:
- Base: `AFEM-8266-AP1-RF1-QA`
- With suffix: `AFEM-8266-AP1-RF1-QA-RF1-POWER`

**Benefits**: Error-free, consistent naming, user-friendly! ✅

