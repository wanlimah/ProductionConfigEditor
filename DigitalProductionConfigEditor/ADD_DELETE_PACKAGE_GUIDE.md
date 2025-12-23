# Add/Delete Package Feature Guide

## Overview

The Digital Production Config Editor now supports **adding new packages** and **deleting existing packages** with user-entered package names and custom attributes. This guide explains the new functionality.

---

## New Features

### ✅ 1. Configuration Node Selection
- **Step 1** now shows a dropdown to select Configuration Nodes (e.g., `GU_ENGINEERING_MODE_ENABLE`, `DC_PDM_TRACE_ENABLE`)
- View all packages within the selected configuration node
- See total package count for each configuration

### ✅ 2. Add New Package
- Click **"➕ Add New Package"** button
- Enter custom **Package Name** (product part number)
- Add any custom attributes (e.g., `enable`, `count`, `sampling`, `threshold`, etc.)
- System validates for duplicate package names

### ✅ 3. Delete Package
- Click **"Delete"** button next to any package
- Confirmation dialog prevents accidental deletion
- Package is immediately removed from the XML

### ✅ 4. Edit Package
- Click **"Edit"** button to modify existing package attributes
- Dynamic attribute editor adapts to the package's current attributes

---

## How to Use

### Adding a New Package

#### Step 1: Select Configuration Node
1. Launch the application
2. Go to **Step 1: Select Configuration Node**
3. Select the configuration node from the dropdown (e.g., `GU_ENGINEERING_MODE_ENABLE`)
4. View the list of existing packages in that configuration

#### Step 2: Click "Add New Package"
1. Click the **"➕ Add New Package"** button
2. The "Add New Package" dialog will open

#### Step 3: Enter Package Details
1. **Package Name*** (Required): Enter the product part number
   - Examples: `WW-PROD`, `SUSER`, `AFEM-8266-AP1-RF1-QA`, `8267-PROD`
   - This becomes the `name` attribute in the XML

2. **Add Attributes**: The dialog starts with the `enable` attribute by default
   - Modify the default value if needed (TRUE/FALSE)

3. **Add More Attributes**: Click **"➕ Add Attribute"** to add additional attributes
   - Select from common attributes or enter custom attribute names
   - Common attributes:
     - `enable`: TRUE or FALSE
     - `count`: Number (e.g., 5, 10, 100)
     - `sampling`: Number (e.g., 5, 10)
     - `threshold`: Number (e.g., 99, 95)
     - `mode`: AUTO, MANUAL, SWEEP, POINT
     - `rule`: DATETIME, REV
     - `avg_channel`: ALL, EACH
     - `url`: Web service URL
     - `note`: Description text

4. **Remove Attributes**: Click the **✖** button next to any attribute to remove it

#### Step 4: Create the Package
1. Click **"Create Package"** button
2. The new package is added to the selected configuration node
3. You'll see a confirmation message

#### Step 5: Save the XML
1. Click the **"Save XML"** button in the main window
2. The XML file is saved with your new package

---

### Deleting a Package

#### Step 1: Select Configuration Node
1. Go to **Step 1: Select Configuration Node**
2. Select the configuration node containing the package you want to delete

#### Step 2: Click "Delete"
1. Find the package in the list
2. Click the **"Delete"** button next to the package

#### Step 3: Confirm Deletion
1. A confirmation dialog appears: "Are you sure you want to delete the package 'XXX'?"
2. Click **"Yes"** to confirm or **"No"** to cancel

#### Step 4: Save the XML
1. Click the **"Save XML"** button to persist the changes

---

## Example Workflow

### Example 1: Adding a New Production Package

**Goal**: Add a new package `NEW-PROD-2024` with `enable="TRUE"` and `count="57"` to `PARAM_COUNT_CHECK`

**Steps**:
1. **Step 1**: Select `PARAM_COUNT_CHECK` from the dropdown
2. Click **"➕ Add New Package"**
3. Enter **Package Name**: `NEW-PROD-2024`
4. Set **enable**: `TRUE`
5. Click **"➕ Add Attribute"**
6. Select **count**, enter value: `57`
7. Click **"Create Package"**
8. Click **"Save XML"**

**Result**:
```xml
<PARAM_COUNT_CHECK>
  <Package name="SUSER" enable="FALSE" count="0" />
  <Package name="8266-PROD" enable="TRUE" count="0" />
  <!-- ... other packages ... -->
  <Package name="NEW-PROD-2024" enable="TRUE" count="57" />  ← NEW!
</PARAM_COUNT_CHECK>
```

---

### Example 2: Adding a Package with Multiple Attributes

**Goal**: Add a new package `TEST-QA-SETUP` with multiple attributes to `ENA_AVERAGE_MODE_ON`

**Steps**:
1. **Step 1**: Select `ENA_AVERAGE_MODE_ON`
2. Click **"➕ Add New Package"**
3. Enter **Package Name**: `TEST-QA-SETUP`
4. Set **enable**: `FALSE`
5. Add **count**: `5`
6. Add **mode**: `SWEEP`
7. Add **avg_channel**: `ALL`
8. Click **"Create Package"**
9. Click **"Save XML"**

