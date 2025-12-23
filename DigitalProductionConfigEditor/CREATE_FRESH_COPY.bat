@echo off
color 0E
cls
echo ════════════════════════════════════════════════════════════════
echo          🔄 CREATE FRESH COPY - CLEAN START
echo ════════════════════════════════════════════════════════════════
echo.
echo This will:
echo   1. Create backup of current folder
echo   2. Copy ONLY source files to fresh location
echo   3. Build in the fresh location (no cache possible!)
echo   4. Run from fresh location
echo.
echo This eliminates ALL caching issues!
echo.
pause

set CURRENT_DIR=%~dp0
set PARENT_DIR=%CURRENT_DIR%..
set TIMESTAMP=%date:~-4%%date:~-10,2%%date:~-7,2%_%time:~0,2%%time:~3,2%%time:~6,2%
set TIMESTAMP=%TIMESTAMP: =0%

echo.
echo [1/6] Killing all instances...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul
echo    ✓ Done

echo.
echo [2/6] Creating backup folder...
cd /d "%PARENT_DIR%"
set BACKUP_NAME=DigitalProductionConfigEditor_BACKUP_%TIMESTAMP%
if not exist "%BACKUP_NAME%\" (
    xcopy /E /I /Q "%CURRENT_DIR%" "%BACKUP_NAME%\" >nul
    echo    ✓ Backup created: %BACKUP_NAME%
) else (
    echo    ⓘ Backup already exists
)

echo.
echo [3/6] Creating FRESH folder...
set FRESH_NAME=DigitalProductionConfigEditor_FRESH
if exist "%FRESH_NAME%\" (
    echo    Deleting old fresh folder...
    rmdir /s /q "%FRESH_NAME%"
)
mkdir "%FRESH_NAME%"
echo    ✓ Created: %FRESH_NAME%

echo.
echo [4/6] Copying source files ONLY (no bin/obj)...
cd /d "%CURRENT_DIR%"
for %%F in (*.cs *.xaml *.csproj *.xml *.ico *.png *.svg) do (
    if exist "%%F" copy "%%F" "%PARENT_DIR%\%FRESH_NAME%\" >nul
)
xcopy /E /I /Q "Views" "%PARENT_DIR%\%FRESH_NAME%\Views\" >nul
xcopy /E /I /Q "ViewModels" "%PARENT_DIR%\%FRESH_NAME%\ViewModels\" >nul
xcopy /E /I /Q "Models" "%PARENT_DIR%\%FRESH_NAME%\Models\" >nul
xcopy /E /I /Q "Converters" "%PARENT_DIR%\%FRESH_NAME%\Converters\" >nul
echo    ✓ Copied source files

echo.
echo [5/6] Building in fresh location...
cd /d "%PARENT_DIR%\%FRESH_NAME%"
echo    This will take 20-30 seconds...
dotnet build -c Debug -f net6.0-windows --nologo

if %errorlevel% neq 0 (
    echo.
    echo    ✗ BUILD FAILED!
    echo    Check errors above.
    pause
    exit /b 1
)

echo.
echo    ✅ BUILD SUCCEEDED in fresh location!

echo.
echo [6/6] Verifying build...
if not exist "bin\Debug\net6.0-windows\DigitalProductionConfigEditor.exe" (
    echo    ✗ EXE not found!
    pause
    exit /b 1
)

dir "bin\Debug\net6.0-windows\DigitalProductionConfigEditor.dll" | findstr "DigitalProductionConfigEditor.dll"

echo.
echo ════════════════════════════════════════════════════════════════
echo                    ✅ FRESH BUILD READY
echo ════════════════════════════════════════════════════════════════
echo.
echo Fresh location: %PARENT_DIR%\%FRESH_NAME%
echo Old location backed up: %PARENT_DIR%\%BACKUP_NAME%
echo.
echo Starting app from FRESH location...
echo.
echo EXPECTED:
echo   1. Startup: "DEBUG VERSION LOADED!" popup
echo   2. After "+ Add Single Product":
echo      - "DEBUG: Button clicked!"
echo      - Multiple constructor popups
echo.
echo If you STILL don't see popups, the source code itself
echo doesn't have the debug messages (very unlikely!)
echo.
pause

cd /d "%PARENT_DIR%\%FRESH_NAME%\bin\Debug\net6.0-windows"
start "" "%CD%\DigitalProductionConfigEditor.exe"

echo.
echo ✓ App started from fresh build!
echo.
echo Location: %CD%
echo.
echo Now test: Click "+ Add Single Product"
echo.
pause

