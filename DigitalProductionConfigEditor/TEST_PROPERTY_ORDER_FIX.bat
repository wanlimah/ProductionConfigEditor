@echo off
color 0B
cls
echo ════════════════════════════════════════════════════════════════
echo        🔧 FIX: Set DropdownOptions BEFORE Value
echo ════════════════════════════════════════════════════════════════
echo.
echo New approach: Set properties in correct order
echo   1. Set DropdownOptions FIRST (so ComboBox knows the list)
echo   2. Set Value SECOND (so ComboBox can select it)
echo.
echo This ensures the ComboBox has its ItemsSource populated
echo before trying to set the SelectedItem!
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
echo SHOULD SHOW: FALSE (finally!)
echo.
echo This fix ensures the dropdown list exists BEFORE
echo we try to set the selected value.
echo.
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ✓ App started!
echo.
pause

