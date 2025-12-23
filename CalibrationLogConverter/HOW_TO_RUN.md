# 🚀 How to Run the Calibration Log Converter

## ✅ FIXED: Now Supports .NET 6.0 and .NET 8.0!

The application has been updated to target both .NET 6.0 and .NET 8.0, and includes comprehensive error handling.

---

## 📍 Quick Start - 3 Methods

### **Method 1: Double-Click Batch File (EASIEST)** ⭐

**Location:**
```
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\START_HERE.bat
```

**Steps:**
1. Navigate to the `CalibrationLogConverter` folder
2. Double-click `START_HERE.bat`
3. The application will launch with .NET 8.0
4. If any errors occur, they will be displayed

✅ **This is the recommended method!**

---

### **Method 2: Command Line with dotnet run**

**Open Command Prompt:**
```bash
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter
```

**For .NET 8.0:**
```bash
dotnet run --framework net8.0-windows
```

**For .NET 6.0:**
```bash
dotnet run --framework net6.0-windows
```

✅ Shows all error messages  
✅ Most reliable method  
✅ No deployment needed  

---

### **Method 3: Run the .EXE Directly**

**For .NET 8.0:**
```
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\
CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe
```

**For .NET 6.0:**
```
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\
CalibrationLogConverter\bin\Release\net6.0-windows\CalibrationLogConverter.exe
```

**Or use the batch files:**
- `bin\Release\net8.0-windows\RUN_ME.bat`
- `bin\Release\net6.0-windows\RUN_ME.bat`

---

## 🔧 What Was Fixed

### Issue: "Nothing appears when clicking .exe"

**Root Cause:** Application was targeting .NET 9.0, but you have .NET 8.0

**Solution Applied:**
1. ✅ Changed `TargetFramework` from `net9.0-windows` to `TargetFrameworks` (plural)
2. ✅ Added support for both `net6.0-windows` and `net8.0-windows`
3. ✅ Added comprehensive error handling to catch and display any startup issues
4. ✅ Created helper batch files for easy launching

**Changes Made:**
```xml
<!-- Before -->
<TargetFramework>net9.0-windows</TargetFramework>

<!-- After -->
<TargetFrameworks>net6.0-windows;net8.0-windows</TargetFrameworks>
```

---

## 📦 Build Outputs

After rebuild, you now have:

### .NET 8.0 Version
```
bin\Release\net8.0-windows\
├── CalibrationLogConverter.exe    ← Your executable
├── CalibrationLogConverter.dll
├── EPPlus.dll
├── ExcelDataReader.dll
├── RUN_ME.bat                      ← Helper script
└── [other dependencies]
```

### .NET 6.0 Version
```
bin\Release\net6.0-windows\
├── CalibrationLogConverter.exe    ← Your executable
├── CalibrationLogConverter.dll
├── EPPlus.dll
├── ExcelDataReader.dll
├── RUN_ME.bat                      ← Helper script
└── [other dependencies]
```

---

## 🛡️ Error Handling Added

The application now has global exception handlers that will:
- ✅ Catch any startup errors
- ✅ Display detailed error messages
- ✅ Show stack trace for debugging
- ✅ Prevent silent crashes

**Error Dialog Example:**
```
╔══════════════════════════════════════════╗
║  Application Startup Error               ║
╠══════════════════════════════════════════╣
║                                          ║
║  Error: [Error message here]             ║
║                                          ║
║  Type: [Exception type]                  ║
║                                          ║
║  Stack Trace:                            ║
║  [Detailed trace]                        ║
║                                          ║
║              [ OK ]                      ║
╚══════════════════════════════════════════╝
```

---

## ✨ Testing the Application

### Test 1: Launch the App
```bash
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter
START_HERE.bat
```

**Expected Result:**
- Window opens showing "Calibration Log File Converter"
- Status shows: "Found 2 Excel file(s) in Raw_Data folder"
- Files listed in Input Files section

