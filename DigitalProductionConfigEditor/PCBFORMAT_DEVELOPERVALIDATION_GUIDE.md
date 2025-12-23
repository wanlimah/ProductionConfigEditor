# PCB Format Config & Developer Validation Config Guide

## Overview

The Digital Production Config Editor now supports **two additional configuration sections**:

1. **`<PcbFormatConfig>`** - PCB panel and island configuration
2. **`<DeveloperValidationConfig>`** - Developer validation and equipment checks

These sections can now be added, edited, and deleted directly from the application without manual XML editing.

---

## 🔲 PCB Format Config

### What is it?

The `<PcbFormatConfig>` section defines the physical layout of PCB panels, including:
- **Islands**: Different PCB configurations identified by ID
- **Strip Unit Count**: X and Y dimensions for strip units
- **Panel Strip Count**: X and Y dimensions for panel strips

### XML Structure

```xml
<PcbFormatConfig>
  <Island id="1">
    <StripUnitCount x="50" y="17" />
    <PanelStripCount x="2" y="4" />
  </Island>
  <Island id="2">
    <StripUnitCount x="52" y="17" />
    <PanelStripCount x="2" y="4" />
  </Island>
</PcbFormatConfig>
```

### How to Use

#### Adding PCB Format Config

1. **Open the application** and navigate to **Step 1: Build Your Configuration**
2. Scroll down in the left panel to find **"🔲 PCB Format Config"** section
3. Click **"➕ Add PCB Format Config"** button
4. The entire PcbFormatConfig section from the Master XML will be copied to your new configuration

**Note**: If PcbFormatConfig already exists, you'll be asked if you want to replace it.

#### Editing PCB Format Config

1. In the right panel, find the **"PCB Format Config:"** section
2. Click the **"✏ Edit"** button
3. The **Edit PCB Format Config** dialog will open
4. You can:
   - **Edit existing islands**: Modify Island ID, Strip Unit Count (X, Y), Panel Strip Count (X, Y)
   - **Add new islands**: Click **"➕ Add Island"** to create a new island with default values
   - **Delete islands**: Click **"🗑 Delete"** next to any island to remove it
5. Click **"💾 Save"** to apply changes or **"Cancel"** to discard

#### Island Configuration Details

Each island has the following properties:

| Property | Description | Example |
|----------|-------------|---------|
| **Island ID** | Unique identifier for the island | `1`, `2`, `3` |
| **Strip Unit X** | X dimension for strip unit count | `50`, `52` |
| **Strip Unit Y** | Y dimension for strip unit count | `17` |
| **Panel Strip X** | X dimension for panel strip count | `2` |
| **Panel Strip Y** | Y dimension for panel strip count | `4` |

**Validation**:
- All fields must be filled
- All numeric values must be valid integers
- Each island must have a unique ID

#### Deleting PCB Format Config

1. In the right panel, find the **"PCB Format Config:"** section
2. Click the **"🗑 Delete"** button
3. Confirm the deletion when prompted
4. The entire PcbFormatConfig section will be removed from your XML

---

## 🔧 Developer Validation Config

### What is it?

The `<DeveloperValidationConfig>` section contains validation checks for development and testing, including:
- Contactor ID validation
- Test board ID validation
- Equipment ID validation
- Self-calibration checks
- PXI calibration log verification
- GU expiration verification
- OTP burn module ID status

### XML Structure

```xml
<DeveloperValidationConfig>
  <CONTACTOR_ID>
    <Package name="SUSER" enable="FALSE" />
  </CONTACTOR_ID>
  <TESTBOARD_ID>
    <Package name="SUSER" enable="FALSE" />
  </TESTBOARD_ID>
  <EQUIPMENT_ID>
    <Package name="SUSER" enable="FALSE" />
  </EQUIPMENT_ID>
  <SELF_CALIBRATION>
    <Package name="SUSER" enable="FALSE" />
  </SELF_CALIBRATION>
  <PXI_CALIBRATION_LOG>
    <Package name="SUSER" enable="FALSE" />
  </PXI_CALIBRATION_LOG>
  <VERIFY_GU_EXPIRATION>
    <Package name="SUSER" enable="FALSE" />
  </VERIFY_GU_EXPIRATION>
  <OTP_BURN_MODULEID_STATUS>
    <Package name="SUSER" enable="FALSE" />
  </OTP_BURN_MODULEID_STATUS>
</DeveloperValidationConfig>
```

