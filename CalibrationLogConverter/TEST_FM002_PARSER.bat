@echo off
echo ========================================
echo FM-002 Parser Test
echo ========================================
echo.
echo This script will test the FM-002 parser
echo with your actual FM-002 file.
echo.
echo Target file: 
echo C:\Users\wanlimah\Documents\Raw_Data\FM-002_Field Calibration Daily Report (Broadcom PG).xlsx
echo.

REM Check if the FM-002 file exists
if not exist "C:\Users\wanlimah\Documents\Raw_Data\FM-002_Field Calibration Daily Report (Broadcom PG).xlsx" (
    echo ERROR: FM-002 file not found!
    echo Please ensure the file exists in the Raw_Data folder.
    echo.
    pause
    exit /b 1
)

echo File found! ✓
echo.
echo Starting application...
echo.
echo NOTE: The application will automatically load the FM-002 file.
echo       Click "Parse Files" to test the parser.
echo.
echo Expected behavior:
echo  1. File should appear in "Input Files" list
echo  2. Click "Parse Files" button
echo  3. Should see: "Successfully parsed: 1 file(s)"
echo  4. Records should appear in preview grid
echo  5. Due dates should be populated
echo.
pause

REM Navigate to the executable directory
cd /d "%~dp0CalibrationLogConverter\bin\Release\net8.0-windows"

REM Check if executable exists
if not exist "CalibrationLogConverter.exe" (
    echo ERROR: Application executable not found!
    echo Please build the project first:
    echo    cd CalibrationLogConverter\CalibrationLogConverter
    echo    dotnet build --configuration Release
    echo.
    pause
    exit /b 1
)

REM Run the application
echo Running CalibrationLogConverter...
echo.
start "" "CalibrationLogConverter.exe"

echo.
echo Application launched!
echo.
echo TESTING CHECKLIST:
echo [ ] 1. FM-002 file appears in "Input Files" list
echo [ ] 2. Click "Parse Files" button
echo [ ] 3. Parsing completes successfully
echo [ ] 4. Preview grid shows records
echo [ ] 5. Record count matches expectations (~180-200)
echo [ ] 6. Due dates are present in preview
echo [ ] 7. Click "Export to Excel"
echo [ ] 8. Export completes successfully
echo [ ] 9. Open exported file
echo [ ] 10. Data looks correct
echo.
echo ========================================
echo Test script complete
echo ========================================
pause











