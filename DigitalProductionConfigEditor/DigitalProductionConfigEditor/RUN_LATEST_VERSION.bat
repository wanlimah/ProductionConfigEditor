@echo off
echo ================================================
echo Running Digital Production Config Editor
echo Latest Version with Enable=FALSE default
echo ================================================
echo.
echo Build location: ..\bin\Debug\net6.0-windows
echo.
echo Navigating to build folder...
cd /d "%~dp0..\bin\Debug\net6.0-windows"
echo.
pause
start "" "%CD%\DigitalProductionConfigEditor.exe"


