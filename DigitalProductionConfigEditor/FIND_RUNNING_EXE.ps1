# This script will find which exe is actually running

# Start transcript to save output
$outputFile = Join-Path $PSScriptRoot "FIND_RUNNING_EXE_RESULTS.txt"
Start-Transcript -Path $outputFile -Force

Write-Host "========================================" -ForegroundColor Yellow
Write-Host "FINDING WHICH EXE IS ACTUALLY RUNNING" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""

# Check if the process is running
$process = Get-Process | Where-Object {$_.ProcessName -like "*DigitalProduction*"}

if ($process) {
    Write-Host "✓ Application IS running!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Process Details:" -ForegroundColor Cyan
    $process | Select-Object ProcessName, Id, Path, StartTime | Format-List
    
    Write-Host ""
    Write-Host "RUNNING FROM THIS LOCATION:" -ForegroundColor Red
    Write-Host $process.Path -ForegroundColor Red
    
    Write-Host ""
    Write-Host "File Details:" -ForegroundColor Cyan
    if ($process.Path) {
        Get-Item $process.Path | Select-Object FullName, LastWriteTime, Length | Format-List
    }
} else {
    Write-Host "✗ Application is NOT running" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please start the application first, then run this script again." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Yellow
Write-Host "OUR COMPILED VERSION DETAILS:" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""

$ourExe = "C:\Users\wanlimah\DigitalProductionConfigEditor\DigitalProductionConfigEditor\bin\Debug\net6.0-windows\DigitalProductionConfigEditor.exe"

if (Test-Path $ourExe) {
    Write-Host "Our compiled exe:" -ForegroundColor Green
    Get-Item $ourExe | Select-Object FullName, LastWriteTime, Length | Format-List
    
    if ($process -and $process.Path -ne $ourExe) {
        Write-Host ""
        Write-Host "⚠️⚠️⚠️ WARNING ⚠️⚠️⚠️" -ForegroundColor Red -BackgroundColor Yellow
        Write-Host ""
        Write-Host "YOU ARE RUNNING A DIFFERENT EXE!" -ForegroundColor Red
        Write-Host ""
        Write-Host "Running exe: $($process.Path)" -ForegroundColor Red
        Write-Host "Our exe:     $ourExe" -ForegroundColor Green
        Write-Host ""
        Write-Host "This is why you don't see the debug messages!" -ForegroundColor Red
    } elseif ($process -and $process.Path -eq $ourExe) {
        Write-Host ""
        Write-Host "✓✓✓ CORRECT EXE IS RUNNING! ✓✓✓" -ForegroundColor Green
        Write-Host ""
        Write-Host "If you still don't see debug popups, something else is wrong." -ForegroundColor Yellow
    }
} else {
    Write-Host "✗ Our exe not found at expected location" -ForegroundColor Red
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Yellow
Write-Host "NEXT STEPS:" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""

if ($process -and $process.Path -ne $ourExe) {
    Write-Host "1. Close the application" -ForegroundColor Cyan
    Write-Host "2. DO NOT run from Visual Studio (F5)" -ForegroundColor Cyan
    Write-Host "3. DO NOT use shortcuts or taskbar pins" -ForegroundColor Cyan
    Write-Host "4. Run directly from:" -ForegroundColor Cyan
    Write-Host "   $ourExe" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Results also saved to: FIND_RUNNING_EXE_RESULTS.txt" -ForegroundColor Green
Write-Host ""

Stop-Transcript

Write-Host "Press Enter to close..." -ForegroundColor Yellow
Read-Host

