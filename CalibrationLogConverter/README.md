# 📊 Calibration Log File Converter

A powerful C# WPF application to convert vendor calibration log files into a standardized Excel format with Model, Serial Number, and Due Date information.

## ✨ Features

### Core Functionality
- ✅ **Multi-Vendor Support** - Extensible parser system for different vendor formats
- ✅ **Automatic File Detection** - Automatically loads files from `C:\Users\wanlimah\Documents\Raw_Data`
- ✅ **Smart Column Mapping** - Intelligently finds and extracts Model, Serial Number, and Due Date columns
- ✅ **Batch Processing** - Process multiple Excel files at once
- ✅ **Live Preview** - See extracted data before exporting
- ✅ **Rich Excel Export** - Professional Excel output with formatting and summary sheet

### Data Extraction
The converter extracts the following fields:
- **Model** (Equipment/Instrument name)
- **Serial Number** (S/N)
- **Due Date** (Next calibration date)
- **Calibration Date** (When last calibrated)
- **Status** (Pass/Fail/Result)
- **Location** (Site/Facility)
- **Technician** (Who performed calibration)
- **Notes** (Remarks/Comments)

### Export Options
- **Standard Export**: Model, Serial Number, Due Date only
- **Extended Export**: All fields including Status, Location, Technician, etc.
- **Auto-Open**: Automatically open Excel after export
- **Summary Sheet**: Includes statistics and overdue warnings

## 🚀 Getting Started

### Prerequisites
- Windows 10 or later
- .NET 9.0 Runtime
- Microsoft Excel (optional, for viewing exported files)

### Installation

1. **Clone or download the project**
   ```bash
   cd C:\Users\wanlimah\DigitalProductionConfigEditor\CalibrationLogConverter
   ```

2. **Build the application**
   ```bash
   cd CalibrationLogConverter
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

## 📖 How to Use

### Quick Start

1. **Launch the Application**
   - The app automatically loads files from `C:\Users\wanlimah\Documents\Raw_Data`
   - Or click **Browse...** to select files manually

2. **Parse Files**
   - Click **🔄 Parse Files** button
   - Review the extracted data in the preview grid
   - Check the record count at the bottom

3. **Export to Excel**
   - Choose export options (extended fields, auto-open)
   - Click **💾 Export to Excel** button
   - Select save location and filename
   - Done! Your standardized calibration report is ready

### Supported File Formats
- `.xlsx` (Excel 2007+)
- `.xls` (Excel 97-2003)
- `.xlsb` (Excel Binary)
- `.eml` (Email files with calibration data)

### Vendor-Specific Parsers

#### Currently Supported:
1. **Email Parser**
   - Extracts calibration data from `.eml` files
   - Supports structured email formats
   - Perfect for calibration campaign emails
   - See: `EMAIL_PARSER_QUICK_START.md`

2. **FM-002 Parser**
   - Specialized for FM-002 Field Calibration Daily Report (Broadcom PG)
   - Handles "On-site update template" worksheets
   - Multi-section data extraction
   - See: `FM002_QUICK_START.md`

3. **Broadcom Parser**
   - Detects files containing: "broadcom", "fm-002", "logsheet"
   - Handles multiple sheet formats
   - Smart column detection

#### Adding New Vendors:
See the **Developer Guide** section below.

## 📊 Output Format

### Standard Export (Model, Serial Number, Due Date)

**Note**: Data starts from **Column B** in the Excel output, leaving Column A empty for your custom use.

```
|    | Model          | Serial Number | Due Date   |
|----|----------------|---------------|------------|
|    | Oscilloscope   | SN12345       | 2025-12-31 |
|    | Multimeter     | SN67890       | 2025-11-15 |
```

### Extended Export (All Fields)
```
|    | Model | Serial Number | Due Date | Cal Date | Status | Location | Technician | Vendor | Notes | Source File | Source Row |
```

### Summary Sheet Includes:
- Total record count
- Records by vendor
- Records by source file
- Records with/without due dates
- **⚠️ Due within 30 days** (highlighted in red)
- **⚠️ Overdue calibrations** (highlighted in dark red)

## 🛠️ Project Structure

```
CalibrationLogConverter/
├── Models/
│   └── CalibrationRecord.cs       # Data model for calibration records
├── Parsers/
│   ├── ICalibrationParser.cs      # Parser interface
│   ├── EmailParser.cs             # Email (.eml) parser
│   ├── FM002Parser.cs             # FM-002 specialized parser
│   └── BroadcomParser.cs          # Broadcom vendor parser
├── Services/
│   └── ExcelExportService.cs      # Excel export functionality
├── MainWindow.xaml                # UI design
├── MainWindow.xaml.cs             # Main application logic
├── CalibrationLogConverter.csproj # Project configuration
├── EMAIL_PARSER_DOCUMENTATION.md  # Email parser guide
├── EMAIL_PARSER_QUICK_START.md    # Email parser quick start
├── FM002_QUICK_START.md           # FM-002 parser guide
└── README.md                      # This file
```

## 👨‍💻 Developer Guide

### Adding a New Vendor Parser

1. **Create a new parser class** in the `Parsers/` folder:

```csharp
using CalibrationLogConverter.Models;
using CalibrationLogConverter.Parsers;

