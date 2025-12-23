using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ExcelDataReader;
using CalibrationLogConverter.Models;

namespace CalibrationLogConverter.Parsers
{
    /// <summary>
    /// Parser specifically for FM-002_Field Calibration Daily Report (Broadcom PG)
    /// </summary>
    public class FM002Parser : ICalibrationParser
    {
        public string VendorName => "FM-002 (Broadcom PG)";

        public bool CanParse(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            // Only parse Excel files, not email files
            var extension = Path.GetExtension(filePath).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".xlsb")
                return false;

            var fileName = Path.GetFileName(filePath).ToLower();
            // Specifically match FM-002 files
            return fileName.Contains("fm-002") || fileName.Contains("fm002");
        }

        public List<CalibrationRecord> ParseFile(string filePath)
        {
            var records = new List<CalibrationRecord>();

            try
            {
                // Register encoding provider for reading all Excel formats
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Read WITHOUT using header row to ensure we get ALL rows
                        // This configuration will read ALL worksheets including hidden ones
                        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            // Read all sheets including hidden ones
                            FilterSheet = (tableReader, sheetIndex) => true,
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = false  // Changed to false to read all rows
                            }
                        });

                        // Process all worksheets that might contain calibration data
                        foreach (DataTable table in dataSet.Tables)
                        {
                            var tableName = table.TableName?.Trim() ?? "";
                            System.Diagnostics.Debug.WriteLine($"FM002Parser: Processing worksheet '{tableName}' with {table.Rows.Count} rows");
                            
                            // Look for common worksheet names in FM-002 files
                            // Priority 1: "On-site update template" (specific to FM-002)
                            // Priority 2: Other common names
                            if (tableName.Equals("On-site update template", StringComparison.OrdinalIgnoreCase) ||
                                tableName.Contains("on-site", StringComparison.OrdinalIgnoreCase) ||
                                tableName.Contains("update template", StringComparison.OrdinalIgnoreCase) ||
                                tableName.Equals("Logsheet", StringComparison.OrdinalIgnoreCase) ||
                                tableName.StartsWith("Logsheet", StringComparison.OrdinalIgnoreCase) ||
                                tableName.Contains("Calibration", StringComparison.OrdinalIgnoreCase) ||
                                tableName.Contains("Daily", StringComparison.OrdinalIgnoreCase) ||
                                tableName.Contains("Report", StringComparison.OrdinalIgnoreCase))
                            {
                                System.Diagnostics.Debug.WriteLine($"FM002Parser: Found matching worksheet: '{tableName}'");
                                var tableRecords = ParseDataTable(table, filePath);
                                records.AddRange(tableRecords);
                                System.Diagnostics.Debug.WriteLine($"FM002Parser: Extracted {tableRecords.Count} records from '{tableName}'");
                            }
                        }
                        
                        // If no specific worksheets found, try the first worksheet
                        if (records.Count == 0 && dataSet.Tables.Count > 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"FM002Parser: No specific worksheet found, trying first worksheet");
                            var firstTable = dataSet.Tables[0];
                            records.AddRange(ParseDataTable(firstTable, filePath));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing FM-002 file '{Path.GetFileName(filePath)}': {ex.Message}", ex);
            }

            return records;
        }

        private List<CalibrationRecord> ParseDataTable(DataTable table, string sourceFile)
        {
            var records = new List<CalibrationRecord>();
            
            if (table.Rows.Count == 0)
                return records;
            
            // Check if this is "On-site update template" worksheet with fixed structure
            var tableName = table.TableName?.Trim() ?? "";
            bool isOnsiteTemplate = tableName.Equals("On-site update template", StringComparison.OrdinalIgnoreCase) ||
                                   tableName.Contains("on-site", StringComparison.OrdinalIgnoreCase);
            
            int modelCol = -1;
            int serialCol = -1;
            int statusCol = -1;
            int calDateCol = -1;
            int dueDateCol = -1;
            int catIntCol = -1;
            int startRow = 1; // Default: start from row 1 (after header at row 0)
            
            if (isOnsiteTemplate)
            {
                // MULTI-SECTION DETECTION for "On-site update template":
                // The file has multiple data sections separated by page breaks
                // Each section has a header row with "Model" and "SN"
                // We'll find ALL header rows and process data after each one
                
                // This will be handled in the parsing loop below
                // For now, just set to process the entire table
                startRow = 0; // We'll check each row individually
                modelCol = -1; // Will be detected per section
                serialCol = -1;
                statusCol = -1;
                calDateCol = -1;
                catIntCol = -1;
                dueDateCol = -1;
                
                System.Diagnostics.Debug.WriteLine($"FM002Parser: Using MULTI-SECTION processing for 'On-site update template'");
                System.Diagnostics.Debug.WriteLine($"  Will detect headers and process data in multiple sections");
                System.Diagnostics.Debug.WriteLine($"  Cal Interval: Fixed 12 months");
            }
            else
            {
                // DYNAMIC DETECTION for other worksheets
                // Find column indices by examining the first row (header row)
                modelCol = FindColumnInFirstRow(table, 
                    "model number", "model no", "model", "equipment", "instrument", "device", "asset", "item", "description",
                    "model no.", "modelno", "modelnumber", "model#", "equipment name", "tool name");
                serialCol = FindColumnInFirstRow(table, 
                    "serial number", "serial no", "serial", "s/n", "sn", "serial no", "s.n", "asset tag",
                    "serial no.", "serialno", "serialnumber", "serial#", "s.no", "sno", "asset no", "asset number");
                statusCol = FindColumnInFirstRow(table,
                    "status", "result", "pass/fail", "pass", "condition");
                calDateCol = FindColumnInFirstRow(table, 
                    "date dd-mm-yy", "date (dd-mm-yy)", "date", "calibration date", "cal date", "calibrated", 
                    "cal. date", "last cal", "caldate", "cal-date", "date of calibration", "cal performed");
                dueDateCol = FindColumnInFirstRow(table,
                    "due date", "next cal", "next calibration", "expiry", "cal due", "due", "next due",
                    "calibration due", "next cal date", "expiry date", "cal expiry");
                catIntCol = FindColumnInFirstRow(table, 
                    "cat int month", "cat int (month)", "cat.int.month", "cat int", "catint", 
                    "cal int month", "cal int (month)", "cal int", "calint",
                    "interval", "cal interval", "months", "due in", "frequency", "period", "cycle");
            }

            // Debug: Show column detection results
            var headerInfo = new System.Text.StringBuilder();
            headerInfo.AppendLine($"FM-002 PARSER - WORKSHEET: {table.TableName}");
            headerInfo.AppendLine("═══════════════════════════════════════");
            headerInfo.AppendLine($"Total rows: {table.Rows.Count}");
            headerInfo.AppendLine("\nCOLUMN DETECTION RESULTS:");
            headerInfo.AppendLine("───────────────────────────────────────");
            headerInfo.AppendLine($"Model Column:     {(modelCol >= 0 ? $"✅ Column {modelCol}" : "❌ NOT FOUND")}");
            headerInfo.AppendLine($"Serial Column:    {(serialCol >= 0 ? $"✅ Column {serialCol}" : "❌ NOT FOUND")}");
            headerInfo.AppendLine($"Cal Date Column:  {(calDateCol >= 0 ? $"✅ Column {calDateCol}" : "❌ NOT FOUND")}");
            headerInfo.AppendLine($"Due Date Column:  {(dueDateCol >= 0 ? $"✅ Column {dueDateCol}" : "❌ NOT FOUND")}");
            headerInfo.AppendLine($"Cat Int Column:   {(catIntCol >= 0 ? $"✅ Column {catIntCol}" : "❌ NOT FOUND")}");
            headerInfo.AppendLine("\nCOLUMN HEADERS (Row 0):");
            headerInfo.AppendLine("───────────────────────────────────────");
            
            if (table.Rows.Count > 0)
            {
                var headerRow = table.Rows[0];
                for (int i = 0; i < Math.Min(20, table.Columns.Count); i++)
                {
                    var cellValue = GetStringValue(headerRow, i);
                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        var marker = "";
                        if (i == modelCol) marker = " ← MODEL";
                        else if (i == serialCol) marker = " ← SERIAL";
                        else if (i == calDateCol) marker = " ← CAL DATE";
                        else if (i == dueDateCol) marker = " ← DUE DATE";
                        else if (i == catIntCol) marker = " ← CAT INT";
                        
                        headerInfo.AppendLine($"[{i}] {cellValue}{marker}");
                    }
                }
                if (table.Columns.Count > 20)
                {
                    headerInfo.AppendLine($"... and {table.Columns.Count - 20} more columns");
                }
            }
            
            System.Diagnostics.Debug.WriteLine(headerInfo.ToString());

            // Parse each row
            int skippedRows = 0;
            int processedRows = 0;
            
            // For On-site template: track current section's column mapping
            int currentModelCol = modelCol;
            int currentSerialCol = serialCol;
            int currentStatusCol = statusCol;
            int currentCalDateCol = calDateCol;
            bool inDataSection = false;
            
            System.Diagnostics.Debug.WriteLine($"FM002Parser: Processing rows from {startRow + 1} to {table.Rows.Count}");
            
            for (int i = startRow; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                
                // For On-site template: Check if this row is a header row
                if (isOnsiteTemplate)
                {
                    bool isHeaderRow = false;
                    
                    // Check if this row contains "Model" and "SN" headers
                    for (int col = 0; col < Math.Min(10, table.Columns.Count); col++)
                    {
                        var cellValue = GetStringValue(row, col).ToLower().Trim();
                        if ((cellValue == "model" || cellValue == "model number") && col < table.Columns.Count)
                        {
                            // Check if "SN" is in the next few columns
                            for (int col2 = col; col2 < Math.Min(col + 5, table.Columns.Count); col2++)
                            {
                                var cellValue2 = GetStringValue(row, col2).ToLower().Trim();
                                if (cellValue2 == "sn" || cellValue2 == "serial number" || cellValue2 == "s/n")
                                {
                                    isHeaderRow = true;
                                    break;
                                }
                            }
                            if (isHeaderRow) break;
                        }
                    }
                    
                    if (isHeaderRow)
                    {
                        // Found a header row! Detect columns for this section
                        System.Diagnostics.Debug.WriteLine($"FM002Parser: Found header row at {i + 1}, skipping this row");
                        
                        currentModelCol = -1;
                        currentSerialCol = -1;
                        currentStatusCol = -1;
                        currentCalDateCol = -1;
                        
                        for (int col = 0; col < table.Columns.Count; col++)
                        {
                            var cellValue = GetStringValue(row, col).ToLower().Trim();
                            
                            if (cellValue == "model" || cellValue == "model number")
                                currentModelCol = col;
                            else if (cellValue == "sn" || cellValue == "serial number" || cellValue == "s/n" || cellValue == "serial no")
                                currentSerialCol = col;
                            else if (cellValue == "status" || cellValue == "result")
                                currentStatusCol = col;
                            else if (cellValue == "cal date" || cellValue == "date" || cellValue == "calibration date")
                                currentCalDateCol = col;
                        }
                        
                        System.Diagnostics.Debug.WriteLine($"  Model: Column {currentModelCol}, Serial: Column {currentSerialCol}, Status: Column {currentStatusCol}, Cal Date: Column {currentCalDateCol}");
                        
                        skippedRows++; // Count as skipped
                        inDataSection = true;
                        continue; // Skip the header row itself - don't export it
                    }
                    
                    // Check if this is a page break row (contains "Company Confidential", "Page", etc.)
                    bool isPageBreak = false;
                    for (int col = 0; col < Math.Min(10, table.Columns.Count); col++)
                    {
                        var cellValue = GetStringValue(row, col).ToLower().Trim();
                        
                        // Check for page break indicators
                        if (cellValue.Contains("company confidential") || 
                            cellValue.Contains("confidential") ||
                            (cellValue.Contains("page") && cellValue.Contains("of")))
                        {
                            isPageBreak = true;
                            break;
                        }
                    }
                    
                    if (isPageBreak)
                    {
                        System.Diagnostics.Debug.WriteLine($"FM002Parser: Page break detected at row {i + 1}, skipping");
                        skippedRows++;
                        inDataSection = false;
                        continue; // Skip this row completely
                    }
                    
                    // If not in a data section yet, skip this row
                    if (!inDataSection || currentModelCol < 0 || currentSerialCol < 0)
                    {
                        continue;
                    }
                    
                    // Use current section's column mapping
                    modelCol = currentModelCol;
                    serialCol = currentSerialCol;
                    statusCol = currentStatusCol;
                    calDateCol = currentCalDateCol;
                }
                
                // Skip empty rows
                if (IsEmptyRow(row))
                {
                    skippedRows++;
                    continue;
                }

                var model = GetStringValue(row, modelCol);
                var serialNumber = GetStringValue(row, serialCol);
                
                // Skip rows with "X" placeholder data (error markers)
                if (model == "X" || serialNumber == "X")
                {
                    skippedRows++;
                    System.Diagnostics.Debug.WriteLine($"Skipped row {i + 1}: Contains 'X' marker");
                    continue;
                }
                
                // Skip rows where BOTH model AND serial number are empty (completely blank rows)
                if (string.IsNullOrWhiteSpace(model) && string.IsNullOrWhiteSpace(serialNumber))
                {
                    skippedRows++;
                    System.Diagnostics.Debug.WriteLine($"Skipped row {i + 1}: Both Model and Serial empty");
                    continue;
                }
                
                // Skip rows that are actually headers (Model, SN, etc.) or page breaks
                var modelLower = model.ToLower().Trim();
                var serialLower = serialNumber.ToLower().Trim();
                if ((modelLower == "model" || modelLower == "model number") ||
                    (serialLower == "sn" || serialLower == "serial number" || serialLower == "s/n") ||
                    model.Contains("Company Confidential") || 
                    serialNumber.Contains("Company Confidential"))
                {
                    skippedRows++;
                    System.Diagnostics.Debug.WriteLine($"Skipped row {i + 1}: Header or page break text - Model='{model}', Serial='{serialNumber}'");
                    continue;
                }
                
                // Get calibration date
                var calibrationDate = GetDateValue(row, calDateCol);
                
                // Try to get due date directly first
                DateTime? dueDate = GetDateValue(row, dueDateCol);
                
                // If no direct due date, calculate from calibration date + interval
                if (!dueDate.HasValue && calibrationDate.HasValue)
                {
                    int catIntMonths;
                    
                    // For "On-site update template", use fixed 12 months
                    if (isOnsiteTemplate)
                    {
                        catIntMonths = 12; // Fixed interval
                    }
                    else
                    {
                        catIntMonths = GetIntValue(row, catIntCol);
                    }
                    
                    if (catIntMonths > 0)
                    {
                        dueDate = calibrationDate.Value.AddMonths(catIntMonths);
                        System.Diagnostics.Debug.WriteLine($"Row {i + 1}: Calculated DueDate={dueDate.Value:yyyy-MM-dd} from CalDate={calibrationDate.Value:yyyy-MM-dd} + {catIntMonths} months");
                    }
                }
                
                // Get status from file if available, otherwise determine automatically
                string status = "PASS";
                if (statusCol >= 0)
                {
                    // Read status from file
                    var fileStatus = GetStringValue(row, statusCol);
                    if (!string.IsNullOrWhiteSpace(fileStatus))
                    {
                        status = fileStatus.ToUpper();
                        System.Diagnostics.Debug.WriteLine($"Row {i + 1}: Status from file='{status}' - Model='{model}', Serial='{serialNumber}'");
                    }
                }
                else
                {
                    // Determine status automatically
                    // Status logic:
                    // 1. "X" marker → FAIL (already filtered above)
                    // 2. No due date → FAIL
                    // 3. Otherwise → PASS
                    if (!dueDate.HasValue)
                    {
                        status = "FAIL";
                        System.Diagnostics.Debug.WriteLine($"Row {i + 1}: Status=FAIL (No due date) - Model='{model}', Serial='{serialNumber}'");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Row {i + 1}: Status=PASS - Model='{model}', Serial='{serialNumber}', DueDate={dueDate.Value:yyyy-MM-dd}");
                    }
                }
                
                var record = new CalibrationRecord
                {
                    Model = model,
                    SerialNumber = serialNumber,
                    DueDate = dueDate,
                    CalibrationDate = calibrationDate,
                    Status = status,
                    // Clear vendor-specific fields for clean export
                    Vendor = string.Empty,
                    Location = string.Empty,
                    Technician = string.Empty,
                    Notes = string.Empty,
                    SourceFile = string.Empty,
                    SourceRow = 0
                };

                records.Add(record);
                processedRows++;
            }

            // Debug: Log summary
            System.Diagnostics.Debug.WriteLine($"FM002Parser Summary: Processed={processedRows}, Skipped={skippedRows}, Total={table.Rows.Count}");
            
            // Count records with and without due dates
            int recordsWithDueDate = records.Count(r => r.DueDate.HasValue);
            int recordsWithoutDueDate = records.Count(r => !r.DueDate.HasValue);
            
            // Show diagnostic message if no records extracted
            if (records.Count == 0 && (modelCol < 0 || serialCol < 0))
            {
                var summaryMessage = new System.Text.StringBuilder();
                summaryMessage.AppendLine("⚠️ FM-002 PARSER: NO RECORDS EXTRACTED");
                summaryMessage.AppendLine();
                summaryMessage.AppendLine($"Worksheet: {table.TableName}");
                summaryMessage.AppendLine($"Total rows: {table.Rows.Count}");
                summaryMessage.AppendLine($"Processed: {processedRows}");
                summaryMessage.AppendLine($"Skipped: {skippedRows}");
                summaryMessage.AppendLine();
                summaryMessage.AppendLine("Column Detection:");
                summaryMessage.AppendLine($"  Model: {(modelCol >= 0 ? "✅ Found" : "❌ NOT FOUND")}");
                summaryMessage.AppendLine($"  Serial: {(serialCol >= 0 ? "✅ Found" : "❌ NOT FOUND")}");
                summaryMessage.AppendLine($"  Cal Date: {(calDateCol >= 0 ? "✅ Found" : "❌ NOT FOUND")}");
                summaryMessage.AppendLine($"  Due Date: {(dueDateCol >= 0 ? "✅ Found" : "❌ NOT FOUND")}");
                summaryMessage.AppendLine($"  Cat Int: {(catIntCol >= 0 ? "✅ Found" : "❌ NOT FOUND")}");
                
                System.Windows.MessageBox.Show(summaryMessage.ToString(), "FM-002 Parser: No Records Found", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
            // Show success message
            else if (records.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"✅ FM-002 PARSER SUCCESS: {records.Count} records extracted, {recordsWithDueDate} with due dates, {recordsWithoutDueDate} without");
            }

            return records;
        }

        private int FindColumnInFirstRow(DataTable table, params string[] possibleNames)
        {
            // Find column by examining the values in the first row (header row)
            if (table.Rows.Count == 0)
                return -1;

            var headerRow = table.Rows[0];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                var cellValue = GetStringValue(headerRow, i).ToLower().Trim();
                
                // Remove newlines and extra spaces (handles multi-line headers)
                cellValue = System.Text.RegularExpressions.Regex.Replace(cellValue, @"[\r\n]+", " ").Trim();
                
                // Remove extra spaces, special chars for more flexible matching
                var normalizedCellValue = System.Text.RegularExpressions.Regex.Replace(cellValue, @"[\s\-_\.\(\)]+", " ").Trim();
                
                foreach (var name in possibleNames)
                {
                    var normalizedName = System.Text.RegularExpressions.Regex.Replace(name.ToLower(), @"[\s\-_\.\(\)]+", " ").Trim();
                    
                    // Check if normalized values match or contain each other
                    if (normalizedCellValue.Equals(normalizedName, StringComparison.OrdinalIgnoreCase) ||
                        normalizedCellValue.Contains(normalizedName) ||
                        cellValue.Contains(name.ToLower()))
                    {
                        System.Diagnostics.Debug.WriteLine($"FM002Parser: Found column '{cellValue}' at index {i} matching: {name}");
                        return i;
                    }
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"FM002Parser: Column not found for: {string.Join(", ", possibleNames)}");
            return -1; // Not found
        }

        private string GetStringValue(DataRow row, int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= row.Table.Columns.Count)
                return string.Empty;

            var value = row[columnIndex];
            if (value == null || value == DBNull.Value)
                return string.Empty;

            return value.ToString()?.Trim() ?? string.Empty;
        }

        private DateTime? GetDateValue(DataRow row, int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= row.Table.Columns.Count)
                return null;

            var value = row[columnIndex];
            if (value == null || value == DBNull.Value)
                return null;

            // Try direct DateTime conversion
            if (value is DateTime dateTime)
                return dateTime;

            // Try parsing string
            var stringValue = value.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            if (DateTime.TryParse(stringValue, out DateTime parsedDate))
                return parsedDate;

            return null;
        }

        private int GetIntValue(DataRow row, int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= row.Table.Columns.Count)
                return 0;

            var value = row[columnIndex];
            if (value == null || value == DBNull.Value)
                return 0;

            // Try direct int conversion
            if (value is int intValue)
                return intValue;

            // Try parsing string
            var stringValue = value.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(stringValue))
                return 0;

            if (int.TryParse(stringValue, out int parsedInt))
                return parsedInt;

            // Try parsing as double then convert to int
            if (double.TryParse(stringValue, out double parsedDouble))
                return (int)parsedDouble;

            return 0;
        }

        private bool IsEmptyRow(DataRow row)
        {
            return row.ItemArray.All(field => 
                field == null || 
                field == DBNull.Value || 
                string.IsNullOrWhiteSpace(field.ToString()));
        }
        
        private string GetExcelColumnName(int columnIndex)
        {
            // Convert 0-based index to Excel column name (A, B, C, ... Z, AA, AB, etc.)
            string columnName = "";
            while (columnIndex >= 0)
            {
                columnName = (char)('A' + (columnIndex % 26)) + columnName;
                columnIndex = (columnIndex / 26) - 1;
            }
            return columnName;
        }
    }
}

