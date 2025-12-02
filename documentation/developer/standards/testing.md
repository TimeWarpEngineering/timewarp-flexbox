# Testing

Unit testing standards using TimeWarp.Fixie and Shouldly.

---

## Configure project

Add packages to your test project:

```xml
<PackageReference Include="TimeWarp.Fixie" />
<PackageReference Include="Fixie.TestAdapter" />
<PackageReference Include="Shouldly" />
```

---

## Create convention

Add a class inheriting from `TimeWarp.Fixie.TestingConvention`:

```csharp
namespace TimeWarp.Flexbox.Tests;

public class TestingConvention : TimeWarp.Fixie.TestingConvention;
```

---

## Write tests

Public methods in public classes are tests by convention. No `[Test]` attribute needed.

```csharp
namespace TimeWarp.Flexbox.Tests.Values;

public class FlexValueTests
{
  public void ShouldCreatePointValue()
  {
    FlexValue value = FlexValue.Point(100);

    value.Value.ShouldBe(100);
    value.Unit.ShouldBe(Unit.Point);
  }
}
```

---

## Skip tests

Use the `[Skip]` attribute with a reason:

```csharp
[Skip("Pending implementation")]
public void ShouldHandleEdgeCase()
{
  // Not implemented yet
}
```

---

## Tag tests

Add tags for filtering test runs:

```csharp
[TestTag(TestTags.Fast)]
public class QuickTests
{
  [TestTag("Integration")]
  public void ShouldConnectToDatabase()
  {
    // Tagged with both Fast and Integration
  }
}
```

Built-in tags: `TestTags.Fast`, `TestTags.Slow`.

---

## Parameterized tests

Use `[Input]` for multiple test cases (similar to xUnit `[Theory]`):

```csharp
[Input(5, 3, 2)]
[Input(8, 5, 3)]
[Input(10, 10, 0)]
public void ShouldSubtract(int x, int y, int expected)
{
  int result = x - y;
  result.ShouldBe(expected);
}
```

---

## Lifecycle methods

Add `Setup` and `Cleanup` methods for test lifecycle:

```csharp
public class DatabaseTests
{
  private DbConnection Connection;

  public void Setup()
  {
    Connection = new DbConnection();
    Connection.Open();
  }

  public void Cleanup()
  {
    Connection?.Close();
  }

  public void ShouldQueryData()
  {
    // Connection is open here
  }
}
```

Lifecycle runs for each test method and each `[Input]` variation.

---

## Exclude from tests

Mark non-test classes with `[NotTest]`:

```csharp
[NotTest]
public class TestHelpers
{
  public static FlexNode CreateTestNode() => new();
}
```

---

## Dependency injection

Tests support constructor injection from the DI container:

```csharp
public class ServiceTests
{
  private readonly IMyService Service;

  public ServiceTests(IMyService service)
  {
    Service = service;
  }

  public static void ConfigureServices(IServiceCollection services)
  {
    services.AddScoped<IMyService, MockMyService>();
  }

  public void ShouldDoWork()
  {
    Service.DoWork().ShouldBeTrue();
  }
}
```

---

## Run tests

Execute tests with dotnet test:

```bash
dotnet test
```

Or with Fixie directly:

```bash
dotnet fixie
```

---

## Filter tests

Run specific tests by name:

```bash
# Single test
dotnet fixie --tests FlexValueTests.ShouldCreatePointValue

# All tests in class
dotnet fixie --tests FlexValueTests.*

# Wildcard matching
dotnet fixie --tests *Point*
```

---

## Assertions

Use Shouldly for readable assertions:

```csharp
// Equality
value.ShouldBe(expected);
list.ShouldBeEmpty();
result.ShouldBeNull();

// Boolean
flag.ShouldBeTrue();
flag.ShouldBeFalse();

// Collections
items.ShouldContain(item);
items.Count.ShouldBe(5);

// Types
obj.ShouldBeOfType<FlexNode>();

// Exceptions
Should.Throw<ArgumentException>(() => Method());
```

---

## File naming

Test files follow kebab-case naming matching the source file:

| Source | Test |
|--------|------|
| `flex-value.cs` | `flex-value-tests.cs` |
| `flex-node.cs` | `flex-node-tests.cs` |

---

## Folder structure

Mirror the source folder structure:

```
source/timewarp-flexbox/
  values/
    flex-value.cs
  nodes/
    flex-node.cs

test/timewarp-flexbox-tests/
  values/
    flex-value-tests.cs
  nodes/
    flex-node-tests.cs
```
