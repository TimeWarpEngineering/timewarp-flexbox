# TimeWarp.Flexbox Samples

This directory contains example applications demonstrating various features and patterns of TimeWarp.Flexbox.

## Purpose

Samples provide working examples that show:
- How to use Flexbox components
- Common patterns and best practices
- Real-world usage scenarios
- Feature demonstrations

## Planned Examples

### Getting Started
- **hello-world** - Basic Flexbox application with simple component
- **button-click** - Basic event handling and user interaction
- **data-binding** - Parameter binding and data flow

### Component Patterns
- **component-composition** - Building complex UIs from smaller components
- **lifecycle-demo** - Component lifecycle methods in action
- **templated-components** - RenderFragment and templating

### State Management
- **local-state** - Managing component-level state
- **cascading-values** - Sharing data down the component tree
- **timewarp-state-integration** - Global state with TimeWarp.State

### Layout and Styling
- **layouts** - Different layout components and positioning
- **themes** - Applying colors and themes
- **responsive-layout** - Adapting to terminal size

### Advanced Features
- **async-operations** - Async lifecycle methods and data loading
- **form-handling** - Input validation and form submission
- **navigation** - Multi-window navigation patterns

## Running Examples

Most examples can be run directly as .NET 10 file-based apps:

```bash
# Make executable (Linux/macOS)
chmod +x samples/hello-world/app.cs

# Run directly
./samples/hello-world/app.cs

# Or run with dotnet
dotnet run samples/hello-world/app.cs
```

For project-based examples:

```bash
cd samples/ExampleName
dotnet run
```

## Example Structure

Each example typically includes:

### Single-File Examples
```
samples/
└── hello-world/
    ├── app.cs              # Main application file
    └── readme.md           # Documentation
```

### Project-Based Examples
```
samples/
└── AdvancedExample/
    ├── AdvancedExample.csproj
    ├── Program.cs
    ├── Components/
    │   └── MyComponent.Flexbox
    └── readme.md
```

## Example Guidelines

When creating examples:

1. **Focus** - Demonstrate one concept clearly
2. **Comments** - Explain key concepts inline
3. **Minimal** - Keep code as simple as possible
4. **Working** - Must compile and run successfully
5. **Documented** - Include readme explaining what it demonstrates

## Learning Path

Recommended order for learning Flexbox:

1. Start with `hello-world` - Understand basic structure
2. Try `button-click` - Learn event handling
3. Explore `data-binding` - Master parameter passing
4. Study `component-composition` - Build complex UIs
5. Dive into `local-state` - Manage application state
6. Advanced topics - Async, forms, navigation

## Contributing Examples

New examples are welcome! When adding an example:

1. Create folder in `samples/`
2. Follow naming conventions (kebab-case)
3. Include `readme.md` with:
   - What it demonstrates
   - Key concepts
   - How to run it
   - Expected output
4. Ensure code follows [standards](../documentation/developer/standards/)
5. Test that it runs successfully

## Integration with MCP Server

Future: May include `examples.json` manifest for dynamic discovery by AI assistants via MCP (Model Context Protocol) server, similar to TimeWarp.Nuru.

## Related Documentation

- [Guides](../documentation/developer/guides/) - Tutorial documentation
- [Design](../documentation/developer/design/) - Architectural patterns
- [Reference](../documentation/developer/reference/) - API documentation
