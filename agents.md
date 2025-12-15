# TimeWarp.Flexbox Agent Guidelines

## Important: Project Objective

**The main objective is to port Facebook's Yoga C++ flexbox library to C#.**

- Keep it as simple as possible: copy the C++ logic and change the syntax to C#
- Do NOT skip anything, do NOT rename anything unless necessary for C# conventions
- Match Yoga's behavior exactly - when in doubt, look at the C++ source

## Reference Implementation

**Facebook Yoga Repository**: `/home/steventcramer/worktrees/github.com/facebook/yoga/main/`

This is the source of truth. When implementing features or fixing bugs, ALWAYS consult the Yoga C++ source first:

- **Algorithm**: `yoga/algorithm/` - Core layout algorithms
  - `CalculateLayout.cpp` - Main layout entry point
  - `FlexDirection.h` - Direction resolution, edge helpers (`flexStartEdge`, `flexEndEdge`, `inlineStartEdge`)
  - `FlexLine.cpp` - Flex line collection and sizing
  - `AbsoluteLayout.cpp` - Absolute positioning logic
  - `PixelGrid.cpp` - Pixel grid rounding
- **Node**: `yoga/node/` - Node and layout result structures
  - `LayoutResults.h` - Layout result storage (positions for all 4 edges)
- **Tests**: `tests/generated/` - Generated test cases from HTML fixtures
  - `YGFlexDirectionTest.cpp` - RTL/LTR direction tests
  - `YGRoundingTest.cpp` - Pixel rounding tests
- **Enums**: `yoga/enums/` - Direction, FlexDirection, Edge, PhysicalEdge

Key patterns from Yoga:
- Positions stored for all 4 physical edges (Left, Top, Right, Bottom)
- `setLayoutPosition(value, flexStartEdge(axis))` - positions set on flex-start edge
- `resolveDirection()` swaps Row/RowReverse for RTL
- `flexStartEdge(RowReverse)` returns Right, not Left

## Build/Test Commands
- Build: `dotnet build`
- Test all: `dotnet test`
- Single test: `dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"`
- Watch mode: `dotnet watch test`
- Format: `dotnet format`
- Package: `dotnet pack -c Release`

## Code Style (see documentation/developer/standards/csharp-coding.md for full details)
- **Indentation**: 2 spaces (NO tabs), LF line endings, Allman bracket style
- **Namespace**: `TimeWarp.Flexbox` - file-scoped declarations
- **Naming**: Class scope = PascalCase for ALL members; Method scope = camelCase
- **Types**: Explicit types (no var unless obvious), targeted type new (`List<int> list = new();`)
- **Structs**: Use readonly structs for value types (`FlexValue`, `LayoutResult`)
- **Float precision**: Use floats throughout for W3C spec compliance - NO integer forcing/rounding
- **Collections**: Prefer IList/IReadOnlyList interfaces, use collection expressions
- **Nullability**: Enable nullable reference types, use `?` for nullable
- **CSS/W3C naming**: Match exact CSS property names (FlexDirection, JustifyContent, AlignItems)
- **Testing**: Match Yoga's test cases for algorithmic compatibility
- **Error handling**: Exceptions for programming errors, return types for expected failures