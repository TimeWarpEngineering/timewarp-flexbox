# XML Documentation

Rules for documenting public APIs with XML comments.

---

## Require for public APIs

```csharp
/// <summary>
/// Represents a base class for all Flexbox components.
/// </summary>
/// <remarks>
/// This class provides lifecycle methods and rendering infrastructure
/// for component-based TUI applications.
/// </remarks>
public abstract class ComponentBase
{
  /// <summary>
  /// Initializes the component.
  /// </summary>
  /// <returns>A task representing the asynchronous operation.</returns>
  /// <remarks>
  /// Called once when the component is first created. Override this method
  /// to perform one-time initialization logic.
  /// </remarks>
  protected virtual Task OnInitializedAsync() => Task.CompletedTask;
}
```

---

## Content guidelines

- Use present tense: "Renders the component" (not "Will render")
- Be specific: Include edge cases and exceptions
- No temporal references: Avoid "currently", "now", "at this time"
- Link related members with `<see cref="MemberName"/>`
