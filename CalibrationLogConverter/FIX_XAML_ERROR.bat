@echo off
REM ============================================================================
REM Calibration Log Converter - XAML Error Fix Script
REM Run this if you get XamlParseException errors
REM ============================================================================

echo.
echo ========================================
echo  XAML Error Fix Tool
echo  Calibration Log Converter
echo ========================================
echo.
echo This script will:
echo  1. Close all running instances
echo  2. Clean all build artifacts
echo  3. Rebuild the application
echo  4. Verify the fix
echo.
echo Press any key to start the fix...
pause >nul
echo.

REM Step 1: Kill all instances
echo [Step 1/4] Closing all running instances...
taskkill /F /IM CalibrationLogConverter.exe >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    echo    Successfully closed running instances.
    timeout /t 2 /nobreak >nul
) else (
    echo    No running instances found.
)
echo.

REM Step 2: Navigate to project directory
echo [Step 2/4] Navigating to project directory...
cd /d "%~dp0CalibrationLogConverter"
if %ERRORLEVEL% NEQ 0 (
    echo    ERROR: Could not find project directory!
    pause
    exit /b 1
)
echo    Current directory: %CD%
echo.

REM Step 3: Clean build artifacts
echo [Step 3/4] Cleaning build artifacts...
echo    Deleting bin folder...
if exist "bin" (
    rmdir /s /q "bin" 2>nul
    echo    - bin folder removed
) else (
    echo    - bin folder not found (already clean)
)

echo    Deleting obj folder...
if exist "obj" (
    rmdir /s /q "obj" 2>nul
    echo    - obj folder removed
) else (
    echo    - obj folder not found (already clean)
)

echo    Running dotnet clean...
dotnet clean >nul 2>&1
echo    - dotnet clean completed
echo.

REM Step 4: Rebuild
echo [Step 4/4] Rebuilding application...
echo    This may take a few seconds...
echo.
dotnet build --configuration Release

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo  SUCCESS! Application Fixed!
    echo ========================================
    echo.
    echo The XAML error has been fixed.
    echo.
    echo NEXT STEPS:
    echo 1. Use RUN_APP_SAFE.bat to launch the application
    echo 2. Or run directly:
    echo    CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe
    echo.
    echo TIPS TO AVOID THIS ERROR:
    echo - Always close the app before rebuilding
    echo - Use RUN_APP_SAFE.bat (it prevents multiple instances)
    echo - Don't double-click the EXE multiple times
    echo.
) else (
    echo.
    echo ========================================
    echo  BUILD FAILED!
    echo ========================================
    echo.
    echo The build encountered errors. Please check the messages above.
    echo.
    echo Common solutions:
    echo 1. Make sure .NET SDK is installed
    echo 2. Check that all source files are present
    echo 3. Restart your computer and try again
    echo.
)

echo.
echo Press any key to exit...
pause >nul













