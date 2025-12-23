# Due Date Calculation - Debug Version
**Date:** November 5, 2025

## Your Issue

✅ **Model and Serial Number**: Extracting correctly  
❌ **Due Date**: Not showing the calculated date

## How Due Date Should Be Calculated

According to the Broadcom logsheet format:

```
Due Date = Calibration Date (Column D) + Cat Int Month (Column I)
```

### Example:
- **Calibration Date**: 22-Sep-2025 (Column D)
- **Cat Int**: 12 months (Column I)
- **Due Date**: 22-Sep-2026 ✅

## What I've Added - Enhanced Debugging

The application now has **comprehensive debugging** that will tell you exactly why due dates aren't being calculated.

### New Debug Features:

1. **Column Detection Display**
   - Shows which columns were found (Model, Serial, Cal Date, Cat Int)
   - Shows the column index number for each
   - Marks each column in the header list

2. **Row-by-Row Due Date Calculation**
   - Shows calibration date, interval, and calculated due date for each row
   - Shows why calculation failed if it did

3. **Automatic Diagnostic Popup**
   - If NO due dates are calculated, you'll see a popup explaining why
   - Shows which columns were found/not found
   - Lists all your column headers with index numbers

## How to Test

### STEP 1: Run the Application
```
RUN_APP.bat
```
or
```
CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe
```

### STEP 2: Load Your File
- Add: `Logsheet_Broadcom_Sep25_WanLing (version 1).xlsb`

### STEP 3: Click "Parse Files"

### STEP 4: Check the Results

You will see ONE of these scenarios:

#### Scenario A: ✅ SUCCESS
```
✅ Records extracted: [NUMBER]
✅ Due dates calculated successfully
```
→ Everything works! Proceed to export.

#### Scenario B: ⚠️ DUE DATE CALCULATION ISSUE
You'll see a popup that shows:
```
⚠️ DUE DATE CALCULATION ISSUE

✅ Records extracted: [NUMBER]
❌ Records with due dates: 0

COLUMN DETECTION:
  Cal Date Column: ❌ NOT FOUND (or ✅ Column X)
  Cat Int Column:  ❌ NOT FOUND (or ✅ Column X)

COLUMN HEADERS IN YOUR FILE:
  [0] Work Order
  [1] Item
  [2] Model Number
  [3] Serial Number
  [4] Date dd-mm-yy
  ...
  [8] Cat Int month
  ...
```

**→ TAKE A SCREENSHOT of this popup and share it with me!**

This will tell me:
1. What your actual column headers are named
2. Whether the columns are being detected
3. What column indices they're in

## What the Debug Output Tells Us

### If Cal Date Column = ❌ NOT FOUND
**Problem:** The calibration date column header doesn't match expected names

**Current Expected Names:**
- "date dd-mm-yy"
- "date (dd-mm-yy)"
- "date"
- "calibration date"
- "cal date"
- "calibrated"
- "cal. date"
- "last cal"
- "caldate"
- "cal-date"
- "date of calibration"

**Solution:** Once you share the screenshot, I can add your actual column name.

### If Cat Int Column = ❌ NOT FOUND
**Problem:** The calibration interval column header doesn't match expected names

**Current Expected Names:**
- "cat int month"
- "cat int (month)"
- "cat.int.month"
- "interval"
- "cal interval"
- "months"
- "due in"
- "frequency"
- "period"
- "catint"
- "cat int"

**Solution:** Once you share the screenshot, I can add your actual column name.

### If Both Columns = ✅ Found
**Problem:** The columns are detected, but data might be in wrong format

**Possible Causes:**
1. Date column contains text instead of dates
2. Cat Int column contains text instead of numbers
3. Date format not recognized by Excel parser
4. Values are empty/null

**Solution:** I'll need to see the actual data to diagnose further.

## Expected Column Layout

Based on your description (Column D and Column I):

```
Column A | Column B | Column C | Column D      | ... | Column I
---------|----------|----------|---------------|-----|---------------
(other)  | (other)  | (other)  | Date dd-mm-yy | ... | Cat Int month
---------|----------|----------|---------------|-----|---------------
```

The parser should automatically find these regardless of column position, as long as the headers match one of the expected variations.

## What Happens Behind the Scenes

### 1. Parser Reads Row 0 (Headers)
```csharp
Row 0: ["Work Order", "Model Number", "Serial Number", "Date dd-mm-yy", ..., "Cat Int month"]
```

### 2. Parser Finds Column Indices
```csharp
modelCol = 1      // "Model Number" found at index 1
serialCol = 2     // "Serial Number" found at index 2
calDateCol = 3    // "Date dd-mm-yy" found at index 3
catIntCol = 8     // "Cat Int month" found at index 8
```

### 3. Parser Reads Each Data Row
```csharp
Row 2: model="M9803A", serial="MY58101860", calDate="22-Sep-25", catInt=12
```

### 4. Parser Calculates Due Date
```csharp
dueDate = calDate + catInt months
dueDate = 22-Sep-2025 + 12 months = 22-Sep-2026 ✅
```

### 5. Parser Creates Record
```csharp
Record: Model="M9803A", Serial="MY58101860", DueDate="22-Sep-2026"
```

## Troubleshooting Steps

### Step 1: Run the Debug Version
Run the application and parse your file. You'll get a diagnostic popup.

### Step 2: Take a Screenshot
Capture the popup showing:
- Which columns were found/not found
- The list of column headers with index numbers

### Step 3: Share the Screenshot
Send me the screenshot so I can see:
- Your exact column header names
- Which columns are at which positions
- Whether columns are being detected

### Step 4: I'll Update the Parser
Based on your column names, I'll:
- Add your specific column name variations to the parser
- Ensure the correct columns are matched
- Handle any special formatting in your file

## Quick Reference: Column Name Matching

The parser uses **fuzzy matching** that handles:
- ✅ Case insensitive: "Date" = "date" = "DATE"
- ✅ Extra spaces: "Date  dd-mm-yy" = "Date dd-mm-yy"
- ✅ Special chars: "Date (dd-mm-yy)" = "Date dd-mm-yy"
- ✅ Partial match: Header contains "calibration date"

But it **still needs** the header to contain at least one of the expected keywords.

## Debug Output Location

If you're running in Visual Studio, you can also see detailed debug output in:
- **Output Window** → **Debug** tab

This shows:
```
ParseDataTable: Total rows = 29, ModelCol = 1, SerialCol = 2, CalDateCol = 3, CatIntCol = 8
Row 2: CalDate=2025-09-22, CatInt=12 months, DueDate=2026-09-22
Row 3: CalDate=2025-09-23, CatInt=12 months, DueDate=2026-09-23
...
```

## Next Steps

1. **Run the application with your file**
2. **Look for the diagnostic popup** (if due dates aren't calculated)
3. **Take a screenshot** of the popup showing column headers
4. **Share it with me** so I can update the parser

Once I see your exact column names, I can add them to the parser and ensure due dates are calculated correctly!

---

**Status:** ✅ DEBUG VERSION BUILT  
**Ready to test:** Yes  
**Waiting for:** Screenshot of column headers from your file












