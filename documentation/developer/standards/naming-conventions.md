# Naming Conventions

Rules for naming identifiers in C# code.

---

## Namespaces

- Prefer flat namespaces: `TimeWarp.Flexbox`
- Only introduce hierarchy if absolutely necessary
- Use PascalCase for all segments
- Avoid abbreviations unless widely known (UI, IO, etc.)

---

## Classes and interfaces

```csharp
// PascalCase for classes
public class ComponentBase { }

// PascalCase with 'I' prefix for interfaces
public interface IComponent { }

// Avoid Hungarian notation
public class FlexboxButton { }  // Good
public class clsButton { }      // Bad
```

---

## Class scope

Use PascalCase for all class-level members:

```csharp
// All methods
public void RenderComponent() { }
private void UpdateState() { }

// All properties
public string ComponentName { get; set; }
private bool IsPressed { get; set; }

// All fields
private readonly IRenderer Renderer;
private int RenderCount;
public const int MaxComponentDepth = 100;
```

---

## Local scope

Use camelCase for parameters and local variables:

```csharp
public void SetParameter(string parameterName, object parameterValue)
{
  int counter = 0;
  string localValue = GetValue();
}
```

---

## Type parameters

```csharp
// Single letter for simple generics
public class Component<T> { }

// Descriptive with 'T' prefix for complex generics
public interface IEventCallback<TEventArgs> { }
```
