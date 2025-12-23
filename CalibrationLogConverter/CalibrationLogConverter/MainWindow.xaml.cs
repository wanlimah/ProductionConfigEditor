using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using CalibrationLogConverter.Models;
using CalibrationLogConverter.Parsers;
using CalibrationLogConverter.Services;

namespace CalibrationLogConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CalibrationRecord> _allRecords = new List<CalibrationRecord>();
        private List<string> _selectedFiles = new List<string>();
        private List<ICalibrationParser> _parsers = new List<ICalibrationParser>();
        private ExcelExportService _exportService = new ExcelExportService();

        public MainWindow()
        {
            InitializeComponent();
            InitializeParsers();
            
            // Set window title with version to confirm correct build is running
            this.Title = "EMAIL ONLY VERSION - v6.0 - TEST BUILD";
            
            // Set default Raw_Data path if it exists
            string defaultPath = @"C:\Users\wanlimah\Documents\Raw_Data";
            if (Directory.Exists(defaultPath))
            {
                LoadFilesFromDirectory(defaultPath);
            }
            
            // Show version in status
            UpdateStatus("Ready - Email Parser Support Added", true);
        }

        private void InitializeParsers()
        {
            // Register all parsers
            // Note: Order matters! More specific parsers should be registered first
            _parsers.Add(new EmailParser());      // Email (.eml) files
            _parsers.Add(new FM002Parser());      // Specific for FM-002 files
            _parsers.Add(new BroadcomParser());   // General Broadcom files
            // Add more parsers here as needed for other vendors
            
            // Debug: Show parser registration
            System.Diagnostics.Debug.WriteLine($"=== PARSERS REGISTERED ===");
            foreach (var p in _parsers)
            {
                System.Diagnostics.Debug.WriteLine($"  - {p.VendorName}");
            }
        }

        private void LoadFilesFromDirectory(string directory)
        {
            try
            {
                var calibrationFiles = Directory.GetFiles(directory, "*.*")
                    .Where(f => f.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                               f.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) ||
                               f.EndsWith(".xlsb", StringComparison.OrdinalIgnoreCase) ||
                               f.EndsWith(".eml", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (calibrationFiles.Any())
                {
                    _selectedFiles = calibrationFiles;
                    UpdateFileDisplay();
                    UpdateStatus($"Found {calibrationFiles.Count} file(s) in Raw_Data folder", true);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading files: {ex.Message}", false);
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Calibration Log Files",
                Filter = "All Supported Files (*.xlsx;*.xls;*.xlsb;*.eml)|*.xlsx;*.xls;*.xlsb;*.eml|Excel Files (*.xlsx;*.xls;*.xlsb)|*.xlsx;*.xls;*.xlsb|Email Files (*.eml)|*.eml|All Files (*.*)|*.*",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                _selectedFiles = dialog.FileNames.ToList();
                UpdateFileDisplay();
                UpdateStatus($"Selected {_selectedFiles.Count} file(s)", true);
            }
        }

        private void UpdateFileDisplay()
        {
            InputFilesTextBox.Text = string.Join("\n", _selectedFiles.Select(f => Path.GetFileName(f)));
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_selectedFiles.Any())
            {
                MessageBox.Show("Please select at least one file to parse.", "No Files Selected", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _allRecords.Clear();
                UpdateStatus("Parsing files...", true);
                ParseButton.IsEnabled = false;

                int successCount = 0;
                int failCount = 0;
                List<string> errors = new List<string>();

                foreach (var file in _selectedFiles)
                {
                    ICalibrationParser parser = null;
                    var fileName = Path.GetFileName(file);
                    
                    try
                    {
                        // Find appropriate parser
                        var extension = Path.GetExtension(file).ToLower();
                        
                        Debug.WriteLine($"Checking file: {fileName} (extension: {extension})");
                        
                        // Check each parser
                        foreach (var p in _parsers)
                        {
                            var canParse = p.CanParse(file);
                            Debug.WriteLine($"  {p.VendorName}: CanParse = {canParse}");
                        }
                        
                        parser = _parsers.FirstOrDefault(p => p.CanParse(file));
                        
                        if (parser != null)
                        {
                            Debug.WriteLine($"Selected parser: {parser.VendorName}");
                            // DebugHelper.Log($"Selected parser for {fileName}: {parser.VendorName}");
                            
                            var records = parser.ParseFile(file);
                            _allRecords.AddRange(records);
                            successCount++;
                            UpdateStatus($"Parsed {fileName} with {parser.VendorName} parser - {records.Count} records found", true);
                        }
                        else
                        {
                            errors.Add($"No suitable parser found for: {fileName}");
                            failCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        var parserName = parser != null ? parser.VendorName : "Unknown";
                        errors.Add($"{fileName} (Parser: {parserName}): {ex.Message}");
                        failCount++;
                        Debug.WriteLine($"Error parsing {fileName} with {parserName}: {ex.Message}");
                        Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    }
                }

                // Update preview
                PreviewDataGrid.ItemsSource = _allRecords;
                RecordCountTextBlock.Text = $"{_allRecords.Count} records";
                ExportButton.IsEnabled = _allRecords.Any();

                // Show detailed summary with file information
                string message = $"Parsing Complete!\n\n" +
                               $"✅ Successfully parsed: {successCount} file(s)\n" +
                               $"📊 Total records extracted: {_allRecords.Count}\n\n";
                
                // Show details for each file
                if (successCount > 0)
                {
                    message += "File Details:\n";
                    message += "─────────────────────────────────────────────\n";
                    foreach (var file in _selectedFiles)
                    {
                        var fileName = Path.GetFileName(file);
                        var fileSize = new FileInfo(file).Length;
                        var recordsFromFile = _allRecords.Count(r => true); // All records for now
                        message += $"📄 {fileName}\n";
                        message += $"   Size: {fileSize:N0} bytes\n";
                    }
                    message += "─────────────────────────────────────────────\n\n";
                }
                
                if (failCount > 0)
                {
                    message += $"\n❌ Failed to parse: {failCount} file(s)\n\nErrors:\n" + 
                              string.Join("\n", errors);
                }
                
                message += $"\n💡 TIP: If you expected more records, check:\n";
                message += $"   • Does your Excel file have a 'Logsheet' worksheet?\n";
                message += $"   • Open the file and press Ctrl+End to see last row\n";
                message += $"   • Check for empty rows or rows with 'X' markers\n";

                UpdateStatus($"Parsed {successCount} file(s), found {_allRecords.Count} records", true);
                MessageBox.Show(message, "Parse Complete", MessageBoxButton.OK, 
                    failCount > 0 ? MessageBoxImage.Warning : MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", false);
                MessageBox.Show($"An error occurred while parsing:\n\n{ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ParseButton.IsEnabled = true;
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_allRecords.Any())
            {
                MessageBox.Show("No records to export. Please parse files first.", "No Data", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Title = "Save Calibration Report",
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"Calibration_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                DefaultExt = "xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    UpdateStatus("Exporting to Excel...", true);
                    ExportButton.IsEnabled = false;

                    bool includeExtended = IncludeExtendedFieldsCheckBox.IsChecked == true;
                    _exportService.ExportToExcel(_allRecords, dialog.FileName, includeExtended);

                    UpdateStatus($"Exported {_allRecords.Count} records successfully", true);
                    
                    var result = MessageBox.Show(
                        $"✅ Export Successful!\n\n" +
                        $"File saved to:\n{dialog.FileName}\n\n" +
                        $"Total records: {_allRecords.Count}\n\n" +
                        $"Would you like to open the file now?",
                        "Export Complete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);

                    if (result == MessageBoxResult.Yes || AutoOpenCheckBox.IsChecked == true)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = dialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Export failed: {ex.Message}", false);
                    MessageBox.Show($"Failed to export:\n\n{ex.Message}", "Export Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    ExportButton.IsEnabled = true;
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Clear all data and start over?", "Confirm Clear", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _allRecords.Clear();
                _selectedFiles.Clear();
                InputFilesTextBox.Clear();
                PreviewDataGrid.ItemsSource = null;
                RecordCountTextBlock.Text = "0 records";
                ExportButton.IsEnabled = false;
                UpdateStatus("Ready", true);
            }
        }

        private void UpdateStatus(string message, bool isSuccess)
        {
            StatusTextBlock.Text = message;
            StatusTextBlock.Foreground = isSuccess ? 
                System.Windows.Media.Brushes.Green : 
                System.Windows.Media.Brushes.Red;
        }
    }
}