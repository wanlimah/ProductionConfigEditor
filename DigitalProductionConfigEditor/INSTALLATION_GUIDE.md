# Digital Production Config Editor - Installation Guide

## 📦 Version 2.0 - Multi-Platform Release

Welcome! This guide will help you install and run the Digital Production Config Editor on your Windows computer.

---

## 🎯 Quick Start (3 Steps)

1. **Choose Your Version** (see below)
2. **Extract the ZIP file**
3. **Run the application**

That's it! No complex installation needed.

---

## 📥 Step 1: Choose Your Version

We provide **two versions** of the application. Choose the one that matches your system:

### 🟢 Recommended: .NET 6.0 Version
**File:** `DigitalProductionConfigEditor_v2.0_NET6.zip`

**Choose this if:**
- ✅ You're not sure which version to use
- ✅ You have Windows 10 or older systems
- ✅ Your organization uses .NET 6.0
- ✅ You want maximum compatibility

**Requires:** .NET 6.0 Desktop Runtime

---

### 🔵 Latest: .NET 8.0 Version
**File:** `DigitalProductionConfigEditor_v2.0_NET8.zip`

**Choose this if:**
- ✅ You have Windows 11
- ✅ You already have .NET 8.0 installed
- ✅ You want the latest features and optimizations

**Requires:** .NET 8.0 Desktop Runtime

---

### ❓ Not Sure Which Version?

**Run this command in PowerShell to check what you have:**

```powershell
dotnet --list-runtimes
```

**Look for these lines:**
- `Microsoft.WindowsDesktop.App 6.x.x` → You have .NET 6.0 ✅
- `Microsoft.WindowsDesktop.App 8.x.x` → You have .NET 8.0 ✅

**If you see either one, download the matching version!**

**If you see neither or get an error:**
- Download .NET 6.0 version (most compatible)
- Follow installation steps below

---

## 📥 Step 2: Extract the ZIP File

1. **Locate the downloaded ZIP file:**
   - `DigitalProductionConfigEditor_v2.0_NET6.zip` OR
   - `DigitalProductionConfigEditor_v2.0_NET8.zip`

2. **Right-click the ZIP file** → Select **"Extract All..."**

