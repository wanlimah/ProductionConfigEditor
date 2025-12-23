@echo off
color 0A
cls
echo ════════════════════════════════════════════════════════════════
echo       🟢 XAML COMBOBOX LOADED EVENT FIX
echo ════════════════════════════════════════════════════════════════
echo.
echo DISCOVERY: Only XAML files are being recompiled, not .CS files!
echo.
echo NEW APPROACH: Added ComboBox Loaded event directly in XAML
echo When the enable ComboBox loads, it will FORCE select "FALSE"
echo.
echo This bypasses all the C# code and fixes it at the UI level!
echo.
pause

echo.
echo Killing app...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

echo Rebuilding...
cd /d "%~dp0"
dotnet build -c Debug -f net6.0-windows --nologo

if %errorlevel% neq 0 (
    echo BUILD FAILED!
    pause
    exit /b 1
)

cd /d "%~dp0bin\Debug\net6.0-windows"

echo.
echo ════════════════════════════════════════════════════════════════
echo                      TEST THIS
echo ════════════════════════════════════════════════════════════════
echo.
echo When you click "+ Add Single Product":
echo.
echo CHECK 1: Dialog shows GREEN TEXT:
echo          "VERSION: COMBOBOX LOADED EVENT FIX"
echo.
echo CHECK 2: enable dropdown should show FALSE!
echo          (Forced by ComboBox.Loaded event)
echo.
echo This fix happens at the XAML/UI level, not in C# code!
echo.
echo ════════════════════════════════════════════════════════════════
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo App started!
echo.
echo Click "+ Add Single Product"
echo Look for GREEN TEXT and check enable value!
echo.
pause

