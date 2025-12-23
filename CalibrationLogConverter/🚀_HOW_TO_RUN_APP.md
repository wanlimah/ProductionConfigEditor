# 🚀 How to Run the Application - Simple Guide

## ✅ **Method 1: Simplest Way (Recommended)**

### Step 1: Navigate to the folder
```
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter
```

### Step 2: Double-click one of these batch files:
- **`QUICK_RUN.bat`** ← **EASIEST** (just created for you!)
- **`RUN_EMAIL_PARSER_SIMPLE.bat`** ← Also simple
- **`RUN_APP.bat`** ← Alternative

If none work, try Method 2 below.

---

## ✅ **Method 2: Run Executable Directly**

### Navigate to:
```
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows
```

### Double-click:
```
CalibrationLogConverter.exe
```

**That's it!** The app should open.

---

## ✅ **Method 3: Using File Explorer**

### Step-by-Step:
1. Open **File Explorer** (Windows key + E)
2. Copy and paste this path in the address bar:
   ```
   C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows
   ```
3. Press **Enter**
4. Find **`CalibrationLogConverter.exe`**
5. **Double-click** it

---

## ✅ **Method 4: Using PowerShell/Command Prompt**

### Open PowerShell:
1. Press **Windows Key + X**
2. Select **"Windows PowerShell"** or **"Terminal"**

### Run these commands:
```powershell
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows
.\CalibrationLogConverter.exe
```

---

## 🔧 **Troubleshooting**

### Problem: "Batch file won't run"

**Try this**:
1. Right-click the `.bat` file
2. Select **"Run as administrator"**

OR

1. Open the `.bat` file with **Notepad**
2. Check if the path looks correct
3. Try running the EXE directly (Method 2 above)

---

### Problem: "Application won't start"

**Check**:
1. ✅ Is .NET 8.0 installed? 
   - Download from: https://dotnet.microsoft.com/download/dotnet/8.0
   
2. ✅ Does the EXE file exist?
   - Check: `CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe`
   
3. ✅ Any error messages?
   - Take a screenshot and check what it says

---

### Problem: "Can't find the executable"

**Solution**: Rebuild the application

```powershell
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter
dotnet build --configuration Release
```

After build completes, the EXE should be at:
```
bin\Release\net8.0-windows\CalibrationLogConverter.exe
```

---

## 📋 **Quick Checklist**

Before running, make sure:
- [ ] Your `.eml` file is in: `C:\Users\wanlimah\Documents\Raw_Data\`
- [ ] The file is named: `Broadcom Calibration Campaign 2025.eml`
- [ ] You're in the correct folder
- [ ] The `.exe` file exists

---

## 🎯 **What Should Happen**

When you run the application:

1. **Window opens** showing the Calibration Log Converter UI
2. **Files auto-load** from `C:\Users\wanlimah\Documents\Raw_Data\`
3. You should see your `.eml` file listed
4. Click **"🔄 Parse Files"** button
5. Records appear in the preview grid
6. Click **"💾 Export to Excel"** to save

---

## 💡 **Quick Test**

### Fastest way to test right now:

1. **Copy this path**:
   ```
   C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe
   ```

2. **Press Windows Key + R** (opens Run dialog)

3. **Paste the path** in the Run dialog

4. **Press Enter**

5. **Application should open!** ✅

---

## 🆘 **Still Having Issues?**

### Try this diagnostic:

1. Open PowerShell
2. Run:
   ```powershell
   cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows
   Test-Path .\CalibrationLogConverter.exe
   ```

3. If it says **"True"** → The file exists, try double-clicking it
4. If it says **"False"** → Need to rebuild (see Problem: Can't find executable above)

---

## 📁 **File Locations Summary**

```
Project Root:
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\

Executable Location:
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe

Data Files Location:
C:\Users\wanlimah\Documents\Raw_Data\

Batch Files (choose one):
- QUICK_RUN.bat
- RUN_EMAIL_PARSER_SIMPLE.bat  
- RUN_APP.bat
- TEST_EMAIL_PARSER.bat
```

---

## ✨ **Recommended: Create a Desktop Shortcut**

1. Navigate to: 
   ```
   C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows
   ```

2. Right-click **`CalibrationLogConverter.exe`**

3. Select **"Send to"** → **"Desktop (create shortcut)"**

4. Now you can run the app from your desktop anytime! 🎉

---

## 🎉 **Success!**

Once the app opens:
- ✅ Your `.eml` file should appear in the file list
- ✅ Click "Parse Files" to extract data
- ✅ Click "Export to Excel" to save results

**You're ready to go!** 🚀

---

**Need more help?** Check:
- `USER_GUIDE.md` - Complete user guide
- `EMAIL_PARSER_QUICK_START.md` - Email parser guide
- `📖_DOCUMENTATION_INDEX.md` - All documentation










