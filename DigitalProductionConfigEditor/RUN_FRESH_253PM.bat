@echo off
color 0E
cls
echo ================================================================
echo           FRESH BUILD TEST - 2:53 PM VERSION
echo ================================================================
echo.
echo Killing any running instances...
taskkill /F /IM DigitalProductionConfigEditor.exe >nul 2>&1
timeout /t 2 /nobreak >nul

echo.
echo Navigating to build folder...
cd /d "%~dp0bin\Debug\net6.0-windows"

echo.
echo ================================================================
echo VERIFYING FILE TIMESTAMPS:
echo ================================================================
dir DigitalProductionConfigEditor.dll | find "DigitalProductionConfigEditor.dll"
dir DigitalProductionConfigEditor.exe | find "DigitalProductionConfigEditor.exe"

echo.
echo ================================================================
echo Both files MUST show 2:53 PM or later!
echo ================================================================
echo.
echo Starting application...
echo.

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ================================================================
echo                    CRITICAL TEST:
echo ================================================================
echo.
echo 1. Wait for app to load completely
echo 2. Navigate to Step 2 (Edit Attributes)  
echo 3. Click the "+ Add Single Product" button
echo 4. COUNT THE POPUPS!
echo.
echo Expected: TWO popups with "DEBUG:" messages
echo - First:  "Constructor called! Code compiled at 2:20 PM"
echo - Second: "After InitializeComponent..."
echo.
echo If you see 0 popups = Windows is caching the old DLL!
echo If you see 2 popups = SUCCESS! New code is running!
echo.
echo ================================================================
pause




