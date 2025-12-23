# FM-002 Parser Documentation

## 📋 Overview

The **FM002Parser** is a dedicated parser for handling `FM-002_Field Calibration Daily Report (Broadcom PG)` files. This parser has been specifically designed to handle the unique structure and format of FM-002 calibration reports.

**Created:** November 5, 2025  
**Parser Class:** `FM002Parser.cs`  
**Vendor Name:** "FM-002 (Broadcom PG)"

---

## ✨ Key Features

### 1. **Dedicated FM-002 File Detection**
- Automatically detects files with "fm-002" or "fm002" in the filename (case-insensitive)
- Takes priority over the general BroadcomParser

### 2. **Flexible Worksheet Processing**
The parser intelligently searches for calibration data in worksheets with these names:
- "Logsheet" (exact match or starts with)
- Any worksheet containing "Calibration"
- Any worksheet containing "Daily"
- Any worksheet containing "Report"
- Falls back to the first worksheet if no specific match found

### 3. **Smart Column Detection**
Enhanced column detection with extensive variations:

#### Model Column
Recognizes: `model number`, `model no`, `model`, `equipment`, `instrument`, `device`, `asset`, `item`, `description`, `equipment name`, `tool name`

#### Serial Number Column
Recognizes: `serial number`, `serial no`, `serial`, `s/n`, `sn`, `asset tag`, `asset no`, `asset number`

#### Calibration Date Column
Recognizes: `date dd-mm-yy`, `date (dd-mm-yy)`, `date`, `calibration date`, `cal date`, `calibrated`, `cal performed`

#### Due Date Column (NEW)
The FM002Parser supports **direct due date reading** in addition to calculation:
- Recognizes: `due date`, `next cal`, `next calibration`, `expiry`, `cal due`, `due`, `next due`, `calibration due`

#### Calibration Interval Column
Recognizes: `cat int month`, `cat int (month)`, `cat int`, `interval`, `cal interval`, `months`, `cycle`

### 4. **Dual Due Date Strategy**
The parser uses an intelligent two-step approach for determining due dates:

**Step 1:** Try to read due date directly from the file
```
If "Due Date" column exists and has a value → Use it
```

**Step 2:** Calculate due date if not directly available
```
If Calibration Date exists AND Calibration Interval exists:
    Due Date = Calibration Date + Interval (months)
```

This makes the parser more flexible and robust!

### 5. **Status Determination**
Calibration status is automatically determined:
- ✅ **PASS**: Record has a valid due date
- ❌ **FAIL**: 
  - Row contains "X" marker (indicates error/placeholder)
  - Missing due date (no direct due date AND unable to calculate)

### 6. **Comprehensive Debugging**
The parser includes extensive diagnostic logging:
- Shows which columns were detected
- Displays actual column headers from the file
- Provides row-by-row processing details
- Shows summary of processed vs skipped rows
- Displays helpful error messages if columns not found

---

## 🔧 Technical Details

### Parser Priority
The parsers are registered in this order in `MainWindow.xaml.cs`:
```csharp
_parsers.Add(new FM002Parser());      // Checked FIRST
_parsers.Add(new BroadcomParser());   // Checked second
```

This ensures FM-002 files are always handled by the dedicated parser.

### File Matching Logic
```csharp
public bool CanParse(string filePath)
{
    var fileName = Path.GetFileName(filePath).ToLower();
    return fileName.Contains("fm-002") || fileName.Contains("fm002");
}
```

### Column Header Normalization
The parser uses regex-based normalization to handle variations:
- Removes newlines from multi-line headers
- Removes extra spaces, dashes, underscores, dots, parentheses
- Case-insensitive matching

Example:
```
"Date (dd-mm-yy)" → "date dd mm yy"
"Cat Int\nmonth"  → "cat int month"
"Model  Number"   → "model number"
```

---

## 📊 Usage

### Automatic Usage
Simply place your FM-002 Excel files in the `Raw_Data` folder or select them using the Browse button. The parser will automatically be used for any file containing "fm-002" in its name.

### Expected File Structure
Your FM-002 file should have:
1. A worksheet named "Logsheet" (or containing "Calibration", "Daily", or "Report")
2. A header row with column names
3. Data rows below the header

### Example File Layout
```
| Model Number | Serial No | Date (dd-mm-yy) | Cat Int (month) | Due Date    | ... |
|--------------|-----------|------------------|------------------|-------------|-----|
| Equipment-1  | SN001     | 2024-01-15       | 12               | 2025-01-15  | ... |
| Equipment-2  | SN002     | 2024-02-20       | 6                | 2024-08-20  | ... |
```

---

## 🎯 What Gets Extracted

The parser extracts these fields for each calibration record:

| Field | Description | Source |
|-------|-------------|--------|
| **Model** | Equipment model/name | Model/Equipment column |
| **Serial Number** | Device serial number | Serial/S/N column |
| **Due Date** | Next calibration due date | Direct from Due Date column OR calculated from Cal Date + Interval |
| **Calibration Date** | Last calibration date | Date/Cal Date column |
| **Status** | PASS or FAIL | Calculated based on due date and X markers |

---

## 🔍 Diagnostic Features

### Column Detection Messages
When you parse a file, the Debug output shows:
```
FM-002 PARSER - WORKSHEET: Logsheet
═══════════════════════════════════════
Total rows: 200
COLUMN DETECTION RESULTS:
───────────────────────────────────────
Model Column:     ✅ Column 2
Serial Column:    ✅ Column 3
Cal Date Column:  ✅ Column 5
Due Date Column:  ✅ Column 8
Cat Int Column:   ✅ Column 6
```

