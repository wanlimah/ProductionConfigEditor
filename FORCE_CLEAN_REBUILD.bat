@echo off
echo ==========================================
echo      FORCE CLEAN AND REBUILD TOOL
echo ==========================================
echo.
echo 1. Closing Visual Studio instances (save your work first if open!)...
taskkill /F /IM devenv.exe >nul 2>&1

echo.
echo 2. Unlocking files and cleaning bin and obj folders...
if exist "DigitalProductionConfigEditor\bin" attrib -r "DigitalProductionConfigEditor\bin\*.*" /s >nul 2>&1
if exist "DigitalProductionConfigEditor\bin" rmdir /s /q "DigitalProductionConfigEditor\bin"
if exist "DigitalProductionConfigEditor\obj" rmdir /s /q "DigitalProductionConfigEditor\obj"

echo.
echo 3. Cleaning completed.
echo.
echo PLEASE DO THE FOLLOWING:
echo 1. Open Visual Studio again.
echo 2. Open the solution "DigitalProductionConfigEditor.sln".
echo 3. Right-click the Solution in Solution Explorer -> Rebuild Solution.
echo.
pause






