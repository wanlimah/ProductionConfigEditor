@echo off
cls
echo ╔═══════════════════════════════════════════════════════════════╗
echo ║                                                               ║
echo ║      📊 CALIBRATION LOG CONVERTER                            ║
echo ║                                                               ║
echo ╚═══════════════════════════════════════════════════════════════╝
echo.
echo Starting application...
echo.

cd /d "%~dp0CalibrationLogConverter"
dotnet run --framework net8.0-windows

if errorlevel 1 (
    echo.
    echo ═══════════════════════════════════════════════════════════════
    echo   ⚠️ ERROR: Application failed to start!
    echo ═══════════════════════════════════════════════════════════════
    echo.
    echo Possible solutions:
    echo  1. Try .NET 6.0 version: dotnet run --framework net6.0-windows
    echo  2. Install .NET 8.0 Desktop Runtime
    echo  3. Check error message above
    echo.
    pause
)















