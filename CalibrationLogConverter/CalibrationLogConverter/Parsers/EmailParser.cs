using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CalibrationLogConverter.Models;

namespace CalibrationLogConverter.Parsers
{
    /// <summary>
    /// Parser for email (.eml) files containing calibration data
    /// </summary>
    public class EmailParser : ICalibrationParser
    {
        public string VendorName => "Email (Broadcom)";

        public bool CanParse(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                System.Diagnostics.Debug.WriteLine($"EmailParser.CanParse: File null or doesn't exist: {filePath}");
                return false;
            }

            var extension = Path.GetExtension(filePath).ToLower();
            bool canParse = extension == ".eml";
            System.Diagnostics.Debug.WriteLine($"EmailParser.CanParse: File={Path.GetFileName(filePath)}, Extension={extension}, Result={canParse}");
            return canParse;
        }

        public List<CalibrationRecord> ParseFile(string filePath)
        {
            var records = new List<CalibrationRecord>();

            try
            {
                System.Diagnostics.Debug.WriteLine($"EmailParser: Starting to parse {Path.GetFileName(filePath)}");
                
                // Read the email file
                string emailContent = File.ReadAllText(filePath);
                
                // Split into lines
                var lines = emailContent.Split('\n');
                
                // Extract quoted lines (lines starting with '>')
                var dataLines = new List<string>();
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (trimmedLine.StartsWith(">"))
                    {
                        // Remove the leading '>' and trim
                        var cleanLine = trimmedLine.Substring(1).Trim();
                        if (!string.IsNullOrWhiteSpace(cleanLine))
                        {
                            dataLines.Add(cleanLine);
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine($"EmailParser: Extracted {dataLines.Count} quoted lines");

                if (dataLines.Count == 0)
                {
                    throw new Exception("No quoted data found in email file");
                }

                // Find where "Model" header starts (headers are on separate lines)
                int startIndex = -1;
                for (int i = 0; i < dataLines.Count; i++)
                {
                    if (dataLines[i].Trim() == "*Model*")
                    {
                        startIndex = i;
                        break;
                    }
                }

                if (startIndex == -1)
                {
                    throw new Exception("Header row (*Model*) not found in email data");
                }

                System.Diagnostics.Debug.WriteLine($"EmailParser: Found *Model* header at line {startIndex}");

                // The data structure is: each column header is on its own line, followed by data values on separate lines
                // We need to group every 8 lines (8 columns based on the structure)
                // *Model*, *Material No*, *Serial No*, *Job No*, *Status*, *Last Cal Date*, *Next Due Date*, *Location*
                
                // Skip to the first data line (after all headers)
                int dataStartIndex = startIndex;
                while (dataStartIndex < dataLines.Count && dataLines[dataStartIndex].Contains("*"))
                {
                    dataStartIndex++;
                }

                System.Diagnostics.Debug.WriteLine($"EmailParser: Data starts at line {dataStartIndex}");

                // Parse data in groups of 8 lines (8 columns per record)
                // Order: Model, Material No, Serial No, Job No, Status, Last Cal Date, Next Due Date, Location
                for (int i = dataStartIndex; i + 7 < dataLines.Count; i += 8)
                {
                    var model = dataLines[i].Trim();
                    var materialNo = dataLines[i + 1].Trim();
                    var serialNo = dataLines[i + 2].Trim();
                    var jobNo = dataLines[i + 3].Trim();
                    var status = dataLines[i + 4].Trim();
                    var lastCalDate = dataLines[i + 5].Trim();
                    var nextDueDate = dataLines[i + 6].Trim();
                    var location = dataLines[i + 7].Trim();

                    // Skip if this looks like signature/footer content or invalid data
                    if (model.Contains("*") || 
                        string.IsNullOrWhiteSpace(model) ||
                        model.StartsWith("Thanks", StringComparison.OrdinalIgnoreCase) ||
                        model.StartsWith("Best", StringComparison.OrdinalIgnoreCase) ||
                        model.StartsWith("Regards", StringComparison.OrdinalIgnoreCase) ||
                        model.Contains("@") || // Email addresses
                        model.StartsWith("http", StringComparison.OrdinalIgnoreCase) || // URLs
                        model.StartsWith("Internet:", StringComparison.OrdinalIgnoreCase) ||
                        model.StartsWith("Do you have", StringComparison.OrdinalIgnoreCase) ||
                        model.StartsWith("We will", StringComparison.OrdinalIgnoreCase) ||
                        model.StartsWith("Phone:", StringComparison.OrdinalIgnoreCase) ||
                        model.StartsWith("Fax:", StringComparison.OrdinalIgnoreCase) ||
                        serialNo.Contains("@") || // Check if serial number is actually an email
                        serialNo.StartsWith("N/A", StringComparison.OrdinalIgnoreCase) ||
                        !char.IsLetterOrDigit(model[0])) // Model should start with letter or digit
                    {
                        System.Diagnostics.Debug.WriteLine($"EmailParser: Skipping non-data line at index {i}: '{model}'");
                        break; // Reached end of valid equipment data
                    }

                    // Parse dates
                    DateTime? calDate = null;
                    DateTime? dueDate = null;
                    
                    if (DateTime.TryParse(lastCalDate, out DateTime parsedCalDate))
                    {
                        calDate = parsedCalDate;
                    }
                    
                    if (DateTime.TryParse(nextDueDate, out DateTime parsedDueDate))
                    {
                        dueDate = parsedDueDate;
                    }

                    var record = new CalibrationRecord
                    {
                        Model = model,
                        SerialNumber = serialNo,
                        Status = status,
                        CalibrationDate = calDate,
                        DueDate = dueDate,
                        Location = location,
                        Notes = $"Material No: {materialNo}, Job No: {jobNo}"
                    };

                    records.Add(record);
                    System.Diagnostics.Debug.WriteLine($"EmailParser: Parsed record #{records.Count} - Model: {model}, SN: {serialNo}, Status: {status}");
                }

                System.Diagnostics.Debug.WriteLine($"EmailParser: Successfully parsed {records.Count} records");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EmailParser ERROR: {ex.Message}");
                throw new Exception($"Error parsing email file '{Path.GetFileName(filePath)}': {ex.Message}", ex);
            }

            return records;
        }

        private List<string> ParseTableLine(string line)
        {
            // Split by * and | characters, which are common table delimiters
            var cells = new List<string>();
            var parts = line.Split(new[] { '*', '|' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var part in parts)
            {
                var trimmed = part.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    cells.Add(trimmed);
                }
            }
            
            return cells;
        }

        private int FindColumnIndex(List<string> headers, string columnName)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                if (headers[i].Contains(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        private string GetCellValue(List<string> cells, int index)
        {
            if (index < 0 || index >= cells.Count)
            {
                return string.Empty;
            }
            return cells[index].Trim();
        }
    }
}