3. **Choose a location**, for example:
   - `C:\Programs\DigitalProductionConfigEditor\`
   - `C:\Users\[YourName]\Desktop\DigitalProductionConfigEditor\`
   - Any folder you prefer

4. **Click "Extract"**

---

## 🚀 Step 3: Run the Application

1. **Navigate to the extracted folder**

2. **Double-click:** `DigitalProductionConfigEditor.exe`

3. **If you see a security warning:**
   - Click **"More info"**
   - Click **"Run anyway"**
   - (This is normal for downloaded applications)

4. **Application should launch!** 🎉

---

## 🛠️ Installing .NET Runtime (If Needed)

If the application doesn't start or you get an error about missing .NET, you need to install the runtime.

### For .NET 6.0 Version Users:

1. **Visit:** https://dotnet.microsoft.com/download/dotnet/6.0

2. **Find the section:** ".NET Desktop Runtime 6.0.x"

3. **Download:** 
   - Click **"Download x64"** (for 64-bit Windows)
   - Most modern computers are 64-bit

4. **Run the installer:**
   - Double-click the downloaded file
   - Follow the installation wizard
   - Click "Install" → Wait → Click "Close"

5. **Try running the application again**

### For .NET 8.0 Version Users:

1. **Visit:** https://dotnet.microsoft.com/download/dotnet/8.0

2. **Find the section:** ".NET Desktop Runtime 8.0.x"

3. **Download:** 
   - Click **"Download x64"** (for 64-bit Windows)

4. **Run the installer** (same steps as above)

5. **Try running the application again**

---

## ✅ First Launch Checklist

When you first run the application, verify these files are present in the same folder as the `.exe`:

| File | Purpose | Required? |
|------|---------|-----------|
| `DigitalProductionConfigEditor.exe` | Main application | ✅ Yes |
| `Master_Digital_ProductionUserConfig.xml` | Master template | ✅ Yes |
| `DigitalProductionConfigEditor.dll` | Application library | ✅ Yes |
| Other `.dll` files | Dependencies | ✅ Yes |

**⚠️ Important:** Keep all files together! Don't move just the `.exe` file.

---

## 🎓 Getting Started with the Application

### Your First Configuration

Once the application launches:

1. You'll see two panels:
   - **Left Panel:** Master Template (all available configurations)
   - **Right Panel:** Your New Configuration (initially empty)

2. **Add configurations:**
   - Click **➕ Add** next to items you want from the Master Template
   - They'll appear in Your New Configuration panel

3. **Navigate steps:**
   - Click **Next ➡** to proceed to Step 2 (Edit Packages)
   - Click **Next ➡** again to proceed to Step 3 (Review)

4. **Save your work:**
   - Click **💾 Save** to save your configuration
   - Choose a filename (e.g., `MyProduct_Config.xml`)

### Detailed Usage Guide

For complete instructions, see these documents in the application folder:
- **README_NEW_WORKFLOW.md** - Complete user guide
- **QUICK_REFERENCE.md** - Quick reference card
- **VISUAL_WORKFLOW.md** - Visual diagrams and workflows

---

## 🔧 Troubleshooting

### Problem: Application Won't Start

**Symptom:** Double-clicking does nothing, or error message appears

**Solutions:**
1. ✅ Verify you have the correct .NET Runtime installed (see above)
2. ✅ Check that `Master_Digital_ProductionUserConfig.xml` exists in the same folder
3. ✅ Right-click the `.exe` → Select "Run as administrator"
4. ✅ Check Windows Defender didn't block the file:
   - Open Windows Security → Virus & threat protection
   - Check Protection history
   - Restore the file if blocked

---

### Problem: "Master XML not found" Error

**Symptom:** Application shows error about missing master template

**Solutions:**
1. ✅ Verify `Master_Digital_ProductionUserConfig.xml` is in the same folder as the `.exe`
2. ✅ Re-extract the ZIP file (don't move files individually)
3. ✅ Click **🔄 Reload Master** button in the application

---

### Problem: .NET Runtime Installation Fails

**Symptom:** .NET installer shows an error

**Solutions:**
1. ✅ Check you have administrator privileges
2. ✅ Install Windows updates first
3. ✅ Try the offline installer instead:
   - Go to the same download page
   - Select "All .NET 6.0 downloads" or "All .NET 8.0 downloads"
   - Download the offline installer

---

### Problem: Application Crashes or Freezes

**Symptom:** Application stops responding

**Solutions:**
1. ✅ Save your work regularly (changes are not auto-saved)
2. ✅ Check the XML file isn't corrupted:
   - Open in Notepad
   - Verify it's valid XML
3. ✅ Try creating a new configuration file

---

### Problem: Can't Find the Downloaded File

**Symptom:** Don't know where the ZIP file went

**Solutions:**
1. ✅ Check your `Downloads` folder:
   - `C:\Users\[YourName]\Downloads\`
2. ✅ Check your browser's download history:
   - Chrome: `Ctrl + J`
   - Edge: `Ctrl + J`
   - Firefox: `Ctrl + Shift + Y`

---

## 📊 System Requirements

### Minimum Requirements

| Component | Requirement |
|-----------|-------------|
| **Operating System** | Windows 10 (64-bit) or later |
| **.NET Runtime** | .NET 6.0 or .NET 8.0 Desktop Runtime |
| **RAM** | 512 MB available |
| **Disk Space** | 50 MB free space |
| **Display** | 1024×768 minimum resolution |

### Recommended Requirements

| Component | Recommendation |
|-----------|----------------|
| **Operating System** | Windows 11 |
| **RAM** | 2 GB or more |
| **Display** | 1920×1080 or higher |
| **Input** | Mouse for easier navigation |

---

## 🔄 Updating to a New Version

When a new version is released:

1. **Save your current XML files** to a safe location
2. **Download the new version** (ZIP file)
3. **Extract to a new folder** (or replace the old one)
4. **Copy your XML files back** to the new folder (if needed)
5. **Run the new version**

**Note:** Your custom XML configuration files are separate and will not be affected.

---

## 🗂️ File Organization Tips

### Recommended Folder Structure

```
C:\Programs\DigitalProductionConfigEditor\
├── DigitalProductionConfigEditor.exe
├── Master_Digital_ProductionUserConfig.xml
├── [Other DLL files]
└── MyConfigs\
    ├── ProductA_Config.xml
    ├── ProductB_Config.xml
    └── ProductC_Config.xml
