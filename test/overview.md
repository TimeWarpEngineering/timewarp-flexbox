# Test Overview

This folder contains test projects that mirror the structure of the [source](../source/) directory.

## Structure

Test projects follow the source folder hierarchy:

```
source/
└── TimeWarp.Flexbox/
    ├── Components/
    └── Rendering/

test/
└── TimeWarp.Flexbox.Tests/
    ├── Components/
    └── Rendering/
```

## Naming Conventions

- Test projects: `{ProjectName}.Tests`
- Test files: `{ClassUnderTest}-tests.cs` (kebab-case)
- Test classes: `{ClassUnderTest}Tests` (PascalCase)

Example:
- Source: `source/TimeWarp.Flexbox/component-base.cs` → `ComponentBase`
- Test: `test/TimeWarp.Flexbox.Tests/component-base-tests.cs` → `ComponentBaseTests`

## Test Organization

### Unit Tests
Test individual classes and methods in isolation:
- Mock dependencies
- Test single responsibility
- Fast execution

### Integration Tests
Test components working together:
- Real dependencies where practical
- Test interactions
- May be slower

### Project Structure
```csharp
namespace TimeWarp.Flexbox.Tests;

public class ComponentBaseTests
{
  [Fact]
  public void Constructor_SetsProperties_Correctly()
  {
    // Arrange
    // Act
    // Assert
  }
}
```

## Test Frameworks

- **xUnit** - Primary test framework
- **FluentAssertions** - Readable assertions
- **NSubstitute** - Mocking framework (if needed)
- **Verify** - Snapshot testing (if needed)

## Running Tests

```bash
# Using runfile
./runfiles/test.cs

# Or traditional dotnet
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Coverage Goals

- **Target**: 80%+ code coverage
- **Critical paths**: 90%+ coverage
- **Public API**: 100% coverage

## Test Standards

### Naming
```csharp
// Method pattern: MethodName_Scenario_ExpectedBehavior
[Fact]
public void Render_WhenNotInitialized_ThrowsInvalidOperationException()

// Class pattern: {ClassUnderTest}Tests
public class RendererTests
```

### Arrange-Act-Assert
```csharp
[Fact]
public void Example_Test()
{
  // Arrange - Set up test data and dependencies
  var component = new MyComponent();

  // Act - Execute the method under test
  var result = component.DoSomething();

  // Assert - Verify the outcome
  result.Should().Be(expected);
}
```

### One Assertion Per Test
Prefer focused tests with single assertions:
```csharp
// Good - focused
[Fact]
public void Name_ReturnsExpectedValue()
{
  component.Name.Should().Be("Expected");
}

// Avoid - testing multiple concerns
[Fact]
public void ComponentProperties_AreCorrect()
{
  component.Name.Should().Be("Expected");
  component.IsActive.Should().BeTrue();
  component.Count.Should().Be(5);
}
```

### Test Data
```csharp
// Use Theory for parameterized tests
[Theory]
[InlineData("input1", "expected1")]
[InlineData("input2", "expected2")]
public void Method_WithVariousInputs_ProducesExpectedOutput(
  string input,
  string expected)
{
  var result = Method(input);
  result.Should().Be(expected);
}
```

## Continuous Integration

Tests run automatically on:
- Every commit
- Pull requests
- Before merging to main

See [standards](../documentation/developer/standards/) for CI/CD configuration.

## Test-Driven Development (TDD)

Consider TDD for:
- Complex algorithms
- Critical business logic
- Public API design

TDD cycle:
1. **Red** - Write failing test
2. **Green** - Write minimal code to pass
3. **Refactor** - Improve code quality

## Debugging Tests

```bash
# Run specific test
dotnet test --filter "FullyQualifiedName~ComponentBaseTests.Constructor"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Debug in IDE
# Set breakpoint and use test runner
```

## Related Documentation

- [Standards](../documentation/developer/standards/) - Coding conventions
- [Guides](../documentation/developer/guides/) - Testing tutorials (TBD)
- [Source](../source/) - Code under test
