# Quick Reference Card

## 🚀 Quick Start

1. **Launch** → Application starts with blank XML
2. **Step 1** → Add configurations from Master Template (left) to Your Config (right)
3. **Step 2** → Edit packages within configurations
4. **Step 3** → Review and save
5. **💾 Save** → Choose filename and location

---

## 🎯 Step 1: Build Configuration Structure

| Panel | Description | Actions |
|-------|-------------|---------|
| **📋 Master Template** (Left) | All available configurations | **➕ Add** - Copy to your config |
| **📝 Your New Configuration** (Right) | Your custom config | **🗑 Delete** - Remove configuration |

**Minimum Required:** At least 1 configuration added before clicking Next

---

## 📦 Step 2: Manage Packages

| Action | Button | Description |
|--------|--------|-------------|
| **Select Config** | Dropdown | Choose which configuration to edit |
| **Add Package** | ➕ Add New Package | Create a new package with custom attributes |
| **Edit Package** | Edit | Modify existing package attributes |
| **Delete Package** | Delete | Remove a package from the configuration |

---

## 💾 File Operations

| Button | Shortcut | Action |
|--------|----------|--------|
| **📄 New** | - | Create blank XML |
| **📂 Open** | - | Load existing XML |
| **🔄 Reload Master** | - | Refresh master template |
| **💾 Save** | - | Save to current file (or prompt if new) |
| **💾 Save As...** | - | Save to new file |

---

## 🎨 Color Code

| Color | Meaning |
|-------|---------|
| **Purple/Lavender** | Master Template (read-only) |
| **Green** | Your Configuration (editable) |
| **Light Blue** | Info/Reload actions |
| **Light Yellow** | Open file action |
| **Light Cyan** | New file action |
| **Light Green** | Save actions |
| **Light Coral** | Delete actions |

---

## ⚡ Navigation

```
Step 1: Build Structure → Step 2: Edit Packages → Step 3: Review & Save
   ↑                           ↑                        ↑
   ⬅ Back               ⬅ Back | Next ➡          ⬅ Back | 💾 Save
```

---

## 🎯 Common Tasks

### Create New Minimal Config
1. **📄 New** → **Add 2-3 configs** → **Next ➡** → **Edit packages** → **💾 Save**

### Modify Existing Config
1. **📂 Open** → Select file → **Delete unwanted configs** → **Next ➡** → **💾 Save**

### Copy Configurations from Master
1. Step 1 → **➕ Add** next to desired configs → **Next ➡** → **💾 Save**

### Delete Configuration
1. Step 1 → Find in right panel → **🗑 Delete** → Confirm

### Add Package
1. Step 2 → Select config → **➕ Add New Package** → Enter details → **Create Package**

### Delete Package
1. Step 2 → Select config → Find package → **Delete** → Confirm

---

## ✅ Validation Rules

- ✅ At least 1 configuration required in Step 1 to proceed
- ✅ Configuration names must be unique in your XML
- ✅ Package names must be unique within a configuration (warning shown)
- ✅ Package name is required when adding packages
- ✅ Attribute names are required when adding attributes

---

## 🔑 Key Concepts

| Term | Definition |
|------|------------|
| **Master XML** | Read-only template library (`Master_Digital_ProductionUserConfig.xml`) |
| **New XML** | Your editable configuration file |
| **Configuration Node** | Top-level category (e.g., `GU_ENGINEERING_MODE_ENABLE`) |
| **Package** | Product-specific settings within a configuration |
| **Attribute** | Key-value pairs on packages (e.g., `enable="TRUE"`) |

---

## ⚠️ Important Notes

- ⚠️ **Master XML is never modified** - it's your template library
- ⚠️ **Changes are not auto-saved** - always click 💾 Save
- ⚠️ **Deleting a configuration** removes all its packages
- ⚠️ **Creating New** clears unsaved changes
- ⚠️ **Opening a file** replaces current unsaved work

---

## 🆘 Common Errors

| Error Message | Solution |
|---------------|----------|
| "Please add at least one Configuration Node" | Add a config from Master Template in Step 1 |
| "Configuration already exists" | You already added this config, delete and re-add if needed |
| "Please select a configuration node first" | Select from dropdown in Step 2 |
| "Master XML not found" | Ensure `Master_Digital_ProductionUserConfig.xml` is present |

---

## 📁 File Structure

```
Your Application Folder/
├── Master_Digital_ProductionUserConfig.xml  ← Read-only template
├── My_Config.xml                            ← Your custom configs
├── Product_A_Config.xml                     ← Product-specific
└── Product_B_Config.xml                     ← Product-specific
```

---

## 🎓 Workflow Summary

```
[Launch App]
    ↓
[Master XML loaded] + [Blank new XML created]
    ↓
[Step 1: Add configs from Master → Your New Config]
    ↓
[Step 2: Add/Edit/Delete packages in each config]
    ↓
[Step 3: Review XML structure]
    ↓
[💾 Save to custom filename]
    ↓
[Done! ✅]
```

---

## 📊 Typical XML Structure

```xml
<?xml version="1.0" encoding="utf-8"?>
<ProductionUserConfig>
  <ProductionUserConfigs viewer="false">
    
    <ConfigurationNode1>
      <Package name="Product1" enable="TRUE" ... />
      <Package name="Product2" enable="FALSE" ... />
    </ConfigurationNode1>
    
    <ConfigurationNode2>
      <ModeOptions> OPTION1 | OPTION2 </ModeOptions>
      <Package name="Product1" mode="OPTION1" ... />
    </ConfigurationNode2>
    
  </ProductionUserConfigs>
</ProductionUserConfig>
```

---

## 💡 Pro Tips

- 💡 Start with New (📄) for clean configs
- 💡 Use Save As (💾) to create variants
- 💡 Name files by product: `ProductX_Config.xml`
- 💡 Keep Master XML as team template
- 💡 Create minimal configs (only what you need)
- 💡 Use Delete liberally - you can always re-add from Master

---

## 🌟 Benefits of New Workflow

✅ **Flexible** - Build exactly what you need  
✅ **Safe** - Master template protected  
✅ **Organized** - Multiple product-specific configs  
✅ **Efficient** - No unnecessary configurations  
✅ **Collaborative** - Share master, customize individually  

---

**Version:** 2.0 - New Workflow Edition  
**Last Updated:** October 2025

























































