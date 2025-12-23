@echo off
color 0A
cls
echo ════════════════════════════════════════════════════════════════
echo          📝 DEBUG WITH LOG FILE - FINAL TEST
echo ════════════════════════════════════════════════════════════════
echo.
echo Instead of showing MANY popups, this will:
echo   ✓ Show ONE popup when button clicked
echo   ✓ Show ONE popup when constructor called  
echo   ✓ Write EVERYTHING to: DEBUG_LOG.txt on your Desktop
echo.
echo This way we can see EXACTLY what's happening!
echo.
pause

echo.
echo [1/3] Deleting old log file...
del "%USERPROFILE%\Desktop\DEBUG_LOG.txt" 2>nul
echo    ✓ Old log deleted

echo.
echo [2/3] Killing app and rebuilding...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

cd /d "%~dp0"
dotnet build -c Debug -f net6.0-windows --force --nologo

if %errorlevel% neq 0 (
    echo ✗ BUILD FAILED!
    pause
    exit /b 1
)
echo    ✓ Build succeeded

echo.
echo [3/3] Starting app...
cd /d "%~dp0bin\Debug\net6.0-windows"

echo.
echo ════════════════════════════════════════════════════════════════
echo                      TESTING STEPS
echo ════════════════════════════════════════════════════════════════
echo.
echo 1. You'll see startup popup ✓
echo.
echo 2. Go to Step 2, click "+ Add Single Product"
echo.
echo 3. You should see TWO popups:
echo    - "DEBUG LOG CREATED! Check Desktop"
echo    - "CONSTRUCTOR CALLED!"
echo.
echo 4. The dialog will open
echo.
echo 5. Check what enable shows in the dialog
echo.
echo 6. CLOSE the app
echo.
echo 7. Open DEBUG_LOG.txt on your Desktop
echo.
echo 8. Copy the contents and show me!
echo.
echo The log will tell us EXACTLY what value enable has!
echo.
echo ════════════════════════════════════════════════════════════════
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ✓ App started!
echo.
echo After testing, check Desktop for: DEBUG_LOG.txt
echo.
pause