### Test 2: Parse Files
1. Click **🔄 Parse Files** button
2. Wait for processing
3. Check for dialog: "Parsing Complete!"
4. Review preview grid

**Expected Result:**
- Dialog shows number of records extracted
- Preview grid displays calibration data
- Record count shown at bottom

### Test 3: Export to Excel
1. Click **💾 Export to Excel** button
2. Choose save location (e.g., Desktop)
3. Click Save
4. File opens automatically (if auto-open enabled)

**Expected Result:**
- Excel file created with 2 sheets
- Sheet 1: Calibration Records
- Sheet 2: Summary with statistics

---

## 🔍 Troubleshooting

### Issue: Batch file shows error

**Check:**
```bash
dotnet --list-runtimes
```

**Look for:**
- `Microsoft.WindowsDesktop.App 8.0.11` (or 8.0.x)
- `Microsoft.WindowsDesktop.App 6.0.36` (or 6.0.x)

**If missing:** Install .NET Desktop Runtime
- .NET 8.0: https://dotnet.microsoft.com/download/dotnet/8.0
- .NET 6.0: https://dotnet.microsoft.com/download/dotnet/6.0

---

### Issue: Error about missing Raw_Data folder

**Current Auto-Load Path:**
```
C:\Users\wanlimah\Documents\Raw_Data
```

**Solutions:**
1. Create the folder if it doesn't exist
2. Or click **Browse...** to select files manually
3. Or modify the path in code (see below)

**To Change Default Path:**
Edit `MainWindow.xaml.cs`, line 30:
```csharp
string defaultPath = @"C:\Your\Custom\Path";
```

---

### Issue: "EPPlus license" error

**Fixed!** The code now properly sets the EPPlus license for NonCommercial use.

---

### Issue: Application crashes immediately

**Now fixed with error handlers!** Any crash will show an error dialog with details.

**If you still see a crash**, please:
1. Run using `dotnet run --framework net8.0-windows`
2. Note the error message
3. Check Windows Event Viewer → Application logs

---

## 📊 Verified Setup

Your system has all required runtimes:

✅ **Microsoft.WindowsDesktop.App 8.0.11**  
✅ **Microsoft.WindowsDesktop.App 8.0.18**  
✅ **Microsoft.WindowsDesktop.App 6.0.36**  

✅ **Microsoft.NETCore.App 8.0.11**  
✅ **Microsoft.NETCore.App 8.0.18**  
✅ **Microsoft.NETCore.App 6.0.36**  

**Conclusion:** You have everything needed to run the application!

---

## 🎯 Next Steps

1. **Test the application:**
   ```bash
   START_HERE.bat
   ```

2. **If it works:**
   - Parse your calibration files
   - Review the preview
   - Export to Excel
   - Enjoy!

3. **If you see an error:**
   - Note the exact error message
   - Check which step fails
   - Review TROUBLESHOOTING_STARTUP.md

4. **Want to distribute:**
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true -f net8.0-windows
   ```
   Creates standalone version in:
   ```
   bin\Release\net8.0-windows\win-x64\publish\
   ```

---

## 📚 Documentation Files

| File | Purpose |
|------|---------|
| **HOW_TO_RUN.md** | This file - How to run the app |
| **START_HERE.bat** | Quick launcher script |
| **TROUBLESHOOTING_STARTUP.md** | Detailed troubleshooting |
| **USER_GUIDE.md** | Complete user manual |
| **README.md** | Developer documentation |
| **QUICK_START.txt** | Quick reference card |

---

## ✅ Summary

**Problem:** Application targeting .NET 9.0, you have .NET 8.0  
**Solution:** Updated to support .NET 6.0 and .NET 8.0  
**Status:** ✅ FIXED and Ready to Use  

**Recommended Way to Run:**
```
Double-click: START_HERE.bat
```

**Alternative:**
```bash
dotnet run --framework net8.0-windows
```

---

**🎉 The application is ready! Try START_HERE.bat now!**















