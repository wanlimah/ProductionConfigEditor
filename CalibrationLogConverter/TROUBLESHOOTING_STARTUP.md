# 🔧 Troubleshooting - Application Won't Start

## Issue: Nothing appears when clicking CalibrationLogConverter.exe

---

## ✅ Quick Fixes (Try These First)

### Fix 1: Use the Batch File
**Location:** 
- `bin\Release\net8.0-windows\RUN_ME.bat` (for .NET 8.0)
- `bin\Release\net6.0-windows\RUN_ME.bat` (for .NET 6.0)

**Action:**
1. Navigate to the folder above
2. Double-click `RUN_ME.bat`
3. The batch file will show any error messages

---

### Fix 2: Run from Command Prompt
```bash
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows
CalibrationLogConverter.exe
```

This will show any error messages in the console.

---

### Fix 3: Use `dotnet run` Instead
```bash
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter
dotnet run
```

This is the most reliable method.

---

## 🔍 Common Causes & Solutions

### Cause 1: Silent Crash (No Error Message)
**Symptoms:** App starts then immediately closes

**Solution A - Add Error Dialog:**
The app needs better error handling at startup. Let me add that...

**Solution B - Check Event Viewer:**
1. Press `Win + X` → Event Viewer
2. Go to: Windows Logs → Application
3. Look for errors with source ".NET Runtime"

---

### Cause 2: Missing Dependencies
**Symptoms:** Error about missing DLL files

**Check Required Files:**
```
net8.0-windows\
├── CalibrationLogConverter.exe          ✅
├── CalibrationLogConverter.dll          ✅
├── EPPlus.dll                           ✅ (must exist)
├── ExcelDataReader.dll                  ✅ (must exist)
├── ExcelDataReader.DataSet.dll          ✅ (must exist)
└── Many other DLL files...
```

**Solution:**
All dependencies should be there after build. If missing, run:
```bash
dotnet build -c Release
```

---

### Cause 3: Antivirus Blocking
**Symptoms:** File disappears or won't run

**Solution:**
1. Add folder to Windows Defender exclusions
2. Right-click CalibrationLogConverter.exe → Properties
3. Check if there's an "Unblock" button at the bottom
4. Click Unblock → OK

---

### Cause 4: Path to Raw_Data Folder
**Symptoms:** App crashes at startup when trying to load files

**Current Code Checks:**
```csharp
string defaultPath = @"C:\Users\wanlimah\Documents\Raw_Data";
if (Directory.Exists(defaultPath))
{
    LoadFilesFromDirectory(defaultPath);
}
```

**Quick Test:** Does this folder exist?
```
C:\Users\wanlimah\Documents\Raw_Data
```

**If folder doesn't exist:**
- App should still open (just won't auto-load files)
- Use Browse button to select files manually

---

## 🛠️ Better Approach: Self-Contained Deployment

To avoid runtime dependencies:

```bash
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter

# For .NET 8.0 (includes runtime)
dotnet publish -c Release -r win-x64 --self-contained true -f net8.0-windows

# Output location:
bin\Release\net8.0-windows\win-x64\publish\
```

This creates a standalone version with ALL dependencies included.

---

## 🧪 Debug Version

To see detailed error messages, I can modify the code to:

1. **Add Try-Catch in App.xaml.cs:**
```csharp
protected override void OnStartup(StartupEventArgs e)
{
    try
    {
        base.OnStartup(e);
    }
    catch (Exception ex)
    {
        MessageBox.Show(
            $"Startup Error:\n\n{ex.Message}\n\n{ex.StackTrace}",
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        Shutdown(1);
    }
}
```

2. **Add Error Handler in MainWindow Constructor:**
```csharp
public MainWindow()
{
    try
    {
        InitializeComponent();
        // ... existing code ...
    }
    catch (Exception ex)
    {
        MessageBox.Show(
            $"Initialization Error:\n\n{ex.Message}\n\n{ex.StackTrace}",
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}
```

Would you like me to add these error handlers?

---

## 📊 Verification Steps

### Step 1: Verify Build Output
```bash
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows
dir
```

**Should see:**
- CalibrationLogConverter.exe (around 200KB)
- CalibrationLogConverter.dll
- EPPlus.dll
- ExcelDataReader.dll
- Many other DLL files

### Step 2: Try Simple Test
```bash
CalibrationLogConverter.exe --version
```

Or just:
```bash
.\CalibrationLogConverter.exe
```

### Step 3: Check Task Manager
After clicking the .exe:
1. Open Task Manager (Ctrl + Shift + Esc)
2. Look for "CalibrationLogConverter"
3. If it appears then disappears immediately → Silent crash
4. If it doesn't appear at all → Antivirus or permissions issue

---

## 🎯 Recommended Solution

**Use `dotnet run` for now:**
```bash
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter
dotnet run --framework net8.0-windows
```

This is the most reliable method and will show any error messages.

**Or create a launcher script:**
Create `LaunchApp.bat` in the CalibrationLogConverter folder:
```batch
@echo off
cd /d "%~dp0CalibrationLogConverter"
echo Starting Calibration Log Converter...
dotnet run --framework net8.0-windows
pause
```

---

## 📞 Need More Help?

If the application still won't start, please provide:

1. **Error message** (if any appears)
2. **Windows Event Viewer** logs (if no message)
3. **Output from:**
   ```bash
   dotnet run --framework net8.0-windows
   ```

I can then add specific error handling or debugging code.

---

## ✨ Alternative: Use the Other Project Template

Since your DigitalProductionConfigEditor works fine, I can:
- Use the exact same project structure
- Copy working settings from that project
- Ensure compatibility

Would you like me to do this?