### Row Processing Details
For each row, you'll see debug output like:
```
Row 15: Status=PASS - Model='Oscilloscope-X100', Serial='SN12345', DueDate=2025-12-31
Row 16: Calculated DueDate=2025-06-15 from CalDate=2024-06-15 + 12 months
Row 17: Status=FAIL (No due date) - Model='Multimeter-200', Serial='SN67890'
```

### Summary Statistics
At the end of parsing:
```
FM002Parser Summary: Processed=183, Skipped=17, Total=200
✅ FM-002 PARSER SUCCESS: 183 records extracted, 180 with due dates, 3 without
```

---

## ⚠️ Error Handling

### If No Records Extracted
The parser shows a detailed diagnostic message with:
- Worksheet name
- Total rows
- Column detection results
- Actual column headers found in your file

This helps you identify why parsing failed.

### Common Issues and Solutions

#### ❌ "No Records Found"
**Possible Causes:**
- Model or Serial Number columns not detected
- All rows are empty or contain only "X" markers
- Wrong worksheet being processed

**Solution:**
- Check that your file has columns named similar to: "Model", "Serial", etc.
- Verify worksheet is named "Logsheet" or contains "Calibration"
- Look at the diagnostic message showing actual column headers

#### ❌ "No Due Dates Calculated"
**Possible Causes:**
- No "Due Date" column in file
- Calibration Date column not found
- Calibration Interval column not found or empty

**Solution:**
- Ensure file has either a "Due Date" column OR both "Cal Date" and "Cat Int" columns
- Check that date columns are formatted as dates (not text)
- Check that interval column contains numbers

---

## 📝 Example Output

### Console Debug Output
```
FM002Parser: Processing worksheet 'Logsheet' with 200 rows
FM002Parser: Found matching worksheet: 'Logsheet'
FM002Parser: Found column 'model number' at index 2 matching: model
FM002Parser: Found column 'serial no' at index 3 matching: serial
FM002Parser: Found column 'date dd-mm-yy' at index 5 matching: date dd-mm-yy
FM002Parser: Found column 'cat int month' at index 6 matching: cat int month
FM002Parser: Extracted 183 records from 'Logsheet'
Row 2: Status=PASS - Model='Oscilloscope', Serial='SN001', DueDate=2025-12-15
Row 3: Status=PASS - Model='Multimeter', Serial='SN002', DueDate=2025-06-20
...
FM002Parser Summary: Processed=183, Skipped=17, Total=200
✅ FM-002 PARSER SUCCESS: 183 records extracted, 180 with due dates, 3 without
```

### Application Message
```
Parsing Complete!

✅ Successfully parsed: 1 file(s)
📊 Total records extracted: 183

File Details:
─────────────────────────────────────────────
📄 FM-002_Field Calibration Daily Report (Broadcom PG).xlsx
   Size: 125,432 bytes
─────────────────────────────────────────────
```

---

## 🆚 Differences from BroadcomParser

| Feature | FM002Parser | BroadcomParser |
|---------|-------------|----------------|
| **Target Files** | Only FM-002 files | Broadcom, Logsheet files |
| **Priority** | Checked first | Checked second |
| **Worksheet Search** | Multiple names | Only "Logsheet" |
| **Due Date** | Direct read OR calculated | Only calculated |
| **Column Variations** | Extended list | Standard list |
| **Diagnostic Messages** | FM-002 specific | General |

---

## 🔧 Maintenance

### Adding New Column Variations
To support additional column name variations, edit the `FindColumnInFirstRow` calls in `FM002Parser.cs`:

```csharp
// Example: Adding "Part Number" as a model column variation
int modelCol = FindColumnInFirstRow(table, 
    "model number", "model no", "model", 
    "part number",  // ADD NEW VARIATION HERE
    "equipment", "instrument", "device");
```

### Testing Changes
1. Edit `FM002Parser.cs`
2. Build: `dotnet build --configuration Release`
3. Run application
4. Parse your FM-002 file
5. Check Debug output for column detection
6. Verify records are extracted correctly

---

## ✅ Verification Checklist

After implementing the FM002Parser, verify:

- [x] ✅ FM002Parser.cs created
- [x] ✅ Parser registered in MainWindow.xaml.cs
- [x] ✅ Build succeeds without errors
- [ ] ⬜ Application runs successfully
- [ ] ⬜ FM-002 file is correctly parsed
- [ ] ⬜ Records displayed in preview grid
- [ ] ⬜ Export to Excel works
- [ ] ⬜ Due dates are correct

---

## 📞 Support

If you encounter issues with the FM002Parser:

1. **Check Debug Output** - Run in Debug mode and check the Output window
2. **Verify Column Names** - Open your Excel file and check the exact column names
3. **Check File Structure** - Ensure worksheet is named correctly
4. **Review Error Messages** - Parser provides detailed diagnostic information
5. **Contact Developer** - Provide error message and sample file

---

## 📚 Related Files

- **Parser Implementation**: `CalibrationLogConverter\Parsers\FM002Parser.cs`
- **Registration**: `CalibrationLogConverter\MainWindow.xaml.cs` (line 41)
- **Interface**: `CalibrationLogConverter\Parsers\ICalibrationParser.cs`
- **Data Model**: `CalibrationLogConverter\Models\CalibrationRecord.cs`
- **User Guide**: `USER_GUIDE.md`
- **Project README**: `README.md`

---

**Last Updated:** November 5, 2025  
**Version:** 1.0  
**Status:** ✅ Ready for Use











