@echo off
echo ============================================
echo Running DIRECTLY with dotnet (v2.0)
echo ============================================
echo.
echo This will run the app without using the .exe file
echo.
pause
echo.
cd /d "%~dp0CalibrationLogConverter"
echo Current directory:
cd
echo.
echo Starting application...
echo.
dotnet run --configuration Release --no-build --framework net8.0-windows
echo.
pause










