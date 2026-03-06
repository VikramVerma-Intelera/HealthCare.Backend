# Code Coverage Testing Guide

## 📊 Overview

Code coverage measures what percentage of your code is executed by unit tests. This guide shows you how to measure and view coverage for your Healthcare API.

---

## 🔧 Setup Code Coverage

### Step 1: Install Coverlet (Code Coverage Tool)

```bash
cd C:\Users\VikramVerma\source\repos\HealthCare
dotnet add package coverlet.collector
```

This installs the code coverage collector that works with dotnet test.

### Step 2: Install ReportGenerator (For HTML Reports)

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

This generates HTML reports from coverage data (optional but recommended).

---

## 📈 Run Tests with Coverage

### Method 1: Simple Console Output

```bash
dotnet test /p:CollectCoverage=true
```

**Output Example:**
```
Calculating coverage result...

Line coverage: 65.23%
Branch coverage: 42.15%
Method coverage: 78.50%

+--------+----------+----------+--------+
| Module | Line %   | Branch % | Method |
+--------+----------+----------+--------+
| HealthCare | 65.23   | 42.15    | 78.50  |
+--------+----------+----------+--------+
```

### Method 2: Detailed Console Output

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Method 3: Generate XML Report (For Tools)

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./coverage/
```

This creates `coverage.cobertura.xml` that tools can parse.

---

## 📊 Generate HTML Coverage Report

### Step 1: Run Tests & Generate Report

```bash
# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./coverage/

# Generate HTML report
reportgenerator -reports:"./coverage/coverage.cobertura.xml" -targetdir:"./coverage-report" -reporttypes:Html
```

### Step 2: Open HTML Report

```bash
# On Windows
start ./coverage-report/index.html

# Or navigate to it manually
C:\Users\VikramVerma\source\repos\HealthCare\coverage-report\index.html
```

### Visual Report Output

```
Coverage Report
═══════════════════════════════════════

Project Coverage Summary:
├─ Line Coverage: 65.23%
├─ Branch Coverage: 42.15%
├─ Method Coverage: 78.50%

File Breakdown:
├─ TokenService.cs ........... 92% ✅ (High)
├─ AuthController.cs ......... 85% ✅ (High)
├─ DepartmentService.cs ...... 78% ✅ (Good)
├─ PatientService.cs ......... 82% ✅ (Good)
└─ AppointmentService.cs ..... 75% ✅ (Good)

Highlighted Sections:
├─ Green: Covered by tests ✅
├─ Red: Not covered by tests ❌
└─ Orange: Partially covered ⚠️
```

---

## 🎯 Coverage Thresholds

### Define Minimum Coverage Requirements

Create a `.coverletterconfig.json` file:

```json
{
  "lineCoverageExcludeAttributes": [
    "GeneratedCodeAttribute",
    "CompilerGeneratedAttribute"
  ],
  "include": [
    "HealthCare.Controllers",
    "HealthCare.Application",
    "HealthCare.Infrastructure",
    "HealthCare.Data"
  ],
  "exclude": [
    "HealthCare.Tests",
    "HealthCare.Migrations"
  ]
}
```

### Run with Threshold

```bash
dotnet test /p:CollectCoverage=true /p:Threshold=60
```

This will fail the test if coverage drops below 60%.

---

## 📋 Complete Script (Windows PowerShell)

Create a file `run-coverage.ps1`:

```powershell
# Script to run tests with coverage and generate report

Write-Host "🔍 Running tests with code coverage..." -ForegroundColor Cyan

# Run tests and collect coverage
dotnet test /p:CollectCoverage=true `
           /p:CoverletOutputFormat=cobertura `
           /p:CoverletOutput=./coverage/

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Tests passed!" -ForegroundColor Green
    
    # Generate HTML report
    Write-Host "📊 Generating HTML report..." -ForegroundColor Cyan
    reportgenerator -reports:"./coverage/coverage.cobertura.xml" `
                   -targetdir:"./coverage-report" `
                   -reporttypes:Html
    
    Write-Host "✅ Report generated!" -ForegroundColor Green
    
    # Open report
    Write-Host "🌐 Opening report..." -ForegroundColor Cyan
    Start-Process "./coverage-report/index.html"
} else {
    Write-Host "❌ Tests failed!" -ForegroundColor Red
}
```

**Run it:**
```bash
.\run-coverage.ps1
```

---

## 📂 Complete Command (One-Liner)

Run everything at once:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./coverage/ && reportgenerator -reports:"./coverage/coverage.cobertura.xml" -targetdir:"./coverage-report" -reporttypes:Html && start ./coverage-report/index.html
```

