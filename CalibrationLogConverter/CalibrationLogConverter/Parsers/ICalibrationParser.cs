using System.Collections.Generic;
using CalibrationLogConverter.Models;

namespace CalibrationLogConverter.Parsers
{
    /// <summary>
    /// Interface for vendor-specific calibration log parsers
    /// </summary>
    public interface ICalibrationParser
    {
        /// <summary>
        /// Name of the vendor/parser
        /// </summary>
        string VendorName { get; }
        
        /// <summary>
        /// Parse calibration log file and extract records
        /// </summary>
        /// <param name="filePath">Path to the calibration log file</param>
        /// <returns>List of calibration records</returns>
        List<CalibrationRecord> ParseFile(string filePath);
        
        /// <summary>
        /// Check if this parser can handle the given file
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>True if parser can handle the file</returns>
        bool CanParse(string filePath);
    }
}
















