# Coding Practices

Rules for common coding patterns and practices.

---

## Null handling

Do not use default values like `= string.Empty` for non-nullable properties. Setting default values prevents proper validation during JSON deserialization.

```csharp
// Nullable properties - may contain null
public class Component
{
  public string? Description { get; set; }
}

// Non-nullable properties with validation
public class Command
{
  // Use null! for non-nullable properties that will be validated
  public string Name { get; set; } = null!;

  // DON'T do this - prevents proper validation
  // public string Name { get; set; } = string.Empty;
}

// Response with constructor validation
public sealed class Response
{
  public string Name { get; }

  public Response(string name)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
  }
}
```

Only use `default!` with generic types:

```csharp
public T GenericProperty { get; set; } = default!;
```

---

## Async/await

```csharp
// Always use async/await for asynchronous operations
public async Task RenderAsync()
{
  await PrepareAsync();
  await DrawAsync();
}

// Suffix async methods with 'Async'
public Task InitializeAsync() => Task.CompletedTask;

// Don't mix sync and async
public void Process()  // Bad - blocking async call
{
  DoWorkAsync().Wait();
}
```

---

## Exception handling

```csharp
// Specific exceptions over generic
throw new InvalidOperationException("Component not initialized");  // Good
throw new Exception("Error");  // Bad

// Document exceptions in XML comments
/// <exception cref="InvalidOperationException">
/// Thrown when the component is not initialized.
/// </exception>
public void Render()
{
  if (!Initialized)
    throw new InvalidOperationException("Component not initialized");
}
```

---

## LINQ and collections

```csharp
// Prefer LINQ for clarity
List<Component> activeComponents = components.Where(c => c.IsActive).ToList();

// Use collection expressions (C# 12)
int[] numbers = [1, 2, 3, 4, 5];
```
