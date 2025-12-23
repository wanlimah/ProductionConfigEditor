@echo off
color 0A
echo ================================================================
echo   RUNNING THE CORRECT NEWLY COMPILED VERSION
echo ================================================================
echo.
echo Compiled: 11/11/2025 at 02:25 PM (308,224 bytes)
echo.
echo ================================================================
echo Step 1: Killing ALL instances of DigitalProductionConfigEditor
echo ================================================================
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
if %errorlevel% == 0 (
    echo Killed existing instances
) else (
    echo No existing instances found
)
timeout /t 2 /nobreak >nul

echo.
echo ================================================================
echo Step 2: Navigating to the correct folder
echo ================================================================
cd /d "%~dp0bin\Debug\net6.0-windows\"
echo Current directory: %CD%
echo.

echo ================================================================
echo Step 3: Verifying the exe exists and showing its timestamp
echo ================================================================
if exist DigitalProductionConfigEditor.exe (
    dir DigitalProductionConfigEditor.exe | findstr "DigitalProductionConfigEditor.exe"
    echo.
    echo ✓ File found!
) else (
    echo ✗ ERROR: File not found!
    echo.
    pause
    exit
)

echo.
echo ================================================================
echo Step 4: Starting the application FROM THIS FOLDER
echo ================================================================
echo.
echo **CRITICAL TEST**:
echo.
echo When you click "+ Add Single Product" you MUST see TWO popups:
echo   1st popup: "DEBUG: Constructor called! Code compiled at 2:20 PM"
echo   2nd popup: "DEBUG: After InitializeComponent - about to load attributes"
echo.
echo If you see these popups = New code is running!
echo If NO popups = Something is very wrong!
echo.
echo Press any key to start the application...
pause >nul

echo.
echo Starting DigitalProductionConfigEditor.exe...
echo.

start "" "%~dp0bin\Debug\net6.0-windows\DigitalProductionConfigEditor.exe"

echo.
echo ================================================================
echo Application started!
echo ================================================================
echo.
echo NOW: Go to Step 2, click "+ Add Single Product"
echo.
echo Question: Did you see the DEBUG popups?
echo   - If YES: Great! New code is running!
echo   - If NO: We have a serious problem!
echo.
echo.
pause




