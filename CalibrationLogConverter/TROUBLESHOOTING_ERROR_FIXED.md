# ✅ Error Fixed - Troubleshooting Guide

## The Error You Encountered

```
Application Error
Error: The invocation of the constructor on type 
'CalibrationLogConverter.MainWindow' that matches the specified 
binding constraints threw an exception.

Type: XamlParseException
```

## ✅ ROOT CAUSE

The error was caused by **leftover build artifacts** from a deleted backup file (`MainWindow_BACKUP.xaml`). Even though the XAML file was deleted, the compiled versions (.baml and .g.cs files) remained in the build output, causing conflicts.

## ✅ SOLUTION APPLIED

1. **Closed all running instances** of CalibrationLogConverter.exe
2. **Cleaned the project** to remove old build artifacts
3. **Rebuilt the application** cleanly

## 🔧 If This Error Happens Again

### Step 1: Close All Running Instances

**Option A: Using Task Manager**
1. Press `Ctrl + Shift + Esc` to open Task Manager
2. Find all `CalibrationLogConverter.exe` processes
3. Right-click → End Task on each one

**Option B: Using Command Line**
```powershell
taskkill /F /IM CalibrationLogConverter.exe
```

### Step 2: Clean the Project

```powershell
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter
dotnet clean
```

### Step 3: Rebuild

```powershell
dotnet build --configuration Release
```

### Step 4: Run the Fixed Application

Navigate to:
```
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\
CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe
```

Double-click to run!

## 📋 Quick Fix Command (All-in-One)

If you get this error again, run these commands:

```powershell
# 1. Kill any running instances
taskkill /F /IM CalibrationLogConverter.exe 2>$null

# 2. Navigate to project
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter

# 3. Clean and rebuild
dotnet clean
dotnet build --configuration Release

# 4. Run the app
& ".\bin\Release\net8.0-windows\CalibrationLogConverter.exe"
```

## ✅ Verification Checklist

After the fix, verify:
- [x] Application launches without XAML error
- [x] Main window appears correctly
- [x] Can browse and select files
- [x] Parse button works
- [x] Export button works
- [x] Excel export starts from Column B

## 🚫 What NOT to Do

❌ **Don't move the files** out of the DigitalProductionConfigEditor folder
   - The file location is fine
   - The error was about build artifacts, not location

❌ **Don't reinstall .NET Runtime**
   - .NET 8.0 Desktop Runtime is correctly installed
   - The error was not runtime-related

❌ **Don't delete the bin or obj folders manually while app is running**
   - Always close the application first
   - Use `dotnet clean` instead

## 📍 Correct File Locations (Do Not Move!)

```
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\
├── CalibrationLogConverter\              # Source code (correct location)
│   ├── bin\Release\net8.0-windows\       # Built application (correct)
│   │   └── CalibrationLogConverter.exe   # ← Run this file
│   ├── Models\
│   ├── Parsers\
│   ├── Services\
│   └── *.xaml, *.cs files
└── Documentation files (.md, .txt)
```

**Do NOT move** - everything is in the right place!

## 🎯 Current Status

✅ **Application fixed and ready to use**
✅ **Data exports starting from Column B**
✅ **All features working correctly**
✅ **.NET 8.0 Desktop Runtime installed**
✅ **Build successful with no errors**

## 💡 Prevention Tips

To avoid this error in the future:

1. **Always close the application** before rebuilding
2. **Don't manually edit files** in bin/ or obj/ folders
3. **Use `dotnet clean`** before rebuilding if you encounter issues
4. **Check Task Manager** if build fails (app might still be running)

## 📞 If You Still Have Issues

If the error persists after following the steps above:

1. **Restart Visual Studio** (if using it)
2. **Restart your computer** (clears all locks)
3. **Delete bin and obj folders** manually after closing everything:
   ```powershell
   cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter
   Remove-Item -Recurse -Force bin, obj
   dotnet build --configuration Release
   ```

## ✅ Ready to Use!

The application is now fixed and ready for use:

**Run**: 
```
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\
CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe
```

**Data Source**: 
```
C:\Users\wanlimah\Documents\Raw_Data\
```

**Output Format**: Data starts from Column B (Column A empty)

---

**Problem Solved!** 🎉