**Result**:
```xml
<ENA_AVERAGE_MODE_ON>
  <ModeOptions> SWEEP | POINT </ModeOptions>
  <AvgChannelOptions> ALL | EACH </AvgChannelOptions>
  <Package name="SUSER" enable="FALSE" count="5" mode="SWEEP" avg_channel="ALL" />
  <Package name="TEST-QA-SETUP" enable="FALSE" count="5" mode="SWEEP" avg_channel="ALL" />  ← NEW!
</ENA_AVERAGE_MODE_ON>
```

---

## Validation Features

### ✅ Duplicate Package Name Warning
- If you try to add a package with a name that already exists in the same configuration, you'll get a warning
- You can choose to add it anyway (for special cases) or cancel

### ✅ Required Fields
- **Package Name** is mandatory - you cannot create a package without a name
- **Attribute Name** is mandatory when adding attributes

### ✅ Reserved Attribute Names
- The `name` attribute is automatically set to the Package Name
- You cannot manually add a `name` attribute

---

## Technical Details

### Updated Components

#### 1. WizardViewModel.cs
**New Properties**:
- `SelectedConfigurationNode`: Currently selected configuration node
- `ConfigurationNodes`: All configuration nodes in the XML
- `PackagesInSelectedConfiguration`: All packages under the selected configuration

**New Methods**:
- `AddPackageToConfiguration(string packageName, Dictionary<string, string> attributes)`: Adds a new package
- `DeletePackage(XmlNode packageNode)`: Deletes a package
- `GetAvailableAttributeNames()`: Returns common attribute names from the configuration

#### 2. Step1_SelectNode.xaml
**New UI Elements**:
- Configuration Node dropdown selector
- Package list with Edit/Delete buttons
- Add New Package button
- Configuration info panel

#### 3. New Dialogs
- **AddPackageDialog**: Dialog for adding new packages with custom attributes
- **AddAttributeDialog**: Dialog for adding individual attributes to a package

---

## XML Structure

### Before (Static Editing)
You could only edit existing packages:
```xml
<GU_ENGINEERING_MODE_ENABLE>
  <Package name="SUSER" enable="TRUE" />
  <Package name="WW-PROD" enable="TRUE" />
</GU_ENGINEERING_MODE_ENABLE>
```

### After (Dynamic Add/Delete)
You can now:
- ✅ Add new packages with custom names
- ✅ Delete existing packages
- ✅ Edit packages with dynamic attributes

```xml
<GU_ENGINEERING_MODE_ENABLE>
  <Package name="SUSER" enable="TRUE" />
  <Package name="WW-PROD" enable="TRUE" />
  <Package name="NEW-CUSTOM-PRODUCT" enable="FALSE" />  ← Added by user
</GU_ENGINEERING_MODE_ENABLE>
```

---

## Benefits

### 🎯 1. Flexibility
- Add any product part number without modifying the code
- Create packages with any combination of attributes

### 🎯 2. User-Friendly
- Intuitive dialog-based interface
- Common attributes suggested automatically
- Validation prevents errors

### 🎯 3. Safe Operations
- Confirmation dialogs for destructive actions (delete)
- Duplicate name warnings
- Required field validation

### 🎯 4. Efficient Workflow
- Quickly add multiple packages to a configuration
- Manage packages without manual XML editing
- Immediate visual feedback

---

## Tips & Best Practices

### ✅ Package Naming Convention
Follow your organization's naming convention for product part numbers:
- Production: `WW-PROD`, `RS-PROD`, `8266-PROD`
- QA/Testing: `WW-QA`, `AFEM-8266-AP1-RF1-QA`
- Special modes: `SUSER`, `WW-REQ`, `WW-PS`, `WW-SMPL`

### ✅ Attribute Consistency
- Use consistent attribute names within the same configuration node
- Check existing packages to see what attributes they use
- The system auto-suggests attributes from existing packages

### ✅ Enable Values
- Use `TRUE` or `FALSE` (all caps) for consistency
- Default is `FALSE` unless specified otherwise

### ✅ Save Regularly
- Click "Save XML" after adding/deleting packages
- The updated XML is saved as `Master_Digital_ProductionUserConfig_Updated.xml`

---

## Troubleshooting

### Issue: "Please select a Configuration Node first"
**Solution**: Select a configuration node from the dropdown in Step 1 before clicking "Add New Package"

### Issue: Duplicate package name warning
**Solution**: Either:
- Use a different package name
- Choose "Yes" to add it anyway (if you have a specific reason)

### Issue: Cannot add `name` attribute
**Solution**: The `name` attribute is automatically set to the Package Name you enter. Enter it in the Package Name field instead.

---

## Summary

The Add/Delete Package feature gives you complete control over your XML configuration:

1. **Select** a configuration node
2. **Add** new packages with custom names and attributes
3. **Edit** existing packages dynamically
4. **Delete** packages you no longer need
5. **Save** your changes to XML

This makes the Digital Production Config Editor a truly dynamic tool that adapts to your needs without requiring code changes!

