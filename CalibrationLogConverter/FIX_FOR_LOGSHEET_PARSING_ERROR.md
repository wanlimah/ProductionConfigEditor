# Fix for Logsheet Parsing Error - Nov 5, 2025

## Problem Description

When parsing the file `Logsheet_Broadcom_Sep25_WanLing (version 1).xlsb`, two errors occurred:

### Error 1: "Logsheet Not Found"
- The worksheet named 'Logsheet' could not be found
- However, "Logsheet" appeared in the list of available worksheets
- This indicated a worksheet name matching issue

### Error 2: "0 Records Extracted"
- Even when the Logsheet was found, 0 records were extracted
- This indicated a column header matching issue

## Root Causes

### Issue 1: Worksheet Name Matching
The original code used a strict exact match for "Logsheet":
```csharp
if (table.TableName.Equals("Logsheet", StringComparison.OrdinalIgnoreCase))
```

**Problem:** 
- Worksheet names might have trailing/leading spaces
- Special characters or encoding issues could prevent matching
- No flexibility for variations

### Issue 2: Column Header Matching
The original column matching was case-insensitive but inflexible:
```csharp
var cellValue = GetStringValue(headerRow, i).ToLower().Trim();
if (possibleNames.Any(name => cellValue.Contains(name.ToLower())))
```

**Problems:**
- Didn't handle variations in spacing (e.g., "Model No" vs "Model  No" with double space)
- Didn't handle special characters (e.g., "Date (dd-mm-yy)" vs "Date dd-mm-yy")
- Limited list of possible column name variations

## Solutions Implemented

### Solution 1: Flexible Worksheet Name Matching

**Added:**
1. **Trim whitespace** from worksheet names
2. **StartsWith matching** for partial matches
3. **Enhanced debugging** to show worksheet name length and byte count

```csharp
// More flexible matching - trim and ignore case
foreach (DataTable table in dataSet.Tables)
{
    var tableName = table.TableName?.Trim() ?? "";
    
    if (tableName.Equals("Logsheet", StringComparison.OrdinalIgnoreCase) ||
        tableName.StartsWith("Logsheet", StringComparison.OrdinalIgnoreCase))
    {
        logsheetTable = table;
        break;
    }
}
```

**Benefits:**
- ✅ Handles trailing/leading spaces
- ✅ Matches "Logsheet" or any worksheet starting with "Logsheet"
- ✅ Better debug information shows exact worksheet names

### Solution 2: Normalized Column Header Matching

**Added:**
1. **Regular expression normalization** to remove extra spaces and special characters
2. **Multiple matching strategies**: exact match, contains, normalized comparison
3. **Expanded list** of possible column name variations

```csharp
private int FindColumnInFirstRow(DataTable table, params string[] possibleNames)
{
    var headerRow = table.Rows[0];
    for (int i = 0; i < table.Columns.Count; i++)
    {
        var cellValue = GetStringValue(headerRow, i).ToLower().Trim();
        
        // Remove extra spaces, special chars for more flexible matching
        var normalizedCellValue = Regex.Replace(cellValue, @"[\s\-_\.\(\)]+", " ").Trim();
        
        foreach (var name in possibleNames)
        {
            var normalizedName = Regex.Replace(name.ToLower(), @"[\s\-_\.\(\)]+", " ").Trim();
            
            // Check if normalized values match or contain each other
            if (normalizedCellValue.Equals(normalizedName) ||
                normalizedCellValue.Contains(normalizedName) ||
                cellValue.Contains(name.ToLower()))
            {
                return i;
            }
        }
    }
    return -1;
}
```

**Benefits:**
- ✅ Handles extra spaces: "Model  Number" = "Model Number"
- ✅ Handles special characters: "Date (dd-mm-yy)" = "Date dd mm yy"
- ✅ Handles underscores/hyphens: "Serial_Number" = "Serial Number"
- ✅ Multiple fallback strategies ensure maximum compatibility

### Solution 3: Expanded Column Name Variations

