@echo off
color 0E
cls
echo ================================================================
echo    FORCE CLEAN REBUILD - Digital Production Config Editor
echo    This will ensure you're running the LATEST code
echo ================================================================
echo.

REM Step 1: Kill all running instances
echo [Step 1/5] Killing all running instances...
taskkill /F /IM ProductionConfigEditor.exe 2>nul
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
if %errorlevel% == 0 (
    echo    ✓ Killed existing instances
    timeout /t 2 /nobreak >nul
) else (
    echo    ✓ No instances were running
)
echo.

REM Step 2: Clean bin and obj folders
echo [Step 2/5] Cleaning bin and obj folders...
cd /d "%~dp0"
if exist "bin\" (
    rmdir /s /q "bin" 2>nul
    echo    ✓ Deleted bin folder
)
if exist "obj\" (
    rmdir /s /q "obj" 2>nul
    echo    ✓ Deleted obj folder
)
echo.

REM Step 3: Rebuild the project
echo [Step 3/5] Rebuilding project with .NET 6.0...
echo    This may take 10-20 seconds...
dotnet build -c Debug -f net6.0-windows --force
if %errorlevel% neq 0 (
    echo.
    echo    ✗ BUILD FAILED!
    echo    Please check for compilation errors above.
    pause
    exit /b 1
)
echo    ✓ Build succeeded!
echo.

REM Step 4: Verify the new build
echo [Step 4/5] Verifying build timestamp...
cd /d "%~dp0bin\Debug\net6.0-windows"
if exist "ProductionConfigEditor.exe" (
    dir ProductionConfigEditor.exe | findstr "ProductionConfigEditor.exe"
    dir DigitalProductionConfigEditor.dll | findstr "DigitalProductionConfigEditor.dll"
    echo    ✓ Files found!
) else (
    echo    ✗ ERROR: Executable not found!
    pause
    exit /b 1
)
echo.

REM Step 5: Run the application
echo [Step 5/5] Starting the application...
echo.
echo ================================================================
echo                    VERIFICATION TEST
echo ================================================================
echo.
echo After the app starts:
echo   1. Go to Step 2 (Manage Packages)
echo   2. Click "+ Add Single Product"
echo   3. Check the "enable" attribute value
echo.
echo EXPECTED: enable should default to "FALSE"
echo.
echo If it shows "TRUE" = Old code is still cached (very unlikely now!)
echo If it shows "FALSE" = SUCCESS! New code is running!
echo.
echo ================================================================
echo.
pause

start "" "%CD%\ProductionConfigEditor.exe"

echo.
echo ✓ Application started!
echo.
echo Now test the enable default value...
echo.
pause


