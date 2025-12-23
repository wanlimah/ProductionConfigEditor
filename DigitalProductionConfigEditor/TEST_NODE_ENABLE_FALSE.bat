@echo off
color 0E
cls
echo ════════════════════════════════════════════════════════════════
echo       🔒 Configuration Node: All Packages Enable=FALSE
echo ════════════════════════════════════════════════════════════════
echo.
echo NEW FEATURE: When you add a configuration node from Step 1,
echo ALL packages in that node will automatically have enable=FALSE
echo.
echo This is safer - you can then enable only the ones you need!
echo.
echo ════════════════════════════════════════════════════════════════
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
echo                      HOW TO TEST
echo ════════════════════════════════════════════════════════════════
echo.
echo STEP 1: Look at the Step 1 panel
echo         Should see green text:
echo         "Safety: All packages will have enable=FALSE by default"
echo.
echo STEP 2: Add a configuration node from Master Template
echo         (Click the Add button next to any config node)
echo.
echo STEP 3: Go to Step 2 to view the packages
echo.
echo STEP 4: Check the packages - they should all show enable="FALSE"
echo.
echo ────────────────────────────────────────────────────────────────
echo.
echo IMPORTANT: If you DON'T see the green text in Step 1,
echo            the XAML wasn't recompiled (rare but possible)
echo.
echo If you DO see green text but packages still show TRUE,
echo the C# code wasn't recompiled (we know this happens!)
echo.
echo ════════════════════════════════════════════════════════════════
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo App started!
echo.
echo Check Step 1 for the green safety message!
echo.
pause

