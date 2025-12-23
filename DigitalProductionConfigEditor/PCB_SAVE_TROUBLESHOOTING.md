# PCB Format Config Save - Troubleshooting Guide

## Issue Report
User reports: "Can't see PCB data in the XML after loading own XML, adding PCB, and saving"

---

## ✅ Enhancement Applied

### Added Debug Logging to Save Function

**File**: `ViewModels/WizardViewModel.cs` → `SaveXml()`

**Changes**:
1. ✅ Added formatted XML output (proper indentation)
2. ✅ Added debug logging to verify PcbFormatConfig exists before save
3. ✅ Added island count verification

### New Save Function Features:

```csharp
public void SaveXml(string path)
{
    // 1. Format XML properly
    var settings = new XmlWriterSettings
    {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n",
        Encoding = UTF8
    };

    // 2. Save with formatting
    using (var writer = XmlWriter.Create(path, settings))
    {
        NewXmlDocument.Save(writer);
    }

    // 3. Debug output
    Debug.WriteLine($"Saved XML - PcbFormatConfig: YES (2 islands)");
    Debug.WriteLine($"Saved XML - DeveloperValidationConfig: YES");
}
```

---

## Testing Steps

### Test 1: Create New XML with PCB Config

1. **Launch Application**
   - Run `DigitalProductionConfigEditor.exe`

2. **Load Master XML**
   - File → Load Master XML
   - Should see: "Loaded Master XML with X configuration nodes"

3. **Create New Blank XML**
   - File → Create New Blank XML
   - Should see: "Created new blank XML configuration"

4. **Add PCB Format Config**
   - Go to Step 1
   - Scroll to "🔲 PCB Format Config" section (blue)
   - Click "➕ Add PCB Format Config"
   - **Expected**: "Added PcbFormatConfig with 2 island(s) to your new XML"
   - **Check**: Right panel shows PcbFormatConfig with "Added (X items)"

5. **Edit PCB Format Config (Optional)**
   - Click "✏ Edit" button next to PcbFormatConfig
   - Modify islands or add new one
   - Click "💾 Save"
   - **Expected**: "PcbFormatConfig updated successfully"

6. **Navigate to Step 3**
   - Click "Next" twice to go to Step 3
   - **CHECK**: Look for "🔲 PCB Format Configuration" section
   - **Expected**: Should see PcbFormatConfig with islands displayed

7. **Save XML**
   - Click "💾 Save As..." button (top menu)
   - Choose filename: `Test_PCB_Config.xml`
   - Click Save
   - **Expected**: "XML saved successfully to: [path]"

8. **Verify Saved File**
   - Open `Test_PCB_Config.xml` in Notepad or VS Code
   - **Search for**: `<PcbFormatConfig>`
   - **Expected to see**:
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

---

### Test 2: Load Existing XML and Add PCB Config

1. **Load Your XML**
   - File → Load XML
   - Select your existing XML file

2. **Check Current State**
   - Go to Step 3 (Review)
   - Look for "🔲 PCB Format Configuration" section
   - **If present**: PCB config already exists
   - **If not present**: Continue to add it

3. **Go to Step 1**
   - Click "Back" to return to Step 1

4. **Add PCB Format Config**
   - Scroll to "🔲 PCB Format Config" section (blue)
   - Click "➕ Add PCB Format Config"
   - **If already exists**: Dialog asks "Replace existing?"
     - Click "Yes" to replace
     - Click "No" to keep current

5. **Edit If Needed**
   - Click "✏ Edit" button
   - Modify islands
   - Click "💾 Save"

6. **Save XML**
   - File → Save XML or Save As...
   - **Expected**: File saved with PcbFormatConfig

7. **Verify in File**
   - Open saved XML file
   - Search for `<PcbFormatConfig>`
   - **Should be present** between `</ProductionUserConfigs>` and `<DeveloperValidationConfig>`

---

## Common Issues & Solutions

### Issue 1: "Can't see PcbFormatConfig in saved XML"

**Possible Causes**:
1. ❌ PCB Config was not added to the XML
2. ❌ Viewing wrong XML file (viewing Master instead of saved file)
3. ❌ File was not saved after adding PCB Config

**Solution**:
1. ✅ Verify in Step 3 that PcbFormatConfig appears **before** saving
2. ✅ Make sure to click "Save XML" after adding PCB Config
3. ✅ Check you're opening the **correct saved file** (not Master)

