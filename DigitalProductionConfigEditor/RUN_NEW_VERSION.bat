@echo off
echo ============================================
echo Closing all instances...
echo ============================================
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul

timeout /t 2 /nobreak >nul

echo.
echo ============================================
echo Running NEWLY COMPILED version from:
echo %~dp0bin\Debug\net6.0-windows\
echo ============================================
echo.

cd /d "%~dp0bin\Debug\net6.0-windows\"

echo File info:
dir DigitalProductionConfigEditor.exe | find "DigitalProductionConfigEditor.exe"
echo.

echo Starting application...
echo.
echo **IMPORTANT**: When you click "+ Add Single Product",
echo you MUST see a DEBUG popup message!
echo.
echo If you see the popup = New code is running
echo If NO popup = Still running old code
echo.

start "" DigitalProductionConfigEditor.exe

echo.
echo Application started!
echo Check for the DEBUG popup when you click + Add Single Product
echo.
pause




