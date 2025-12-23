using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using CalibrationLogConverter.Models;

namespace CalibrationLogConverter.Services
{
    /// <summary>
    /// Service for exporting calibration records to Excel
    /// </summary>
    public class ExcelExportService
    {
        static ExcelExportService()
        {
            // EPPlus 8+ license is configured via epplus.json file
            // The file sets LicenseContext to "NonCommercial" (free for internal use)
        }

        public ExcelExportService()
        {
        }

        /// <summary>
        /// Export calibration records to Excel file
        /// </summary>
        public void ExportToExcel(List<CalibrationRecord> records, string outputPath, bool includeExtendedFields = false)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Calibration Records");

                // Add headers starting from column B (column 2)
                int col = 2;
                worksheet.Cells[1, col++].Value = "Model";
                worksheet.Cells[1, col++].Value = "Serial Number";
                worksheet.Cells[1, col++].Value = "Due Date";
                worksheet.Cells[1, col++].Value = "Status";  // Always include Status column (Column E)

                if (includeExtendedFields)
                {
                    worksheet.Cells[1, col++].Value = "Calibration Date";
                    worksheet.Cells[1, col++].Value = "Location";
                    worksheet.Cells[1, col++].Value = "Technician";
                    worksheet.Cells[1, col++].Value = "Vendor";
                    worksheet.Cells[1, col++].Value = "Notes";
                    worksheet.Cells[1, col++].Value = "Source File";
                    worksheet.Cells[1, col++].Value = "Source Row";
                }

                // Style headers (starting from column B)
                using (var range = worksheet.Cells[1, 2, 1, col - 1])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129, 189));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Add data starting from column B (column 2)
                int row = 2;
                foreach (var record in records)
                {
                    col = 2;
                    worksheet.Cells[row, col++].Value = record.Model;
                    worksheet.Cells[row, col++].Value = record.SerialNumber;
                    
                    // Due Date column (Column D) - show "N/A" if blank
                    if (record.DueDate.HasValue)
                    {
                        worksheet.Cells[row, col].Value = record.DueDate.Value;
                        worksheet.Cells[row, col].Style.Numberformat.Format = "yyyy-MM-dd";
                    }
                    else
                    {
                        worksheet.Cells[row, col].Value = "N/A";
                    }
                    col++;

                    // Status column (Column E) - always included
                    worksheet.Cells[row, col].Value = record.Status;
                    // Highlight FAIL/FAILED status in red
                    var statusUpper = record.Status?.ToUpper() ?? "";
                    if (statusUpper == "FAIL" || statusUpper == "FAILED" || statusUpper.Contains("FAIL"))
                    {
                        worksheet.Cells[row, col].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        worksheet.Cells[row, col].Style.Font.Bold = true;
                    }
                    col++;

                    if (includeExtendedFields)
                    {
                        if (record.CalibrationDate.HasValue)
                        {
                            worksheet.Cells[row, col].Value = record.CalibrationDate.Value;
                            worksheet.Cells[row, col].Style.Numberformat.Format = "yyyy-MM-dd";
                        }
                        col++;

                        worksheet.Cells[row, col++].Value = record.Location;
                        worksheet.Cells[row, col++].Value = record.Technician;
                        worksheet.Cells[row, col++].Value = record.Vendor;
                        worksheet.Cells[row, col++].Value = record.Notes;
                        worksheet.Cells[row, col++].Value = record.SourceFile;
                        worksheet.Cells[row, col++].Value = record.SourceRow;
                    }

                    row++;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Apply borders (starting from column B)
                using (var range = worksheet.Cells[1, 2, row - 1, col - 1])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                // Freeze header row at row 2, column B
                worksheet.View.FreezePanes(2, 2);

                // Add summary sheet
                AddSummarySheet(package, records);

                // Save file
                var fileInfo = new FileInfo(outputPath);
                package.SaveAs(fileInfo);
            }
        }

        private void AddSummarySheet(ExcelPackage package, List<CalibrationRecord> records)
        {
            var worksheet = package.Workbook.Worksheets.Add("Summary");

            worksheet.Cells[1, 1].Value = "Calibration Export Summary";
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Bold = true;

            int row = 3;
            worksheet.Cells[row++, 1].Value = "Total Records:";
            worksheet.Cells[row - 1, 2].Value = records.Count;

            worksheet.Cells[row++, 1].Value = "Export Date:";
            worksheet.Cells[row - 1, 2].Value = DateTime.Now;
            worksheet.Cells[row - 1, 2].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

            row++;
            worksheet.Cells[row++, 1].Value = "Records by Vendor:";
            var vendorGroups = records.GroupBy(r => r.Vendor).OrderBy(g => g.Key);
            foreach (var group in vendorGroups)
            {
                worksheet.Cells[row, 1].Value = group.Key;
                worksheet.Cells[row, 2].Value = group.Count();
                row++;
            }

            row++;
            worksheet.Cells[row++, 1].Value = "Records by Source File:";
            var fileGroups = records.GroupBy(r => r.SourceFile).OrderBy(g => g.Key);
            foreach (var group in fileGroups)
            {
                worksheet.Cells[row, 1].Value = group.Key;
                worksheet.Cells[row, 2].Value = group.Count();
                row++;
            }

            row++;
            worksheet.Cells[row++, 1].Value = "Records with Due Dates:";
            worksheet.Cells[row - 1, 2].Value = records.Count(r => r.DueDate.HasValue);

            worksheet.Cells[row++, 1].Value = "Records without Due Dates:";
            worksheet.Cells[row - 1, 2].Value = records.Count(r => !r.DueDate.HasValue);

            // Calibration Status Statistics
            row++;
            worksheet.Cells[row++, 1].Value = "Calibration Status:";
            
            var passCount = records.Count(r => r.Status?.ToUpper().Contains("PASS") == true);
            worksheet.Cells[row, 1].Value = "PASS/PASSED:";
            worksheet.Cells[row, 2].Value = passCount;
            worksheet.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.Green);
            worksheet.Cells[row, 2].Style.Font.Bold = true;
            row++;
            
            var failCount = records.Count(r => r.Status?.ToUpper().Contains("FAIL") == true);
            worksheet.Cells[row, 1].Value = "FAIL/FAILED:";
            worksheet.Cells[row, 2].Value = failCount;
            if (failCount > 0)
            {
                worksheet.Cells[row, 2].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                worksheet.Cells[row, 2].Style.Font.Bold = true;
            }
            row++;

            // Upcoming calibrations (within 30 days)
            row++;
            var upcomingCount = records.Count(r => r.DueDate.HasValue && 
                r.DueDate.Value <= DateTime.Now.AddDays(30) && 
                r.DueDate.Value >= DateTime.Now);
            
            worksheet.Cells[row++, 1].Value = "Due within 30 days:";
            worksheet.Cells[row - 1, 2].Value = upcomingCount;
            if (upcomingCount > 0)
            {
                worksheet.Cells[row - 1, 2].Style.Font.Bold = true;
                worksheet.Cells[row - 1, 2].Style.Font.Color.SetColor(System.Drawing.Color.Red);
            }

            // Overdue calibrations
            var overdueCount = records.Count(r => r.DueDate.HasValue && r.DueDate.Value < DateTime.Now);
            worksheet.Cells[row++, 1].Value = "Overdue:";
            worksheet.Cells[row - 1, 2].Value = overdueCount;
            if (overdueCount > 0)
            {
                worksheet.Cells[row - 1, 2].Style.Font.Bold = true;
                worksheet.Cells[row - 1, 2].Style.Font.Color.SetColor(System.Drawing.Color.DarkRed);
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }
    }
}