public class VendorXParser : ICalibrationParser
{
    public string VendorName => "VendorX";

    public bool CanParse(string filePath)
    {
        var fileName = Path.GetFileName(filePath).ToLower();
        return fileName.Contains("vendorx");
    }

    public List<CalibrationRecord> ParseFile(string filePath)
    {
        // Your parsing logic here
        var records = new List<CalibrationRecord>();
        
        // Example:
        // - Open Excel file
        // - Find relevant columns
        // - Extract data row by row
        // - Create CalibrationRecord objects
        
        return records;
    }
}
```

2. **Register the parser** in `MainWindow.xaml.cs`:

```csharp
private void InitializeParsers()
{
    // Register parsers in priority order (most specific first)
    _parsers.Add(new EmailParser());      // Email files
    _parsers.Add(new FM002Parser());      // FM-002 specific
    _parsers.Add(new BroadcomParser());   // General Broadcom
    _parsers.Add(new VendorXParser());    // Add your new parser
}
```

### Parser Development Tips

- Use `FindColumn()` helper method to locate columns by multiple possible names
- Handle empty rows and null values gracefully
- Set the `Vendor` field to identify source
- Set `SourceFile` and `SourceRow` for traceability
- Support multiple sheets in workbooks
- Test with actual vendor files

### Column Detection Strategy

The `BroadcomParser` uses flexible column matching:
```csharp
// Will match "Model", "Equipment", "Instrument", or "Device"
int modelCol = FindColumn(table, "model", "equipment", "instrument", "device");

// Will match "Serial", "Serial Number", "S/N", or "SN"
int serialCol = FindColumn(table, "serial", "serial number", "s/n", "sn");
```

## 📦 NuGet Packages Used

- **EPPlus** (8.2.1) - Excel file writing with rich formatting
- **ExcelDataReader** (3.8.0) - Reading Excel files (.xlsx, .xls, .xlsb)
- **ExcelDataReader.DataSet** (3.8.0) - DataSet support for ExcelDataReader

## 🔧 Configuration

### Default Raw_Data Path
The application looks for files in:
```
C:\Users\wanlimah\Documents\Raw_Data
```

To change this, edit `MainWindow.xaml.cs`:
```csharp
string defaultPath = @"C:\Your\Custom\Path";
```

### EPPlus License
The application uses EPPlus under the **NonCommercial** license. For commercial use, purchase an EPPlus license and update:
```csharp
OfficeOpenXml.ExcelPackage.License = OfficeOpenXml.LicenseMode.Commercial;
```

## 🐛 Troubleshooting

### Issue: "No suitable parser found"
**Solution**: The filename doesn't match any registered parser. Either:
- Rename the file to include vendor keywords (e.g., "broadcom")
- Create a custom parser for that vendor

### Issue: "Error parsing file"
**Solution**: 
- Ensure the Excel file is not corrupted
- Check if the file has the expected column structure
- Review the error message for specific details

### Issue: Dates not parsing correctly
**Solution**: 
- The parser tries multiple date formats
- Ensure dates are in recognizable format (YYYY-MM-DD, MM/DD/YYYY, etc.)
- Check Excel cell format (should be Date, not Text)

### Issue: Missing data in export
**Solution**:
- Check the preview grid to see what was extracted
- Verify source Excel file has data in recognized columns
- Enable "Include extended fields" for more data

## 📝 Version History

### Version 1.2 (November 2025)
- ✨ **NEW**: Email parser for `.eml` files
- Extract calibration data directly from emails
- Support for Broadcom Calibration Campaign emails
- Comprehensive email parser documentation

### Version 1.1 (November 2025)
- ✨ **NEW**: FM-002 specialized parser
- Multi-section data extraction
- "On-site update template" support
- Enhanced status logic

### Version 1.0 (October 2025)
- Initial release
- Broadcom vendor support
- Smart column detection
- Excel export with summary sheet
- Auto-load from Raw_Data folder
- Preview grid with live data

## 🤝 Contributing

To add support for additional vendors:
1. Obtain sample calibration log files
2. Create a new parser class implementing `ICalibrationParser`
3. Test with actual files
4. Register in `InitializeParsers()`

## 📄 License

This application is for internal use. EPPlus is used under NonCommercial license.

## 📧 Support

For issues or questions:
- Check the Troubleshooting section
- Review the error messages in the application
- Examine the source Excel files for data quality

---

**Built with ❤️ using C# and WPF**



