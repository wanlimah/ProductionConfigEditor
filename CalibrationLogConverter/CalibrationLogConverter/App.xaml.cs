using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CalibrationLogConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Register encoding provider for ExcelDataReader (MUST be done before any Excel operations)
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            
            // Set EPPlus license for version 8+ (NonCommercial = Free for internal use)
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            
            // Add global exception handler
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            try
            {
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                ShowError("Application Startup Error", ex);
                Shutdown(1);
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                ShowError("Unhandled Exception", ex);
            }
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ShowError("Application Error", e.Exception);
            e.Handled = true;
        }

        private void ShowError(string title, Exception ex)
        {
            var message = $"{title}\n\n" +
                         $"Error: {ex.Message}\n\n" +
                         $"Type: {ex.GetType().Name}\n\n";
            
            // Show inner exception if it exists (this is usually the REAL error)
            if (ex.InnerException != null)
            {
                message += $"\n=== INNER EXCEPTION (Real Cause) ===\n" +
                          $"Error: {ex.InnerException.Message}\n" +
                          $"Type: {ex.InnerException.GetType().Name}\n\n";
                
                if (ex.InnerException.InnerException != null)
                {
                    message += $"\n=== DEEPER INNER EXCEPTION ===\n" +
                              $"Error: {ex.InnerException.InnerException.Message}\n" +
                              $"Type: {ex.InnerException.InnerException.GetType().Name}\n\n";
                }
            }
            
            message += $"Stack Trace:\n{ex.StackTrace}";

            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

