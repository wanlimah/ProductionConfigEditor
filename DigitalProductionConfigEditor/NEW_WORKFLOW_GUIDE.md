# New Workflow Guide - Build Your Own Configuration

## Overview

The Digital Production Config Editor now supports a **completely new workflow** where you:

1. **Start with a blank XML file**
2. **Select configuration nodes** from a Master Template
3. **Add them to your new configuration** 
4. **Delete unwanted configurations**
5. **Edit packages within each configuration**
6. **Save your custom XML file**

This approach gives you complete control to build exactly the configuration you need!

---

## Key Concepts

### 🎯 Master Template XML
- **Read-only source** containing all available configuration nodes
- Located at: `Master_Digital_ProductionUserConfig.xml`
- Acts as a template library you can pick from
- Never modified by your edits

### 📝 Your New Configuration XML
- **Your editable document** that you're building
- Starts blank or load an existing file
- You add only the configurations you need
- Can be saved anywhere with any name

---

## Step-by-Step Workflow

### Step 1: Build Your Configuration

When you launch the application, you'll see:

**LEFT PANEL - Master Template**
- Shows all available configuration nodes from the master XML
- Each node displays its name and number of packages
- Click **"➕ Add"** to copy a configuration to your new XML

**RIGHT PANEL - Your New Configuration**
- Shows the configurations you've added to your new XML
- Initially empty (0 configurations)
- Each node can be deleted with **"🗑 Delete"**

#### How to Add a Configuration Node

1. Browse the **Master Template** list on the left
2. Find a configuration you want (e.g., `GU_ENGINEERING_MODE_ENABLE`)
3. Click the **"➕ Add"** button next to it
4. The configuration appears in **Your New Configuration** on the right
5. Repeat for all configurations you need

#### How to Delete a Configuration Node

1. Find the configuration in **Your New Configuration** (right panel)
2. Click the **"🗑 Delete"** button next to it
3. Confirm the deletion
4. The configuration is removed from your XML

**✅ Important:** You must add at least one configuration before proceeding to Step 2!

---

### Step 2: Edit Packages and Attributes

After adding configurations in Step 1, click **"Next ➡"** to edit packages:

1. **Select a Configuration Node** from the dropdown
2. **View all packages** within that configuration
3. **Add new packages** with the **"➕ Add New Package"** button
4. **Edit existing packages** by clicking **"Edit"**
5. **Delete packages** by clicking **"Delete"**

This step works exactly as before - you can manage packages and their attributes dynamically.

---

### Step 3: Review and Save

Click **"Next ➡"** again to review your complete configuration:

1. See a summary of all your changes
2. Review the XML structure
3. Click **"💾 Save"** to save your configuration

---

## File Operations

### 📄 New - Create a Blank Configuration

**Button:** `📄 New`

- Creates a completely blank XML file
- Starts with empty `<ProductionUserConfigs>` root
- You build it from scratch by adding configurations from the Master Template

**Use Case:** Starting a new configuration from zero

---

### 📂 Open - Load an Existing Configuration

**Button:** `📂 Open`

- Opens an XML file you previously saved
- Loads it as your "New Configuration"
- You can continue editing it

**Use Case:** Resume working on a configuration you started earlier

---

### 🔄 Reload Master - Refresh the Template Library

**Button:** `🔄 Reload Master`

- Reloads the `Master_Digital_ProductionUserConfig.xml` template
- Updates the Master Template list on the left
- Does NOT affect your new configuration

**Use Case:** Master XML was updated by someone else

---

### 💾 Save - Save Your Configuration

**Button:** `💾 Save`

- Saves your new XML configuration
- If it's a new file, prompts for location and filename
- If previously saved, overwrites the existing file

**Use Case:** Save your work

---

### 💾 Save As... - Save to a New File

**Button:** `💾 Save As...`

- Always prompts for a new location and filename
- Saves a copy of your configuration
- Future saves will use this new location

**Use Case:** Create a backup or variant of your configuration

---

## Example Workflows

### Example 1: Creating a Minimal Configuration

**Goal:** Create a small XML with only 2 specific configurations

**Steps:**

1. Launch the application
2. Click **"📄 New"** (starts blank)
3. In Step 1:
   - Click **"➕ Add"** next to `GU_ENGINEERING_MODE_ENABLE`
   - Click **"➕ Add"** next to `PARAM_COUNT_CHECK`
4. Click **"Next ➡"** to proceed to Step 2
5. Select `GU_ENGINEERING_MODE_ENABLE` from the dropdown
6. Edit or add packages as needed
7. Select `PARAM_COUNT_CHECK` from the dropdown
8. Edit or add packages as needed
9. Click **"💾 Save"**
10. Choose filename: `Minimal_Config.xml`

