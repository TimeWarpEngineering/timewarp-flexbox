# Reference Documentation

This section documents the **actual implementation** of TimeWarp.Flexbox as it exists in source code.

## Purpose

Reference documentation describes **what is** - the current state of the codebase. These documents are:

- **Descriptive**: They explain how things work as implemented
- **Generated from source**: Must reflect actual code, not design intentions
- **Up-to-date**: Updated whenever implementation changes
- **Accurate**: No aspirational or planned features

## Critical Rule

**Reference Writers MUST NOT read Design documents.**

This prevents the circular reasoning trap where "what is" documentation is contaminated by "what should be" thinking. Reference docs must be derived exclusively from:

- Source code analysis
- Running the actual implementation
- Test behavior observation
- API exploration

## What Belongs Here

### API Documentation
- Public classes, methods, and properties
- Method signatures and return types
- Parameter descriptions
- Usage examples from actual code

### Current Features
- Implemented functionality
- Known limitations
- Actual behavior (even if it differs from design)
- Breaking changes between versions

### Implementation Details
- How algorithms work in practice
- Performance characteristics
- Memory usage patterns
- Threading behavior

### Examples
- Working code samples
- Integration examples
- Common patterns in use
- Migration guides

## What Does NOT Belong Here

- Design rationale (belongs in [Design](../design/))
- Future plans (belongs in [Roadmap](../roadmap/))
- Tutorials (belong in [Guides](../guides/))
- Conventions (belong in [Standards](../standards/))

## Documentation Structure

### API Reference
Auto-generated documentation from XML comments in source code:
- Class documentation
- Method documentation
- Property documentation
- Event documentation

### Feature Documentation
Human-written documentation of implemented features:
- Component system capabilities
- Rendering behavior
- State management features
- Event handling

### Examples Repository
Real, working examples that demonstrate features:
- Minimal reproductions
- Common scenarios
- Integration patterns
- Performance optimizations

## Generation Process

Reference documentation follows this workflow:

1. **Source Analysis**: Extract information from actual source code
2. **Behavior Testing**: Verify behavior through tests
3. **Documentation Writing**: Describe what was observed
4. **Example Creation**: Build working code samples
5. **Verification**: Ensure docs match reality

## Keeping Reference Accurate

### When Code Changes
1. Update tests to verify new behavior
2. Regenerate API documentation
3. Update feature documentation
4. Update or add examples
5. Note breaking changes

### Quality Checks
- Does documentation match source code?
- Do examples actually compile and run?
- Are limitations accurately described?
- Is performance data based on measurements?

## Key Reference Documents

### Component API
- [ComponentBase API](./component-base-api.md) - Base class reference
- [Parameter Attributes](./parameter-attributes.md) - Parameter binding attributes
- [Lifecycle Methods](./lifecycle-methods.md) - Available lifecycle hooks

### Rendering API
- [Renderer API](./renderer-api.md) - Core rendering engine
- [RenderTree Structure](./render-tree-structure.md) - Internal tree representation
- [Render Fragment](./render-fragment.md) - Template rendering

### Built-in Components
- [Primitive Components](./primitive-components.md) - Terminal.Gui wrappers
- [Layout Components](./layout-components.md) - Structural components
- [Utility Components](./utility-components.md) - Helper components

### State Management
- [Component State](./component-state.md) - Local state APIs
- [Cascading Values](./cascading-values.md) - Value propagation
- [TimeWarp.State Integration](./timewarp-state-integration.md) - Global state APIs

## Documentation Standards

### Code Examples
All code examples must:
- Compile without errors
- Run without exceptions
- Use actual Flexbox APIs (not mock or pseudo-code)
- Include necessary using statements
- Be tested as part of CI/CD

### API Signatures
Show complete, accurate signatures:
```csharp
// Good - actual signature
public abstract class ComponentBase : IComponent, IDisposable
{
    protected virtual Task OnInitializedAsync() => Task.CompletedTask;
}

// Bad - simplified or aspirational
public class Component
{
    void OnInitialized(); // Missing visibility, return type, async
}
```

### Behavioral Description
Describe actual behavior, including edge cases:
```csharp
// Good
// Returns null if no value is provided by an ancestor.
// Throws InvalidOperationException if multiple ancestors provide the value.

// Bad
// Gets the cascaded value.
```

## Version Tracking

Reference documentation is versioned alongside the codebase:
- Major version: Breaking API changes
- Minor version: New features added
- Patch version: Bug fixes

Each version has corresponding reference documentation that accurately reflects that version's implementation.

## Related Sections

- See [Design](../design/) for architectural intent (don't use for reference writing!)
- See [Guides](../guides/) for practical how-to documentation
- See [Roadmap](../roadmap/) for upcoming features (not yet in reference)
