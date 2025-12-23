@echo off
color 0B
cls
echo ════════════════════════════════════════════════════════════════
echo      📦 BUILD RELEASE VERSIONS - .NET 6 and .NET 8
echo ════════════════════════════════════════════════════════════════
echo.
echo This will create clean Release builds for distribution:
echo   - .NET 6.0-windows (for older systems)
echo   - .NET 8.0-windows (for newer systems)
echo.
echo Both versions will have:
echo   ✓ Enable=FALSE default for safety
echo   ✓ No debug popups or log files
echo   ✓ Optimized performance
echo.
pause

set PROJ_DIR=%~dp0
cd /d "%PROJ_DIR%"

echo.
echo [1/6] Killing all instances...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul
echo    ✓ Done

echo.
echo [2/6] Cleaning old builds...
rmdir /s /q "bin\Release" 2>nul
rmdir /s /q "obj" 2>nul
echo    ✓ Cleaned

echo.
echo [3/6] Building .NET 6.0-windows Release...
dotnet build -c Release -f net6.0-windows --nologo
if %errorlevel% neq 0 (
    echo    ✗ .NET 6 build FAILED!
    pause
    exit /b 1
)
echo    ✓ .NET 6 build succeeded

echo.
echo [4/6] Building .NET 8.0-windows Release...
dotnet build -c Release -f net8.0-windows --nologo
if %errorlevel% neq 0 (
    echo    ✗ .NET 8 build FAILED!
    pause
    exit /b 1
)
echo    ✓ .NET 8 build succeeded

echo.
echo [5/6] Verifying builds...
if not exist "bin\Release\net6.0-windows\DigitalProductionConfigEditor.exe" (
    echo    ✗ .NET 6 EXE not found!
    pause
    exit /b 1
)
if not exist "bin\Release\net8.0-windows\DigitalProductionConfigEditor.exe" (
    echo    ✗ .NET 8 EXE not found!
    pause
    exit /b 1
)
echo    ✓ Both EXE files verified

echo.
echo [6/6] Creating distribution folders...
set TIMESTAMP=%date:~-4%%date:~-10,2%%date:~-7,2%
set DIST_DIR=%PROJ_DIR%Release_%TIMESTAMP%

if exist "%DIST_DIR%" rmdir /s /q "%DIST_DIR%"
mkdir "%DIST_DIR%"
mkdir "%DIST_DIR%\NET6"
mkdir "%DIST_DIR%\NET8"

echo    Copying .NET 6 files...
xcopy /E /I /Q "bin\Release\net6.0-windows\*.*" "%DIST_DIR%\NET6\" >nul

echo    Copying .NET 8 files...
xcopy /E /I /Q "bin\Release\net8.0-windows\*.*" "%DIST_DIR%\NET8\" >nul

echo    ✓ Distribution folders created

echo.
echo ════════════════════════════════════════════════════════════════
echo                   ✅ BUILD COMPLETE!
echo ════════════════════════════════════════════════════════════════
echo.
echo Release location: %DIST_DIR%
echo.
echo Contents:
echo   📁 NET6\  - For Windows 10/11 (.NET 6.0 required)
echo   📁 NET8\  - For Windows 11 (.NET 8.0 required)
echo.
echo Each folder contains:
echo   ✓ DigitalProductionConfigEditor.exe
echo   ✓ All required DLL files
echo   ✓ Master_Digital_ProductionUserConfig.xml
echo.
echo ────────────────────────────────────────────────────────────────
echo.
echo File sizes:
dir "%DIST_DIR%\NET6\DigitalProductionConfigEditor.exe" | findstr "DigitalProductionConfigEditor.exe"
dir "%DIST_DIR%\NET8\DigitalProductionConfigEditor.exe" | findstr "DigitalProductionConfigEditor.exe"
echo.
echo ════════════════════════════════════════════════════════════════
echo.
echo NEXT STEPS:
echo   1. Test both versions to make sure they work
echo   2. Create README.txt with usage instructions
echo   3. Zip the Release folder for distribution
echo.
echo Both versions have enable=FALSE as default! ✓
echo.
pause

echo.
echo Would you like to open the Release folder? (Y/N)
set /p OPEN_FOLDER=
if /i "%OPEN_FOLDER%"=="Y" (
    explorer "%DIST_DIR%"
)

