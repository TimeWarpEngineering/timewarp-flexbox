# Task 008-implement-measurement-delegates

## Summary
Implement the MeasureFunc and BaselineFunc delegate types for leaf node measurement callbacks, enabling text and custom content sizing.

## Todo List
- [ ] Create Nodes/MeasureFunc.cs delegate definition
- [ ] Create Nodes/BaselineFunc.cs delegate definition
- [ ] Create Nodes/MeasureMode.cs enum (Undefined, Exactly, AtMost)
- [ ] Create Nodes/Size.cs readonly struct for measurement results
- [ ] Document delegate signatures and expected behavior
- [ ] Verify code follows csharp-coding.md standards

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
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
