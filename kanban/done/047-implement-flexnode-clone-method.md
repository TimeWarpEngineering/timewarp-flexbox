# Task 047-implement-flexnode-clone-method

## Summary
Implement a Clone() method on FlexNode that creates a deep copy of the node and its subtree. This is needed for tree mutation operations where a complete copy of a node hierarchy is required.

## Todo List
- [ ] Design Clone() method signature and behavior
- [ ] Implement deep cloning of FlexNode properties
- [ ] Handle cloning of child nodes recursively
- [ ] Ensure cloned nodes have no parent reference (detached)
- [ ] Handle cloning of style properties (EdgeValues, FlexValue, etc.)
- [ ] Handle cloning of layout results appropriately
- [ ] Decide on MeasureFunc/BaselineFunc cloning behavior
- [ ] Add unit tests for Clone() method
- [ ] Test cloning of complex node hierarchies
- [ ] Document the method behavior

## Notes
Discovered during implementation of Task 046 (tree mutation tests). The Yoga flexbox library includes Clone() functionality for duplicating node trees. Key considerations:

- Cloned nodes should be detached (Parent = null)
- Style properties should be deep copied
- Layout results may need to be reset or copied depending on use case
- MeasureFunc and BaselineFunc delegates: decide whether to copy reference or leave null
- Consider whether to provide options for shallow vs deep clone
