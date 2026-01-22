# Task 052-add-display-contents

## Summary
Add `Contents` value to the Display enum and implement display:contents behavior. In Yoga, this is `YGDisplayContents`. When a node has display:contents, it doesn't generate a box but its children are laid out as if they were children of the node's parent.

## Todo List
- [x] Add `Contents` to Display enum with XML documentation
- [x] Add `contentsChildrenCount` tracking to FlexNode (like Yoga's `contentsChildrenCount_`)
- [x] Update child insertion to track contents children count
- [x] Update child removal to track contents children count
- [x] Update ReplaceChild to track contents children count
- [x] Implement `GetLayoutChildren()` that flattens contents nodes
- [x] Update FlexLayoutEngine to use layout children instead of direct children
- [x] Add `HasContentsChildren` property to FlexNode
- [x] Add unit tests for display:contents behavior
- [x] Test nested display:contents scenarios

## Notes
Yoga reference (YGEnums.h lines 43-47):
```cpp
YG_ENUM_DECL(
    YGDisplay,
    YGDisplayFlex,
    YGDisplayNone,
    YGDisplayContents)  // <-- Missing in TimeWarp
```

Yoga Node.cpp shows how contents children are tracked:
```cpp
void Node::insertChild(Node* child, size_t index) {
  if (child->style().display() == Display::Contents) {
    contentsChildrenCount_++;
  }
  children_.insert(...);
}
```

display:contents makes an element's children behave as direct children of the element's parent, effectively "unwrapping" the element.

## Implementation Details

### Files Modified
- `source/timewarp-flexbox/enums/display.cs` - Added `Contents` enum value
- `source/timewarp-flexbox/nodes/flex-node.cs` - Added `ContentsChildrenCount` tracking, `HasContentsChildren` property, and `GetLayoutChildren()` method
- `source/timewarp-flexbox/layout/flex-layout-engine.cs` - Updated to use `GetLayoutChildren()` and skip `Display.Contents` nodes
- `source/timewarp-flexbox/layout/flex-lines.cs` - Updated `CollectLines` to use `GetLayoutChildren()`

### Files Created
- `test/timewarp-flexbox-tests/layout/display-contents-tests.cs` - Comprehensive test suite

### Key Implementation Notes
1. `ContentsChildrenCount` is tracked at child insertion/removal time (matching Yoga's approach)
2. `GetLayoutChildren()` performs recursive flattening of contents nodes
3. Optimization: When `HasContentsChildren` is false, `GetLayoutChildren()` avoids recursion
4. Contents nodes are skipped in `LayoutNode()` - they don't generate boxes
5. All 541 tests pass including 29 new display:contents tests
