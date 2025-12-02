# Task 008-implement-measurement-delegates

## Summary
Implement the MeasureFunc and BaselineFunc delegate types for leaf node measurement callbacks, enabling text and custom content sizing.

## Todo List
- [x] Create nodes/measure-func.cs delegate definition
- [x] Create nodes/baseline-func.cs delegate definition
- [x] Create nodes/measure-mode.cs enum (Undefined, Exactly, AtMost)
- [x] Create nodes/size.cs readonly struct for measurement results
- [x] Document delegate signatures and expected behavior
- [x] Verify code follows csharp-coding.md standards

## Notes
Measurement delegates allow leaf nodes to report their intrinsic size:

```csharp
// Returns the measured size of content given constraints
public delegate Size MeasureFunc(
  FlexNode node,
  float width,
  MeasureMode widthMode,
  float height,
  MeasureMode heightMode
);

// Returns the baseline offset from top of node
public delegate float BaselineFunc(
  FlexNode node,
  float width,
  float height
);

public readonly struct Size
{
  public float Width { get; }
  public float Height { get; }
  
  public Size(float width, float height);
}

public enum MeasureMode
{
  Undefined,  // No constraint
  Exactly,    // Must be exactly this size
  AtMost      // Can be at most this size
}
```

MeasureMode mirrors CSS sizing behavior:
- Undefined: Content can be any size
- Exactly: Fixed size constraint
- AtMost: Maximum size constraint (content can be smaller)

## Results
- Created nodes/measure-mode.cs enum: Undefined, Exactly, AtMost
- Created nodes/size.cs readonly struct with Width, Height, Zero, and IEquatable
- Created nodes/measure-func.cs delegate with full XML documentation
- Created nodes/baseline-func.cs delegate with full XML documentation
- Created nodes/flex-node.cs as partial class stub (required for delegate compilation)
- Removed .gitkeep from nodes folder
- Build verified: 0 warnings, 0 errors
