# Task 105 - YGEnums Base Definitions

## Summary

Port the YGEnums base enum definitions and string conversion utilities from C++ to C#. This is a Level 1 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path               | Lines |
| ---------- | ------------------ | ----- |
| C++ Header | `yoga/YGEnums.h`   | 145   |
| C++ Source | `yoga/YGEnums.cpp` | 268   |

## Target Files

| Type      | Path                                  |
| --------- | ------------------------------------- |
| C# Source | `source/timewarp-flexbox/yg-enums.cs` |

## Dependencies

- Task 102: YogaEnums utilities (for ordinal helpers)

## Todo List

- [ ] Port `YGEnums.h` enum declarations
- [ ] Port `YGEnums.cpp` string conversion functions
- [ ] Add `ToString()` overrides for enums
- [ ] Add `Parse()` methods for string-to-enum conversion

## Acceptance Criteria

- [ ] All enums defined with correct values
- [ ] String conversion working both directions
- [ ] Enum names match C++ exactly (for test compatibility)

## Notes

Key C++ constructs to convert:

- `YG_ENUM_DECL` macro -> C# enum declarations
- `toString()` functions -> Override `ToString()` or use attributes
- Consider `[EnumMember]` attributes for serialization compatibility
