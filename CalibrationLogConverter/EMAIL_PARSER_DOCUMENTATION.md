# Email Parser (.eml) Documentation

## Overview

The **Email Parser** is a specialized parser designed to extract calibration data directly from email files (`.eml` format). This is particularly useful when calibration reports are received via email in a structured text format.

---

## Supported Format

The parser is designed to handle emails with calibration data structured as a table in the email body, with the following fields:

1. **Model** - Equipment model number
2. **Material No** - Material/part number
3. **Serial No** - Serial number of the equipment
4. **Job No** - Calibration job number
5. **Status** - Calibration status (Passed/Failed)
6. **Last Cal Date** - Date of calibration
7. **Next Due Date** - Next calibration due date
8. **Location** - Equipment location

### Example Email Format

```
> *Model*
> *Material No*
> *Serial No*
> *Job No*
> *Status*
> *Last Cal Date*
> *Next Due Date*
> *Location*
>
> ZNBT8
> 1318.7006K24
> 101930
> 313009631
> Passed
> 10-Mar-2025
> 10-Mar-2026
> Broadcom
>
> ZNB8
> 1311.6010K44
> 103487
> 313009811
> Passed
> 10-Mar-2025
> 10-Mar-2026
> Broadcom
```

---

## Features

### Automatic Detection
- The parser automatically detects `.eml` files
- Extracts data from quoted reply sections (lines starting with `>`)
- Handles multiple records in a single email

### Data Extraction
- **Model**: Equipment model name/number
- **Serial Number**: Equipment serial number
- **Due Date**: Next calibration due date
- **Calibration Date**: Last calibration date
- **Status**: PASS or other status from email
- **Location**: Equipment location (preserved in record)
- **Notes**: Material number and Job number stored in notes field

### Date Format Support
The parser supports multiple date formats:
- `dd-MMM-yyyy` (e.g., 10-Mar-2025)
- `d-MMM-yyyy` (e.g., 1-Mar-2025)
- `dd-MM-yyyy` (e.g., 10-03-2025)
- `yyyy-MM-dd` (e.g., 2025-03-10)
- `MM/dd/yyyy` (e.g., 03/10/2025)
- `dd/MM/yyyy` (e.g., 10/03/2025)

---

## How to Use

### Step 1: Place Email File in Raw_Data Folder

Place your `.eml` file in:
```
C:\Users\wanlimah\Documents\Raw_Data\
```

The application will automatically detect it along with Excel files.

### Step 2: Run the Application

**Option 1: Use the test batch file**
```batch
TEST_EMAIL_PARSER.bat
```

**Option 2: Run directly**
```batch
cd CalibrationLogConverter\bin\Release\net8.0-windows
CalibrationLogConverter.exe
```

### Step 3: Parse the Email

1. The application will show the `.eml` file in the file list
2. Click **"🔄 Parse Files"** button
3. The parser will extract all calibration records from the email
4. Review the extracted data in the preview grid

### Step 4: Export Results

1. Click **"💾 Export to Excel"** button
2. Choose a save location
3. The exported Excel file will contain all extracted records

---

## Output Format

### Standard Fields (Always Exported)
- **Model**: Equipment model
- **Serial Number**: Serial number
- **Due Date**: Next calibration due date
- **Status**: Calibration status

### Extended Fields (Optional)
When "Include Extended Fields" is checked:
- **Calibration Date**: Last calibration date
- **Location**: Equipment location
- **Notes**: Additional information (Material No, Job No)

---

## Example: Broadcom Calibration Campaign 2025

**File**: `Broadcom Calibration Campaign 2025.eml`

This email contains calibration data for equipment calibrated at Broadcom in March 2025.

**Expected Results**:
- Multiple records extracted from the email
- Each record contains: Model, Serial No, Calibration Date, Due Date, Status
- Material numbers and Job numbers preserved in Notes field
- Location (Broadcom) preserved

---

## Technical Details

### Parser Priority

The Email Parser is registered **first** in the parser chain, ensuring `.eml` files are properly handled:

```csharp
_parsers.Add(new EmailParser());      // Email (.eml) files
_parsers.Add(new FM002Parser());      // Specific for FM-002 files
_parsers.Add(new BroadcomParser());   // General Broadcom files
```

