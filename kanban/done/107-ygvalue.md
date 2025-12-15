# Task 107 - YGValue

## Summary

Port the YGValue type (value with unit: point, percent, auto, undefined) from C++ to C#. This is a Level 2 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                    | Lines |
| ---------- | ----------------------- | ----- |
| C++ Header | `yoga/YGValue.h`        | 87    |
| C++ Source | `yoga/YGValue.cpp`      | 20    |
| C++ Test   | `tests/YGValueTest.cpp` | 23    |

## Target Files

| Type      | Path                                             |
| --------- | ------------------------------------------------ |
| C# Source | `source/timewarp-flexbox/yg-value.cs`            |
| C# Test   | `tests/timewarp-flexbox-tests/yg-value-tests.cs` |

## Dependencies

- Task 105: YGEnums (for YGUnit enum)

## Todo List

- [ ] Port `YGValue.h` to C# readonly struct
- [ ] Port `YGValue.cpp` equality implementation
- [ ] Port `YGValueTest.cpp` to xUnit tests
- [ ] Ensure all tests pass

## Acceptance Criteria

- [ ] All 23 lines of test logic ported
- [ ] Point values working
- [ ] Percent values working
- [ ] Auto value working
- [ ] Undefined value working
- [ ] Equality comparison working
- [ ] All tests pass with identical behavior to C++

## Notes

Key C++ constructs to convert:

- `struct YGValue` -> `readonly struct YGValue`
- `YGUnit` enum values (Point, Percent, Auto, Undefined)

```csharp
public readonly struct YGValue : IEquatable<YGValue>
{
    public float Value { get; }
    public YGUnit Unit { get; }

    public static YGValue Auto => new(0, YGUnit.Auto);
    public static YGValue Undefined => new(float.NaN, YGUnit.Undefined);

    public static YGValue Point(float value) => new(value, YGUnit.Point);
    public static YGValue Percent(float value) => new(value, YGUnit.Percent);
}
```
