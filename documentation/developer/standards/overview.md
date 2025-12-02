# Standards

Enforced conventions, coding standards, and rules for TimeWarp.Flexbox development.

## Purpose

Standards define **how we work** - the rules, conventions, and practices that ensure consistency and quality across the codebase. Standards are:

- **Mandatory**: Not suggestions, but requirements
- **Enforced**: Through analyzers, CI/CD, and code review
- **Consistent**: Applied uniformly across the project
- **Justified**: Each standard has clear reasoning

## Standard Categories

### Code Standards
Rules about how code should be written.

### Architectural Standards
Rules about project structure and design patterns.

### Process Standards
Rules about development workflow and practices.

### Documentation Standards
Rules about writing and maintaining documentation.

## Code Standards

### Naming Conventions

#### Namespaces
- Prefer flat namespaces: `TimeWarp.Flexbox`
- Only introduce hierarchy if absolutely necessary
- Staying in a single namespace is ideal
- Use PascalCase for all segments
- Avoid abbreviations unless widely known (UI, IO, etc.)

#### Classes and Interfaces
```csharp
// PascalCase for classes
public class ComponentBase { }

// PascalCase with 'I' prefix for interfaces
public interface IComponent { }

// Avoid Hungarian notation
public class FlexboxButton { }  // Good
public class clsButton { }    // Bad
```

#### Class Scope - PascalCase
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

#### Local Scope - camelCase
```csharp
// Parameters
public void SetParameter(string parameterName, object parameterValue)
{
    // Local variables
    var localVariable = GetValue();
    int counter = 0;
}
```

#### Type Parameters
```csharp
// Single letter for simple generics
public class Component<T> { }

// Descriptive with 'T' prefix for complex generics
public interface IEventCallback<TEventArgs> { }
```

### Code Organization

#### Using Directives
- Prefer global usings in a `GlobalUsings.cs` file
- Only use file-level usings for static usings
- Place usings inside the namespace, not outside
- Keep files clean of common namespace imports

```csharp
// GlobalUsings.cs - at project root
global using System;
global using System.Threading.Tasks;
global using TimeWarp.Flexbox;
```

#### File Structure
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
    [Parameter]
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

#### File Naming
See [file-naming.md](file-naming.md) for complete details.

- Use kebab-case for all files and folders
- Each class, interface, or enum in its own file
- Nested types are acceptable within parent type's file

Examples: `flex-node.cs`, `global-usings.cs`, `timewarp-flexbox.csproj`

### Coding Practices

#### Null Handling

**DO NOT use default values like `= string.Empty` for non-nullable properties.**

Setting default values prevents proper validation. If JSON deserialization skips a null property, it retains the default value instead of being null, causing validation to pass incorrectly.

```csharp
// Nullable properties - may contain null
public class Component
{
    public string? Description { get; set; }  // Explicitly nullable
}

// Non-nullable properties with validation
public class Command
{
    // Use null! for non-nullable properties that will be validated
    public string Name { get; set; } = null!;

    // DON'T do this - prevents proper validation
    // public string Name { get; set; } = string.Empty;  // BAD
}

// Validated non-nullable property example
public class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
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

**Note**: Only use `default!` with generic types where the actual default value is type-dependent:
```csharp
public T GenericProperty { get; set; } = default!;  // OK for generics
```

See [TimeWarp.Architecture nullability guide](https://github.com/TimeWarpEngineering/timewarp-architecture) for detailed rationale.

#### Async/Await
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
public async Task ProcessAsync()  // Good
{
    await DoWorkAsync();
}

public void Process()  // Bad - blocking async call
{
    DoWorkAsync().Wait();
}
```

#### Exception Handling
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

#### LINQ and Collections
```csharp
// Prefer LINQ for clarity
var activeComponents = components.Where(c => c.IsActive).ToList();  // Good
var activeComponents = new List<Component>();  // Verbose
foreach (var c in components)
    if (c.IsActive)
        activeComponents.Add(c);

// Use collection expressions (C# 12)
int[] numbers = [1, 2, 3, 4, 5];
```

### XML Documentation

#### Required for Public APIs
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

#### Content Guidelines
- Use present tense: "Renders the component" (not "Will render")
- Be specific: Include edge cases and exceptions
- No temporal references: Avoid "currently", "now", "at this time"
- Link related members with `<see cref="MemberName"/>`

## Architectural Standards

### Dependency Injection
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

### Immutability
```csharp
// Prefer immutable types with constructor validation
public record ComponentState(string Name, bool IsActive);

// Init-only properties for partial immutability
public class Component
{
    public string Name { get; init; } = null!;
}
```

### Separation of Concerns
- Components handle rendering and user interaction
- Services handle business logic and data access
- Models represent data structures
- Keep components thin; push logic to services

## Process Standards

### Git Workflow
- Never squash or rebase
- Always use `--head` flag with `gh pr create`
- Write descriptive commit messages
- Reference issues in commit messages: `Fixes #123`

### Commit Messages
```
Add component lifecycle methods

Implement OnInitialized, OnParametersSet, and OnAfterRender
lifecycle hooks for ComponentBase.

Fixes #42

🤖 Generated with [Claude Code](https://claude.com/claude-code)

Co-Authored-By: Claude <noreply@anthropic.com>
```

### Branch Naming
```
UserName/YYYY-MM-DD/feature-description
Cramer/2025-10-14/component-lifecycle
```

### Code Review
- All code requires review before merge
- Reviewers check standards compliance
- Address all comments before merge
- No force-push after review starts

## Documentation Standards

### Temporal References
NEVER use temporal language in code or documentation:
- ❌ "currently", "now", "at this time", "for now"
- ✅ Describe what IS, not when it was written
- ✅ Use versions for historical context: "As of v0.2.0..."

### File Naming
See [file-naming.md](file-naming.md) for complete details.

- Use kebab-case for all files and folders
- Examples: `readme.md`, `global-usings.cs`, `timewarp-flexbox.csproj`
- Exception: MSBuild files require specific casing (`Directory.Build.props`, `Directory.Packages.props`)

### Markdown Structure
```markdown
# Title (H1 - Once per document)

Brief description.

## Section (H2)
Content...

### Subsection (H3)
Content...

#### Details (H4)
Content...
```

### Code Blocks
````markdown
```csharp
// Always specify language
public class Example { }
```
````

## Enforcement

### Analyzers
- StyleCop for code style
- SonarAnalyzer for code quality
- Custom analyzers for Flexbox-specific rules

### CI/CD Checks
- Build must succeed
- All tests must pass
- Code coverage threshold (80%)
- Analyzer warnings as errors

### Editor Configuration
`.editorconfig` enforces:
- Indentation (2 spaces)
- Line endings (LF)
- Encoding (UTF-8)
- Trailing whitespace removal

## Exceptions

Standards may be violated when:
1. Performance critical path requires optimization
2. Interoperating with third-party code
3. Maintaining backward compatibility

**All exceptions must be:**
- Documented with `#pragma warning disable` and justification
- Reviewed during code review
- Tracked as technical debt if temporary

## Related Sections

- See [Design](../design/) for architectural principles
- See [Guides](../guides/) for practical implementation
- See [Reference](../reference/) for current API
