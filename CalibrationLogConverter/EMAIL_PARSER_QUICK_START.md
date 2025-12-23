# 📧 Email Parser - Quick Start Guide

## What is it?

The Email Parser allows you to extract calibration data directly from **email files** (`.eml` format) without manually copying data to Excel!

---

## ⚡ Quick Start (3 Steps)

### Step 1: Put your email file here
```
C:\Users\wanlimah\Documents\Raw_Data\
```

Example: `Broadcom Calibration Campaign 2025.eml`

### Step 2: Run the app
Double-click: `TEST_EMAIL_PARSER.bat`

OR

Run directly:
```
CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe
```

### Step 3: Parse & Export
1. Click **"🔄 Parse Files"**
2. Review extracted records
3. Click **"💾 Export to Excel"**

**Done!** ✅

---

## 📋 What Gets Extracted?

From your email, the parser extracts:

| Field | Description | Example |
|-------|-------------|---------|
| Model | Equipment model | ZNBT8 |
| Serial Number | Serial number | 101930 |
| Due Date | Next calibration due | 2026-03-10 |
| Calibration Date | Last calibration | 2025-03-10 |
| Status | Pass/Fail | PASS |
| Location | Equipment location | Broadcom |
| Notes | Material No, Job No | Material: 1318.7006K24, Job: 313009631 |

---

## 📁 Supported Email Format

Your email should contain calibration data in this format:

```
Model
Serial No
Status
Last Cal Date
Next Due Date
Location
(etc.)
```

The parser handles:
✅ Quoted email replies (lines starting with `>`)  
✅ Multiple records in one email  
✅ Various date formats  
✅ Mixed with Excel files (parse both together)

---

## 🎯 Example: Broadcom Calibration Campaign 2025

**Input**: Email file `Broadcom Calibration Campaign 2025.eml`

**Content**: Equipment calibrated at Broadcom in March 2025

**Expected Output**:
- 40+ calibration records
- All dates properly parsed
- Status (Passed) correctly extracted
- Material numbers and Job numbers preserved

---

## 🔧 Tips

### Tip 1: Mix with Excel files
You can process `.eml` and `.xlsx` files together!
- Put both in Raw_Data folder
- Click "Parse Files" once
- All records combined in one export

### Tip 2: Check the preview
After clicking "Parse Files", review the data grid to ensure:
- Model names look correct
- Serial numbers are properly extracted
- Dates are parsed correctly
- Status shows PASS or appropriate value

### Tip 3: Use Extended Fields
Check **"Include Extended Fields"** to export:
- Calibration dates
- Location
- Notes (Material No, Job No)

---

## ❓ Troubleshooting

### "No records extracted"

**Check**:
1. Is the file actually a `.eml` file?
2. Does the email contain quoted lines (`>`)?
3. Does it have headers like "Model", "Serial No"?

**Fix**: Open the `.eml` file in Notepad and verify the format

### "Dates are wrong"

**Check**:
1. What date format is in the email?
2. Is it one of the supported formats?

**Supported formats**:
- `10-Mar-2025`
- `10-03-2025`
- `2025-03-10`
- `03/10/2025`

**Fix**: If your format is different, contact support

### "Some fields are empty"

**Check**:
1. Are all fields present in the email?
2. Is each record complete (8 fields)?

**Note**: Empty fields in the email will result in empty cells in Excel

---

## 📊 Output Example

After export, your Excel file will contain:

| Model | Serial Number | Due Date | Status |
|-------|---------------|----------|---------|
| ZNBT8 | 101930 | 2026-03-10 | PASS |
| ZNB8 | 103487 | 2026-03-10 | PASS |
| NRP-Z11 | 121099 | 2027-03-11 | PASS |
| ... | ... | ... | ... |

Clean, formatted, ready to use! ✨

---

## 🚀 Advanced Usage

### Process Multiple Emails
1. Put multiple `.eml` files in Raw_Data
2. Parse all at once
3. All records combined in export

### Combine with Excel Reports
1. Mix `.eml` and `.xlsx` files
2. Parser automatically detects file types
3. Single export with all data

### Debug Mode
For troubleshooting:
1. Open in Visual Studio
2. Run in Debug mode (F5)
3. Check Output window for detailed logs

---

## 📚 More Information

For detailed technical documentation, see:
- `EMAIL_PARSER_DOCUMENTATION.md` - Complete technical guide
- `USER_GUIDE.md` - General application guide
- `README.md` - Project overview

---

## ✅ Checklist

Before using the Email Parser:

- [ ] Email file is in `.eml` format
- [ ] File is in `C:\Users\wanlimah\Documents\Raw_Data\`
- [ ] Email contains structured calibration data
- [ ] Application is built (run build if needed)

Ready to parse!

---

## 🎉 Success!

You can now process calibration data from emails automatically!

No more manual copying, no more Excel data entry - just drag, drop, and export! 📧➡️📊

---

**Questions?** Check `EMAIL_PARSER_DOCUMENTATION.md` for troubleshooting and detailed explanations.










