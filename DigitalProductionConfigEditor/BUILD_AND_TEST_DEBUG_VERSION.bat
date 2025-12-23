@echo off
color 0E
cls
echo ================================================================
echo    DEBUG VERSION - Find Why Enable Shows TRUE
echo ================================================================
echo.
echo This version will show MULTIPLE popups that tell you:
echo   - What values are being loaded
echo   - When enable is being set to FALSE
echo   - What the final value is
echo.
echo This will help us figure out what's going wrong!
echo.
echo ================================================================
pause

REM Kill running instances
echo.
echo [1/4] Closing app...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

REM Clean everything
echo [2/4] Cleaning old files...
cd /d "%~dp0"
rmdir /s /q "bin" 2>nul
rmdir /s /q "obj" 2>nul
echo    ✓ Cleaned

REM Build with debug messages
echo.
echo [3/4] Building DEBUG version (10-20 seconds)...
dotnet build -c Debug -f net6.0-windows --force --nologo

if %errorlevel% neq 0 (
    echo.
    echo ✗ BUILD FAILED!
    pause
    exit /b 1
)
echo    ✓ Build complete!

REM Run
echo.
echo [4/4] Starting DEBUG version...
timeout /t 2 /nobreak >nul

cd /d "%~dp0bin\Debug\net6.0-windows"

echo.
echo ================================================================
echo                  DEBUG TESTING INSTRUCTIONS
echo ================================================================
echo.
echo 1. Wait for the app to load
echo 2. Go to Step 2 (Manage Packages)
echo 3. Click "+ Add Single Product"
echo.
echo YOU WILL SEE MULTIPLE DEBUG POPUPS!
echo Each popup shows what's happening with the enable attribute.
echo.
echo Click OK on each popup and READ THEM CAREFULLY.
echo Take screenshots if needed!
echo.
echo The popups will tell us:
echo   - What value is loaded from existing packages
echo   - When we try to change it to FALSE
echo   - What the final value is before showing the dialog
echo.
echo If the final popup says FALSE but the dialog shows TRUE,
echo then something in the UI binding is wrong!
echo.
echo ================================================================
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ✓ App started!
echo Now click "+ Add Single Product" and watch the debug popups!
echo.
pause


