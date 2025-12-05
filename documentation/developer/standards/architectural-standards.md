# Architectural Standards

Rules for project structure and design patterns.

---

## Dependency injection

```csharp
// Constructor injection for required dependencies
public class Renderer
{
  private readonly IComponentFactory Factory;
  private readonly ILogger<Renderer> Logger;

  public Renderer(IComponentFactory factory, ILogger<Renderer> logger)
  {
    Factory = factory ?? throw new ArgumentNullException(nameof(factory));
    Logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }
}

// Register in DI container
services.AddSingleton<IRenderer, Renderer>();
```

---

## Immutability

```csharp
// Prefer immutable types with constructor validation
public record ComponentState(string Name, bool IsActive);

// Init-only properties for partial immutability
public class Component
{
  public string Name { get; init; } = null!;
}
```

---

## Separation of concerns

- Components handle rendering and user interaction
- Services handle business logic and data access
- Models represent data structures
- Keep components thin; push logic to services