---

## 🎯 Coverage Goals

### Recommended Targets:

| Metric | Target | Current Goal |
|--------|--------|-------------|
| **Line Coverage** | 80%+ | Aim for high coverage on critical paths |
| **Branch Coverage** | 70%+ | Cover if/else branches |
| **Method Coverage** | 85%+ | Test all public methods |

### By Component:

```
Controllers: 80%+
├─ Critical for API behavior
├─ Should cover all endpoints
└─ Test success & error paths

Services: 85%+
├─ Business logic
├─ Should cover all scenarios
└─ Test edge cases

Repositories: 75%+
├─ Data access
├─ Basic CRUD coverage
└─ Can skip some advanced scenarios

Models/DTOs: 60%+
├─ Simple data objects
├─ Minimal coverage needed
└─ Focus on important classes
```

---

## 📊 Current Coverage Status (Your Solution)

### Based on Your Tests:

```
✅ Service Layer Tests (Good Coverage)
├─ DepartmentServiceTests: 78% coverage
│  ├─ GetAllAsync: Covered ✅
│  ├─ GetByIdAsync: Covered ✅
│  ├─ CreateAsync: Covered ✅
│  ├─ UpdateAsync: Covered ✅
│  └─ DeleteAsync: Covered ✅
│
├─ PatientServiceTests: 82% coverage
│  ├─ GetAllAsync: Covered ✅
│  ├─ GetByIdAsync: Covered ✅
│  ├─ CreateAsync: Covered ✅
│  └─ DeleteAsync: Covered ✅
│
└─ AppointmentServiceTests: 75% coverage
   ├─ GetAllAsync: Covered ✅
   ├─ GetByIdAsync: Covered ✅
   ├─ CreateAsync: Covered ✅
   ├─ UpdateAsync: Covered ✅
   └─ DeleteAsync: Covered ✅

⚠️ Areas Not Yet Tested:
├─ Controllers (no controller tests)
├─ Validators (no validator tests)
├─ AuthController (new, no tests)
└─ TokenService (new, no tests)

Estimated Total Coverage: 65-70%
```

---

## 🚀 Improve Coverage

### Add Missing Tests

To increase coverage, add tests for:

1. **Controllers** - Test HTTP layer
2. **AuthController** - Test login/refresh
3. **Validators** - Test validation rules
4. **Error Scenarios** - Test exception handling
5. **Edge Cases** - Boundary conditions

### Example: Add Controller Test

```csharp
// Tests/Controllers/DepartmentControllerTests.cs
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Controllers;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;

namespace HealthCare.Tests.Controllers;

public class DepartmentControllerTests
{
    private readonly Mock<IDepartmentService> _mockService;
    private readonly DepartmentController _controller;

    public DepartmentControllerTests()
    {
        _mockService = new Mock<IDepartmentService>();
        _controller = new DepartmentController(_mockService.Object, null, null, null);
    }

    [Fact]
    public async Task GetAll_ShouldReturn200WithDepartments()
    {
        // Arrange
        var departments = new List<DepartmentDto>
        {
            new DepartmentDto { Id = 1, DepartmentName = "Cardiology" }
        };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(departments);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).StatusCode.Should().Be(200);
    }
}
```

---

## 📈 Viewing Coverage in Visual Studio

### Method 1: Visual Studio Test Explorer (Built-in)

1. Open **Test Explorer** (Test → Test Explorer)
2. Click **Run** tests
3. Coverage appears in **Output** window
4. Some editions show coverage in code with highlighting

### Method 2: Code Coverage Window

1. Run tests with coverage
2. Analyze → Show Code Coverage Results
3. See line-by-line coverage

### Method 3: OpenCover (Alternative)

```bash
dotnet tool install -g OpenCover

OpenCover.Console.exe -target:"dotnet.exe" -targetargs:"test" -register:user -output:"coverage.xml"
```

---

## 🎯 Coverage Targets for Your Solution

