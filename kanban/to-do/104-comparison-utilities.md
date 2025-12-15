# Task 104 - Comparison Utilities

## Summary

Port the numeric comparison utilities (inexactEquals, etc.) from C++ to C#. This is a Level 1 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                        | Lines |
| ---------- | --------------------------- | ----- |
| C++ Header | `yoga/numeric/Comparison.h` | 81    |

## Target Files

| Type      | Path                                            |
| --------- | ----------------------------------------------- |
| C# Source | `source/timewarp-flexbox/numeric/comparison.cs` |

## Dependencies

- None (only uses standard library)

## Todo List

- [ ] Port `Comparison.h` to C#
- [ ] Implement `inexactEquals` for float comparison with tolerance
- [ ] Implement other comparison utilities

## Acceptance Criteria

- [ ] All comparison functions ported
- [ ] Float tolerance comparison working correctly
- [ ] Used by FloatOptional (Task 106)

## Notes

Key C++ constructs to convert:

- `std::isnan()` -> `float.IsNaN()`
- `std::numeric_limits<float>::epsilon()` -> `float.Epsilon`
- Consider using `MathF` class for C# float operations
