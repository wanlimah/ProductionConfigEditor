@echo off
color 0C
cls
echo ════════════════════════════════════════════════════════════════
echo              ⚠️ FINAL NUCLEAR TEST ⚠️
echo ════════════════════════════════════════════════════════════════
echo.
echo This version will show:
echo   1. Red text "VERSION: ENABLE=FALSE FIX APPLIED" in dialog
echo   2. Forces enable=FALSE multiple times
echo   3. Creates DEBUG_LOG.txt
echo.
echo If you DON'T see the red text, you're running old version!
echo.
pause

echo.
echo Killing app...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

echo Deleting log...
del "%USERPROFILE%\Desktop\DEBUG_LOG.txt" 2>nul

echo Rebuilding...
cd /d "%~dp0"
dotnet build -c Debug -f net6.0-windows --force --nologo

if %errorlevel% neq 0 (
    echo BUILD FAILED!
    pause
    exit /b 1
)

cd /d "%~dp0bin\Debug\net6.0-windows"

echo.
echo ════════════════════════════════════════════════════════════════
echo                  CRITICAL CHECKS
echo ════════════════════════════════════════════════════════════════
echo.
echo When you click "+ Add Single Product":
echo.
echo CHECK 1: Dialog should show RED TEXT at top:
echo          "VERSION: ENABLE=FALSE FIX APPLIED"
echo.
echo CHECK 2: enable dropdown should show FALSE
echo.
echo CHECK 3: Desktop should have DEBUG_LOG.txt file
echo.
echo ────────────────────────────────────────────────────────────────
echo.
echo If NO RED TEXT = Old version still running (impossible!)
echo If RED TEXT but enable=TRUE = Binding problem
echo.
echo ════════════════════════════════════════════════════════════════
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo App started!
echo.
echo TEST: Click "+ Add Single Product"
echo Look for the RED TEXT!
echo.
pause

echo.
echo After testing:
echo   1. Did you see RED TEXT in dialog?
echo   2. What does enable show?
echo   3. Check Desktop for DEBUG_LOG.txt
echo.
pause

