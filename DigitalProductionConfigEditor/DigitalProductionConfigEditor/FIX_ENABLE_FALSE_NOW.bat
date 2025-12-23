@echo off
color 0A
cls
echo ================================================================
echo         FIX ENABLE=FALSE DEFAULT - ONE CLICK SOLUTION
echo ================================================================
echo.
echo This will:
echo   1. Close the running app
echo   2. Clean all cached files
echo   3. Rebuild with fresh code
echo   4. Run the fixed version
echo.
echo The enable attribute will default to FALSE after this.
echo.
echo ================================================================
pause

REM Kill the app
echo.
echo Closing app...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

REM Clean
echo Cleaning old files...
cd /d "%~dp0"
rmdir /s /q "bin" 2>nul
rmdir /s /q "obj" 2>nul

REM Rebuild
echo.
echo Rebuilding (please wait 10-20 seconds)...
dotnet build -c Debug -f net6.0-windows --force --nologo

if %errorlevel% neq 0 (
    echo.
    echo ✗ BUILD FAILED! Check errors above.
    pause
    exit /b 1
)

REM Run
echo.
echo ✓ Build successful!
echo.
echo Starting fixed version...
timeout /t 2 /nobreak >nul

cd /d "%~dp0bin\Debug\net6.0-windows"
start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ================================================================
echo                      TEST NOW
echo ================================================================
echo.
echo 1. Go to Step 2
echo 2. Click "+ Add Single Product"
echo 3. Check enable attribute
echo.
echo Should show: enable = FALSE ✓
echo.
echo ================================================================
pause

