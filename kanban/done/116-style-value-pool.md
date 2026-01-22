# Task 116 - StyleValuePool

## Summary

Port the StyleValuePool from C++ to C#. This is an arena allocator for style values to reduce memory fragmentation. This is a Level 5 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                           | Lines |
| ---------- | ------------------------------ | ----- |
| C++ Header | `yoga/style/StyleValuePool.h`  | 184   |
| C++ Test   | `tests/StyleValuePoolTest.cpp` | ~200  |

## Target Files

| Type      | Path                                                     |
| --------- | -------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/style/style-value-pool.cs`      |
| C# Test   | `tests/timewarp-flexbox-tests/style-value-pool-tests.cs` |

## Dependencies

- Task 103: SmallValueBuffer
- Task 106: FloatOptional
- Task 108: StyleLength, StyleSizeLength
- Task 111: StyleValueHandle

## Todo List

- [ ] Port `StyleValuePool.h` to C#
- [ ] Implement value storage and retrieval
- [ ] Port `StyleValuePoolTest.cpp` to xUnit tests
- [ ] Ensure all tests pass

## Acceptance Criteria

- [ ] All ~200 lines of test logic ported
- [ ] Value storage working
- [ ] Value retrieval working
- [ ] Handle-based access working
- [ ] All tests pass with identical behavior to C++

## Notes

Key design decisions:

- C# has garbage collection, so arena allocation is less critical
- Consider using `List<T>` or `ArrayPool<T>` for backing storage
- May simplify to direct storage if performance is acceptable

```csharp
public sealed class StyleValuePool
{
    private readonly List<float> _floats = new();
    private readonly List<StyleLength> _lengths = new();

    public StyleValueHandle Store(StyleLength value);
    public StyleLength GetLength(StyleValueHandle handle);
}
```
