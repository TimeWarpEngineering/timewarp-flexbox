# Task 110 - LayoutableChildren

## Summary

Port the LayoutableChildren iterator from C++ to C#. This provides iteration over children that participate in layout (handles display:contents specially). This is a Level 3 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                             | Lines |
| ---------- | -------------------------------- | ----- |
| C++ Header | `yoga/node/LayoutableChildren.h` | ~100  |

## Target Files

| Type      | Path                                                |
| --------- | --------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Node/LayoutableChildren.cs`|
| C# Source | `source/timewarp-flexbox/Node/ILayoutableNode.cs`   |
| C# Test   | `test/timewarp-flexbox-tests/Node/LayoutableChildrenTests.cs` |

## Dependencies

- Task 109: Display enum

## Todo List

- [x] Port `LayoutableChildren.h` iterator to C#
- [x] Implement `IEnumerable<T>` interface
- [x] Handle display:contents flattening
- [x] Add comprehensive tests

## Acceptance Criteria

- [x] Iterator handles display:contents by traversing into those children
- [x] Works with foreach loops
- [x] Ready for use by Node (Task 122)

## Implementation Notes

The C++ implementation specifically handles `display:contents` nodes by "flattening" them:
- When encountering a `display:contents` node, the iterator descends into its children
- This is recursive (nested contents nodes are handled)
- Uses a backtrack stack to remember where to resume after exhausting contents children

**Note:** The original task description mentioned `display:none` filtering, but the actual C++ implementation filters `display:contents` for special handling, not `display:none`. The `display:none` filtering is done elsewhere in the algorithm, not in this iterator.

### Key Design Decisions

1. `ILayoutableNode` interface - Abstraction for nodes that can be iterated
2. Generic `LayoutableChildren<T>` - Works with any `ILayoutableNode` implementation  
3. Custom enumerator with backtrack stack for nested contents traversal
4. Value type (struct) for zero allocation iteration
