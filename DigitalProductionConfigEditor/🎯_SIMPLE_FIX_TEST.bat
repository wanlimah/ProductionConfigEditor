@echo off
color 0A
cls
echo ════════════════════════════════════════════════════════════════
echo          🎯 SIMPLE FIX - Changed Dropdown Order
echo ════════════════════════════════════════════════════════════════
echo.
echo I found the REAL problem!
echo.
echo The dropdown list was: {"TRUE", "FALSE"}
echo ComboBox was defaulting to the FIRST item!
echo.
echo FIXED: Changed to {"FALSE", "TRUE"}
echo Now FALSE is the first item = default selection!
echo.
pause

echo.
echo [1/3] Killing app...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

echo [2/3] Rebuilding...
cd /d "%~dp0"
dotnet build -c Debug -f net6.0-windows --nologo

if %errorlevel% neq 0 (
    echo ✗ BUILD FAILED!
    pause
    exit /b 1
)
echo    ✓ Built

echo [3/3] Starting...
cd /d "%~dp0bin\Debug\net6.0-windows"

echo.
echo ════════════════════════════════════════════════════════════════
echo                      TEST NOW
echo ════════════════════════════════════════════════════════════════
echo.
echo 1. Go to Step 2
echo 2. Click "+ Add Single Product"
echo 3. Check enable dropdown
echo.
echo EXPECTED: enable should show "FALSE" as selected!
echo.
echo This is a SIMPLE fix - just reordered the list items!
echo.
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ✓ App started!
echo.
echo Test: Click "+ Add Single Product"
echo The enable dropdown should default to FALSE now!
echo.
pause

