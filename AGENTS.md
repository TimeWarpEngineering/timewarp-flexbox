# TimeWarp.Flexbox Agent Guidelines

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