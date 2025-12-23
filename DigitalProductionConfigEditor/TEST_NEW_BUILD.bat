@echo off
color 0A
echo ================================================================
echo ULTRA AGGRESSIVE TEST - KILL EVERYTHING AND RUN FRESH
echo ================================================================
echo.

echo Step 1: Killing ALL instances...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
if %errorlevel% == 0 (
    echo    [SUCCESS] Killed running instances
    timeout /t 2 /nobreak >nul
) else (
    echo    [INFO] No instances were running
)

echo.
echo Step 2: Navigating to compiled folder...
cd /d "%~dp0bin\Debug\net6.0-windows"
echo    Current dir: %CD%

echo.
echo Step 3: Checking file timestamp...
dir DigitalProductionConfigEditor.exe | find "DigitalProductionConfigEditor.exe"

echo.
echo Step 4: Checking DLL timestamp (this contains the actual code!)...
dir DigitalProductionConfigEditor.dll | find "DigitalProductionConfigEditor.dll"

echo.
echo ================================================================
echo CRITICAL: Both EXE and DLL must be from 2:46 PM or later!
echo If DLL is older = That's why old code is running!
echo ================================================================
echo.

echo Step 5: Deleting any PDB cache files...
del /F /Q *.pdb.tmp 2>nul
del /F /Q *.pdb.lock 2>nul

echo.
echo Step 6: Starting application with FULL PATH...
echo.
set EXE_PATH=%CD%\DigitalProductionConfigEditor.exe
echo Launching: %EXE_PATH%
echo.
start "" "%EXE_PATH%"

echo.
echo [CRITICAL TEST]:
echo  1. Wait for app to fully load
echo  2. Go to Step 2 (Edit Attributes)
echo  3. Click "+ Add Single Product"
echo  4. You MUST see TWO popups if new code is running
echo  5. If 0 popups = DLL is cached somewhere!
echo.
pause




