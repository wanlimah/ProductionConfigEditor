# 📘 Calibration Log Converter - User Guide

## Table of Contents
1. [Overview](#overview)
2. [Step-by-Step Instructions](#step-by-step-instructions)
3. [Understanding the Interface](#understanding-the-interface)
4. [Export Options](#export-options)
5. [Tips & Best Practices](#tips--best-practices)
6. [FAQ](#faq)

---

## Overview

The Calibration Log Converter helps you consolidate calibration data from multiple vendor files into a single, standardized Excel report containing:
- **Model** - Equipment/instrument name
- **Serial Number** - Unique identifier
- **Due Date** - When next calibration is due

### What It Does
✅ Reads Excel files from 3 different vendors  
✅ Automatically finds Model, Serial Number, and Due Date columns  
✅ Combines all data into one standardized format  
✅ Creates professional Excel report with summary statistics  
✅ Highlights overdue and upcoming calibrations  

---

## Step-by-Step Instructions

### Step 1: Launch the Application

Run the application:
```bash
dotnet run
```

Or double-click `CalibrationLogConverter.exe` (if published)

**What happens:**
- The app automatically loads Excel files from `C:\Users\wanlimah\Documents\Raw_Data`
- You'll see the files listed in the "Input Files" section

![Main Window](screenshot-main.png)

---

### Step 2: Select Files (Optional)

If you need to select different files:

1. Click **Browse...** button
2. Navigate to your calibration log files
3. Select one or multiple Excel files
4. Click **Open**

**Supported formats:**
- Excel 2007+ (`.xlsx`)
- Excel 97-2003 (`.xls`)
- Excel Binary (`.xlsb`)

---

### Step 3: Parse the Files

1. Click **🔄 Parse Files** button
2. Wait for processing (usually a few seconds)
3. Review the results dialog showing:
   - ✅ Successfully parsed files
   - 📊 Total records extracted
   - ❌ Any errors (if applicable)
4. Click **OK**

**What happens:**
- Each file is analyzed
- The appropriate vendor parser is selected
- Data is extracted and displayed in the preview grid

---

### Step 4: Review the Preview

Check the preview grid to verify extracted data:

| Column | Description |
|--------|-------------|
| **Model** | Equipment name |
| **Serial Number** | Device serial number |
| **Due Date** | Next calibration due date |
| **Cal Date** | Last calibration date |
| **Status** | Pass/Fail result |
| **Location** | Site/Facility |
| **Vendor** | Which vendor file it came from |
| **Source File** | Original filename |

**Tips:**
- Scroll through to check data quality
- Look for missing dates or blank entries
- Verify serial numbers are correct

---

### Step 5: Configure Export Options

Before exporting, choose your preferences:

#### ✅ Include extended fields
- **Checked**: Exports all columns (Calibration Date, Status, Location, Technician, etc.)
- **Unchecked**: Exports only Model, Serial Number, and Due Date

#### ✅ Automatically open Excel file after export
- **Checked**: Opens the file immediately after export
- **Unchecked**: Just saves the file

**Recommendation:** Keep both checked for first-time use.

---

### Step 6: Export to Excel

1. Click **💾 Export to Excel** button
2. Choose save location and filename
   - Default: `Calibration_Report_20251030_150000.xlsx`
3. Click **Save**
4. Wait for export to complete
5. Review the success message
6. Click **Yes** to open the file (or **No** if auto-open is enabled)

**The exported file contains:**
- **Calibration Records** sheet - All your data
- **Summary** sheet - Statistics and warnings

---

## Understanding the Interface

### Main Window Layout

```
┌─────────────────────────────────────────────────┐
│  📊 Calibration Log File Converter              │
│  Convert vendor calibration logs to Excel       │
├─────────────────────────────────────────────────┤
│                                                  │
│  📂 Input Files                                 │
│  ┌──────────────────────────┐ [Browse...]       │
│  │ FM-002_Field Calibration │                   │
│  │ Logsheet_Broadcom_Sep25  │                   │
│  └──────────────────────────┘                   │
│                                                  │
│  ⚙️ Export Options                              │
│  ☑ Include extended fields                      │
│  ☑ Automatically open Excel file                │
│                                                  │
│  📋 Preview                                      │
│  ┌─────────────────────────────────────────┐   │
│  │ Model    │ Serial │ Due Date │ Vendor   │   │
│  │──────────│────────│──────────│──────────│   │
│  │ Device-A │ SN123  │ 2025-12  │ Broadcom │   │
│  │ Device-B │ SN456  │ 2025-11  │ Broadcom │   │
│  └─────────────────────────────────────────┘   │
│                                                  │
│  Status: Parsed 2 files, found 45 records       │
│                                      45 records  │
│                                                  │
│  [🔄 Parse Files] [💾 Export to Excel] [Clear] │
└─────────────────────────────────────────────────┘
```

### Status Messages

| Color | Meaning |
|-------|---------|
| 🟢 **Green** | Success or ready |
| 🔴 **Red** | Error occurred |

---

## Export Options

### Standard Export (Model, Serial Number, Due Date Only)

**Best for:**
- Simple calibration tracking
- Quick reference lists
- Sharing with external parties

**Example output:**
```
Model              Serial Number    Due Date
─────────────────  ──────────────   ──────────
Oscilloscope-X100  SN001234         2025-12-31
Multimeter-DMM500  SN005678         2025-11-15
Function Gen-FG10  SN009012         2026-01-20
```

### Extended Export (All Fields)

**Best for:**
- Detailed record keeping
- Audit purposes
- Internal tracking
- Identifying who performed calibration
- Tracking locations and status

**Additional fields:**
- Calibration Date
- Status (Pass/Fail)
- Location (Site/Facility)
- Technician (Who calibrated)
- Vendor (Source vendor)
- Notes (Remarks/Comments)
- Source File (Original filename)
- Source Row (Row number in original file)

---

## Summary Sheet Features

The exported Excel file includes a **Summary** sheet with:

### Basic Statistics
- Total Records
- Export Date/Time
- Records by Vendor
- Records by Source File
- Records with Due Dates
- Records without Due Dates

### ⚠️ Important Warnings

#### Due within 30 days (Red Bold)
Equipment that needs calibration soon:
```
Due within 30 days: 5
```

#### Overdue (Dark Red Bold)
Equipment past due date:
```
Overdue: 2
```

**Example Summary Sheet:**
```
Calibration Export Summary

Total Records:          45
Export Date:            2025-10-30 15:30:00

Records by Vendor:
Broadcom                45

Records by Source File:
FM-002_Field Calibration Daily Report (Broadcom PG).xlsx    20
Logsheet_Broadcom_Sep25_WanLing (version 1).xlsb.xlsx       25

Records with Due Dates:     42
Records without Due Dates:  3

Due within 30 days:     5  ⚠️ (Red)
Overdue:                2  ⚠️ (Dark Red)
```

---

## Tips & Best Practices

### Before Using the Converter

1. **Organize Your Files**
   - Place all vendor calibration logs in `Raw_Data` folder
   - Use descriptive filenames
   - Ensure files are not corrupted

2. **Check Source Data**
   - Open source Excel files
   - Verify they contain Model, Serial Number, and Due Date columns
   - Ensure dates are in date format (not text)

3. **Backup Original Files**
   - Keep copies of original vendor files
   - The converter only reads, never modifies source files

### During Conversion

4. **Review the Preview**
   - Always check the preview grid before exporting
   - Look for missing or incorrect data
   - Verify dates are parsed correctly

5. **Use Extended Export First**
   - For first-time use, include extended fields
   - This helps verify all data is captured correctly
   - You can always create a simple export later

### After Export

6. **Validate the Output**
   - Open the exported Excel file
   - Spot-check a few records against source files
   - Review the Summary sheet for warnings

7. **Address Overdue Calibrations**
   - Check the "Overdue" count in Summary sheet
   - Take action on equipment past due date

8. **Save Exports with Dates**
   - Default filename includes timestamp
   - Keep historical exports for tracking

---

## FAQ

### Q: What if my vendor files have different formats?

**A:** The converter uses smart column detection. It looks for common column names like:
- Model: "model", "equipment", "instrument", "device"
- Serial: "serial", "serial number", "s/n", "sn"
- Due Date: "due date", "next cal", "next calibration", "cal due"

If your vendor uses different names, a custom parser may be needed.

---

### Q: Can I process files from multiple vendors at once?

**A:** Yes! Select all vendor files and click Parse. The converter automatically uses the right parser for each file.

---

### Q: What if some records are missing due dates?

**A:** 
- Records without due dates are still included in the export
- The "Due Date" column will be blank
- Summary sheet shows count of records without due dates
- You can manually add dates in the exported Excel file

---

### Q: How do I know which vendor a record came from?

**A:** 
- Enable "Include extended fields"
- The **Vendor** column shows the source vendor
- The **Source File** column shows the original filename

---

### Q: Can I edit the data before exporting?

**A:** 
- The preview grid is read-only
- To make changes:
  1. Export to Excel
  2. Edit the Excel file
  3. Save your changes

---

### Q: What if the parser doesn't recognize my file?

**A:**
- Error: "No suitable parser found"
- **Solution 1:** Rename the file to include "broadcom", "fm-002", or "logsheet"
- **Solution 2:** Contact the developer to create a custom parser

---

### Q: How do I add support for a new vendor?

**A:** See the Developer Guide in README.md. You'll need to:
1. Create a new parser class
2. Implement the `ICalibrationParser` interface
3. Register it in `InitializeParsers()`

---

### Q: The dates are showing as numbers or text?

**A:** This usually means:
- Source Excel cells are formatted as text
- **Solution:** In source file, format the date column as Date format
- Or: The parser may need adjustment for that specific date format

---

### Q: Can I run this without the Raw_Data folder?

**A:** Yes! 
- Click **Browse...** button
- Select files from any location
- The Raw_Data folder is just the default location

---

### Q: How do I clear everything and start over?

**A:** Click the **🗑️ Clear** button. This:
- Clears all loaded files
- Removes parsed data
- Resets the preview grid
- Resets status to "Ready"

---

### Q: Is my data secure?

**A:** Yes!
- The converter only reads files locally
- No data is sent to the internet
- All processing happens on your computer
- Original files are never modified

---

### Q: What's the difference between Parse and Export?

**A:**
- **Parse** = Read the vendor files and extract data (preview only)
- **Export** = Save the extracted data to a new Excel file

You must Parse before you can Export.

---

## Need More Help?

1. Check the **Troubleshooting** section in README.md
2. Review error messages carefully
3. Verify your source Excel files are correct
4. Contact technical support with:
   - Error message
   - Screenshot
   - Sample file (if possible)

---

**Happy Calibrating! 📊✨**
















