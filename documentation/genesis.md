# TimeWarp.Flexbox Genesis

## Project Origin

This project is a C# clone of Facebook's Yoga layout library, created as part of the TimeWarp ecosystem for building terminal user interfaces.

## Initial Discussion (2025-11-21)

### Context

- **Source Library**: Facebook Yoga - A cross-platform flexbox layout engine
- **Target**: Pure C# implementation with idiomatic .NET design
- **Namespace**: `TimeWarp.Flexbox`
- **Purpose**: Foundation for TimeWarp.TUI, which will be used in TimeWarp.Flexbox for Blazor-like TUI syntax

### Key Design Decisions

#### 1. Pure C# Implementation

- Not a wrapper around native Yoga
- Idiomatic C# with properties, modern language features
- Following W3C Flexbox specification naming conventions

#### 2. Float-Based Positioning

After analyzing Yoga's implementation and discussing OpenTUI's approach:

- **Use floats throughout** for layout calculations (matching Yoga and W3C spec)
- **No integer forcing** at the layout engine level
- Terminal rendering layer (TimeWarp.TUI) will handle float-to-cell conversion
- Optional pixel grid rounding feature like Yoga

#### 3. Architecture Overview

##### Core Components Identified from Yoga:

1. **Node System**: Tree structure with parent-child relationships
2. **Style Properties**: Flexbox properties matching CSS spec
3. **Layout Algorithm**: Core flexbox calculation engine
4. **Value Types**: Support for points, percentages, auto values
5. **Configuration**: Global settings and feature flags
6. **Measurement Callbacks**: For leaf nodes like text

##### Yoga's Structure:

- Written in C++20 with C API wrapper
- Main headers: YGNode, YGConfig, YGEnums, YGValue, YGNodeStyle, YGNodeLayout
- Layout algorithm in `yoga/algorithm/`
- Style system in `yoga/style/`
- Pixel grid rounding utilities for high DPI displays

### Final Architecture Design

```csharp
namespace TimeWarp.Flexbox
{
    public class FlexNode
    {
        // Tree structure
        public FlexNode? Parent { get; internal set; }
        public IList<FlexNode> Children { get; }

        // CSS/W3C compliant properties
        public FlexDirection FlexDirection { get; set; }
        public FlexWrap FlexWrap { get; set; }
        public JustifyContent JustifyContent { get; set; }
        public AlignItems AlignItems { get; set; }
        public AlignContent AlignContent { get; set; }
        public AlignSelf AlignSelf { get; set; }

        // Flex properties
        public float FlexGrow { get; set; }
        public float FlexShrink { get; set; }
        public FlexValue FlexBasis { get; set; }

        // Dimensions
        public FlexValue Width { get; set; }
        public FlexValue Height { get; set; }
        public FlexValue MinWidth { get; set; }
        public FlexValue MinHeight { get; set; }
        public FlexValue MaxWidth { get; set; }
        public FlexValue MaxHeight { get; set; }

        // Spacing
        public EdgeValues<FlexValue> Margin { get; set; }
        public EdgeValues<FlexValue> Padding { get; set; }
        public EdgeValues<float> Border { get; set; }
        public EdgeValues<FlexValue> Position { get; set; }

        // Layout results (all floats)
        public LayoutResult Layout { get; private set; }

        // Callbacks
        public MeasureFunc? MeasureFunc { get; set; }
        public BaselineFunc? BaselineFunc { get; set; }
    }

    public readonly struct FlexValue
    {
        public float Value { get; }
        public Unit Unit { get; }

        // Static factories matching CSS
        public static FlexValue Point(float value);
        public static FlexValue Percent(float value);
        public static readonly FlexValue Auto;
        public static readonly FlexValue Undefined;
    }

    public class LayoutResult
    {
        // All float values for spec compliance
        public float Left { get; internal set; }
        public float Top { get; internal set; }
        public float Width { get; internal set; }
        public float Height { get; internal set; }
    }
}
```

### Design Rationale

1. **W3C Naming Compliance**: Using exact CSS property names for familiarity
2. **Float Precision**: Maintaining accuracy during flexbox calculations
3. **No Rounding in Core**: Terminal cell conversion happens at render time, not layout
4. **Struct for Values**: `FlexValue` as readonly struct for performance
5. **EdgeValues Pattern**: Smart cascade handling for margin/padding/border
6. **Separation of Concerns**: Layout engine remains pure, TUI layer handles terminal specifics

### Next Steps

1. Create project structure
2. Implement core data structures and enums
3. Port the flexbox layout algorithm
4. Add comprehensive unit tests
5. Create TUI-specific extensions

## Technical Notes

### Key Differences from Yoga

- Pure managed C# (no P/Invoke)
- Property-based API instead of Get/Set methods
- .NET collections instead of manual memory management
- Nullable reference types
- Modern C# features (records, pattern matching where appropriate)

### Performance Considerations

- Use structs for value types
- Implement caching similar to Yoga
- Consider object pooling for high-frequency allocations
- Minimize boxing/unboxing
- Use Span<T> where beneficial

### Compatibility Goals

- Maintain algorithmic compatibility with Yoga
- Support same feature set
- Produce identical layout results (within floating-point precision)
- Support same measurement and baseline callbacks
