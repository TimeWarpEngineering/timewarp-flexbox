# Code Organization

Rules for organizing code within files and projects.

---

## Using directives

- Prefer global usings in `global-usings.cs`
- Only use file-level usings for static usings
- Place usings inside the namespace, not outside

```csharp
// global-usings.cs - at project root
global using System;
global using System.Threading.Tasks;
global using TimeWarp.Flexbox;
```

---

## File structure

```csharp
// 1. Namespace (flat is preferred)
namespace TimeWarp.Flexbox;

// 2. Static usings inside namespace (if needed)
using static System.Math;

// 3. Class documentation
/// <summary>
/// Represents a button component.
/// </summary>

// 4. Class definition
public class Button : ComponentBase
{
  // 5. Constants
  private const int DefaultTimeout = 5000;

  // 6. Fields (class scope - PascalCase)
  private readonly IRenderer Renderer;
  private int ClickCount;

  // 7. Constructors
  public Button(IRenderer renderer)
  {
    Renderer = renderer;
  }

  // 8. Properties (public, then private)
  public string Text { get; set; } = null!;
  private bool IsPressed { get; set; }

  // 9. Events
  public event EventHandler? Clicked;

  // 10. Public methods
  public void Click() => Clicked?.Invoke(this, EventArgs.Empty);

  // 11. Protected/Internal methods
  protected override void OnInitialized() { }

  // 12. Private methods
  private void UpdateState() { }
}
```

---

## File naming

See [file-naming.md](file-naming.md) for complete details.

- Use kebab-case for all files and folders
- Each class, interface, or enum in its own file
- Nested types are acceptable within parent type's file
