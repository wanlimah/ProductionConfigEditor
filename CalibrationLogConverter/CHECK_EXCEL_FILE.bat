@echo off
REM ============================================================================
REM Excel File Row Counter
REM This script opens your source Excel file so you can manually check rows
REM ============================================================================

echo.
echo ========================================
echo  Excel File Checker
echo ========================================
echo.

set SOURCE_FILE=C:\Users\wanlimah\Documents\Raw_Data\FM-002_Field Calibration Daily Report (Broadcom PG).xlsx

if exist "%SOURCE_FILE%" (
    echo Found source file:
    echo %SOURCE_FILE%
    echo.
    echo File Details:
    dir "%SOURCE_FILE%" | find "FM-002"
    echo.
    echo Opening file in Excel...
    echo.
    echo INSTRUCTIONS:
    echo -------------
    echo 1. Look for the "Logsheet" worksheet tab
    echo 2. Press Ctrl+End to go to the last cell with data
    echo 3. Note the row number (e.g., Row 184, Row 500, etc.)
    echo 4. Subtract 1 for the header row to get total data rows
    echo.
    echo Example:
    echo - If last row is 184, you have 183 data rows
    echo - If last row is 500, you have 499 data rows
    echo.
    pause
    
    REM Open the file in Excel
    start "" "%SOURCE_FILE%"
    
) else (
    echo ERROR: Source file not found!
    echo.
    echo Expected location:
    echo %SOURCE_FILE%
    echo.
    echo Please check if the file exists in:
    echo C:\Users\wanlimah\Documents\Raw_Data\
    echo.
    pause
)

echo.
echo After checking the file, compare with application results.
echo.
pause












