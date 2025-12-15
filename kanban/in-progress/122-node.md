# Task 122 - Node

## Summary

Port the Node class from C++ to C#. This is the core layout node containing style, children, and layout results. This is a Level 7 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                    | Lines |
| ---------- | ----------------------- | ----- |
| C++ Header | `yoga/node/Node.h`      | ~400  |
| C++ Source | `yoga/node/Node.cpp`    | ~600  |
| C++ Header | `yoga/YGNode.h`         | ~150  |
| C++ Source | `yoga/YGNode.cpp`       | ~200  |
| C++ Header | `yoga/YGNodeStyle.h`    | ~100  |
| C++ Source | `yoga/YGNodeStyle.cpp`  | ~300  |
| C++ Header | `yoga/YGNodeLayout.h`   | ~50   |
| C++ Source | `yoga/YGNodeLayout.cpp` | ~100  |

## Test Files (Unit Tests)

| Type            | Path                                 | Lines      |
| --------------- | ------------------------------------ | ---------- |
| C++ Test        | `tests/YGDefaultValuesTest.cpp`      | ~100       |
| C++ Test        | `tests/YGDirtiedTest.cpp`            | ~100       |
| C++ Test        | `tests/YGDirtyMarkingTest.cpp`       | ~150       |
| C++ Test        | `tests/YGNodeChildTest.cpp`          | ~200       |
| C++ Test        | `tests/YGCloneNodeTest.cpp`          | ~150       |
| C++ Test        | `tests/YGTreeMutationTest.cpp`       | ~200       |
| C++ Test        | `tests/YGLayoutableChildrenTest.cpp` | ~100       |
| C++ Test        | `tests/YGNodeCallbackTest.cpp`       | ~150       |
| **Total Tests** |                                      | **~1,150** |

## Target Files

| Type      | Path                                                           |
| --------- | -------------------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/node/node.cs`                         |
| C# Source | `source/timewarp-flexbox/yg-node.cs`                           |
| C# Source | `source/timewarp-flexbox/yg-node-style.cs`                     |
| C# Source | `source/timewarp-flexbox/yg-node-layout.cs`                    |
| C# Test   | `tests/timewarp-flexbox-tests/yg-default-values-tests.cs`      |
| C# Test   | `tests/timewarp-flexbox-tests/yg-dirtied-tests.cs`             |
| C# Test   | `tests/timewarp-flexbox-tests/yg-dirty-marking-tests.cs`       |
| C# Test   | `tests/timewarp-flexbox-tests/yg-node-child-tests.cs`          |
| C# Test   | `tests/timewarp-flexbox-tests/yg-clone-node-tests.cs`          |
| C# Test   | `tests/timewarp-flexbox-tests/yg-tree-mutation-tests.cs`       |
| C# Test   | `tests/timewarp-flexbox-tests/yg-layoutable-children-tests.cs` |
| C# Test   | `tests/timewarp-flexbox-tests/yg-node-callback-tests.cs`       |

## Dependencies

- Task 110: LayoutableChildren
- Task 114: Config
- Task 119: LayoutResults
- Task 121: Style

## Todo List

- [ ] Port `Node.h/.cpp` internal implementation
- [ ] Port `YGNode.h/.cpp` public C API wrapper
- [ ] Port `YGNodeStyle.h/.cpp` style accessors
- [ ] Port `YGNodeLayout.h/.cpp` layout accessors
- [ ] Port all 8 test files to xUnit
- [ ] Ensure all tests pass

## Acceptance Criteria

- [ ] All ~1,150 lines of test logic ported
- [ ] Child management working (add, remove, insert)
- [ ] Style property access working
- [ ] Layout results access working
- [ ] Dirty marking working
- [ ] Cloning working
- [ ] Measure function callback working
- [ ] Baseline function callback working
- [ ] All tests pass with identical behavior to C++

## Notes

```csharp
public sealed class Node
{
    // Tree structure
    public Node? Owner { get; private set; }
    public IReadOnlyList<Node> Children => _children;

    // Configuration
    public Config Config { get; set; }
    public Style Style { get; }
    public LayoutResults Layout { get; }

    // Dirty tracking
    public bool IsDirty { get; private set; }
    public void MarkDirty();

    // Child management
    public void AddChild(Node child);
    public void RemoveChild(Node child);
    public void InsertChild(Node child, int index);
    public void RemoveAllChildren();

    // Callbacks
    public MeasureFunc? MeasureFunc { get; set; }
    public BaselineFunc? BaselineFunc { get; set; }

    // Cloning
    public Node Clone();
}
```