```

**Tips:**
- Keep the application files together in one folder
- Create a subfolder for your custom configurations
- Use descriptive filenames for your XML files
- Back up your configurations regularly

---

## 🆘 Getting Help

### Documentation Files

All documentation is included in the application folder:

| Document | Purpose |
|----------|---------|
| `INSTALLATION_GUIDE.md` | This file - Installation help |
| `README_NEW_WORKFLOW.md` | Complete user guide |
| `QUICK_REFERENCE.md` | Quick reference card |
| `NEW_WORKFLOW_GUIDE.md` | Detailed workflow guide |
| `VISUAL_WORKFLOW.md` | Visual diagrams |
| `ADD_DELETE_PACKAGE_GUIDE.md` | Package management |
| `USAGE_EXAMPLES.md` | Example workflows |

### Common Questions

**Q: Do I need to install anything?**  
A: Only the .NET Runtime (one-time, if not already installed)

**Q: Can I install both .NET 6.0 and .NET 8.0?**  
A: Yes! They can coexist. Both versions will work.

**Q: Can I move the application to another computer?**  
A: Yes! Just copy the entire folder. The other computer needs the same .NET Runtime.

**Q: Will this work on Windows 7?**  
A: No. Requires Windows 10 or later.

**Q: Can I run this on Mac or Linux?**  
A: No. This is a Windows-only application (WPF framework).

**Q: Is internet required?**  
A: No. After installation, the application works completely offline.

---

## 📞 Support Information

### Before Reporting an Issue

Please check:
1. ✅ Correct .NET Runtime is installed
2. ✅ All files are in the same folder
3. ✅ You're using a supported Windows version
4. ✅ You've reviewed the troubleshooting section above

### Reporting Issues

When reporting a problem, please provide:
- Windows version (e.g., "Windows 10 Pro 64-bit")
- .NET version installed (run `dotnet --list-runtimes`)
- Which ZIP file you downloaded (NET6 or NET8)
- Error message (screenshot or exact text)
- Steps to reproduce the problem

---

## 📜 Version Information

**Application Version:** 2.0  
**Release Date:** October 2025  
**Supported Platforms:** Windows 10+, Windows 11  
**Supported .NET Versions:** .NET 6.0, .NET 8.0  

---

## ✅ Installation Complete!

If you successfully:
- ✅ Extracted the ZIP file
- ✅ Installed the .NET Runtime (if needed)
- ✅ Launched the application

**You're ready to go!** 🎉

### Next Steps:

1. **Read:** `README_NEW_WORKFLOW.md` for a complete walkthrough
2. **Try:** Create your first configuration
3. **Explore:** The various features and options

---

## 🎯 Quick Reference Card

### Essential Commands

| Action | Button |
|--------|--------|
| Create new config | 📄 New |
| Open existing config | 📂 Open |
| Save current config | 💾 Save |
| Save with new name | 💾 Save As... |
| Add configuration | ➕ Add |
| Remove configuration | 🗑 Delete |
| Next step | Next ➡ |
| Previous step | ⬅ Back |

### Essential Shortcuts

- **Ctrl + S** - Save (if implemented)
- **Escape** - Close dialogs
- **Tab** - Navigate between fields

---

**Happy Configuring!** 🚀

**For detailed usage instructions, see README_NEW_WORKFLOW.md**






















































