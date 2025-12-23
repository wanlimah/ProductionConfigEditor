@echo off
color 0B
cls
echo ════════════════════════════════════════════════════════════════
echo              ⚡ DEFINITIVE FIX - RUN THIS NOW ⚡
echo ════════════════════════════════════════════════════════════════
echo.
echo This will:
echo   ✓ Kill the app
echo   ✓ Delete ALL cache
echo   ✓ Rebuild from scratch  
echo   ✓ Show DEBUG popup when app starts
echo.
echo ════════════════════════════════════════════════════════════════
pause

echo.
echo [1/4] Killing app...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

echo [2/4] Deleting cache...
cd /d "%~dp0"
rmdir /s /q "bin" 2>nul
rmdir /s /q "obj" 2>nul

echo [3/4] Rebuilding (20 seconds)...
dotnet build -c Debug -f net6.0-windows --force --no-incremental --nologo

if %errorlevel% neq 0 (
    echo ✗ BUILD FAILED!
    pause
    exit /b 1
)

echo [4/4] Starting app...
cd /d "%~dp0bin\Debug\net6.0-windows"

echo.
echo ════════════════════════════════════════════════════════════════
echo                      IMPORTANT!
echo ════════════════════════════════════════════════════════════════
echo.
echo When the app starts, you MUST see a DEBUG popup immediately!
echo.
echo The popup will say: "DEBUG VERSION LOADED!"
echo.
echo If you see this popup = New code is running ✓
echo If NO popup = Old code still cached (reboot computer) ✗
echo.
echo ════════════════════════════════════════════════════════════════
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ✓ App started!
echo.
echo DID YOU SEE THE DEBUG POPUP?
echo.
pause


