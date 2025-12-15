# Task 053a-add-static-enum-value

## Summary
Add `Static` value to the PositionType enum. This is the first step in implementing position:static support. Static must be the first enum value (index 0) to match Yoga's ordering.

**Parent Task:** 053-add-position-static

## Todo List
- [ ] Add `Static = 0` to PositionType enum before Relative
- [ ] Add XML documentation for Static value
- [ ] Update Relative to `= 1` and Absolute to `= 2` for clarity
- [ ] Add basic unit test verifying enum values match expected indices
- [ ] Verify existing tests still pass (no behavior change yet)

## Notes
### Yoga Reference (YGEnums.h)
```cpp
YG_ENUM_DECL(
    YGPositionType,
    YGPositionTypeStatic,   // index 0
    YGPositionTypeRelative, // index 1
    YGPositionTypeAbsolute) // index 2
```

### Current TimeWarp Code (position-type.cs)
```csharp
public enum PositionType
{
    Relative,  // Currently index 0
    Absolute   // Currently index 1
}
```

### Target Code
```csharp
public enum PositionType
{
    /// <summary>
    /// Static positioning - element ignores inset properties (left, top, right, bottom).
    /// This is the CSS default behavior.
    /// </summary>
    Static = 0,
    
    /// <summary>
    /// Relative positioning - element can be offset from its normal position using insets.
    /// </summary>
    Relative = 1,
    
    /// <summary>
    /// Absolute positioning - element is removed from normal flow and positioned relative to containing block.
    /// </summary>
    Absolute = 2
}
```

### Important
This change alone should not affect behavior since the default PositionType in FlexNode is likely `Relative`. The behavior change comes in subtask 053b/053c.
