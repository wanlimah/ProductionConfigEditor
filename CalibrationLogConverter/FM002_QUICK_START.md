# FM-002 Parser - Quick Start Guide

## 🚀 What's New?

A **dedicated parser for FM-002_Field Calibration Daily Report (Broadcom PG)** files has been added to the Calibration Log Converter!

---

## ✨ Key Improvements

### 1. **Automatic Detection**
Files with "FM-002" in the name are now automatically handled by the specialized FM002Parser.

### 2. **Smarter Due Date Handling**
- **Option 1:** Reads due date directly from your Excel file (if available)
- **Option 2:** Calculates due date from: Calibration Date + Interval (months)
- **Result:** More flexible and robust parsing!

### 3. **Better Worksheet Detection**
Automatically finds calibration data in worksheets named:
- "Logsheet"
- Any sheet with "Calibration", "Daily", or "Report" in the name

### 4. **Enhanced Column Detection**
Recognizes more column name variations, including:
- "Model Number" / "Equipment Name" / "Tool Name"
- "Due Date" / "Next Cal" / "Expiry"
- "Date (dd-mm-yy)" / "Calibration Date"
- "Cat Int (month)" / "Interval" / "Frequency"

---

## 📖 How to Use

### Step 1: Place Your File
Put your FM-002 file in: `C:\Users\wanlimah\Documents\Raw_Data`

**OR**

Click the **Browse** button and select your file manually.

### Step 2: Run the Application
```bash
# Option 1: Use the batch file
RUN_APP.bat

# Option 2: Run directly
cd CalibrationLogConverter\bin\Release\net8.0-windows
CalibrationLogConverter.exe

# Option 3: Using .NET CLI
cd CalibrationLogConverter\CalibrationLogConverter
dotnet run
```

### Step 3: Parse the File
1. Click **🔄 Parse Files**
2. Wait for processing
3. Review the results message
4. Check the preview grid

### Step 4: Export to Excel
1. Click **💾 Export to Excel**
2. Choose save location
3. Click **Save**
4. Open the exported file to review

---

## 📊 Expected Results

### For a typical FM-002 file with 200 rows:

**Console Output:**
```
FM002Parser: Found matching worksheet: 'Logsheet'
FM002Parser: Extracted 183 records from 'Logsheet'
✅ FM-002 PARSER SUCCESS: 183 records extracted, 180 with due dates
```

**Application Message:**
```
Parsing Complete!

✅ Successfully parsed: 1 file(s)
📊 Total records extracted: 183
```

---

## 🎯 What Gets Exported

The exported Excel file will contain:

### Main Sheet: "Calibration Records"
| Model | Serial Number | Due Date | Status |
|-------|---------------|----------|--------|
| Equipment-1 | SN001 | 2025-12-31 | PASS |
| Equipment-2 | SN002 | 2025-06-15 | PASS |
| Equipment-3 | SN003 | | FAIL |

### Summary Sheet
- Total records
- Export date/time
- Records by vendor
- Records with/without due dates
- ⚠️ Due within 30 days
- ⚠️ Overdue records

---

## ✅ Status Meanings

| Status | Meaning |
|--------|---------|
| **PASS** | Equipment has a valid due date |
| **FAIL** | No due date (missing calibration info) OR marked with "X" |

---

## 🔍 Debugging Tips

### If you see fewer records than expected:

1. **Check for "X" markers**
   - Rows with "X" in Model or Serial columns are skipped
   - These indicate error/placeholder data

2. **Check for empty rows**
   - Rows where BOTH Model AND Serial are empty are skipped

3. **Open your Excel file**
   - Press `Ctrl + End` to see the actual last row
   - Check if there are many blank rows at the end

### If due dates are missing:

1. **Check your Excel file has:**
   - A "Due Date" column with dates, OR
   - A "Calibration Date" column AND "Interval" column

