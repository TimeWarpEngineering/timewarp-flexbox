# Ubiquitous Language

The canonical terminology for TimeWarp.Flexbox, inspired by Domain-Driven Design principles. These terms define the shared vocabulary used consistently across design documents, source code, and documentation.

## Core Concepts

### Component
A reusable, declarative UI element that encapsulates rendering logic and state. Components are the fundamental building blocks of Flexbox applications.

**Usage:**
- Base class for all UI elements
- Contains lifecycle methods
- Can be composed into larger components
- Analogous to Blazor's `ComponentBase`

### Renderer
The engine responsible for converting component hierarchies into Terminal.Gui visual elements and managing the rendering pipeline.

**Usage:**
- Processes component trees
- Manages virtual DOM diffing
- Coordinates updates to Terminal.Gui controls
- Handles render batching

### Render Tree
The hierarchical structure of components that represents the application's UI at any given moment.

**Usage:**
- Virtual representation of UI
- Used for diffing and updates
- Maintains component relationships
- Drives actual Terminal.Gui rendering

### Parameter
A property on a component marked with `[Parameter]` that allows parent components to pass data down the component hierarchy.

**Usage:**
- Decorated with `[Parameter]` attribute
- Enables data flow from parent to child
- Can be bound to expressions
- Similar to Blazor parameters

### Event Callback
A mechanism for child components to communicate events up to parent components.

**Usage:**
- Typed delegate for event handling
- Enables upward communication
- Supports async operations
- Uses `EventCallback<T>` type

### Lifecycle
The sequence of method calls that occur during a component's existence, from initialization through disposal.

**Lifecycle Methods:**
- `OnInitialized` / `OnInitializedAsync`
- `OnParametersSet` / `OnParametersSetAsync`
- `OnAfterRender` / `OnAfterRenderAsync`
- `Dispose` / `DisposeAsync`

### State
The data that drives component rendering and behavior. State changes trigger re-renders.

**Types:**
- **Component State**: Local to a single component
- **Cascading State**: Provided down the component tree
- **Application State**: Global state managed by TimeWarp.State

### Binding
The connection between component parameters/properties and data sources, enabling reactive updates.

**Usage:**
- Two-way binding with `@bind`
- One-way binding with parameter assignment
- Event binding with `@on*` syntax
- Similar to Blazor binding syntax

## Rendering Concepts

### Virtual DOM
An in-memory representation of the UI used to calculate minimal changes needed for updates.

**Purpose:**
- Enables efficient diffing
- Minimizes Terminal.Gui operations
- Improves performance
- Abstracts platform details

### Diffing
The process of comparing two render trees to determine what changed and needs updating.

**Process:**
1. Generate new render tree
2. Compare with previous tree
3. Calculate minimal change set
4. Apply changes to Terminal.Gui

### Render Batch
A collection of component updates processed together for efficiency.

**Benefits:**
- Reduces redundant renders
- Improves performance
- Ensures consistent UI state
- Batches Terminal.Gui updates

## Terminal.Gui Integration

### Control
A Terminal.Gui visual element (View, Window, Button, etc.) that represents actual rendered UI.

**Usage:**
- Platform-specific rendering target
- Managed by Renderer
- Wrapped by Components
- Direct Terminal.Gui API

### Layout
The system for positioning and sizing controls within the terminal space.

**Concepts:**
- Computed layout (Pos.Left, Dim.Fill)
- Absolute positioning
- Relative positioning
- Constraint-based sizing

## State Management

### Store
A centralized container for application state, provided by TimeWarp.State integration.

**Features:**
- Immutable state updates
- Action-based mutations
- Effect handling
- State subscriptions

### Action
An immutable object describing a state change intention.

**Usage:**
- Dispatched to Store
- Processed by Handlers
- Triggers state updates
- Enables time-travel debugging

### Effect
A side effect triggered by state changes, such as API calls or navigation.

**Usage:**
- Responds to Actions
- Performs async operations
- Can dispatch additional Actions
- Integrates with external systems

## Component Patterns

### Razor Component
A component defined using `.razor` syntax with HTML-like markup and C# code.

**Features:**
- Declarative UI syntax
- Code-behind support
- Parameter binding
- Event handling

### Code-Only Component
A component defined entirely in C# without Razor syntax.

**Usage:**
- Programmatic rendering
- Complex logic
- Reusable primitives
- Performance-critical components

### Layout Component
A component that provides structure and manages child component placement.

**Examples:**
- WindowLayout
- PanelLayout
- StackLayout
- GridLayout

### Primitive Component
A low-level component that directly wraps a Terminal.Gui control.

**Examples:**
- Button
- Label
- TextField
- ListView

## Terminology Guidelines

### DO Use These Terms
- Component (not "control", "widget", "element")
- Renderer (not "engine", "framework")
- Parameter (not "prop", "attribute")
- Render Tree (not "component tree", "UI tree")

### DON'T Use These Terms
- "Control" for Flexbox components (use Component; Control refers to Terminal.Gui)
- "Page" (use Component or View)
- "Template" (use Component or RenderFragment)
- "Directive" (use Attribute or Component)

## Cross-Framework Terminology

| Flexbox          | Blazor         | Terminal.Gui | React       |
| -------------- | -------------- | ------------ | ----------- |
| Component      | Component      | View         | Component   |
| Renderer       | Renderer       | Application  | Reconciler  |
| Parameter      | Parameter      | Property     | Prop        |
| Render Tree    | Render Tree    | View Tree    | Virtual DOM |
| Event Callback | EventCallback  | Event        | Callback    |
| Control        | Element        | View         | Element     |

## Evolution of Terms

As the project evolves, terms may be refined. When changing terminology:

1. Update this document first
2. Update design documents
3. Refactor source code
4. Regenerate reference documentation
5. Update guides and examples

**Version History:**
- v0.1.0 - Initial ubiquitous language based on genesis document
