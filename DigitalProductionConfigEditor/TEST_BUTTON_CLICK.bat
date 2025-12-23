@echo off
color 0D
cls
echo ════════════════════════════════════════════════════════════════
echo         🔍 DEBUG: Test if Button Click Works
echo ════════════════════════════════════════════════════════════════
echo.
echo This will show popups at EVERY step to trace the issue!
echo.
pause

echo [1/3] Killing app...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

echo [2/3] Rebuilding...
cd /d "%~dp0"
dotnet build -c Debug -f net6.0-windows --force --nologo

if %errorlevel% neq 0 (
    echo BUILD FAILED!
    pause
    exit /b 1
)

echo [3/3] Starting...
cd /d "%~dp0bin\Debug\net6.0-windows"

echo.
echo ════════════════════════════════════════════════════════════════
echo                       TEST SEQUENCE
echo ════════════════════════════════════════════════════════════════
echo.
echo When app starts: 
echo   ✓ You'll see "DEBUG VERSION LOADED!" popup
echo.
echo Then go to Step 2 and click "+ Add Single Product"
echo.
echo You should see these popups IN ORDER:
echo   1. "DEBUG: Button clicked!"
echo   2. "DEBUG: Creating AddPackageDialog NOW..."
echo   3. "DEBUG: AddPackageDialog constructor called!" (with timestamp)
echo   4. "Found X existing attributes..."
echo   5. More debug popups about enable attribute
echo   6. "DEBUG: Dialog created!"
echo.
echo If you DON'T see popup #1 = Button isn't wired correctly
echo If you see #1 but not #3 = Constructor not being called
echo If you see #3 = We can trace the enable issue
echo.
echo ════════════════════════════════════════════════════════════════
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ✓ App started! Now test clicking "+ Add Single Product"
echo.
pause


