@echo off
color 0C
cls
echo ================================================================
echo          ⚠️  NUCLEAR OPTION - ULTRA AGGRESSIVE REBUILD  ⚠️
echo ================================================================
echo.
echo This will COMPLETELY clean everything and force a fresh rebuild.
echo.
echo What this does:
echo   1. Kill ALL instances of the app
echo   2. Delete bin, obj folders
echo   3. Clear .NET build cache
echo   4. Clear Visual Studio cache
echo   5. Force complete rebuild
echo   6. Verify timestamps
echo   7. Run from absolute path
echo.
echo ================================================================
pause

REM Get current directory
set PROJ_DIR=%~dp0
echo Project directory: %PROJ_DIR%
echo.

REM Step 1: Kill ALL instances
echo [Step 1/8] Killing ALL instances...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
taskkill /F /IM dotnet.exe 2>nul
timeout /t 3 /nobreak >nul
echo    ✓ Killed all processes
echo.

REM Step 2: Clean project folders
echo [Step 2/8] Deleting bin and obj folders...
cd /d "%PROJ_DIR%"
if exist "bin\" (
    rmdir /s /q "bin"
    echo    ✓ Deleted bin
)
if exist "obj\" (
    rmdir /s /q "obj"
    echo    ✓ Deleted obj
)
echo.

REM Step 3: Clear .NET cache
echo [Step 3/8] Clearing .NET build cache...
dotnet clean --nologo >nul 2>&1
echo    ✓ Cleaned .NET cache
echo.

REM Step 4: Clear VS ComponentModelCache
echo [Step 4/8] Clearing Visual Studio cache...
set VS_CACHE=%LOCALAPPDATA%\Microsoft\VisualStudio\*\ComponentModelCache
if exist "%VS_CACHE%" (
    rmdir /s /q "%VS_CACHE%" 2>nul
    echo    ✓ Cleared VS cache
) else (
    echo    ⓘ No VS cache found
)
echo.

REM Step 5: Clear temp build files
echo [Step 5/8] Clearing temp files...
del /F /Q "%TEMP%\*.dll" 2>nul
del /F /Q "%TEMP%\*.pdb" 2>nul
echo    ✓ Cleared temp files
echo.

REM Step 6: Restore packages
echo [Step 6/8] Restoring NuGet packages...
dotnet restore --nologo
echo    ✓ Restored packages
echo.

REM Step 7: FORCE REBUILD
echo [Step 7/8] FORCE REBUILDING (this takes 20-30 seconds)...
echo    Please wait...
dotnet build -c Debug -f net6.0-windows --force --no-incremental --nologo

if %errorlevel% neq 0 (
    echo.
    echo    ✗✗✗ BUILD FAILED! ✗✗✗
    echo    Check the errors above.
    echo.
    pause
    exit /b 1
)

echo.
echo    ✅ BUILD SUCCEEDED!
echo.

REM Step 8: Verify the build
echo [Step 8/8] Verifying build...
cd /d "%PROJ_DIR%bin\Debug\net6.0-windows"

if not exist "DigitalProductionConfigEditor.exe" (
    echo    ✗ ERROR: EXE not found!
    pause
    exit /b 1
)

if not exist "DigitalProductionConfigEditor.dll" (
    echo    ✗ ERROR: DLL not found!
    pause
    exit /b 1
)

echo.
echo    File timestamps:
for %%F in (DigitalProductionConfigEditor.exe DigitalProductionConfigEditor.dll) do (
    echo    %%F
    dir "%%F" | findstr "%%F"
)

echo.
echo    ✓ Files verified!
echo.

REM Final step: Run with absolute path
echo ================================================================
echo                    READY TO RUN
echo ================================================================
echo.
echo Build location: %CD%
echo.
echo Starting application with ABSOLUTE PATH to prevent any caching...
echo.
echo AFTER THE APP STARTS:
echo   1. Go to Step 2
echo   2. Click "+ Add Single Product"  
echo   3. YOU MUST SEE DEBUG POPUPS!
echo.
echo If NO popups = Something is VERY wrong (may need to reboot)
echo If you see popups = SUCCESS! New code is running!
echo.
echo ================================================================
pause

echo.
echo Starting application NOW...
echo.
set EXE_PATH=%CD%\DigitalProductionConfigEditor.exe
echo Full path: %EXE_PATH%
echo.

start "" "%EXE_PATH%"

echo.
echo ================================================================
echo Application started from: %EXE_PATH%
echo.
echo NOW: Go to Step 2 and click "+ Add Single Product"
echo You MUST see debug message popups!
echo.
echo If you still see NO popups after this nuclear rebuild,
echo you may need to RESTART YOUR COMPUTER to clear Windows DLL cache.
echo ================================================================
pause


