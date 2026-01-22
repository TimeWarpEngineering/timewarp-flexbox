# Task 102 - YogaEnums Utilities

## Summary

Port the YogaEnums utilities (ordinalCount, bitCount, ordinals iteration) from C++ to C#. This is a Level 0 task with zero dependencies.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                     | Lines |
| ---------- | ------------------------ | ----- |
| C++ Header | `yoga/enums/YogaEnums.h` | 85    |
| C++ Test   | `tests/OrdinalsTest.cpp` | 35    |

## Target Files

| Type      | Path                                             |
| --------- | ------------------------------------------------ |
| C# Source | `source/timewarp-flexbox/enums/yoga-enums.cs`    |
| C# Test   | `tests/timewarp-flexbox-tests/ordinals-tests.cs` |

## Dependencies

- None (Level 0)

## Todo List

- [ ] Create C# project structure if not exists
- [ ] Port `YogaEnums.h` template utilities to C# generic helpers
- [ ] Port `OrdinalsTest.cpp` to xUnit tests
- [ ] Ensure all tests pass

## Acceptance Criteria

- [ ] All 35 lines of test logic ported
- [ ] `ordinalCount<T>()` equivalent working
- [ ] `bitCount<T>()` equivalent working
- [ ] Ordinal iteration working
- [ ] All tests pass with identical behavior to C++

## Notes

Key C++ constructs to convert:

- `template<typename T>` -> C# generics with constraints
- `std::underlying_type_t<T>` -> `Enum.GetUnderlyingType()` or direct cast
- Consider using source generators for compile-time enum utilities
