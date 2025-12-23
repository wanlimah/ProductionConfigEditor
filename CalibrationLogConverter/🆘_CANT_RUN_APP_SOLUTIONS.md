# 🆘 "This app can't run on your PC" - Solutions

## Error You're Seeing

```
This app can't run on your PC
To find a version for your PC, check with the software publisher.
```

---

## 🔧 **Solution 1: Try .NET 6.0 Version** (EASIEST)

Double-click: **`RUN_NET6_VERSION.bat`**

This runs the .NET 6.0 compiled version instead of .NET 8.0.

---

## 🔧 **Solution 2: Run with dotnet command**

Double-click: **`RUN_WITH_DOTNET.bat`**

This uses `dotnet run` instead of running the .exe directly.

---

## 🔧 **Solution 3: Manual dotnet run**

1. Open PowerShell
2. Run these commands:
```powershell
cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter
dotnet run --configuration Release
```

---

## 🔧 **Solution 4: Unblock the file**

The .exe might be blocked by Windows. Try this:

1. Navigate to:
   ```
   C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows
   ```

2. Right-click `CalibrationLogConverter.exe`

3. Click **Properties**

4. At the bottom, if you see:
   ```
   ☑ Unblock    [This file came from another computer...]
   ```
   Check the **Unblock** box

5. Click **OK**

6. Try running the .exe again

---

## 🔧 **Solution 5: Run as Administrator**

1. Navigate to:
   ```
   C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows
   ```

2. Right-click `CalibrationLogConverter.exe`

3. Select **"Run as administrator"**

---

## 🔧 **Solution 6: Build with .NET 6 only**

If none of the above work, I can rebuild targeting only .NET 6:

Let me know and I'll do this!

---

## 📊 Quick Decision Tree

**Try these in order:**

1. ✅ **`RUN_NET6_VERSION.bat`** ← Try this FIRST
2. ✅ **`RUN_WITH_DOTNET.bat`** ← Try this SECOND
3. ✅ **Unblock the file** (Solution 4) ← Try this THIRD
4. ✅ **Manual dotnet run** (Solution 3) ← Try this if above fails

---

## 🎯 **What to Try RIGHT NOW:**

### Step 1:
Go to folder:
```
C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter
```

### Step 2:
Double-click: **`RUN_NET6_VERSION.bat`**

### Step 3:
If that works, check for "v2.0 (Email Support)" in the window title!

---

## 💡 Why This Happens

This error usually means:
- The .exe was built for wrong architecture (x64 vs x86)
- Windows Defender/SmartScreen is blocking it
- Missing .NET runtime (but you have it installed)
- File is "blocked" by Windows

The .NET 6 version or `dotnet run` should bypass this!

---

**Try `RUN_NET6_VERSION.bat` now and let me know if it works!**










