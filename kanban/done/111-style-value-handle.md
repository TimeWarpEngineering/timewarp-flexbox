# Task 111 - StyleValueHandle

## Summary

Port the StyleValueHandle type from C++ to C#. This is a compact handle for referencing values in the StyleValuePool. This is a Level 4 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                            | Lines |
| ---------- | ------------------------------- | ----- |
| C++ Header | `yoga/style/StyleValueHandle.h` | 106   |

## Target Files

| Type      | Path                                                  |
| --------- | ----------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/style/style-value-handle.cs` |

## Dependencies

- Task 103: SmallValueBuffer
- Task 106: FloatOptional
- Task 108: StyleLength types

## Todo List

- [ ] Port `StyleValueHandle.h` to C# readonly struct
- [ ] Implement compact value encoding
- [ ] Handle inline vs pooled values

## Acceptance Criteria

- [ ] Handle correctly stores/retrieves values
- [ ] Inline optimization for small values working
- [ ] Used by StyleValuePool (Task 116)

## Notes

Key C++ constructs to convert:

- Bit-packed handle representation
- Inline value optimization for common cases
- Pool index for larger values

```csharp
public readonly struct StyleValueHandle
{
    private readonly uint _data;

    public bool IsInline => (_data & InlineBit) != 0;
    public int PoolIndex => (int)(_data & IndexMask);
}
```