**Result:**
```xml
<?xml version="1.0" encoding="utf-8"?>
<ProductionUserConfig>
  <ProductionUserConfigs viewer="false">
    <GU_ENGINEERING_MODE_ENABLE>
      <Package name="SUSER" enable="TRUE" />
      <Package name="WW-PROD" enable="TRUE" />
    </GU_ENGINEERING_MODE_ENABLE>
    <PARAM_COUNT_CHECK>
      <Package name="SUSER" enable="FALSE" count="0" />
      <Package name="8266-PROD" enable="TRUE" count="0" />
    </PARAM_COUNT_CHECK>
  </ProductionUserConfigs>
</ProductionUserConfig>
```

---

### Example 2: Copying Specific Configurations for a Product

**Goal:** Create a configuration specific to product `8266-PROD`

**Steps:**

1. Launch the application
2. Click **"📄 New"**
3. In Step 1, add these configurations from the Master:
   - `GU_ENGINEERING_MODE_ENABLE`
   - `PARAM_COUNT_CHECK`
   - `ENA_AVERAGE_MODE_ON`
   - `STOP_TEST_ON_CONTINUOUS_FAILURE_COUNT_1A`
4. Click **"Next ➡"**
5. For each configuration, keep only packages relevant to `8266-PROD`:
   - Delete unwanted packages
   - Add new packages if needed
6. Click **"💾 Save As..."**
7. Save as: `8266-PROD_Config.xml`

---

### Example 3: Modifying an Existing Configuration

**Goal:** Load an existing XML, remove some configurations, save it

**Steps:**

1. Launch the application
2. Click **"📂 Open"**
3. Select your existing XML file (e.g., `My_Old_Config.xml`)
4. In Step 1, review **Your New Configuration** (right panel)
5. Click **"🗑 Delete"** next to configurations you don't need
6. Optionally add new configurations from the Master Template
7. Click **"Next ➡"** to edit packages
8. Make any package-level changes
9. Click **"💾 Save"** to overwrite the file
10. Or click **"💾 Save As..."** to save as a new file

---

## Comparison: Old vs New Workflow

### Old Workflow
❌ You edited the master XML directly  
❌ All configurations were always present  
❌ Hard to create minimal configs  
❌ Risk of corrupting the master template  

### New Workflow
✅ Master XML is read-only (protected)  
✅ Build your config from scratch  
✅ Add only what you need  
✅ Create multiple custom configurations  
✅ Easy to maintain separate configs per product  

---

## Tips & Best Practices

### ✅ Start Small
- Begin with a blank XML and add only configurations you actually need
- Don't copy everything from the master

### ✅ Organize by Product
- Create separate XML files for different products
- Name them clearly: `ProductX_Config.xml`, `ProductY_Config.xml`

### ✅ Save Frequently
- Click **"💾 Save"** after making significant changes
- Use **"💾 Save As..."** to create backups before major edits

### ✅ Master Template is Shared
- The Master XML is your team's template library
- Your individual configs are personal/product-specific
- Keep the Master clean and well-organized

### ✅ Delete Wisely
- When deleting a configuration, all its packages are removed
- Use Step 2 to delete individual packages instead

---

## Keyboard Shortcuts & Navigation

- **Next ➡** - Move forward through the wizard steps
- **⬅ Back** - Move backward through the wizard steps
- **Step 1** - Build configuration structure (add/delete nodes)
- **Step 2** - Edit packages and attributes
- **Step 3** - Review and save

---

## Troubleshooting

### Issue: "Please add at least one Configuration Node"
**Cause:** You clicked "Next" from Step 1 without adding any configurations

**Solution:** 
1. Go back to Step 1
2. Click **"➕ Add"** next to at least one configuration in the Master Template list

---

### Issue: "Configuration already exists in your new XML"
**Cause:** You tried to add a configuration that's already in your new XML

**Solution:** 
- Each configuration can only be added once
- If you need to edit it, proceed to Step 2
- If you want to reset it, delete it first and re-add it

---

### Issue: Master Template list is empty
**Cause:** Master XML file not found or empty

**Solution:**
1. Ensure `Master_Digital_ProductionUserConfig.xml` is in the application folder
2. Click **"🔄 Reload Master"** to reload it

---

### Issue: Changes not saved
**Cause:** You didn't click the Save button

**Solution:**
- Always click **"💾 Save"** or **"💾 Save As..."** before closing
- The application doesn't auto-save

---

## Summary

The new workflow empowers you to:

1. **🏗 Build** custom configurations from a master template
2. **✂️ Select** only the configurations you need
3. **✏️ Edit** packages and attributes dynamically
4. **💾 Save** multiple configuration files for different purposes
5. **🔒 Protect** the master template from accidental changes

This makes the Digital Production Config Editor a powerful tool for managing product-specific configurations while maintaining a shared master template!

---

## Quick Start Checklist

- [ ] Launch the application
- [ ] Click **"📄 New"** to start fresh
- [ ] Add configurations from the Master Template (left panel)
- [ ] Click **"Next ➡"** when you have at least 1 configuration
- [ ] Edit packages in Step 2
- [ ] Review in Step 3
- [ ] Click **"💾 Save"** and choose a filename
- [ ] Done! Your custom configuration is ready

Enjoy building your configurations! 🎉

























































