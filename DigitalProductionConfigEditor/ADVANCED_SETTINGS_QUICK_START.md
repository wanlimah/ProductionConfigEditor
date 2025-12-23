# Advanced Settings - Quick Start ⚙️

## 🚀 3-Step Quick Start

### Step 1: Open Advanced Settings
```
Main Window → Click "⚙️ Advanced Settings" button
```

### Step 2: Edit Attributes
```
1. Select a node in tree view (left panel)
2. Edit attributes in right panel
3. Click "💾 Save Changes"
```

### Step 3: Save to File
```
1. Close dialog (click "Yes" to save)
2. Main window → Click "💾 Save"
```

---

## 🎯 What Will I See?

Any XML configuration that **doesn't use `<Package>` elements**, such as:

```xml
<DC_CONTACT_MODE_SETTING_OVERWRITE>
  <Pin Name="Vdd" VSet="0.1" ISet="10e-6" TestMode="FORCEV"/>
  <Delayms Value="80"/>
</DC_CONTACT_MODE_SETTING_OVERWRITE>
```

**Appears in Advanced Settings as:**
```
📁 DC_CONTACT_MODE_SETTING_OVERWRITE
   ├─ 📄 Pin [Name="Vdd"]
   └─ 📄 Delayms
```

---

## 🔍 Attribute Types

| Type | Example | Input |
|------|---------|-------|
| **Numeric** | `10e-6`, `0.1`, `-0.3` | Validates numbers only |
| **Dropdown** | `TestMode="FORCEV"` | Select from list |
| **Text** | `Name="Vdd"` | Free text |

---

## 💡 Pro Tips

### ✅ Add Dropdown Options
```xml
<!-- Add this line to create dropdown for TestMode attribute -->
<TestModeOptions>FORCEV | FORCEI | MEASURE</TestModeOptions>
<Pin Name="Vdd" TestMode="FORCEV" />
```

### ✅ Numeric Validation
- Automatically detects numeric values
- Accepts scientific notation: `10e-6`, `1.5e-6`
- Rejects invalid input: `abc`, `10x6`

### ✅ Auto-Detection
- New custom sections appear automatically
- No code changes needed
- Just add XML and reload

---

## ⚠️ Common Issues

### "No Advanced Settings Available"
→ **All your configs use `<Package>` format** (this is normal)
→ Edit them in main wizard instead

### Changes not saved
→ **Must click "💾 Save" in main window** after closing dialog

### Can't add dropdown
→ **Add `<AttributeNameOptions>VALUE1 | VALUE2</AttributeNameOptions>` to XML**

---

## 📖 Full Documentation

See **ADVANCED_SETTINGS_GUIDE.md** for complete details.

---

**Questions?** Click ❓ Help button in main window.





