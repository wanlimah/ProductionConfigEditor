@echo off
REM ============================================================================
REM Calibration Log Converter - Launch Script
REM ============================================================================

echo.
echo ========================================
echo  Calibration Log Converter
echo  Starting Application...
echo ========================================
echo.

REM Check if Raw_Data folder exists
if not exist "C:\Users\wanlimah\Documents\Raw_Data\" (
    echo WARNING: Raw_Data folder not found!
    echo Creating: C:\Users\wanlimah\Documents\Raw_Data\
    mkdir "C:\Users\wanlimah\Documents\Raw_Data\"
    echo.
    echo Folder created. Place your calibration files there.
    echo.
)

REM Launch the application
echo Launching CalibrationLogConverter...
echo.

start "" "%~dp0CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe"

if %ERRORLEVEL% EQU 0 (
    echo Application launched successfully!
    echo.
    echo Instructions:
    echo 1. Place calibration files in: C:\Users\wanlimah\Documents\Raw_Data\
    echo 2. Click "Parse Files" in the application
    echo 3. Check record count - should show ALL records (not limited to 183)
    echo 4. Click "Export to Excel" to save
    echo.
    echo Data will export starting from Column B.
    echo FIXED: No longer limited to 183 records - all rows will be exported.
    echo.
) else (
    echo.
    echo ERROR: Failed to launch application!
    echo.
    echo Troubleshooting:
    echo - Make sure .NET 8.0 Desktop Runtime is installed
    echo - Try running as Administrator
    echo - Check TROUBLESHOOTING_ERROR_FIXED.md for help
    echo.
    pause
)

echo.
echo You can close this window.
timeout /t 5