### File Detection

The parser uses a simple extension check:
```csharp
public bool CanParse(string filePath)
{
    var extension = Path.GetExtension(filePath).ToLower();
    return extension == ".eml";
}
```

### Parsing Logic

1. **Read email file**: Load entire `.eml` file content
2. **Extract quoted lines**: Find lines starting with `>` (quoted email body)
3. **Locate header**: Find the table header row
4. **Skip headers**: Skip past all header lines
5. **Parse records**: Extract 8 fields per record in sequence
6. **Create records**: Convert extracted data into `CalibrationRecord` objects

---

## Troubleshooting

### Problem: No records extracted

**Possible causes**:
- Email format doesn't match expected structure
- Headers not detected correctly
- Data not in quoted reply format (lines should start with `>`)

**Solutions**:
1. Open the `.eml` file in a text editor
2. Verify it contains quoted lines starting with `>`
3. Check that the header line contains "Model" and field names
4. Ensure data follows the 8-field pattern

### Problem: Dates not parsing correctly

**Possible causes**:
- Date format not supported
- Dates contain extra characters or formatting

**Solutions**:
1. Check the date format in the email
2. If needed, add the format to `ParseDate()` method in `EmailParser.cs`

### Problem: Records have missing data

**Possible causes**:
- Some fields are empty in the email
- Field order doesn't match expected sequence

**Solutions**:
1. Verify the email has all 8 fields for each record
2. Check that fields appear in the correct order
3. Empty fields will result in empty strings in the output

---

## Debug Mode

To see detailed parsing logs:

1. Open the project in Visual Studio
2. Run in Debug mode (F5)
3. Check the Output window for messages like:
```
EmailParser: Starting to parse Broadcom Calibration Campaign 2025.eml
EmailParser: Found 1024 quoted lines
EmailParser: Found header at line 40: *Model*
EmailParser: Data starts at line 48
EmailParser: Record 1: Model=ZNBT8, Serial=101930, Status=Passed, DueDate=2026-03-10
EmailParser: Record 2: Model=ZNB8, Serial=103487, Status=Passed, DueDate=2026-03-10
...
EmailParser: Extracted 45 records, skipped 150 empty lines
```

---

## File Support Summary

The Calibration Log Converter now supports:

| File Type | Extension | Parser | Priority |
|-----------|-----------|--------|----------|
| Email | `.eml` | EmailParser | 1st (Highest) |
| FM-002 Report | `.xlsx`, `.xls` | FM002Parser | 2nd |
| Broadcom General | `.xlsx`, `.xls`, `.xlsb` | BroadcomParser | 3rd |

---

## Code Files

### EmailParser.cs
- **Location**: `CalibrationLogConverter\Parsers\EmailParser.cs`
- **Lines**: 230+
- **Purpose**: Parse `.eml` files containing calibration data

### MainWindow.xaml.cs (Updated)
- **Changes**:
  - Added EmailParser registration
  - Updated file filter to include `.eml` files
  - Modified auto-load to detect email files

---

## Testing

### Quick Test

1. Run `TEST_EMAIL_PARSER.bat`
2. Application loads automatically
3. Click "Parse Files"
4. Verify records appear in preview grid
5. Export to Excel to verify output

### Manual Test

1. Place test `.eml` file in Raw_Data folder
2. Run application
3. Verify file appears in file list
4. Parse and review results
5. Check exported Excel file

---

## Future Enhancements

Possible improvements:
- Support for HTML-formatted emails
- Attachment extraction (if calibration data is in attachments)
- Multiple table formats within same email
- Email metadata extraction (sender, date, subject)

---

## Support

For issues or questions:
1. Check the Debug output for detailed parsing logs
2. Verify the email format matches the expected structure
3. Test with the sample file: `Broadcom Calibration Campaign 2025.eml`
4. Review this documentation for troubleshooting tips

---

## Summary

✅ **Email Parser successfully added to Calibration Log Converter**

- Supports `.eml` files with structured calibration data
- Automatically extracts all records from email body
- Preserves all important fields (Model, Serial, Dates, Status)
- Fully integrated with existing export functionality
- Works alongside Excel parsers seamlessly

**Ready to use!** 🎉










