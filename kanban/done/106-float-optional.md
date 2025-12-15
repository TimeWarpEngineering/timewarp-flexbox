# Task 106 - FloatOptional

## Summary

Port the FloatOptional type (optional float with NaN representation) from C++ to C#. This is a Level 2 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                           | Lines |
| ---------- | ------------------------------ | ----- |
| C++ Header | `yoga/numeric/FloatOptional.h` | 93    |
| C++ Test   | `tests/FloatOptionalTest.cpp`  | 214   |

## Target Files

| Type      | Path                                                   |
| --------- | ------------------------------------------------------ |
| C# Source | `source/timewarp-flexbox/numeric/float-optional.cs`    |
| C# Test   | `tests/timewarp-flexbox-tests/float-optional-tests.cs` |

## Dependencies

- Task 104: Comparison utilities

## Todo List

- [ ] Port `FloatOptional.h` to C# readonly struct
- [ ] Implement NaN-based optional representation
- [ ] Port `FloatOptionalTest.cpp` to xUnit tests
- [ ] Ensure all tests pass

## Acceptance Criteria

- [ ] All 214 lines of test logic ported
- [ ] `isUndefined()` working
- [ ] `unwrap()` working
- [ ] Arithmetic operators working
- [ ] Comparison operators working
- [ ] All tests pass with identical behavior to C++

## Notes

Key C++ constructs to convert:

- `class FloatOptional` -> `readonly struct FloatOptional`
- Use `float.NaN` for undefined state
- Implement `IEquatable<FloatOptional>`
- Consider implementing `IComparable<FloatOptional>`

```csharp
public readonly struct FloatOptional : IEquatable<FloatOptional>
{
    private readonly float _value;

    public bool IsUndefined => float.IsNaN(_value);
    public float Unwrap() => _value;

    public static FloatOptional Undefined => new(float.NaN);
}
```
