# Task 121 - Style

## Summary

Port the Style class from C++ to C#. This is the largest style file containing all node style properties. This is a Level 6 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                  | Lines |
| ---------- | --------------------- | ----- |
| C++ Header | `yoga/style/Style.h`  | 759   |
| C++ Test   | `tests/StyleTest.cpp` | ~250  |

## Target Files

| Type      | Path                                          |
| --------- | --------------------------------------------- |
| C# Source | `source/timewarp-flexbox/style/style.cs`      |
| C# Test   | `tests/timewarp-flexbox-tests/style-tests.cs` |

## Dependencies

- Task 106: FloatOptional
- Task 108: StyleLength, StyleSizeLength
- Task 109: All enums
- Task 116: StyleValuePool
- Task 117: FlexDirection utilities

## Todo List

- [ ] Port `Style.h` to C#
- [ ] Implement all style properties
- [ ] Implement style getters/setters
- [ ] Port `StyleTest.cpp` to xUnit tests
- [ ] Ensure all tests pass

## Acceptance Criteria

- [ ] All ~250 lines of test logic ported
- [ ] All flex properties working (direction, wrap, grow, shrink, basis)
- [ ] All alignment properties working (justify, align items/self/content)
- [ ] All dimension properties working (width, height, min/max)
- [ ] All spacing properties working (margin, padding, border)
- [ ] All position properties working (position type, insets)
- [ ] All tests pass with identical behavior to C++

## Notes

This is a large file (~759 lines). Key properties to port:

```csharp
public sealed class Style
{
    // Flex container
    public FlexDirection FlexDirection { get; set; }
    public Wrap FlexWrap { get; set; }
    public Justify JustifyContent { get; set; }
    public Align AlignItems { get; set; }
    public Align AlignContent { get; set; }

    // Flex item
    public Align AlignSelf { get; set; }
    public FloatOptional FlexGrow { get; set; }
    public FloatOptional FlexShrink { get; set; }
    public StyleLength FlexBasis { get; set; }

    // Dimensions
    public StyleSizeLength Width { get; set; }
    public StyleSizeLength Height { get; set; }
    public StyleSizeLength MinWidth { get; set; }
    public StyleSizeLength MinHeight { get; set; }
    public StyleSizeLength MaxWidth { get; set; }
    public StyleSizeLength MaxHeight { get; set; }

    // Position
    public PositionType PositionType { get; set; }
    public StyleLength GetPosition(Edge edge);
    public void SetPosition(Edge edge, StyleLength value);

    // Spacing
    public StyleLength GetMargin(Edge edge);
    public StyleLength GetPadding(Edge edge);
    public float GetBorder(Edge edge);
}
```
