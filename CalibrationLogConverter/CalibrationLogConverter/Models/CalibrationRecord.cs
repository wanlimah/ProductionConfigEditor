using System;

namespace CalibrationLogConverter.Models
{
    /// <summary>
    /// Represents a single calibration record with standardized fields
    /// </summary>
    public class CalibrationRecord
    {
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        
        // Additional fields for reference
        public string Vendor { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime? CalibrationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Technician { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        
        // Source tracking
        public string SourceFile { get; set; } = string.Empty;
        public int SourceRow { get; set; }
        
        public override string ToString()
        {
            return $"{Model} - {SerialNumber} - Due: {DueDate?.ToString("yyyy-MM-dd") ?? "N/A"}";
        }
    }
}
















