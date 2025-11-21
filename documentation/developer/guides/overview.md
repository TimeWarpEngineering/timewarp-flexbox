# Guides

Practical, step-by-step instructions for common tasks and patterns in TimeWarp.Flexbox development.

## Purpose

Guides provide **how-to instructions** for developers working with Flexbox. They are:

- **Task-oriented**: Focus on accomplishing specific goals
- **Step-by-step**: Provide clear, ordered instructions
- **Practical**: Include working code examples
- **Current**: Based on the actual implementation (reference docs), not design intentions

## How Guides Differ

| Section   | Question           | Example                              |
| --------- | ------------------ | ------------------------------------ |
| Design    | Why this way?      | Why use virtual DOM diffing?         |
| Reference | What exists?       | What methods does ComponentBase have?|
| Guides    | How do I do this?  | How do I build a custom component?   |
| Roadmap   | What's coming?     | What features are planned?           |
| Standards | What are the rules?| What naming conventions are enforced?|

## Getting Started Guides

### [Building Your First Application](./building-first-app.md)
Create a simple Flexbox application from scratch.
- Project setup
- Basic component structure
- Running the application
- Basic navigation

### [Understanding Components](./understanding-components.md)
Learn the fundamentals of Flexbox components.
- Component lifecycle
- Parameters and data binding
- Event handling
- Component composition

### [Working with State](./working-with-state.md)
Manage application state effectively.
- Local component state
- Cascading values
- TimeWarp.State integration
- State best practices

## Component Development

### [Creating Custom Components](./creating-custom-components.md)
Build reusable components for your applications.
- Razor component syntax
- Code-only components
- Parameter definition
- Event callbacks
- Templated components

### [Wrapping Terminal.Gui Controls](./wrapping-terminal-gui-controls.md)
Integrate Terminal.Gui controls into Flexbox.
- Control lifecycle management
- Property mapping
- Event bridging
- Layout integration

### [Component Testing](./component-testing.md)
Write tests for your components.
- Unit testing components
- Testing lifecycle methods
- Mocking dependencies
- Integration testing

## Layout and Styling

### [Working with Layouts](./working-with-layouts.md)
Understand Flexbox's layout system.
- Layout components
- Positioning strategies
- Responsive layouts
- Custom layouts

### [Terminal Colors and Themes](./colors-and-themes.md)
Apply colors and themes to your application.
- Color schemes
- Theme switching
- Custom themes
- Accessibility considerations

## State Management

### [Local State Management](./local-state-management.md)
Manage state within components.
- State initialization
- State updates
- Derived state
- State performance

### [Cascading Values](./cascading-values.md)
Share data across component trees.
- Providing values
- Consuming values
- Multiple cascading values
- Type-safe cascading

### [TimeWarp.State Integration](./timewarp-state-integration.md)
Use TimeWarp.State for global state management.
- Store setup
- Dispatching actions
- Subscribing to state
- Effect handling
- State debugging

## Advanced Topics

### [Render Optimization](./render-optimization.md)
Improve rendering performance.
- Understanding render cycles
- Preventing unnecessary renders
- Render batching
- Performance profiling

### [Async Patterns](./async-patterns.md)
Handle asynchronous operations.
- Async lifecycle methods
- Loading states
- Error handling
- Cancellation tokens

### [Interop with Terminal.Gui](./terminal-gui-interop.md)
Work directly with Terminal.Gui when needed.
- Accessing underlying controls
- Custom rendering
- Event handling
- Integration patterns

### [Debugging Techniques](./debugging-techniques.md)
Debug Flexbox applications effectively.
- Component inspection
- Render tree visualization
- State inspection
- Performance profiling
- Common issues and solutions

## Project Setup

### [Project Structure](./project-structure.md)
Organize your Flexbox projects.
- Recommended folder structure
- Component organization
- Shared code
- Testing structure

### [Build and Deployment](./build-and-deployment.md)
Build and deploy Flexbox applications.
- Build configuration
- Publishing options
- Platform-specific considerations
- Distribution strategies

### [CI/CD Integration](./ci-cd-integration.md)
Set up continuous integration and deployment.
- GitHub Actions
- Azure DevOps
- Docker containers
- Automated testing

## Migration Guides

### [From Terminal.Gui](./migrating-from-terminal-gui.md)
Migrate existing Terminal.Gui applications to Flexbox.
- Conceptual mapping
- Code conversion patterns
- Migration strategies
- Common pitfalls

### [From Console Applications](./migrating-from-console.md)
Convert console applications to Flexbox.
- Adding UI to console apps
- Gradual migration
- Preserving existing logic

## Contributing

### [Writing New Guides](./writing-guides.md)
Contribute guides to the Flexbox documentation.
- Guide structure
- Writing style
- Code examples
- Testing guides

## Guide Template

Each guide follows this structure:

```markdown
# Guide Title

Brief description of what this guide covers.

## Prerequisites
- Required knowledge
- Required tools
- Required packages

## Overview
High-level explanation of the task.

## Step-by-Step Instructions

### Step 1: First Task
Detailed instructions...

### Step 2: Next Task
Detailed instructions...

## Complete Example
Full working example with all code.

## Common Issues
- Issue 1: Solution
- Issue 2: Solution

## Next Steps
- Related guides
- Further reading

## Related Documentation
- Reference docs
- Design docs (for context only)
```

## Related Sections

- See [Reference](../reference/) for API documentation
- See [Design](../design/) for architectural context (background only)
- See [Standards](../standards/) for coding conventions
- See [Roadmap](../roadmap/) for upcoming features
