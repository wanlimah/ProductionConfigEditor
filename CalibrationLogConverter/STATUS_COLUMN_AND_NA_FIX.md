# Status Column and N/A Fix
**Date:** November 5, 2025

## User Request

> "The calibration fail row will be blank should we put the NA and add a Status column to Identified PASS or FAIL"

## Requirements

1. **Show "N/A"** for blank due dates instead of leaving them empty
2. **Add Status column** (Column E) to show "PASS" or "FAIL"

## Implementation

### 1. Status Detection Logic

**How PASS/FAIL is Determined:**
- Rows with **"X" marker** in Model or Serial Number = **FAIL**
- All other rows = **PASS**

**Code Changes in `BroadcomParser.cs`:**

```csharp
// Determine calibration status (PASS or FAIL)
// "X" marker indicates calibration failure
string status = "PASS";
if (model == "X" || serialNumber == "X")
{
    status = "FAIL";
    // Don't skip FAIL records - include them in export but mark as FAIL
}
```

**Important Change:**
- **BEFORE:** Rows with "X" markers were **skipped** entirely
- **AFTER:** Rows with "X" markers are **included** but marked as **FAIL**

This means failed calibrations are now visible in the export!

### 2. Export Format Changes

**New Column Structure:**

```
Column A: (Empty/Reserved)
Column B: Model
Column C: Serial Number
Column D: Due Date (shows "N/A" if blank)
Column E: Status (PASS or FAIL)  ← NEW!
```

**Code Changes in `ExcelExportService.cs`:**

#### Headers (Always Include Status):
```csharp
worksheet.Cells[1, col++].Value = "Model";
worksheet.Cells[1, col++].Value = "Serial Number";
worksheet.Cells[1, col++].Value = "Due Date";
worksheet.Cells[1, col++].Value = "Status";  // Always included (Column E)
```

#### Data Rows:
```csharp
// Due Date - show "N/A" if blank
if (record.DueDate.HasValue)
{
    worksheet.Cells[row, col].Value = record.DueDate.Value;
    worksheet.Cells[row, col].Style.Numberformat.Format = "yyyy-MM-dd";
}
else
{
    worksheet.Cells[row, col].Value = "N/A";
}

// Status - with red highlighting for FAIL
worksheet.Cells[row, col].Value = record.Status;
if (record.Status == "FAIL")
{
    worksheet.Cells[row, col].Style.Font.Color.SetColor(System.Drawing.Color.Red);
    worksheet.Cells[row, col].Style.Font.Bold = true;
}
```

### 3. Summary Sheet Enhancements

Added calibration status statistics:

```
Calibration Status:
  PASS: [count] (green, bold)
  FAIL: [count] (red, bold if > 0)
```

## Example Output

### Scenario 1: Normal Record (PASS)
```
Model    | Serial Number | Due Date    | Status
---------|---------------|-------------|--------
M9803A   | MY58101860    | 2026-09-22  | PASS
```

### Scenario 2: Failed Calibration
```
Model    | Serial Number | Due Date    | Status
---------|---------------|-------------|--------
X        | MY58101860    | N/A         | FAIL (red, bold)
```

### Scenario 3: No Due Date (PASS)
```
Model    | Serial Number | Due Date    | Status
---------|---------------|-------------|--------
E5071C   | MY46519787    | N/A         | PASS
```

## Visual Formatting

### Status Column (Column E):
- **PASS**: Normal text, black
- **FAIL**: **Red, Bold** for easy identification

### Due Date Column (Column D):
- **Has date**: Formatted as `yyyy-MM-dd` (e.g., 2026-09-22)
- **No date**: Shows `N/A` as text

## Files Modified

### 1. `BroadcomParser.cs`
**Changes:**
- Lines 195-203: Added status detection logic
- Line 246: Set `Status = status` in record creation
- Removed skipping of "X" marker rows (now included as FAIL)

### 2. `ExcelExportService.cs`
**Changes:**
- Line 40: Added "Status" header (always included)
- Lines 71-91: Show "N/A" for blank due dates, add Status column with red highlighting for FAIL
- Lines 180-199: Added PASS/FAIL statistics to Summary sheet

## Impact

### More Records Exported
**BEFORE:**
- Records with "X" markers were skipped
- Only PASS records were exported

**AFTER:**
- Records with "X" markers are included as FAIL
- Both PASS and FAIL records are exported
- You get a complete picture of all calibrations

### Better Visibility
- ✅ Easy to identify failed calibrations (red, bold)
- ✅ Easy to identify missing due dates ("N/A")
- ✅ Status column provides clear PASS/FAIL indication
- ✅ Summary sheet shows PASS/FAIL counts

## Build Status

Ready to build after closing the application.

## Testing Instructions

### 1. Close Application
Close CalibrationLogConverter if it's running.

### 2. Rebuild
```powershell
cd CalibrationLogConverter\CalibrationLogConverter
dotnet build -c Release
```

### 3. Run Application
```
RUN_APP.bat
```

### 4. Test with Your File
- Load: `Logsheet_Broadcom_Sep25_WanLing (version 1).xlsb`
- Click "Parse Files"
- Click "Export to Excel"

### 5. Verify Output

#### Check Exported Excel File:

**Column Headers:**
```
B: Model
C: Serial Number
D: Due Date
E: Status  ← NEW!
```

**Data Verification:**
- ☐ Due Date shows "N/A" for records without dates
- ☐ Status column shows "PASS" or "FAIL"
- ☐ FAIL rows have red, bold Status
- ☐ All records are included (even failed calibrations)

**Summary Sheet:**
- ☐ Shows "Calibration Status:" section
- ☐ Shows PASS count (green)
- ☐ Shows FAIL count (red if > 0)

## Expected Results

### Parse Complete:
```
✅ Records extracted: 28 (or more, including FAIL records now)
✅ Records with due dates: [count]
✅ Records with Status: 28 (all records have status)
```

### Export Format:
```
Model      | Serial Number | Due Date    | Status
-----------|---------------|-------------|--------
M9803A     | MY58101860    | 2026-09-22  | PASS
E5071C     | MY46519787    | 2026-09-23  | PASS
X          | MY123456      | N/A         | FAIL (red)
M8000A     |               | N/A         | PASS
```

### Summary Sheet:
```
Total Records: 28

Records with Due Dates: 25
Records without Due Dates: 3

Calibration Status:
  PASS: 25 (green)
  FAIL: 3 (red)

Due within 30 days: 0
Overdue: 0
```

## Benefits

### 1. Complete Data Visibility
- See all calibrations, including failures
- No records are hidden or skipped

### 2. Clear Status Indication
- PASS/FAIL clearly visible in Column E
- Red highlighting makes FAIL easy to spot

### 3. Better Data Quality
- "N/A" clearly indicates missing due dates
- Not confusing empty cells with actual blanks

### 4. Improved Reporting
- Summary sheet shows PASS/FAIL statistics
- Easy to track calibration success rate

## Technical Notes

### Status Assignment Logic
```
IF model == "X" OR serialNumber == "X"
  THEN status = "FAIL"
  ELSE status = "PASS"
```

### Due Date Display Logic
```
IF dueDate.HasValue
  THEN show formatted date (yyyy-MM-dd)
  ELSE show "N/A"
```

### FAIL Record Handling
- Previously: Skipped during parsing
- Now: Included in export with FAIL status
- Result: More complete dataset

---

**Status:** ✅ CODE UPDATED - READY TO BUILD  
**Date:** November 5, 2025  
**Changes:** Status column added, N/A for blank due dates, FAIL records included  
**Next Step:** Close app, rebuild, and test