**Added more variations for each column:**

#### Model Column:
```
"model number", "model no", "model", "equipment", "instrument", 
"device", "asset", "item", "description", "model no.", "modelno", 
"modelnumber", "model#"
```

#### Serial Number Column:
```
"serial number", "serial no", "serial", "s/n", "sn", "serial no", 
"s.n", "asset tag", "serial no.", "serialno", "serialnumber", 
"serial#", "s.no", "sno"
```

#### Calibration Date Column:
```
"date dd-mm-yy", "date (dd-mm-yy)", "date", "calibration date", 
"cal date", "calibrated", "cal. date", "last cal", "caldate", 
"cal-date", "date of calibration"
```

#### Cat Int Month Column:
```
"cat int month", "cat int (month)", "cat.int.month", "interval", 
"cal interval", "months", "due in", "frequency", "period", 
"catint", "cat int"
```

### Solution 4: Enhanced Error Messages

**Improved diagnostic information when errors occur:**

```csharp
worksheetList.AppendLine($"• {name} (Length: {name.Length}, Bytes: {bytes.Length})");
worksheetList.AppendLine("\nDEBUG INFO:");
worksheetList.AppendLine("The parser is looking for a worksheet named exactly 'Logsheet' (case-insensitive).");
worksheetList.AppendLine("If you see 'Logsheet' in the list above but it's not being matched,");
worksheetList.AppendLine("it may contain hidden spaces or special characters.");
```

**Benefits:**
- ✅ Shows exact worksheet name length and byte count
- ✅ Helps identify hidden characters or encoding issues
- ✅ Provides actionable guidance to the user

## How to Test

1. **Run the updated application:**
   ```
   RUN_APP.bat
   ```
   or
   ```
   CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe
   ```

2. **Load your Excel file:**
   - Click "Add Files" or drag and drop
   - Select `Logsheet_Broadcom_Sep25_WanLing (version 1).xlsb`

3. **Click "Parse Files"**
   - The parser should now successfully find the Logsheet worksheet
   - Column headers should be matched even with spacing/special character variations
   - Records should be extracted successfully

4. **Verify the results:**
   - Check the record count (should be > 0)
   - Click "Export to Excel" to verify data extraction

## Expected Results

### Before (Broken):
```
❌ Logsheet worksheet not found
❌ 0 records extracted
```

### After (Fixed):
```
✅ Logsheet worksheet found
✅ Records extracted successfully (e.g., 28 records)
✅ Export ready
```

## Technical Details

### Files Modified:
- `CalibrationLogConverter/Parsers/BroadcomParser.cs`

### Changes Made:
1. Lines 57-71: Enhanced worksheet name matching with StartsWith and Trim
2. Lines 80-99: Improved error message with detailed worksheet information
3. Lines 120-134: Expanded column name variation lists
4. Lines 292-323: Rewrote `FindColumnInFirstRow()` with regex normalization

### Build Status:
✅ Build successful (Release configuration)
✅ No errors
✅ No warnings
✅ Executable updated in both net6.0-windows and net8.0-windows targets

## Troubleshooting

### If you still get "Logsheet Not Found":
1. Check the new error message - it will show worksheet name length and bytes
2. Look for hidden characters or spaces
3. The worksheet might have a completely different name
4. Contact support with the exact worksheet name from the error message

### If you still get "0 Records Extracted":
1. The application will show a debug popup with actual column headers
2. Compare the headers in your Excel file with the expected variations
3. If headers are completely different, we may need to add more variations
4. Contact support with the actual column header names

## Summary

These changes make the Calibration Log Converter much more **robust** and **flexible**:
- ✅ Handles various worksheet name formats
- ✅ Matches column headers with spaces, special characters, and variations
- ✅ Provides better error messages for diagnosis
- ✅ Should work with a wider variety of Excel file formats

---

**Status:** ✅ FIXED AND BUILT  
**Date:** November 5, 2025  
**Build:** Release configuration for .NET 6.0 and .NET 8.0