### How to Use

#### Adding Developer Validation Configs

1. **Open the application** and navigate to **Step 1: Build Your Configuration**
2. Scroll down in the left panel to find **"🔧 Developer Validation Config"** section
3. You'll see a list of available validation configurations:
   - `CONTACTOR_ID`
   - `TESTBOARD_ID`
   - `EQUIPMENT_ID`
   - `SELF_CALIBRATION`
   - `PXI_CALIBRATION_LOG`
   - `VERIFY_GU_EXPIRATION`
   - `OTP_BURN_MODULEID_STATUS`
4. Click **"➕ Add"** next to any configuration to add it to your XML
5. The configuration will appear in the right panel under **"Developer Validation Configs:"**

#### Managing Packages within Developer Validation Configs

Developer Validation Config nodes work **exactly like Production User Config nodes**:

1. **Add packages**: Select the configuration node in Step 2 and add packages
2. **Edit packages**: Modify the `enable` attribute and other properties
3. **Delete packages**: Remove packages you don't need

**Note**: The Developer Validation Config section is automatically created if it doesn't exist when you add your first validation configuration.

#### Deleting Developer Validation Configs

1. In the right panel, find the **"Developer Validation Configs:"** section
2. Find the configuration you want to delete (e.g., `CONTACTOR_ID`)
3. Click the **"🗑 Delete"** button next to it
4. Confirm the deletion when prompted
5. The configuration and all its packages will be removed

---

## UI Layout Changes

### Step 1: Build Your Configuration

The left panel now has **three sections**:

1. **📋 Production User Configs** (Purple)
   - Standard production configurations
   - Includes all the original configuration nodes

2. **🔧 Developer Validation Config** (Orange)
   - Developer validation and equipment checks
   - 7 validation configuration types

3. **🔲 PCB Format Config** (Blue)
   - PCB panel and island configuration
   - Single button to add entire config

### Right Panel Display

The right panel now displays:

1. **Production User Configs**
   - Shows all added production configuration nodes
   - Green color scheme

2. **Developer Validation Configs**
   - Shows all added developer validation nodes
   - Orange color scheme

3. **PCB Format Config**
   - Shows status: "Not Added" or "Added (X items)"
   - Edit and Delete buttons
   - Blue color scheme

---

## Example Workflow

### Scenario: Adding Both PcbFormatConfig and Developer Validation

**Goal**: Add PCB Format Config and Contactor ID validation to a new configuration

**Steps**:

1. **Load Master XML**: File → Load Master XML → Select `Master_Digital_ProductionUserConfig.xml`

2. **Create New XML**: File → Create New Blank XML

3. **Add Production Configs** (if needed):
   - Click "➕ Add" next to desired production configurations

4. **Add Developer Validation**:
   - Scroll to "🔧 Developer Validation Config" section
   - Click "➕ Add" next to `CONTACTOR_ID`
   - The configuration appears in the right panel

5. **Add PCB Format Config**:
   - Scroll to "🔲 PCB Format Config" section
   - Click "➕ Add PCB Format Config"
   - The config is copied to your XML

6. **Edit PCB Format Config**:
   - In the right panel, click "✏ Edit" next to PcbFormatConfig
   - Modify islands as needed (add, edit, or delete)
   - Click "💾 Save"

7. **Configure Packages**:
   - Go to Step 2 to edit package attributes
   - Add/remove packages as needed

8. **Review and Save**:
   - Go to Step 3 to review your complete configuration
   - Click "💾 Save XML"
   - Your file now includes:
     - ProductionUserConfigs
     - PcbFormatConfig
     - DeveloperValidationConfig

---

## Configuration Examples

### Example 1: Minimal Developer Validation Setup

```xml
<DeveloperValidationConfig>
  <CONTACTOR_ID>
    <Package name="SUSER" enable="FALSE" />
  </CONTACTOR_ID>
  <EQUIPMENT_ID>
    <Package name="SUSER" enable="FALSE" />
  </EQUIPMENT_ID>
</DeveloperValidationConfig>
```

### Example 2: Complete PCB Format Config with Multiple Islands

