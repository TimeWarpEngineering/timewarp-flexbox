# Roadmap

Planned features, improvements, and changes for TimeWarp.Flexbox.

## Purpose

The roadmap documents **what will be** - future development plans, feature priorities, and the path from current implementation to design goals. Roadmap items are:

- **Forward-looking**: Describe what's planned but not yet implemented
- **Prioritized**: Ordered by importance and dependencies
- **Traceable**: Link to design docs (the goal) and reference docs (current state)
- **Flexible**: Can change based on feedback and constraints

## Roadmap Structure

### Phases

#### Phase 0: Foundation (Current)
Core architecture and basic functionality needed for any Flexbox application.

**Status**: In Progress
**Target**: v0.1.0

**Goals:**
- Establish project structure
- Define ubiquitous language
- Document design principles
- Create documentation framework

#### Phase 1: Core Components
Basic component system with lifecycle and rendering.

**Status**: Planned
**Target**: v0.2.0

**Goals:**
- ComponentBase implementation
- Basic lifecycle methods (OnInitialized, OnParametersSet, Dispose)
- Simple rendering to Terminal.Gui
- Parameter support

**Deliverables:**
- [ ] ComponentBase class
- [ ] Renderer infrastructure
- [ ] Basic render tree
- [ ] Parameter attribute
- [ ] Simple examples

#### Phase 2: Declarative Syntax
Razor component support for declarative UI.

**Status**: Planned
**Target**: v0.3.0

**Goals:**
- Razor compiler integration
- HTML-like syntax for components
- Code-behind support
- Build tooling

**Deliverables:**
- [ ] Razor component compilation
- [ ] @code blocks
- [ ] @using directives
- [ ] Component templates
- [ ] Project templates

#### Phase 3: Event Handling
Complete event system with callbacks and handlers.

**Status**: Planned
**Target**: v0.4.0

**Goals:**
- EventCallback implementation
- @onclick and event directives
- Event bubbling
- Async event handlers

**Deliverables:**
- [ ] EventCallback<T> types
- [ ] Event directive support
- [ ] Terminal.Gui event bridging
- [ ] Event testing framework

#### Phase 4: Advanced Rendering
Optimized rendering with diffing and batching.

**Status**: Planned
**Target**: v0.5.0

**Goals:**
- Virtual DOM implementation
- Efficient diffing algorithm
- Render batching
- Performance optimization

**Deliverables:**
- [ ] Virtual DOM structure
- [ ] Diff algorithm
- [ ] Batch renderer
- [ ] Performance benchmarks

#### Phase 5: State Integration
TimeWarp.State integration for global state management.

**Status**: Planned
**Target**: v0.6.0

**Goals:**
- Store integration
- Action dispatching from components
- State subscriptions
- Effect handling

**Deliverables:**
- [ ] StateComponent base class
- [ ] Store provider component
- [ ] State binding directives
- [ ] DevTools integration

#### Phase 6: Component Library
Rich set of built-in components wrapping Terminal.Gui.

**Status**: Planned
**Target**: v0.7.0

**Goals:**
- Complete primitive components
- Layout components
- Common patterns
- Documentation and examples

**Deliverables:**
- [ ] All Terminal.Gui controls wrapped
- [ ] Layout primitives
- [ ] Common compositions
- [ ] Component gallery

#### Phase 7: Developer Experience
Tooling and diagnostics for productive development.

**Status**: Planned
**Target**: v0.8.0

**Goals:**
- Hot reload support
- Component inspector
- Performance profiler
- Error diagnostics

**Deliverables:**
- [ ] Hot reload implementation
- [ ] Debug visualizers
- [ ] Performance tools
- [ ] Error messages

#### Phase 8: Production Ready
Polish, performance, and production-readiness.

**Status**: Planned
**Target**: v1.0.0

**Goals:**
- Performance optimization
- Comprehensive testing
- Complete documentation
- Production deployment guides

**Deliverables:**
- [ ] Performance benchmarks
- [ ] 90%+ test coverage
- [ ] Complete API documentation
- [ ] Production best practices

## Feature Backlog

Features being considered but not yet scheduled:

### High Priority
- **Data binding expressions**: @bind syntax for two-way binding
- **Cascading parameters**: Automatic value propagation
- **Templated components**: RenderFragment parameters
- **Form components**: Input validation and submission
- **Navigation system**: Component-based routing

### Medium Priority
- **Animation support**: Smooth transitions
- **Accessibility features**: Screen reader support, keyboard navigation
- **Theme system**: Pluggable themes and styling
- **Localization**: Multi-language support
- **Testing utilities**: Component test helpers

### Low Priority
- **Visual designer**: Drag-and-drop UI builder
- **Code generation**: Scaffolding tools
- **REPL integration**: Interactive component development
- **Remote rendering**: Client-server architecture

## Breaking Changes

Planned breaking changes with migration paths:

### v0.x → v1.0
- API stabilization
- Namespace reorganization
- Component naming conventions

**Migration Strategy**: Provide automated migration tool and detailed guide.

### Future Considerations
- Rendering engine abstractions (support platforms beyond Terminal.Gui)
- Cross-platform layout engine

## Design Gaps

Areas where current implementation differs from design, with plans to close gaps:

### Gap: Async Lifecycle
**Design**: All lifecycle methods should have async equivalents
**Current**: Basic synchronous methods only
**Planned**: Phase 1
**Effort**: Medium

### Gap: Parameter Validation
**Design**: Compile-time parameter type checking
**Current**: Not implemented
**Planned**: Phase 2
**Effort**: High

### Gap: Render Optimization
**Design**: Minimal Terminal.Gui calls via diffing
**Current**: Not implemented
**Planned**: Phase 4
**Effort**: High

## Dependencies and Blockers

### External Dependencies
- **Terminal.Gui**: Following their v2 development
- **Roslyn**: Razor compilation integration
- **TimeWarp.State**: v6+ for state management

### Internal Blockers
- Component system must be stable before Razor integration
- Rendering pipeline must be complete before optimization
- Core features before developer tooling

## Community Input

Areas where community feedback is especially valuable:

- **API design**: Component lifecycle and parameter syntax
- **Use cases**: Real-world scenarios and requirements
- **Terminal.Gui integration**: Control priorities and patterns
- **Documentation**: Clarity and completeness

## Versioning Strategy

Following Semantic Versioning (SemVer):

- **Major (x.0.0)**: Breaking API changes
- **Minor (0.x.0)**: New features, backwards compatible
- **Patch (0.0.x)**: Bug fixes, no API changes

Pre-1.0 versions may include breaking changes in minor versions.

## Release Cadence

- **Alpha**: Monthly releases during Phase 1-3
- **Beta**: Bi-weekly releases during Phase 4-6
- **RC**: Weekly releases during Phase 7-8
- **Stable**: Quarterly feature releases after v1.0

## How to Contribute

See [Contributing Guide](../../CONTRIBUTING.md) for:
- Proposing new features
- Claiming roadmap items
- Submitting implementations
- Review process

## Related Sections

- See [Design](../design/) for the vision being implemented
- See [Reference](../reference/) for current implementation status
- See [Guides](../guides/) for how to use available features
