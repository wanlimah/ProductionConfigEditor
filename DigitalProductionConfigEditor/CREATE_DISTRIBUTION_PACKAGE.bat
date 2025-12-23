@echo off
color 0A
cls
echo ════════════════════════════════════════════════════════════════
echo       📦 CREATE DISTRIBUTION PACKAGE WITH ZIP FILES
echo ════════════════════════════════════════════════════════════════
echo.
echo This will:
echo   1. Build Release versions (.NET 6 and .NET 8)
echo   2. Create distribution folders
echo   3. Add README files
echo   4. Create ZIP files for easy distribution
echo.
pause

set PROJ_DIR=%~dp0
cd /d "%PROJ_DIR%"

echo.
echo [1/8] Killing all instances...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

echo.
echo [2/8] Cleaning old builds...
rmdir /s /q "bin\Release" 2>nul
rmdir /s /q "obj" 2>nul

echo.
echo [3/8] Building .NET 6.0-windows Release...
dotnet build -c Release -f net6.0-windows --nologo
if %errorlevel% neq 0 (
    echo BUILD FAILED!
    pause
    exit /b 1
)

echo.
echo [4/8] Building .NET 8.0-windows Release...
dotnet build -c Release -f net8.0-windows --nologo
if %errorlevel% neq 0 (
    echo BUILD FAILED!
    pause
    exit /b 1
)

echo.
echo [5/8] Creating distribution structure...
set TIMESTAMP=%date:~-4%%date:~-10,2%%date:~-7,2%
set DIST_DIR=%PROJ_DIR%DigitalProductionConfigEditor_Release_%TIMESTAMP%

if exist "%DIST_DIR%" rmdir /s /q "%DIST_DIR%"
mkdir "%DIST_DIR%"

echo.
echo [6/8] Packaging .NET 6 version...
mkdir "%DIST_DIR%\DigitalProductionConfigEditor_NET6"
xcopy /E /I /Q "bin\Release\net6.0-windows\*.*" "%DIST_DIR%\DigitalProductionConfigEditor_NET6\" >nul
copy "RELEASE_README.txt" "%DIST_DIR%\DigitalProductionConfigEditor_NET6\README.txt" >nul

REM Create backup copy of Master XML with read-only protection
if exist "%DIST_DIR%\DigitalProductionConfigEditor_NET6\Master_Digital_ProductionUserConfig.xml" (
    copy "%DIST_DIR%\DigitalProductionConfigEditor_NET6\Master_Digital_ProductionUserConfig.xml" "%DIST_DIR%\DigitalProductionConfigEditor_NET6\Master_Digital_ProductionUserConfig.BACKUP.xml" >nul
    attrib +R "%DIST_DIR%\DigitalProductionConfigEditor_NET6\Master_Digital_ProductionUserConfig.xml"
    echo    ✓ Master XML protected and backup created
)

echo.
echo [7/8] Packaging .NET 8 version...
mkdir "%DIST_DIR%\DigitalProductionConfigEditor_NET8"
xcopy /E /I /Q "bin\Release\net8.0-windows\*.*" "%DIST_DIR%\DigitalProductionConfigEditor_NET8\" >nul
copy "RELEASE_README.txt" "%DIST_DIR%\DigitalProductionConfigEditor_NET8\README.txt" >nul

REM Create backup copy of Master XML with read-only protection
if exist "%DIST_DIR%\DigitalProductionConfigEditor_NET8\Master_Digital_ProductionUserConfig.xml" (
    copy "%DIST_DIR%\DigitalProductionConfigEditor_NET8\Master_Digital_ProductionUserConfig.xml" "%DIST_DIR%\DigitalProductionConfigEditor_NET8\Master_Digital_ProductionUserConfig.BACKUP.xml" >nul
    attrib +R "%DIST_DIR%\DigitalProductionConfigEditor_NET8\Master_Digital_ProductionUserConfig.xml"
    echo    ✓ Master XML protected and backup created
)

echo.
echo [8/8] Creating ZIP files...
cd /d "%DIST_DIR%"

powershell -Command "Compress-Archive -Path 'DigitalProductionConfigEditor_NET6\*' -DestinationPath 'DigitalProductionConfigEditor_NET6.zip' -Force"
if %errorlevel% == 0 (
    echo    ✓ NET6 ZIP created
) else (
    echo    ✗ NET6 ZIP failed
)

powershell -Command "Compress-Archive -Path 'DigitalProductionConfigEditor_NET8\*' -DestinationPath 'DigitalProductionConfigEditor_NET8.zip' -Force"
if %errorlevel% == 0 (
    echo    ✓ NET8 ZIP created
) else (
    echo    ✗ NET8 ZIP failed
)

cd /d "%PROJ_DIR%"

echo.
echo ════════════════════════════════════════════════════════════════
echo                   ✅ PACKAGE COMPLETE!
echo ════════════════════════════════════════════════════════════════
echo.
echo Package location: %DIST_DIR%
echo.
echo Contents:
echo   📁 DigitalProductionConfigEditor_NET6\     (folder)
echo   📦 DigitalProductionConfigEditor_NET6.zip  (ready to distribute)
echo.
echo   📁 DigitalProductionConfigEditor_NET8\     (folder)
echo   📦 DigitalProductionConfigEditor_NET8.zip  (ready to distribute)
echo.
echo ════════════════════════════════════════════════════════════════
echo.
echo Each package includes:
echo   ✓ DigitalProductionConfigEditor.exe
echo   ✓ All required DLLs
echo   ✓ Master_Digital_ProductionUserConfig.xml
echo   ✓ README.txt (installation instructions)
echo.
echo ════════════════════════════════════════════════════════════════
echo.
echo NEXT STEPS:
echo   1. Test both versions
echo   2. Distribute ZIP files to users
echo   3. Users extract and run the .exe
echo.
echo 🔒 Both versions have enable=FALSE as default!
echo.
pause

echo.
echo Open the distribution folder? (Y/N)
set /p OPEN=
if /i "%OPEN%"=="Y" (
    explorer "%DIST_DIR%"
)

