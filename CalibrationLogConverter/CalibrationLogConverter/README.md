# 📊 Calibration Log File Converter

A C# WPF application for converting vendor-specific calibration log files into a standardized Excel format.

## 🎯 Purpose

This application helps you extract calibration data from multiple vendor log files and consolidate them into a single, standardized Excel report with:
- **Model** - Equipment model number
- **Serial Number** - Unique device identifier  
- **Due Date** - Next calibration due date
- **Extended fields** (optional) - Calibration date, status, location, technician, notes, etc.

## ✨ Features

### Core Features
- ✅ **Multi-file Support** - Process multiple Excel files at once
- ✅ **Automatic Detection** - Smart parser selection based on file content
- ✅ **Live Preview** - See extracted data before exporting
- ✅ **Flexible Export** - Choose basic or extended field output
- ✅ **Summary Reports** - Automatic statistics and analysis
- ✅ **Auto-load** - Automatically loads files from `C:\Users\wanlimah\Documents\Raw_Data`

### Supported File Formats
- Excel 2007+ (`.xlsx`)
- Excel 97-2003 (`.xls`)
- Excel Binary (`.xlsb`)

### Supported Vendors
- **Broadcom** - Calibration logs from Broadcom field operations
  - FM-002 Field Calibration Daily Reports
  - Logsheet formats
  - Campaign reports

## 🚀 Getting Started

### Prerequisites
- Windows 10 or later
- .NET 9.0 Runtime (or SDK for development)

### Installation
1. Download the latest release
2. Extract to your preferred location
3. Run `CalibrationLogConverter.exe`

### Quick Start
1. **Launch** the application
2. **Auto-load**: If files exist in `C:\Users\wanlimah\Documents\Raw_Data`, they'll load automatically
   - Or click **Browse...** to select files manually
3. **Click** "🔄 Parse Files" to extract data
4. **Review** the data in the preview grid
5. **Click** "💾 Export to Excel" to save the standardized report

## 📖 User Guide

### Main Interface

#### 📂 Input Files Section
- Shows selected files
- Click **Browse...** to select different files
- Supports multi-file selection

#### ⚙️ Export Options
- **Include extended fields**: Adds calibration date, status, location, technician, vendor, notes, and source tracking
- **Automatically open Excel file**: Opens the exported file immediately after creation

#### 📋 Preview Grid
- Displays extracted calibration records
- Shows all fields from all parsed files
- Sortable columns
- Scrollable for large datasets

#### Status Bar
- **Left**: Current operation status
- **Right**: Total record count

#### Action Buttons
- **🔄 Parse Files**: Extract data from selected files
- **💾 Export to Excel**: Save to standardized format
- **🗑️ Clear**: Reset and start over

### Workflow Example

```
1. Start Application
   ↓
2. Files auto-load from Raw_Data folder
   ↓
3. Click "Parse Files"
   ↓
4. Review data in preview grid
   ↓
5. Select export options
   ↓
6. Click "Export to Excel"
   ↓
7. Save file and review
```

## 📊 Output Format

### Basic Output (3 columns)
| Model | Serial Number | Due Date |
|-------|---------------|----------|
| DMM-6500 | 12345678 | 2025-12-31 |

### Extended Output (11 columns)
| Model | Serial Number | Due Date | Cal Date | Status | Location | Technician | Vendor | Notes | Source File | Source Row |
|-------|---------------|----------|----------|--------|----------|------------|--------|-------|-------------|------------|
| DMM-6500 | 12345678 | 2025-12-31 | 2025-01-15 | PASS | Site A | John Doe | Broadcom | OK | report.xlsx | 5 |

### Summary Sheet
The exported Excel file includes a **Summary** sheet with:
- Total record count
- Export timestamp
- Records by vendor
- Records by source file
- Records with/without due dates
- **Upcoming calibrations** (within 30 days)
- **Overdue calibrations**

## 🔧 Technical Details

### Architecture
```
CalibrationLogConverter/
├── Models/
│   └── CalibrationRecord.cs      # Data model
├── Parsers/
│   ├── ICalibrationParser.cs     # Parser interface
│   └── BroadcomParser.cs         # Broadcom-specific parser
├── Services/
│   └── ExcelExportService.cs     # Excel export functionality
└── MainWindow.xaml/.cs           # UI and logic
```

### How It Works

1. **File Selection**: User selects Excel files
2. **Parser Detection**: System identifies appropriate parser based on filename/content
3. **Data Extraction**: Parser reads Excel sheets and extracts calibration records
4. **Smart Column Mapping**: Finds columns by name variations (case-insensitive)
   - Model: "model", "equipment", "instrument", "device"
   - Serial: "serial", "serial number", "s/n", "sn"
   - Due Date: "due date", "next cal", "next calibration", "expiry", "cal due"
5. **Validation**: Skips empty rows, validates required fields
6. **Preview**: Displays in data grid
7. **Export**: Generates formatted Excel with styling and summary

### Adding New Vendor Parsers

To support additional vendors, create a new parser class:

```csharp
public class NewVendorParser : ICalibrationParser
{
    public string VendorName => "New Vendor";
    
    public bool CanParse(string filePath)
    {
        // Logic to identify vendor files
        var fileName = Path.GetFileName(filePath).ToLower();
        return fileName.Contains("vendor-specific-keyword");
    }
    
    public List<CalibrationRecord> ParseFile(string filePath)
    {
        // Implementation for parsing vendor-specific format
        // Return list of CalibrationRecord objects
    }
}
```

Then register it in `MainWindow.xaml.cs`:

```csharp
private void InitializeParsers()
{
    _parsers.Add(new BroadcomParser());
    _parsers.Add(new NewVendorParser());  // Add your parser here
}
```

## 🐛 Troubleshooting

### Issue: No files auto-load on startup
**Solution**: Check that files exist in `C:\Users\wanlimah\Documents\Raw_Data`

### Issue: Parser not found for file
**Solution**: 
- Check if filename contains vendor-specific keywords
- Verify file format is supported (xlsx, xls, xlsb)
- Add a custom parser for your vendor

### Issue: Parsing error
**Solution**:
- Check if Excel file is corrupted
- Verify file contains expected data structure
- Review error message for specific details

### Issue: Export fails
**Solution**:
- Ensure destination folder exists and is writable
- Close Excel if target file is already open
- Check disk space

### Issue: Missing columns in output
**Solution**:
- Enable "Include extended fields" option
- Verify source files contain the expected columns

## 📦 Dependencies

- **EPPlus 8.2.1** - Excel file creation and formatting
- **ExcelDataReader 3.8.0** - Excel file parsing
- **ExcelDataReader.DataSet 3.8.0** - DataSet support for Excel

## 📄 License

This application uses EPPlus under the NonCommercial license. For commercial use, please obtain an EPPlus commercial license.

## 🤝 Support

For issues or questions:
1. Check the Troubleshooting section
2. Review the error messages carefully
3. Contact the development team

## 📝 Version History

### Version 1.0.0 (October 30, 2025)
- ✅ Initial release
- ✅ Broadcom parser support
- ✅ Multi-file processing
- ✅ Extended field support
- ✅ Summary reports
- ✅ Auto-load from Raw_Data folder

## 🎓 Tips & Best Practices

1. **Always preview** data before exporting
2. **Keep source files** in a consistent location
3. **Use extended fields** for comprehensive tracking
4. **Review the Summary sheet** for quick insights
5. **Regular exports** - Export frequently to track calibration status
6. **Backup** - Keep copies of source files and exports

---

**Built with ❤️ using C# and WPF**

