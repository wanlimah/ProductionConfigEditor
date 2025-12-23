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
    /// Parser for Broadcom calibration log files
    /// </summary>
    public class BroadcomParser : ICalibrationParser
    {
        public string VendorName => "Broadcom";

        public bool CanParse(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                System.Diagnostics.Debug.WriteLine($"BroadcomParser.CanParse: File null or doesn't exist: {filePath}");
                return false;
            }

            // Only parse Excel files, not email files
            var extension = Path.GetExtension(filePath).ToLower();
            System.Diagnostics.Debug.WriteLine($"BroadcomParser.CanParse: File={Path.GetFileName(filePath)}, Extension={extension}");
            
            if (extension != ".xlsx" && extension != ".xls" && extension != ".xlsb")
            {
                System.Diagnostics.Debug.WriteLine($"BroadcomParser.CanParse: REJECTED - Not an Excel file");
                return false;
            }

            var fileName = Path.GetFileName(filePath).ToLower();
            bool canParse = fileName.Contains("broadcom") || 
                   fileName.Contains("fm-002") || 
                   fileName.Contains("logsheet");
            
            System.Diagnostics.Debug.WriteLine($"BroadcomParser.CanParse: Excel file, filename check result = {canParse}");
            return canParse;
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

                        // Only process the "Logsheet" worksheet
                        DataTable? logsheetTable = null;
                        
                        // Find "Logsheet" worksheet (case-insensitive, trim spaces, handle special chars)
                        foreach (DataTable table in dataSet.Tables)
                        {
                            var tableName = table.TableName?.Trim() ?? "";
                            System.Diagnostics.Debug.WriteLine($"Checking worksheet: '{tableName}' (Length: {tableName.Length})");
                            
                            // More flexible matching - trim and ignore case
                            if (tableName.Equals("Logsheet", StringComparison.OrdinalIgnoreCase) ||
                                tableName.StartsWith("Logsheet", StringComparison.OrdinalIgnoreCase))
                            {
                                System.Diagnostics.Debug.WriteLine($"MATCHED worksheet: '{tableName}'");
                                logsheetTable = table;
                                break;
                            }
                        }
                        
                        if (logsheetTable != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Found Logsheet worksheet with {logsheetTable.Rows.Count} rows");
                            records.AddRange(ParseDataTable(logsheetTable, filePath));
                        }
                        else
                        {
                            // Show warning if Logsheet not found
                            var worksheetList = new System.Text.StringBuilder();
                            worksheetList.AppendLine("WARNING: 'Logsheet' worksheet not found!");
                            worksheetList.AppendLine("\nAvailable worksheets:");
                            foreach (DataTable table in dataSet.Tables)
                            {
                                var name = table.TableName ?? "";
                                var bytes = System.Text.Encoding.UTF8.GetBytes(name);
                                worksheetList.AppendLine($"• {name} (Length: {name.Length}, Bytes: {bytes.Length})");
                            }
                            worksheetList.AppendLine("\nDEBUG INFO:");
                            worksheetList.AppendLine("The parser is looking for a worksheet named exactly 'Logsheet' (case-insensitive).");
                            worksheetList.AppendLine("If you see 'Logsheet' in the list above but it's not being matched,");
                            worksheetList.AppendLine("it may contain hidden spaces or special characters.");
                            
                            System.Windows.MessageBox.Show(worksheetList.ToString(), 
                                "Logsheet Not Found", 
                                System.Windows.MessageBoxButton.OK, 
                                System.Windows.MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error parsing Broadcom file '{Path.GetFileName(filePath)}': {ex.Message}", ex);
            }

            return records;
        }

        private List<CalibrationRecord> ParseDataTable(DataTable table, string sourceFile)
        {
            var records = new List<CalibrationRecord>();
            
            if (table.Rows.Count == 0)
                return records;
            
            // Find column indices by examining the first row (header row)
            // More flexible column detection with many variations
            int modelCol = FindColumnInFirstRow(table, 
                "model number", "model no", "model", "equipment", "instrument", "device", "asset", "item", "description",
                "model no.", "modelno", "modelnumber", "model#");
            int serialCol = FindColumnInFirstRow(table, 
                "serial number", "serial no", "serial", "s/n", "sn", "serial no", "s.n", "asset tag",
                "serial no.", "serialno", "serialnumber", "serial#", "s.no", "sno");
            
            // For Broadcom files, "Date dd-mm-yy" is the CALIBRATION DATE
            // We need to find the calibration interval (Cat Int month) to calculate due date
            int calDateCol = FindColumnInFirstRow(table, 
                "date dd-mm-yy", "date (dd-mm-yy)", "date", "calibration date", "cal date", "calibrated", 
                "cal. date", "last cal", "caldate", "cal-date", "date of calibration");
            int catIntCol = FindColumnInFirstRow(table, 
                "cat int month", "cat int (month)", "cat.int.month", "cat int", "catint", 
                "cal int month", "cal int (month)", "cal int", "calint",
                "interval", "cal interval", "months", "due in", "frequency", "period");

            // Debug: Show actual column headers found in row 0
            var headerInfo = new System.Text.StringBuilder();
            headerInfo.AppendLine($"WORKSHEET: {table.TableName}");
            headerInfo.AppendLine("═══════════════════════════════════════");
            headerInfo.AppendLine($"Total rows: {table.Rows.Count}");
            headerInfo.AppendLine("\nCOLUMN DETECTION RESULTS:");
            headerInfo.AppendLine("───────────────────────────────────────");
            headerInfo.AppendLine($"Model Column:     {(modelCol >= 0 ? $"✅ Column {modelCol}" : "❌ NOT FOUND")}");
            headerInfo.AppendLine($"Serial Column:    {(serialCol >= 0 ? $"✅ Column {serialCol}" : "❌ NOT FOUND")}");
            headerInfo.AppendLine($"Cal Date Column:  {(calDateCol >= 0 ? $"✅ Column {calDateCol}" : "❌ NOT FOUND")}");
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
                        else if (i == catIntCol) marker = " ← CAT INT";
                        
                        headerInfo.AppendLine($"[{i}] {cellValue}{marker}");
                    }
                }
                if (table.Columns.Count > 20)
                {
                    headerInfo.AppendLine($"... and {table.Columns.Count - 20} more columns");
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"ParseDataTable: Total rows = {table.Rows.Count}, ModelCol = {modelCol}, SerialCol = {serialCol}, CalDateCol = {calDateCol}, CatIntCol = {catIntCol}");
            System.Diagnostics.Debug.WriteLine(headerInfo.ToString());

            // Parse each row (starting from row 1, since row 0 is header)
            int skippedRows = 0;
            int processedRows = 0;
            
            for (int i = 1; i < table.Rows.Count; i++)  // Start from row 1 (skip header)
            {
                var row = table.Rows[i];
                
                // Skip empty rows
                if (IsEmptyRow(row))
                {
                    skippedRows++;
                    continue;
                }

                var model = GetStringValue(row, modelCol);
                var serialNumber = GetStringValue(row, serialCol);
                
                // Skip rows where BOTH model AND serial number are empty (completely blank rows)
                if (string.IsNullOrWhiteSpace(model) && string.IsNullOrWhiteSpace(serialNumber))
                {
                    skippedRows++;
                    System.Diagnostics.Debug.WriteLine($"Skipped row {i + 1}: Both Model and Serial empty");
                    continue;
                }
                
                // Get calibration date
                var calibrationDate = GetDateValue(row, calDateCol);
                
                // Calculate due date: Calibration Date + Cat Int (months)
                DateTime? dueDate = null;
                var catIntMonths = GetIntValue(row, catIntCol);
                
                if (calibrationDate.HasValue && catIntMonths > 0)
                {
                    dueDate = calibrationDate.Value.AddMonths(catIntMonths);
                    System.Diagnostics.Debug.WriteLine($"Row {i + 1}: CalDate={calibrationDate.Value:yyyy-MM-dd}, CatInt={catIntMonths} months, DueDate={dueDate.Value:yyyy-MM-dd}");
                }
                else
                {
                    // Debug: Show why due date calculation failed
                    if (!calibrationDate.HasValue)
                    {
                        System.Diagnostics.Debug.WriteLine($"Row {i + 1}: No due date - Calibration date is NULL (CalDateCol={calDateCol})");
                    }
                    else if (catIntMonths <= 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Row {i + 1}: No due date - Cat Int is {catIntMonths} (CatIntCol={catIntCol})");
                    }
                }
                
                // Determine calibration status (PASS or FAIL)
                // Status logic:
                // 1. "X" marker in Model or Serial Number → FAIL
                // 2. No due date (missing calibration date or interval) → FAIL
                // 3. Otherwise → PASS
                string status = "PASS";
                if (model == "X" || serialNumber == "X")
                {
                    status = "FAIL";
                    System.Diagnostics.Debug.WriteLine($"Row {i + 1}: Status=FAIL (X marker) - Model='{model}', Serial='{serialNumber}'");
                }
                else if (!dueDate.HasValue)
                {
                    status = "FAIL";
                    System.Diagnostics.Debug.WriteLine($"Row {i + 1}: Status=FAIL (No due date) - Model='{model}', Serial='{serialNumber}'");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Row {i + 1}: Status=PASS - Model='{model}', Serial='{serialNumber}'");
                }
                
                var record = new CalibrationRecord
                {
                    Model = model,
                    SerialNumber = serialNumber,
                    DueDate = dueDate,
                    // Keep calibration date for reference but don't export it
                    CalibrationDate = calibrationDate,
                    // Set status based on "X" marker detection
                    Status = status,
                    // Clear other fields as requested
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
            System.Diagnostics.Debug.WriteLine($"ParseDataTable Summary: Processed={processedRows}, Skipped={skippedRows}, Total={table.Rows.Count}");
            
            // Count records with and without due dates
            int recordsWithDueDate = records.Count(r => r.DueDate.HasValue);
            int recordsWithoutDueDate = records.Count(r => !r.DueDate.HasValue);
            
            // Show diagnostic message if columns not found OR if no due dates calculated
            if (records.Count == 0 && (modelCol < 0 || serialCol < 0))
            {
                var summaryMessage = new System.Text.StringBuilder();
                summaryMessage.AppendLine("⚠️ NO RECORDS EXTRACTED");
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
                summaryMessage.AppendLine($"  Cat Int: {(catIntCol >= 0 ? "✅ Found" : "❌ NOT FOUND")}");
                
                // Show actual headers if columns not found
                if (modelCol < 0 || serialCol < 0)
                {
                    summaryMessage.AppendLine();
                    summaryMessage.AppendLine("ACTUAL HEADERS (Row 0):");
                    if (table.Rows.Count > 0)
                    {
                        var headerRow = table.Rows[0];
                        for (int i = 0; i < Math.Min(10, table.Columns.Count); i++)
                        {
                            var cellValue = GetStringValue(headerRow, i);
                            if (!string.IsNullOrWhiteSpace(cellValue))
                            {
                                summaryMessage.AppendLine($"  [{i}] {cellValue}");
                            }
                        }
                    }
                }
                
                System.Windows.MessageBox.Show(summaryMessage.ToString(), "Debug: No Records Found", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
            // Show diagnostic message if records were extracted but NO due dates calculated
            else if (records.Count > 0 && recordsWithDueDate == 0)
            {
                var dueDateMessage = new System.Text.StringBuilder();
                dueDateMessage.AppendLine("⚠️ DUE DATE CALCULATION ISSUE");
                dueDateMessage.AppendLine();
                dueDateMessage.AppendLine($"✅ Records extracted: {records.Count}");
                dueDateMessage.AppendLine($"❌ Records with due dates: 0");
                dueDateMessage.AppendLine();
                dueDateMessage.AppendLine("COLUMN DETECTION:");
                dueDateMessage.AppendLine($"  Cal Date Column: {(calDateCol >= 0 ? $"✅ Column {calDateCol}" : "❌ NOT FOUND")}");
                dueDateMessage.AppendLine($"  Cat Int Column:  {(catIntCol >= 0 ? $"✅ Column {catIntCol}" : "❌ NOT FOUND")}");
                dueDateMessage.AppendLine();
                dueDateMessage.AppendLine("POSSIBLE CAUSES:");
                dueDateMessage.AppendLine("  • Calibration date column not found or empty");
                dueDateMessage.AppendLine("  • Cat Int (interval) column not found or empty");
                dueDateMessage.AppendLine("  • Date format not recognized");
                dueDateMessage.AppendLine();
                dueDateMessage.AppendLine("COLUMN HEADERS IN YOUR FILE:");
                if (table.Rows.Count > 0)
                {
                    var headerRow = table.Rows[0];
                    for (int i = 0; i < Math.Min(15, table.Columns.Count); i++)
                    {
                        var cellValue = GetStringValue(headerRow, i);
                        if (!string.IsNullOrWhiteSpace(cellValue))
                        {
                            dueDateMessage.AppendLine($"  [{i}] {cellValue}");
                        }
                    }
                }
                
                System.Windows.MessageBox.Show(dueDateMessage.ToString(), "Due Date Calculation Issue", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
            // Show success message with due date statistics
            else if (records.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"✅ SUCCESS: {records.Count} records extracted, {recordsWithDueDate} with due dates, {recordsWithoutDueDate} without");
            }

            return records;
        }

        private int FindColumn(DataTable table, params string[] possibleNames)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                var columnName = table.Columns[i].ColumnName.ToLower().Trim();
                if (possibleNames.Any(name => columnName.Contains(name.ToLower())))
                {
                    return i;
                }
            }
            return -1; // Not found
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
                
                // Remove newlines and extra spaces (handles multi-line headers like "Cal Int\nmonth")
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
                        System.Diagnostics.Debug.WriteLine($"Found column '{cellValue}' (normalized: '{normalizedCellValue}') at index {i} matching: {name}");
                        return i;
                    }
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"Column not found for: {string.Join(", ", possibleNames)}");
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
    }
}