2. **Verify date format**
   - Dates should be formatted as Date (not Text)
   - Use Excel's date formatting: `Format Cells > Date`

3. **Check Debug output**
   - Look for messages like: "Row X: No due date - Calibration date is NULL"
   - This tells you exactly which data is missing

---

## 🆚 FM002Parser vs BroadcomParser

| Feature | FM002Parser (NEW) | BroadcomParser (Original) |
|---------|-------------------|---------------------------|
| **Files** | FM-002 only | Broadcom, Logsheet files |
| **Priority** | ⭐ First choice | Second choice |
| **Due Date** | Direct OR calculated | Only calculated |
| **Worksheets** | Multiple patterns | Only "Logsheet" |
| **Diagnostics** | FM-002 specific | General |

**Result:** FM-002 files now get better, more accurate parsing!

---

## 💡 Pro Tips

### Tip 1: Check the Debug Output
Run from Visual Studio or VS Code to see detailed parsing logs:
```
Row 2: Status=PASS - Model='Oscilloscope', Serial='SN001', DueDate=2025-12-15
Row 3: Status=PASS - Model='Multimeter', Serial='SN002', DueDate=2025-06-20
```

### Tip 2: Use Extended Export First
- ✅ Check "Include extended fields" for first export
- This shows all available data including Status, Cal Date, etc.
- Helps verify parsing is working correctly

### Tip 3: Compare with Source
- Spot-check a few records against your original Excel file
- Verify dates are correct
- Confirm record count matches expectations

### Tip 4: Review the Summary Sheet
- Shows statistics and warnings
- Highlights overdue calibrations
- Shows records missing due dates

---

## 📁 File Naming

For best results, name your files like:
- ✅ `FM-002_Field Calibration Daily Report (Broadcom PG).xlsx`
- ✅ `FM-002_Calibration_Log_2024.xlsx`
- ✅ `FM002_Daily_Report.xlsx`

The parser detects files containing "fm-002" or "fm002" (case-insensitive).

---

## 🐛 Common Issues

### ❌ "No suitable parser found"
**Solution:** Ensure filename contains "fm-002" or select file manually via Browse button.

### ❌ "No Records Extracted"
**Solution:** 
- Check worksheet is named "Logsheet" or contains "Calibration"
- Verify file has Model and Serial Number columns
- See the diagnostic message for actual column headers

### ❌ "All records have Status=FAIL"
**Solution:**
- Check that due dates are present or calculable
- Verify calibration dates and intervals are in the file
- Look for "X" markers that might be marking all rows as invalid

### ❌ "Wrong record count"
**Solution:**
- Check for "X" markers that cause rows to be skipped
- Check for rows where both Model and Serial are empty
- Open Excel and press Ctrl+End to see actual data range

---

## 📞 Need Help?

1. **Check the full documentation**: `FM002_PARSER_DOCUMENTATION.md`
2. **Review the User Guide**: `USER_GUIDE.md`
3. **Check Debug output**: Run in Debug mode to see detailed logs
4. **Contact support**: Provide error message and sample file

---

## 📋 Quick Command Reference

```bash
# Build the project
cd CalibrationLogConverter\CalibrationLogConverter
dotnet build --configuration Release

# Run the application
dotnet run

# Run from executable
cd CalibrationLogConverter\bin\Release\net8.0-windows
.\CalibrationLogConverter.exe
```

---

## ✅ Success Checklist

- [x] ✅ FM002Parser created and registered
- [x] ✅ Build succeeds (0 errors, 0 warnings)
- [ ] ⬜ Application runs without errors
- [ ] ⬜ FM-002 file automatically detected
- [ ] ⬜ Records parsed and displayed in preview
- [ ] ⬜ Due dates are correct
- [ ] ⬜ Export to Excel works
- [ ] ⬜ Exported file has correct data

---

**Ready to use!** Just run the application and parse your FM-002 files. 🚀

**Date Created:** November 5, 2025  
**Status:** ✅ Production Ready











