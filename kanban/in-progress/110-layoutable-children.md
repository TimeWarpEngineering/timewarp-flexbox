# Task 110 - LayoutableChildren

## Summary

Port the LayoutableChildren iterator from C++ to C#. This provides iteration over children that participate in layout (excludes display:none). This is a Level 3 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                             | Lines |
| ---------- | -------------------------------- | ----- |
| C++ Header | `yoga/node/LayoutableChildren.h` | ~100  |

## Target Files

| Type      | Path                                                  |
| --------- | ----------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/node/layoutable-children.cs` |

## Dependencies

- Task 109: Display enum

## Todo List

- [ ] Port `LayoutableChildren.h` iterator to C#
- [ ] Implement `IEnumerable<Node>` interface
- [ ] Handle display:none filtering

## Acceptance Criteria

- [ ] Iterator skips display:none children
- [ ] Works with foreach loops
- [ ] Used correctly by Node (Task 122)

## Notes

Key C++ constructs to convert:

- C++ iterator pattern -> C# `IEnumerable<T>` / `IEnumerator<T>`
- `begin()/end()` -> `GetEnumerator()`

```csharp
public readonly struct LayoutableChildren : IEnumerable<Node>
{
    private readonly Node _parent;

    public LayoutableChildren(Node parent) => _parent = parent;

    public Enumerator GetEnumerator() => new(_parent);

    public ref struct Enumerator
    {
        // Skip children with Display.None
    }
}
```
