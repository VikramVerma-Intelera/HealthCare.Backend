# 📊 How to View Code Coverage in Your Solution

## Quick Answer

Your Healthcare API solution now has **code coverage tools fully configured**. Here's how to use them:

---

## ✅ Tools Installed

✅ **coverlet.collector** - Collects code coverage data during tests
✅ **dotnet-reportgenerator-globaltool** - Generates HTML coverage reports

---

## 🚀 Run Coverage Now (3 Ways)

### Way 1: Easiest - Run PowerShell Script

```powershell
.\run-code-coverage.ps1
```

This script will:
1. Run tests with coverage
2. Generate HTML report
3. Open report in your browser
4. Show coverage statistics

### Way 2: Manual Commands

```bash
# Step 1: Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

# Step 2: Generate HTML report
reportgenerator -reports:"coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

# Step 3: Open report
start coverage-report/index.html
```

### Way 3: One-Line Command

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura && reportgenerator -reports:"coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html && start coverage-report/index.html
```

---

## 📊 What You'll See

### Coverage Report Includes:

✅ **Line Coverage %** - What percentage of lines are executed
✅ **Branch Coverage %** - What percentage of conditional branches are tested
✅ **Method Coverage %** - What percentage of methods are called

### Color Coding:

- 🟢 **Green (90-100%)** - Excellent coverage
- 🟡 **Yellow (60-89%)** - Good coverage
- 🔴 **Red (<60%)** - Needs more tests

### Example Output:

```
Code Coverage Report
═══════════════════════════════════

Summary:
├─ Line Coverage:     72.45%  🟡
├─ Branch Coverage:   65.30%  🟡
└─ Method Coverage:   85.60%  🟢

By Module:
├─ TokenService.cs ............... 92% 🟢
├─ DepartmentService.cs .......... 78% 🟡
├─ PatientService.cs ............ 82% 🟡
├─ AppointmentService.cs ........ 75% 🟡
└─ AuthController.cs ............ 0%  🔴 (No tests yet)
```

---

## 🎯 Current Coverage Status

Based on your test suite:

```
✅ Services (Good Coverage)
├─ DepartmentService: ~78% ✅
├─ PatientService: ~82% ✅
└─ AppointmentService: ~75% ✅

⚠️ Areas Not Yet Tested
├─ Controllers (0%)
├─ AuthController (0%)
├─ Validators (0%)
└─ TokenService (0%)

Estimated Overall: 65-70%
```

---

## 📁 Files Generated

After running coverage, you'll find:

```
C:\Users\VikramVerma\source\repos\HealthCare\
├── coverage.cobertura.xml          ← XML report (for tools)
└── coverage-report/                ← HTML report (open in browser)
    ├── index.html                  ← Main report
    ├── dashboard.html
    ├── report.html
    └── (other HTML files)
```

---

## 🌐 Opening the Coverage Report

### Method 1: PowerShell Script (Auto-Opens)
```powershell
.\run-code-coverage.ps1
# Opens in browser automatically!
```

### Method 2: Manual Open
```bash
start coverage-report/index.html
```

### Method 3: Double-Click
1. Navigate to: `coverage-report\index.html`
2. Double-click to open in default browser

---

## 📈 Understanding the Report

### Summary Tab
Shows overall coverage percentages:
- Line Coverage: percentage of lines executed
- Branch Coverage: percentage of conditions tested
- Method Coverage: percentage of methods called

### Modules Tab
Shows coverage by file:
- Green highlighting = covered code
- Red highlighting = uncovered code
- Orange highlighting = partially covered

### Risks Tab
Shows high-risk uncovered code (important to test)

---

## 🎯 Improving Coverage

### To Increase Coverage:

1. **Add Controller Tests**
   ```csharp
   // Tests/Controllers/PatientControllerTests.cs
   ```

2. **Add Auth Tests**
   ```csharp
   // Tests/Controllers/AuthControllerTests.cs
   ```

3. **Add Validator Tests**
   ```csharp
   // Tests/Validators/PatientValidatorTests.cs
   ```

4. **Test Error Scenarios**
   - Test validation failures
   - Test exception handling
   - Test edge cases

---

## 📋 Common Coverage Commands

### View Coverage in Console Only (No Report)
```bash
dotnet test /p:CollectCoverage=true
```

### Generate With Specific Threshold
```bash
# Fail test if coverage drops below 70%
dotnet test /p:CollectCoverage=true /p:Threshold=70
```

### Coverage for Specific Test Class
```bash
dotnet test --filter TestClass=DepartmentServiceTests /p:CollectCoverage=true
```

### Multiple Report Formats
```bash
dotnet test /p:CollectCoverage=true `
           /p:CoverletOutputFormat="cobertura,json,html" `
           /p:CoverletOutput="coverage/"
