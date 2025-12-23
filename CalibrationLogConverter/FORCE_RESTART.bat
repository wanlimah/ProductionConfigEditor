@echo off
echo ============================================
echo FORCE RESTART - Calibration Log Converter
echo ============================================
echo.
echo This will:
echo 1. Kill any running instances
echo 2. Wait 2 seconds
echo 3. Start the NEW version (v2.0)
echo.
pause
echo.

REM Kill any running instances
echo Stopping any running instances...
taskkill /F /IM CalibrationLogConverter.exe 2>nul
if %errorlevel% == 0 (
    echo   Old instance stopped.
) else (
    echo   No running instances found.
)

echo.
echo Waiting 2 seconds...
timeout /t 2 /nobreak >nul

echo.
echo Starting NEW version (v2.0)...
echo.

REM Change to the directory where this batch file is located
cd /d "%~dp0"

REM Start the application
start "" "CalibrationLogConverter\bin\Release\net8.0-windows\CalibrationLogConverter.exe"

echo.
echo ============================================
echo Application started!
echo.
echo CHECK: Window title should say "v2.0 (Email Support)"
echo.
echo If it doesn't say "v2.0", something is wrong!
echo ============================================
echo.
pause