```xml
<PcbFormatConfig>
  <Island id="1">
    <StripUnitCount x="50" y="17" />
    <PanelStripCount x="2" y="4" />
  </Island>
  <Island id="2">
    <StripUnitCount x="52" y="17" />
    <PanelStripCount x="2" y="4" />
  </Island>
  <Island id="3">
    <StripUnitCount x="48" y="16" />
    <PanelStripCount x="3" y="4" />
  </Island>
</PcbFormatConfig>
```

### Example 3: Full Configuration with All Sections

```xml
<ProductionUserConfig>
  <ProductionUserConfigs viewer="false">
    <GU_ENGINEERING_MODE_ENABLE>
      <Package name="SUSER" enable="TRUE" />
    </GU_ENGINEERING_MODE_ENABLE>
    <MQTT_ENABLE>
      <Package name="SUSER" enable="FALSE" />
    </MQTT_ENABLE>
  </ProductionUserConfigs>
  
  <PcbFormatConfig>
    <Island id="1">
      <StripUnitCount x="50" y="17" />
      <PanelStripCount x="2" y="4" />
    </Island>
  </PcbFormatConfig>
  
  <DeveloperValidationConfig>
    <CONTACTOR_ID>
      <Package name="SUSER" enable="FALSE" />
    </CONTACTOR_ID>
    <SELF_CALIBRATION>
      <Package name="SUSER" enable="FALSE" />
    </SELF_CALIBRATION>
  </DeveloperValidationConfig>
</ProductionUserConfig>
```

---

## Tips & Best Practices

### PCB Format Config

✅ **DO**:
- Verify island dimensions match your physical PCB layout
- Use sequential island IDs (1, 2, 3...)
- Test with a small number of islands first
- Document island purposes in external documentation

❌ **DON'T**:
- Leave any X or Y values empty
- Use non-numeric values for dimensions
- Create duplicate island IDs

### Developer Validation Config

✅ **DO**:
- Add only the validation checks you need
- Start with basic validations (CONTACTOR_ID, EQUIPMENT_ID)
- Test in development mode (SUSER) first
- Enable validation one at a time

❌ **DON'T**:
- Enable all validations without understanding their purpose
- Mix development and production validation settings
- Delete validation configs that are actively used in tests

---

## Keyboard Shortcuts

| Action | Shortcut |
|--------|----------|
| Save XML | (via Save button) |
| Close Dialog | Esc |
| Confirm Dialog | Enter |

---

## Troubleshooting

### Issue: "No PcbFormatConfig found in Master XML"

**Cause**: The Master XML doesn't contain a PcbFormatConfig section.

**Solution**: 
1. Verify your Master XML file contains `<PcbFormatConfig>` section
2. Use the provided `Master_Digital_ProductionUserConfig.xml` file

### Issue: "PcbFormatConfig already exists"

**Cause**: You're trying to add PcbFormatConfig but it's already in your configuration.

**Solution**: 
- Click "Yes" to replace the existing configuration, or
- Click "No" to keep the current configuration and edit it instead

### Issue: Cannot save island with empty values

**Cause**: Some X or Y values are not filled in.

**Solution**: Fill in all X and Y values for Strip Unit Count and Panel Strip Count.

### Issue: Developer Validation Config not appearing

**Cause**: The DeveloperValidationConfig section may not be properly structured.

**Solution**: 
1. The section is auto-created when you add your first validation config
2. Ensure you're loading the correct Master XML file

---

## Summary

The enhanced Digital Production Config Editor now provides:

1. ✅ **PcbFormatConfig Management**
   - Add entire config from Master XML
   - Edit islands with visual dialog
   - Add/remove islands dynamically
   - Validate all numeric values

2. ✅ **DeveloperValidationConfig Management**
   - Add individual validation configurations
   - Manage packages like Production configs
   - Delete unused validations
   - Auto-create section when needed

3. ✅ **Integrated Workflow**
   - All three sections in one editor
   - Color-coded sections for clarity
   - Consistent add/edit/delete operations
   - Validation and error checking

This makes the tool a **complete solution** for managing all aspects of the Digital Production Configuration XML!

---

## Version Information

**Feature Added**: October 2025
**Compatible With**: Digital Production Config Editor v2.0+
**Supported Frameworks**: .NET 6.0-windows, .NET 8.0-windows

---

For more information on the existing features, refer to:
- `ADD_DELETE_PACKAGE_GUIDE.md` - Package management
- `DEVELOPER_GUIDE.md` - Developer documentation
- `USAGE_EXAMPLES.md` - Usage examples

















































