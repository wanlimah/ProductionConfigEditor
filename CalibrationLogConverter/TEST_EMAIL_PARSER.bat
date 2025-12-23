@echo off
echo ========================================
echo Testing Email Parser (.eml files)
echo ========================================
echo.
echo This will test the EmailParser with the Broadcom Calibration Campaign 2025.eml file
echo.
echo File location: C:\Users\wanlimah\Documents\Raw_Data\Broadcom Calibration Campaign 2025.eml
echo.
echo Steps:
echo 1. The app will auto-load files from Raw_Data folder (including .eml files)
echo 2. Click "Parse Files" button
echo 3. Verify records are extracted from the email
echo 4. Export to Excel if needed
echo.
pause
echo.
echo Starting the application...
echo.
cd CalibrationLogConverter\bin\Release\net8.0-windows
start CalibrationLogConverter.exe
echo.
echo Application started!
echo Check the Debug output in Visual Studio (if running) for detailed parsing logs
echo.
pause