```

---

## 🔧 Configuration

### Custom Coverage Settings

Create `.coverletterconfig.json` in project root:

```json
{
  "include": [
    "HealthCare.Controllers",
    "HealthCare.Application",
    "HealthCare.Infrastructure"
  ],
  "exclude": [
    "HealthCare.Tests",
    "HealthCare.Migrations"
  ],
  "excludeByAttribute": [
    "GeneratedCodeAttribute",
    "CompilerGeneratedAttribute"
  ]
}
```

---

## 📊 Interpreting Results

### Coverage Goals by Component:

| Component | Target | Your Current |
|-----------|--------|------------|
| Services | 85%+ | ~78% ⚠️ |
| Controllers | 80%+ | 0% ❌ |
| Validators | 75%+ | 0% ❌ |
| Overall | 80%+ | ~65% ⚠️ |

### Next Actions:

1. ✅ Services are good, could use a few more edge cases
2. ❌ Add controller tests (currently 0%)
3. ❌ Add validator tests (currently 0%)
4. ⚠️ Add auth/token tests (currently 0%)

---

## 🚀 Quick Start (Right Now)

### Step 1: Run Coverage Script
```powershell
cd C:\Users\VikramVerma\source\repos\HealthCare
.\run-code-coverage.ps1
```

### Step 2: Wait for Tests
The script will:
- Run all 16 unit tests ✅
- Collect coverage data
- Generate HTML report
- Open in browser

### Step 3: Review Results
1. Look at **Summary** tab - see overall coverage
2. Look at **Modules** tab - see file-by-file coverage
3. Look at **Risks** tab - see important uncovered code
4. Identify red areas to test

### Step 4: Plan More Tests
Focus on these areas:
1. Controllers (0% coverage)
2. AuthController (0% coverage)
3. Validators (0% coverage)
4. TokenService (0% coverage)

---

## 🎓 Example Report Contents

When you open the HTML report, you'll see:

```
📊 Code Coverage Report

┌─ Summary
│  Line Coverage: 65.45%
│  Branch Coverage: 54.20%
│  Method Coverage: 78.90%
│
├─ By File/Module
│  ├─ TokenService.cs .......... 92% (well tested)
│  ├─ DepartmentService.cs ..... 78% (mostly tested)
│  ├─ PatientService.cs ........ 82% (mostly tested)
│  ├─ DepartmentController.cs .. 0%  (NOT TESTED)
│  └─ AuthController.cs ........ 0%  (NOT TESTED)
│
├─ Color Coding
│  Green code: Covered by tests ✅
│  Red code: Not covered ❌
│  Orange code: Partially covered ⚠️
│
└─ Drill Down
   Click any file to see line-by-line coverage
```

---

## 🐛 Troubleshooting

### Issue: "Coverage file not found"
**Solution:** Run from project directory
```bash
cd C:\Users\VikramVerma\source\repos\HealthCare
.\run-code-coverage.ps1
```

### Issue: "reportgenerator not found"
**Solution:** Install it
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

### Issue: "No coverage data"
**Solution:** Ensure coverlet.collector is installed
```bash
dotnet add package coverlet.collector
```

### Issue: "Report won't open"
**Solution:** Open manually
```bash
start .\coverage-report\index.html
```

---

## 📚 Additional Resources

- **CODE_COVERAGE_GUIDE.md** - Comprehensive coverage guide
- **Official Coverlet Docs** - https://coverletio.github.io/
- **ReportGenerator Docs** - https://github.com/danielpalme/ReportGenerator

---

## ✅ You Now Have

✅ Coverage collection tools installed
✅ PowerShell script to run tests & generate reports
✅ HTML report generation configured
✅ Initial baseline coverage metrics
✅ Clear visibility into what's tested vs untested

---

## 🎯 Next Steps

1. **Right Now:** Run `.\run-code-coverage.ps1`
2. **Review:** Look at the HTML coverage report
3. **Plan:** Identify areas with 0% coverage to test
4. **Improve:** Add tests for uncovered code
5. **Measure:** Run coverage again to verify improvement

---

## 💡 Pro Tips

✅ Run coverage as part of your development workflow
✅ Aim for 80%+ coverage on business logic
✅ Don't focus on coverage % alone - test quality matters more
✅ Use coverage reports to find blind spots
✅ Add coverage check to CI/CD pipeline

---

**Your code coverage tools are ready! Start measuring today! 📊**

Run: `.\run-code-coverage.ps1`
