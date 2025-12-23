@echo off
echo ============================================
echo Running .NET 6.0 version (v2.0)
echo ============================================
echo.
cd /d "%~dp0"
start "" "CalibrationLogConverter\bin\Release\net6.0-windows\CalibrationLogConverter.exe"
echo.
echo App should start now...
echo Check window title for "v2.0 (Email Support)"
echo.
pause