### Issue 2: "PcbFormatConfig shows in Step 3 but not in saved file"

**Possible Causes**:
1. ❌ XML save failed silently
2. ❌ File permissions issue
3. ❌ Viewing cached/old version of file

**Solution**:
1. ✅ Check for error message when saving
2. ✅ Save to a different location (e.g., Desktop)
3. ✅ Close and reopen the saved file in text editor
4. ✅ Check the file's "Modified" timestamp

### Issue 3: "Edit dialog opens but changes don't save"

**Possible Causes**:
1. ❌ Clicked "Cancel" instead of "Save" in dialog
2. ❌ Validation errors prevented save

**Solution**:
1. ✅ Make sure to click "💾 Save" button (green)
2. ✅ Check all fields are filled:
   - Island ID
   - Strip Unit Count X, Y
   - Panel Strip Count X, Y
3. ✅ Check all values are numbers

---

## Debug Output

With the enhanced save function, debug information is now written to the Output window.

### To View Debug Output (Visual Studio):
1. Debug → Windows → Output
2. Look for lines like:
   ```
   Saved XML - PcbFormatConfig: YES (2 islands)
   Saved XML - DeveloperValidationConfig: YES
   ```

### To View Debug Output (DebugView):
1. Download DebugView from Microsoft Sysinternals
2. Run DebugView
3. Run the application
4. Save XML
5. See debug messages in DebugView

---

## Expected XML Structure

### Complete XML with All Sections

```xml
<?xml version="1.0" encoding="utf-8"?>
<ProductionUserConfig>
  <ProductionUserConfigs viewer="false">
    <!-- Production configs here -->
    <GU_ENGINEERING_MODE_ENABLE>
      <Package name="SUSER" enable="TRUE" />
    </GU_ENGINEERING_MODE_ENABLE>
  </ProductionUserConfigs>
  
  <PcbFormatConfig>                    ← SHOULD BE HERE
    <Island id="1">
      <StripUnitCount x="50" y="17" />
      <PanelStripCount x="2" y="4" />
    </Island>
    <Island id="2">
      <StripUnitCount x="52" y="17" />
      <PanelStripCount x="2" y="4" />
    </Island>
  </PcbFormatConfig>
  
  <DeveloperValidationConfig>         ← SHOULD BE AFTER PCB
    <CONTACTOR_ID>
      <Package name="SUSER" enable="FALSE" />
    </CONTACTOR_ID>
  </DeveloperValidationConfig>
</ProductionUserConfig>
```

---

## Verification Checklist

After saving, verify these in the saved XML file:

- [ ] XML declaration at top: `<?xml version="1.0" encoding="utf-8"?>`
- [ ] Root element: `<ProductionUserConfig>`
- [ ] ProductionUserConfigs section present
- [ ] **`<PcbFormatConfig>` section present**
- [ ] Islands inside PcbFormatConfig
- [ ] Each island has: id attribute, StripUnitCount, PanelStripCount
- [ ] DeveloperValidationConfig section (if added)
- [ ] Proper XML formatting (indented)

---

## Quick Test Command

Run this in PowerShell to check if PCB data is in your saved file:

```powershell
# Replace with your saved file path
$file = "Test_PCB_Config.xml"
Get-Content $file | Select-String -Pattern "PcbFormatConfig" -Context 0,5
```

**Expected Output**:
```
<PcbFormatConfig>
  <Island id="1">
    <StripUnitCount x="50" y="17" />
    <PanelStripCount x="2" y="4" />
  </Island>
```

**If no output**: PcbFormatConfig is NOT in the file!

---

## Summary

### What Was Fixed
✅ Enhanced SaveXml() with proper formatting
✅ Added debug logging for verification
✅ Improved XML indentation and readability

### What to Check
1. ✅ PcbFormatConfig appears in Step 3 before saving
2. ✅ Status message confirms "Added PcbFormatConfig"
3. ✅ Save dialog completes successfully
4. ✅ Saved file contains `<PcbFormatConfig>` section
5. ✅ Islands are present in the saved file

### If Still Not Working
1. Try the test steps above **exactly**
2. Check the saved file location (not Master)
3. Verify file was modified after save (check timestamp)
4. Send me the saved XML file to analyze

---

**Last Updated**: October 2025  
**Build Status**: ✅ Enhanced with debug logging  
**Test Status**: Ready for testing

















































