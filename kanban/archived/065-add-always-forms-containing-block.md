# Task 065-add-always-forms-containing-block

## Summary
Add AlwaysFormsContainingBlock flag to FlexNode to match Yoga's alwaysFormsContainingBlock_. This flag indicates that a node always forms a containing block for absolutely positioned descendants, even without explicit positioning (useful for CSS transforms).

## Todo List
- [ ] Add `AlwaysFormsContainingBlock` property to FlexNode with default false
- [ ] Update absolute positioning logic to check this flag
- [ ] Add XML documentation explaining the flag's purpose
- [ ] Add unit tests for AlwaysFormsContainingBlock behavior
- [ ] Verify absolute child positioning respects the flag

## Notes
Yoga reference (YGNode.h lines 265-282):
```cpp
/**
 * Make it so that this node will always form a containing block for any
 * descendant nodes. This is useful for when a node has a property outside of
 * of Yoga that will form a containing block. For example, transforms or some of
 * the others listed in
 * https://developer.mozilla.org/en-US/docs/Web/CSS/Containing_block
 */
YG_EXPORT void YGNodeSetAlwaysFormsContainingBlock(
    YGNodeRef node,
    bool alwaysFormsContainingBlock);

YG_EXPORT bool YGNodeGetAlwaysFormsContainingBlock(YGNodeConstRef node);
```

CSS transforms, filters, and other properties can create containing blocks. This flag allows the layout engine to be informed of such cases.
