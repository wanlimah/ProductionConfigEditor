@echo off
echo ========================================
echo FINDING WHICH EXE IS RUNNING
echo ========================================
echo.

REM Save output to file
echo Checking for running process... > FIND_RUNNING_EXE_RESULTS.txt
echo. >> FIND_RUNNING_EXE_RESULTS.txt

REM Get the running process path
for /f "tokens=*" %%a in ('wmic process where "name='DigitalProductionConfigEditor.exe'" get ExecutablePath ^| findstr /i ".exe"') do (
    echo RUNNING FROM: >> FIND_RUNNING_EXE_RESULTS.txt
    echo %%a >> FIND_RUNNING_EXE_RESULTS.txt
    echo. >> FIND_RUNNING_EXE_RESULTS.txt
    echo RUNNING FROM:
    echo %%a
    echo.
)

echo. >> FIND_RUNNING_EXE_RESULTS.txt
echo OUR COMPILED EXE SHOULD BE AT: >> FIND_RUNNING_EXE_RESULTS.txt
echo %~dp0bin\Debug\net6.0-windows\DigitalProductionConfigEditor.exe >> FIND_RUNNING_EXE_RESULTS.txt
echo. >> FIND_RUNNING_EXE_RESULTS.txt

echo OUR COMPILED EXE SHOULD BE AT:
echo %~dp0bin\Debug\net6.0-windows\DigitalProductionConfigEditor.exe
echo.

if exist "%~dp0bin\Debug\net6.0-windows\DigitalProductionConfigEditor.exe" (
    echo File date of OUR compiled exe: >> FIND_RUNNING_EXE_RESULTS.txt
    dir "%~dp0bin\Debug\net6.0-windows\DigitalProductionConfigEditor.exe" | findstr "DigitalProductionConfigEditor.exe" >> FIND_RUNNING_EXE_RESULTS.txt
    
    echo File date of OUR compiled exe:
    dir "%~dp0bin\Debug\net6.0-windows\DigitalProductionConfigEditor.exe" | findstr "DigitalProductionConfigEditor.exe"
) else (
    echo ERROR: Our compiled exe not found! >> FIND_RUNNING_EXE_RESULTS.txt
    echo ERROR: Our compiled exe not found!
)

echo.
echo ========================================
echo Results saved to: FIND_RUNNING_EXE_RESULTS.txt
echo ========================================
echo.
echo Please check the file FIND_RUNNING_EXE_RESULTS.txt
echo and send me the contents!
echo.
pause




