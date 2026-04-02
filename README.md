# Test Results Summary

## Скриншоты результатов

| NUnit Results | xUnit Results |
|---------------|----------------|
| ![NUnit тесты](./screenshots/nunit-result.png) | ![xUnit тесты](./screenshots/xunit-result.png) |

## Overview

Two test projects were executed to compare the behavior of **NUnit** and **xUnit** test runners in the context of UI/browser testing.

| Project | Total Tests | Result | Duration |
|---------|-------------|--------|----------|
| EhuTests.NUnit | 5 | ✅ All Passed | 1.2 min |
| EhuTests.xUnit | 5 | ✅ All Passed | 1.5 min |

---

## Test Execution Details

### EhuTests.NUnit
- **Tests in group:** 5
- **Total duration:** 1.2 min
- **Outcome:** 5 passed



### EhuTests.xUnit
- **Tests in group:** 5
- **Total duration:** 1.5 min
- **Outcome:** 5 passed



---

## Key Features Implemented

| Feature | NUnit | xUnit |
|---------|-------|-------|
| Parallel test execution (within project) | ✅ | ✅ |
| Setup / Teardown | `[SetUp]` / `[TearDown]` | Constructor / `Dispose` |
| Data Providers | `[TestCase]` | `[InlineData]` |
| Test filtering (Categories / Traits) | `[Category]` | `[Trait]` |

---

### Conclusion
For UI testing, I consider NUnit more convenient because its [SetUp]/[TearDown] attributes and [TestCase] allow for faster and clearer browser and data setup, while test parallelism works stably.
