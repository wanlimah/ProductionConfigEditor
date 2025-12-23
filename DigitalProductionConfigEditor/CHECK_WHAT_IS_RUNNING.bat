@echo off
color 0C
cls
echo ════════════════════════════════════════════════════════════════
echo          🔍 CHECK WHAT VERSION IS ACTUALLY RUNNING
echo ════════════════════════════════════════════════════════════════
echo.
echo Let's find out EXACTLY which executable you're running!
echo.
pause

echo.
echo [Step 1] Killing all instances...
taskkill /F /IM DigitalProductionConfigEditor.exe 2>nul
timeout /t 2 /nobreak >nul

echo.
echo [Step 2] Finding all copies of the executable...
echo.
echo Searching entire C: drive for DigitalProductionConfigEditor.exe...
echo This may take 30-60 seconds...
echo.

powershell -Command "Get-ChildItem -Path C:\ -Recurse -Filter 'DigitalProductionConfigEditor.exe' -ErrorAction SilentlyContinue | Select-Object FullName, LastWriteTime, @{Name='Size';Expression={$_.Length}} | Format-Table -AutoSize"

echo.
echo ════════════════════════════════════════════════════════════════
echo.
echo Look at the list above. You should see:
echo   - OLD location: ...DigitalProductionConfigEditor\bin\...
echo   - FRESH location: ...DigitalProductionConfigEditor_FRESH\bin\...
echo.
echo Check the LastWriteTime - the FRESH one should be most recent!
echo.
pause

echo.
echo [Step 3] Starting the FRESH version with ABSOLUTE path...
echo.

set FRESH_PATH=C:\Users\wanlimah\DigitalProductionConfigEditor\DigitalProductionConfigEditor_FRESH\bin\Debug\net6.0-windows\DigitalProductionConfigEditor.exe

if exist "%FRESH_PATH%" (
    echo Found FRESH version at:
    echo %FRESH_PATH%
    echo.
    dir "%FRESH_PATH%" | findstr "DigitalProductionConfigEditor.exe"
    echo.
    echo Starting this exact file with full path...
    echo.
    pause
    start "" "%FRESH_PATH%"
    echo.
    echo ✓ Started!
    echo.
    echo IMPORTANT: DO NOT click any shortcuts or taskbar icons!
    echo           That window that just opened is the fresh version!
    echo.
    echo Now test: Click "+ Add Single Product"
    echo.
) else (
    echo.
    echo ✗ FRESH version not found at expected location!
    echo.
    echo Expected: %FRESH_PATH%
    echo.
    echo Did CREATE_FRESH_COPY.bat complete successfully?
    echo.
)

pause

