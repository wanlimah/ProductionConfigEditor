@echo off
echo ============================================
echo Running with dotnet run (v2.0)
echo ============================================
echo.
cd /d "%~dp0CalibrationLogConverter"
dotnet run --configuration Release
echo.
echo If app closed, press any key to exit...
pause










