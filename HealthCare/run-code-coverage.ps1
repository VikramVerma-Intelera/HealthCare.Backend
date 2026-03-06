#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Runs tests with code coverage and generates HTML report
.DESCRIPTION
    This script:
    1. Runs xUnit tests with Coverlet coverage collection
    2. Generates HTML report with ReportGenerator
    3. Opens the report in the default browser
.EXAMPLE
    .\run-code-coverage.ps1
#>

Write-Host "
╔════════════════════════════════════════════════════════════╗
║     Code Coverage Analysis - Healthcare API                ║
╚════════════════════════════════════════════════════════════╝
" -ForegroundColor Cyan

# Step 1: Run tests with coverage
Write-Host "`n📊 Step 1: Running tests with code coverage..." -ForegroundColor Yellow

$testCommand = @(
    "test",
    "/p:CollectCoverage=true",
    "/p:CoverletOutputFormat=cobertura",
    "/p:CoverletOutput=coverage.cobertura.xml",
    "/p:SkipAutoProps=true",
    "/p:IncludeTestAssembly=false"
)

Write-Host "Command: dotnet $($testCommand -join ' ')" -ForegroundColor Gray
$testResult = & dotnet $testCommand

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Tests passed!" -ForegroundColor Green
} else {
    Write-Host "❌ Tests failed!" -ForegroundColor Red
    exit 1
}

# Step 2: Check if coverage file exists
Write-Host "`n📁 Step 2: Checking coverage file..." -ForegroundColor Yellow

$coverageFile = ".\coverage.cobertura.xml"
if (Test-Path $coverageFile) {
    Write-Host "✅ Coverage file found: $coverageFile" -ForegroundColor Green
    Get-Item $coverageFile | ForEach-Object { Write-Host "   Size: $($_.Length) bytes" }
} else {
    Write-Host "❌ Coverage file not found at $coverageFile" -ForegroundColor Red
    Write-Host "   Checking alternative locations..." -ForegroundColor Yellow
    
    $altPaths = @(
        ".\HealthCare\coverage.cobertura.xml",
        ".\bin\Debug\net8.0\coverage.cobertura.xml",
        ".\coverage\coverage.cobertura.xml"
    )
    
    foreach ($path in $altPaths) {
        if (Test-Path $path) {
            Write-Host "✅ Found at: $path" -ForegroundColor Green
            $coverageFile = $path
            break
        }
    }
    
    if (-not (Test-Path $coverageFile)) {
        Write-Host "⚠️  Could not find coverage file. Running dotnet test directly..." -ForegroundColor Yellow
        & dotnet test /p:CollectCoverage=true
        exit 1
    }
}

# Step 3: Generate HTML report
Write-Host "`n📊 Step 3: Generating HTML coverage report..." -ForegroundColor Yellow

$reportDir = "coverage-report"

Write-Host "Command: reportgenerator -reports:$coverageFile -targetdir:$reportDir -reporttypes:Html" -ForegroundColor Gray

$generatorResult = & reportgenerator -reports:"$coverageFile" -targetdir:"$reportDir" -reporttypes:Html

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Report generated successfully!" -ForegroundColor Green
} else {
    Write-Host "❌ Failed to generate report!" -ForegroundColor Red
    Write-Host $generatorResult -ForegroundColor Red
    exit 1
}

# Step 4: Display results
Write-Host "`n📊 Step 4: Coverage Report Summary" -ForegroundColor Yellow

$reportIndex = ".\$reportDir\index.html"
if (Test-Path $reportIndex) {
    Write-Host "✅ Report created at: $reportIndex" -ForegroundColor Green
    Write-Host "" -ForegroundColor Green
    Write-Host "Report Location:" -ForegroundColor Cyan
    Write-Host "  $(Get-Item $reportIndex | ForEach-Object { $_.FullName })" -ForegroundColor Gray
} else {
    Write-Host "⚠️  Report index not found" -ForegroundColor Yellow
}

# Step 5: Show coverage metrics
Write-Host "`n📈 Step 5: Coverage Metrics" -ForegroundColor Yellow

if (Test-Path $coverageFile) {
    [xml]$xmlContent = Get-Content $coverageFile
    
    $lineCoverage = $xmlContent.coverage.'line-rate'
    $branchCoverage = $xmlContent.coverage.'branch-rate'
    
    if ($lineCoverage) {
        $linePct = [math]::Round([double]$lineCoverage * 100, 2)
        $color = if ($linePct -ge 80) { "Green" } elseif ($linePct -ge 60) { "Yellow" } else { "Red" }
        Write-Host "Line Coverage: $linePct%" -ForegroundColor $color
    }
    
    if ($branchCoverage) {
        $branchPct = [math]::Round([double]$branchCoverage * 100, 2)
        $color = if ($branchPct -ge 80) { "Green" } elseif ($branchPct -ge 60) { "Yellow" } else { "Red" }
        Write-Host "Branch Coverage: $branchPct%" -ForegroundColor $color
    }
    
    Write-Host "" -ForegroundColor Gray
    Write-Host "Coverage Targets:" -ForegroundColor Cyan
    Write-Host "  🟢 Green: 80%+ (Excellent)" -ForegroundColor Green
    Write-Host "  🟡 Yellow: 60-79% (Good)" -ForegroundColor Yellow
    Write-Host "  🔴 Red: <60% (Needs Improvement)" -ForegroundColor Red
}

# Step 6: Open report
Write-Host "`n🌐 Step 6: Opening coverage report in browser..." -ForegroundColor Yellow

if (Test-Path $reportIndex) {
    try {
        Start-Process $reportIndex
        Write-Host "✅ Report opened in default browser!" -ForegroundColor Green
    } catch {
        Write-Host "⚠️  Could not auto-open report. Please open manually:" -ForegroundColor Yellow
        Write-Host "   $reportIndex" -ForegroundColor Gray
    }
} else {
    Write-Host "⚠️  Report file not found at $reportIndex" -ForegroundColor Yellow
}

Write-Host "`n
╔════════════════════════════════════════════════════════════╗
║                    ✅ COVERAGE ANALYSIS COMPLETE          ║
╚════════════════════════════════════════════════════════════╝

📊 Next Steps:
  1. Review the coverage report that just opened
  2. Identify uncovered code (red sections)
  3. Add tests for critical uncovered areas
  4. Re-run this script to verify improvement

📁 Files Generated:
  • coverage.cobertura.xml (XML report - for tools)
  • coverage-report/ (HTML report - human readable)

📈 View Report:
  Open: $reportIndex

" -ForegroundColor Green