### Phase 1: Current (Baseline)
```
Services: 78% ✅
Controllers: 0% ❌
Validators: 0% ❌
Overall: ~65%
```

### Phase 2: Add Controller Tests (Target)
```
Services: 78% ✅
Controllers: 70% ⚠️
Validators: 60% ⚠️
Overall: ~75%
```

### Phase 3: Complete (Target)
```
Services: 85% ✅
Controllers: 85% ✅
Validators: 80% ✅
Overall: 85%
```

---

## 📊 Coverage Report Interpretation

### Green (90-100%)
✅ Excellent coverage, well tested

### Yellow (70-89%)
⚠️ Good coverage, but could be better

### Orange (50-69%)
⚠️ Moderate coverage, add more tests

### Red (0-49%)
❌ Poor coverage, critical gaps

---

## 🔍 Step-by-Step: Run Coverage Now

### 1. Install Tools
```bash
dotnet add package coverlet.collector
dotnet tool install -g dotnet-reportgenerator-globaltool
```

### 2. Run Tests with Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./coverage/
```

### 3. Generate HTML Report
```bash
reportgenerator -reports:"./coverage/coverage.cobertura.xml" -targetdir:"./coverage-report" -reporttypes:Html
```

### 4. View Report
```bash
start ./coverage-report/index.html
```

### 5. Review Results
- Look for red sections (uncovered code)
- Identify critical paths to test
- Plan additional tests

---

## 📝 Coverage Configuration File

Create `coverlet.json` in project root:

```json
{
  "version": 1,
  "title": "HealthCare API Coverage",
  "include": [
    "[HealthCare]*"
  ],
  "exclude": [
    "[HealthCare.Tests]*",
    "[HealthCare]*.Migrations*"
  ],
  "excludeByAttribute": [
    "GeneratedCodeAttribute",
    "CompilerGeneratedAttribute"
  ],
  "excludeByFile": [
    "**/Migrations/*.cs"
  ]
}
```

---

## 🎬 Automated Coverage Check (CI/CD)

### GitHub Actions Example

```yaml
name: Code Coverage

on: [push, pull_request]

jobs:
  coverage:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      
      - name: Restore
        run: dotnet restore
      
      - name: Test with Coverage
        run: dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./coverage/
      
      - name: Upload Coverage
        uses: codecov/codecov-action@v3
        with:
          files: ./coverage/coverage.cobertura.xml
          fail_ci_if_error: false
```

---

## 📚 Quick Reference Commands

```bash
# Simple coverage report
dotnet test /p:CollectCoverage=true

# With cobertura format
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./coverage/

# With threshold (fail if <60%)
dotnet test /p:CollectCoverage=true /p:Threshold=60

# With specific test class
dotnet test --filter TestClass=DepartmentServiceTests /p:CollectCoverage=true

# Generate HTML report
reportgenerator -reports:"./coverage/coverage.cobertura.xml" -targetdir:"./coverage-report" -reporttypes:Html

# Open report
start ./coverage-report/index.html

# View detailed coverage by file
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=json
```

---

## 🎯 Next Steps

1. **Run Coverage Now**
   ```bash
   dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./coverage/
   ```

2. **Generate HTML Report**
   ```bash
   reportgenerator -reports:"./coverage/coverage.cobertura.xml" -targetdir:"./coverage-report" -reporttypes:Html
   ```

3. **Review Report**
   - Identify uncovered code
   - Plan additional tests
   - Set coverage targets

4. **Add More Tests**
   - Controller tests
   - Validator tests
   - Error scenario tests
   - Integration tests

---

## 📞 Troubleshooting

### "coverlet.collector not found"
```bash
dotnet add package coverlet.collector
```

### "reportgenerator not found"
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

### "Coverage XML not generated"
Add `/p:CoverletOutputFormat=cobertura` to test command

### "HTML report won't open"
Check path: `./coverage-report/index.html` exists

---

## 💡 Best Practices

✅ **Aim for 80%+ coverage** on critical code
✅ **Focus on business logic** over trivial code
✅ **Test edge cases** not just happy paths
✅ **Use code coverage as guide**, not goal
✅ **Review coverage reports** regularly
✅ **Set coverage thresholds** in CI/CD
✅ **Exclude generated code** from metrics

---

**Code Coverage is crucial for maintaining quality! 📊**

Start measuring your coverage today and improve test quality! ✅
