@echo off
REM ============================================================================
REM Calibration Log Converter - SAFE Launch Script
REM This version ensures only ONE instance runs at a time
REM ============================================================================

echo.
echo ========================================
echo  Calibration Log Converter
echo  Safe Launch Mode
echo ========================================
echo.

REM Step 1: Kill any existing instances
echo [Step 1/4] Checking for running instances...
taskkill /F /IM CalibrationLogConverter.exe >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    echo    Found and closed existing instances.
    timeout /t 2 /nobreak >nul
) else (
    echo    No existing instances found. Good!
)
echo.

REM Step 2: Check if Raw_Data folder exists
echo [Step 2/4] Checking Raw_Data folder...
if not exist "C:\Users\wanlimah\Documents\Raw_Data\" (
    echo    WARNING: Raw_Data folder not found!
    echo    Creating: C:\Users\wanlimah\Documents\Raw_Data\
    mkdir "C:\Users\wanlimah\Documents\Raw_Data\"
    echo    Folder created successfully.
) else (
    echo    Raw_Data folder exists. Good!
)
echo.

REM Step 3: Verify application exists
echo [Step 3/4] Verifying application file...
set "APP_PATH=%~dp0CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe"
if not exist "%APP_PATH%" (
    echo    ERROR: Application not found!
    echo    Expected location: %APP_PATH%
    echo.
    echo    Please rebuild the application:
    echo    1. Open PowerShell
    echo    2. cd %~dp0CalibrationLogConverter
    echo    3. dotnet build --configuration Release
    echo.
    pause
    exit /b 1
)
echo    Application found. Good!
echo.

REM Step 4: Launch the application
echo [Step 4/4] Launching CalibrationLogConverter...
echo.
start "" "%APP_PATH%"

if %ERRORLEVEL% EQU 0 (
    echo ========================================
    echo  Application Launched Successfully!
    echo ========================================
    echo.
    echo INSTRUCTIONS:
    echo 1. Place calibration files in: C:\Users\wanlimah\Documents\Raw_Data\
    echo 2. Click "Parse Files" button
    echo 3. Review the extracted data
    echo 4. Click "Export to Excel" button
    echo 5. Data will export starting from Column B
    echo.
    echo IMPORTANT: 
    echo - Only run ONE instance at a time
    echo - Close the app before running this script again
    echo - If you get XAML errors, run FIX_XAML_ERROR.bat
    echo.
) else (
    echo.
    echo ========================================
    echo  ERROR: Failed to Launch!
    echo ========================================
    echo.
    echo Troubleshooting:
    echo 1. Make sure .NET 8.0 Desktop Runtime is installed
    echo 2. Try running as Administrator
    echo 3. Run FIX_XAML_ERROR.bat to clean and rebuild
    echo.
    pause
    exit /b 1
)

echo You can close this window now.
echo.
timeout /t 3













