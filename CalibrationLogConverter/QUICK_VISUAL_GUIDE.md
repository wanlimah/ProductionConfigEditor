# 📊 Quick Visual Guide - Export Format Change

## Excel Output Layout

### ✅ NEW FORMAT (Starting Column B)

```
┌─────────┬────────────────┬───────────────┬────────────┐
│    A    │       B        │       C       │     D      │
├─────────┼────────────────┼───────────────┼────────────┤
│ (Empty) │ Model          │ Serial Number │ Due Date   │  ← Row 1: Headers
├─────────┼────────────────┼───────────────┼────────────┤
│ (Empty) │ Oscilloscope   │ SN12345       │ 2025-12-31 │  ← Row 2: Data
├─────────┼────────────────┼───────────────┼────────────┤
│ (Empty) │ Multimeter     │ SN67890       │ 2025-11-15 │  ← Row 3: Data
├─────────┼────────────────┼───────────────┼────────────┤
│ (Empty) │ Power Supply   │ SN11223       │ 2026-01-20 │  ← Row 4: Data
└─────────┴────────────────┴───────────────┴────────────┘
```

**Column A is FREE for your use!**

### 💡 What You Can Do With Column A

Add any of these:
- ☑ Checkboxes for tracking
- 🔢 Custom numbering (1, 2, 3...)
- 🚦 Status indicators (✓, ✗, ⚠)
- 🎯 Priority levels (High, Medium, Low)
- 📝 Custom notes or flags
- 🏢 Department codes
- 👤 Reviewer initials

## 🚀 How to Use

### Step 1: Place Files in Raw_Data
```
C:\Users\wanlimah\Documents\Raw_Data\
  ├── vendor_calibration_2025.xlsx
  ├── equipment_log.xlsx
  └── onsite_cal_october.xlsx
```

### Step 2: Run the Application
```powershell
# Option 1: Run from source
cd CalibrationLogConverter\CalibrationLogConverter
dotnet run

# Option 2: Run the executable
.\CalibrationLogConverter\CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe
```

### Step 3: Parse and Export
1. ✅ Application auto-loads files from Raw_Data
2. ✅ Click "🔄 Parse Files" button
3. ✅ Review extracted data in preview grid
4. ✅ Click "💾 Export to Excel" button
5. ✅ Choose save location
6. ✅ Done! Your file has data starting from Column B

## 📋 Export Options

### Standard Export (Default)
```
Column B: Model
Column C: Serial Number
Column D: Due Date
```

### Extended Export (Optional)
```
Column B: Model
Column C: Serial Number
Column D: Due Date
Column E: Calibration Date
Column F: Status
Column G: Location
Column H: Technician
Column I: Vendor
Column J: Notes
Column K: Source File
Column L: Source Row
```

## ✨ Features Preserved

✅ Automatic file detection from Raw_Data  
✅ Multi-vendor parser support (Broadcom, etc.)  
✅ Smart column detection  
✅ Live data preview  
✅ Batch processing  
✅ Summary sheet with statistics  
✅ Auto-open Excel after export  
✅ Due date warnings and overdue alerts  

## 🎯 Key Information Extracted

**Required Fields:**
1. **Model** - Equipment/Instrument name
2. **Serial Number** - S/N identification
3. **Due Date** - Next calibration due date

**Optional Fields (Extended Export):**
- Calibration Date (last calibrated)
- Status (Pass/Fail/Result)
- Location (Site/Facility)
- Technician (Who performed calibration)
- Vendor information
- Notes/Remarks

## 📍 Data Source Location

**Default Path**: `C:\Users\wanlimah\Documents\Raw_Data`

**Supported File Types**:
- `.xlsx` (Excel 2007+)
- `.xls` (Excel 97-2003)
- `.xlsb` (Excel Binary)

## 🔧 Customization

Want to change the starting column? Edit `ExcelExportService.cs`:

```csharp
// Current: starts at column B (column 2)
int col = 2;

// To start at column C instead:
int col = 3;

// To start at column A:
int col = 1;
```

---

**Pro Tip**: Use Column A for sequential numbering to make it easy to reference specific equipment during meetings or reviews!

**Example**:
```
A    B              C           D
1    Oscilloscope   SN12345     2025-12-31
2    Multimeter     SN67890     2025-11-15
3    Power Supply   SN11223     2026-01-20
```

---

✅ **Ready to Use!** No additional configuration needed.














