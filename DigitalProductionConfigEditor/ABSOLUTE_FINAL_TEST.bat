@echo off
color 0C
cls
echo ════════════════════════════════════════════════════════════════
echo           ⚠️ ABSOLUTE FINAL TEST - LAST CHANCE ⚠️
echo ════════════════════════════════════════════════════════════════
echo.
echo This will FORCE the correct executable to run!
echo.
pause

echo.
echo [1] Killing EVERYTHING...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
taskkill /F /IM explorer.exe 2>nul
timeout /t 3 /nobreak >nul
start explorer.exe
timeout /t 2 /nobreak >nul
echo    ✓ Killed all (including Explorer to clear cache)

echo.
echo [2] Delete old DEBUG_LOG.txt...
del "%USERPROFILE%\Desktop\DEBUG_LOG.txt" 2>nul
echo    ✓ Deleted

echo.
echo [3] Rebuilding with TIMESTAMP in title bar...
cd /d "%~dp0"

REM Add unique identifier to window title
powershell -Command "(Get-Content MainWindow.xaml) -replace 'Title=\"Digital Production Config Wizard\"', 'Title=\"Digital Production Config Wizard [BUILD %date:~-4%%date:~-10,2%%date:~-7,2%_%time:~0,2%%time:~3,2%]\"' | Set-Content MainWindow.xaml"

dotnet build -c Debug -f net6.0-windows --force --nologo

if %errorlevel% neq 0 (
    echo ✗ BUILD FAILED!
    pause
    exit /b 1
)
echo    ✓ Built

echo.
echo [4] Verify the EXE exists...
cd /d "%~dp0bin\Debug\net6.0-windows"
if not exist "DigitalProductionConfigEditor.exe" (
    echo ✗ EXE NOT FOUND!
    pause
    exit /b 1
)

set EXE_PATH=%CD%\DigitalProductionConfigEditor.exe
echo.
echo Found at: %EXE_PATH%
dir "DigitalProductionConfigEditor.exe" | findstr "DigitalProductionConfigEditor.exe"

echo.
echo [5] RUNNING WITH ABSOLUTE PATH...
echo.
echo ════════════════════════════════════════════════════════════════
echo                    CRITICAL CHECKS
echo ════════════════════════════════════════════════════════════════
echo.
echo After app starts:
echo.
echo CHECK 1: Window title bar should show:
echo          "Digital Production Config Wizard [BUILD 20251112_XX]"
echo          ^^ This proves it's the NEW build!
echo.
echo CHECK 2: Click "+ Add Single Product"
echo          Should see popup: "DEBUG LOG CREATED!"
echo.
echo CHECK 3: Desktop should have: DEBUG_LOG.txt
echo.
echo If ALL THREE work = New code is running!
echo If ANY fail = You're running wrong executable!
echo.
echo ════════════════════════════════════════════════════════════════
pause

echo.
echo Starting from: %EXE_PATH%
echo.
start "" "%EXE_PATH%"

echo.
echo ✓ STARTED!
echo.
echo CRITICAL: Look at the window TITLE BAR!
echo Does it show [BUILD 20251112_XX]?
echo.
echo   YES = New build running ✓
echo   NO = Old cached version ✗
echo.
pause

echo.
echo After testing, check:
echo   1. Window title has [BUILD ...] ?
echo   2. Popup appeared?
echo   3. DEBUG_LOG.txt on Desktop?
echo.
echo If NO to all = System cache issue beyond our control
echo.
pause

