@echo off
color 0C
cls
echo ════════════════════════════════════════════════════════════════
echo       ⚠️ FORCE RECOMPILE EVERYTHING - NO EXCEPTIONS ⚠️
echo ════════════════════════════════════════════════════════════════
echo.
echo The problem: Only SOME files got recompiled!
echo   - MainWindow.xaml.cs = NEW ✓
echo   - Step2_EditAttributes.xaml.cs = OLD ✗
echo.
echo This script will FORCE recompile ALL .cs files!
echo.
pause

set PROJ_DIR=%~dp0
cd /d "%PROJ_DIR%"

echo.
echo [1/7] Killing all instances...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
taskkill /F /IM dotnet.exe 2>nul
taskkill /F /IM MSBuild.exe 2>nul
timeout /t 3 /nobreak >nul
echo    ✓ Done

echo.
echo [2/7] Deleting ALL build outputs...
if exist "bin\" rmdir /s /q "bin"
if exist "obj\" rmdir /s /q "obj"
echo    ✓ Deleted bin and obj

echo.
echo [3/7] Touching all .cs files to force recompile...
echo    This makes the build system think ALL files changed!
powershell -Command "Get-ChildItem -Recurse -Filter *.cs | ForEach-Object { $_.LastWriteTime = Get-Date }"
echo    ✓ Touched all .cs files

echo.
echo [4/7] Touching all .xaml files...
powershell -Command "Get-ChildItem -Recurse -Filter *.xaml | ForEach-Object { $_.LastWriteTime = Get-Date }"
echo    ✓ Touched all .xaml files

echo.
echo [5/7] Cleaning solution...
dotnet clean --nologo -v q
echo    ✓ Cleaned

echo.
echo [6/7] REBUILDING from scratch (30 seconds)...
echo    --force = force dependencies rebuild
echo    --no-incremental = no incremental build
echo    -v detailed = show what's being compiled
echo.
dotnet build -c Debug -f net6.0-windows --force --no-incremental -v minimal

if %errorlevel% neq 0 (
    echo.
    echo ✗✗✗ BUILD FAILED! ✗✗✗
    pause
    exit /b 1
)

echo.
echo    ✅ BUILD SUCCEEDED!

echo.
echo [7/7] Verifying files...
cd /d "%PROJ_DIR%bin\Debug\net6.0-windows"

if not exist "DigitalProductionConfigEditor.dll" (
    echo ✗ DLL not found!
    pause
    exit /b 1
)

echo.
echo Build timestamp:
dir DigitalProductionConfigEditor.dll | findstr "DigitalProductionConfigEditor.dll"
echo.

echo ════════════════════════════════════════════════════════════════
echo                    ✅ READY TO TEST
echo ════════════════════════════════════════════════════════════════
echo.
echo Starting app now...
echo.
echo EXPECTED POPUPS:
echo.
echo 1. At startup: "DEBUG VERSION LOADED!"
echo    Build time: 11/12/2025 09:XX:XX (just now)
echo.
echo 2. After clicking "+ Add Single Product":
echo    - "DEBUG: Button clicked!"
echo    - "DEBUG: Creating AddPackageDialog NOW..."
echo    - "DEBUG: AddPackageDialog constructor called!"
echo    - Several more about enable attribute
echo.
echo If you DON'T see the button popups this time,
echo Windows DLL cache is the problem - REBOOT COMPUTER.
echo.
echo ════════════════════════════════════════════════════════════════
pause

start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ✓ App started!
echo.
echo TEST: Click "+ Add Single Product" and count the popups!
echo.
pause


