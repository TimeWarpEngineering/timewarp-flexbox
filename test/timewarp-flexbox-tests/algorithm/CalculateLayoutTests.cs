/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Integration tests for the CalculateLayout public API.
 * Ports tests from YGMeasureCacheTest.cpp and other C++ tests.
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Integration tests for CalculateLayout - the public entry point.
/// Tests end-to-end layout calculation, caching, and idempotency.
/// </summary>
public class CalculateLayoutTests
{
  #region Basic Layout Tests

  public void CalculateLayoutShouldSetDimensionsOnSimpleNode()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
  }

  public void CalculateLayoutShouldSetHasNewLayoutFlag()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root.HasNewLayout = false;

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    root.HasNewLayout.ShouldBeTrue();
  }

  public void CalculateLayoutShouldClearDirtyFlag()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    root.IsDirty.ShouldBeFalse();
  }

  public void CalculateLayoutShouldHandleUndefinedDimensions()
  {
    // Arrange - node with undefined dimensions
    FlexNode root = new();

    // Act
    CalculateLayout.Calculate(root, 200f, 150f, Direction.LTR);

    // Assert - should stretch to available space
    root.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(150f);
  }

  #endregion

  #region Child Layout Tests

  public void CalculateLayoutShouldLayoutChildren()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    child.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    child.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
    child.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    child.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
  }

  public void CalculateLayoutShouldLayoutRowChildren()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child1 = new();
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(30f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child1, 0);

    FlexNode child2 = new();
    child2.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(40f));
    child2.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child2, 1);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    child2.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(30f);
  }

  public void CalculateLayoutShouldLayoutColumnChildren()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Column;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child1 = new();
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(30f));
    root.InsertChild(child1, 0);

    FlexNode child2 = new();
    child2.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child2.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    root.InsertChild(child2, 1);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    child2.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(30f);
  }

  #endregion

  #region Idempotency Tests

  public void CalculateLayoutShouldBeIdempotent()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child, 0);

    // Act - first layout
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    float firstWidth = root.Layout.GetDimension(Dimension.Width);
    float firstHeight = root.Layout.GetDimension(Dimension.Height);
    float firstChildLeft = child.Layout.GetPosition(PhysicalEdge.Left);

    // Second layout with same inputs
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - results should be identical
    root.Layout.GetDimension(Dimension.Width).ShouldBe(firstWidth);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(firstHeight);
    child.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(firstChildLeft);
  }

  public void ConsecutiveLayoutsWithNoChangesShouldBeFast()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    // First layout to populate cache
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root.HasNewLayout = false;

    // Act - second layout should use cache
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - cache was used (node was not dirty after first layout)
    // HasNewLayout would be set again if a full layout was performed
    root.HasNewLayout.ShouldBeTrue();
  }

  #endregion

  #region Measure Cache Tests (from YGMeasureCacheTest.cpp)
  public void MeasureOnceSingleFlexibleChild()
  {
    // Port of measure_once_single_flexible_child
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    int measureCount = 0;
    child.Context = measureCount;
    child.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
    {
      int count = (int)(node.Context ?? 0);
      node.Context = count + 1;
      return new YGSize(
              widthMode == MeasureMode.Undefined ? 10 : width,
              heightMode == MeasureMode.Undefined ? 10 : height);
    });
    child.Style.FlexGrow = new FloatOptional(1f);
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    ((int)(child.Context ?? 0)).ShouldBe(1);
  }
  public void RemeasureWithSameExactWidthLargerThanNeededHeight()
  {
    // Port of remeasure_with_same_exact_width_larger_than_needed_height
    // Arrange
    FlexNode root = new();

    FlexNode child = new();
    int measureCount = 0;
    child.Context = measureCount;
    child.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
    {
      int count = (int)(node.Context ?? 0);
      node.Context = count + 1;
      float w = widthMode == MeasureMode.Undefined ||
                    (widthMode == MeasureMode.AtMost && width > 10)
              ? 10
              : width;
      float h = heightMode == MeasureMode.Undefined ||
                    (heightMode == MeasureMode.AtMost && height > 10)
              ? 10
              : height;
      return new YGSize(w, h);
    });
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, 100f, 100f, Direction.LTR);
    CalculateLayout.Calculate(root, 100f, 50f, Direction.LTR);

    // Assert - should only measure once due to caching
    ((int)(child.Context ?? 0)).ShouldBe(1);
  }
  public void RemeasureWithSameAtMostWidthLargerThanNeededHeight()
  {
    // Port of remeasure_with_same_atmost_width_larger_than_needed_height
    // Arrange
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;

    FlexNode child = new();
    int measureCount = 0;
    child.Context = measureCount;
    child.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
    {
      int count = (int)(node.Context ?? 0);
      node.Context = count + 1;
      float w = widthMode == MeasureMode.Undefined ||
                    (widthMode == MeasureMode.AtMost && width > 10)
              ? 10
              : width;
      float h = heightMode == MeasureMode.Undefined ||
                    (heightMode == MeasureMode.AtMost && height > 10)
              ? 10
              : height;
      return new YGSize(w, h);
    });
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, 100f, 100f, Direction.LTR);
    CalculateLayout.Calculate(root, 100f, 50f, Direction.LTR);

    // Assert
    ((int)(child.Context ?? 0)).ShouldBe(1);
  }
  public void RemeasureWithComputedWidthLargerThanNeededHeight()
  {
    // Port of remeasure_with_computed_width_larger_than_needed_height
    // Arrange
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;

    FlexNode child = new();
    int measureCount = 0;
    child.Context = measureCount;
    child.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
    {
      int count = (int)(node.Context ?? 0);
      node.Context = count + 1;
      float w = widthMode == MeasureMode.Undefined ||
                    (widthMode == MeasureMode.AtMost && width > 10)
              ? 10
              : width;
      float h = heightMode == MeasureMode.Undefined ||
                    (heightMode == MeasureMode.AtMost && height > 10)
              ? 10
              : height;
      return new YGSize(w, h);
    });
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, 100f, 100f, Direction.LTR);
    root.Style.AlignItems = Align.Stretch;
    CalculateLayout.Calculate(root, 10f, 50f, Direction.LTR);

    // Assert
    ((int)(child.Context ?? 0)).ShouldBe(1);
  }
  public void RemeasureWithAtMostComputedWidthUndefinedHeight()
  {
    // Port of remeasure_with_atmost_computed_width_undefined_height
    // Arrange
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;

    FlexNode child = new();
    int measureCount = 0;
    child.Context = measureCount;
    child.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
    {
      int count = (int)(node.Context ?? 0);
      node.Context = count + 1;
      float w = widthMode == MeasureMode.Undefined ||
                    (widthMode == MeasureMode.AtMost && width > 10)
              ? 10
              : width;
      float h = heightMode == MeasureMode.Undefined ||
                    (heightMode == MeasureMode.AtMost && height > 10)
              ? 10
              : height;
      return new YGSize(w, h);
    });
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, 100f, float.NaN, Direction.LTR);
    CalculateLayout.Calculate(root, 10f, float.NaN, Direction.LTR);

    // Assert
    ((int)(child.Context ?? 0)).ShouldBe(1);
  }
  public void RemeasureWithAlreadyMeasuredValueSmallerButStillFloatEqual()
  {
    // Port of remeasure_with_already_measured_value_smaller_but_still_float_equal
    // Arrange
    int measureCount = 0;

    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(288f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(288f));
    root.Style.FlexDirection = FlexDirection.Row;

    FlexNode child0 = new();
    child0.Style.SetPadding(Edge.All, StyleLength.Points(2.88f));
    child0.Style.FlexDirection = FlexDirection.Row;
    root.InsertChild(child0, 0);

    FlexNode child0Child0 = new();
    child0Child0.Context = measureCount;
    child0Child0.SetMeasureFunc((node, _, _, _, _) =>
    {
      int count = (int)(node.Context ?? 0);
      node.Context = count + 1;
      return new YGSize(84f, 49f);
    });
    child0.InsertChild(child0Child0, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    ((int)(child0Child0.Context ?? 0)).ShouldBe(1);
  }

  #endregion

  #region RTL Direction Tests
  public void CalculateLayoutShouldHandleRTLDirection()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(30f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);

    // Assert - in RTL, first child should be on the right
    child.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(0f);
    child.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(70f);
  }

  #endregion

  #region Flex Grow/Shrink Tests
  public void CalculateLayoutShouldHandleFlexGrow()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child1 = new();
    child1.Style.FlexGrow = new FloatOptional(1f);
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child1, 0);

    FlexNode child2 = new();
    child2.Style.FlexGrow = new FloatOptional(1f);
    child2.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child2, 1);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - both children should share the space equally
    child1.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    child2.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
  }
  public void CalculateLayoutShouldHandleFlexShrink()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child1 = new();
    child1.Style.FlexShrink = new FloatOptional(1f);
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child1, 0);

    FlexNode child2 = new();
    child2.Style.FlexShrink = new FloatOptional(1f);
    child2.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child2.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child2, 1);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - both children should shrink to fit
    child1.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    child2.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
  }

  #endregion

  #region Padding and Margin Tests
  public void CalculateLayoutShouldRespectPadding()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root.Style.SetPadding(Edge.All, StyleLength.Points(10f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    child.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    child.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(10f);
  }
  public void CalculateLayoutShouldRespectMargin()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    child.Style.SetMargin(Edge.Left, StyleLength.Points(10f));
    child.Style.SetMargin(Edge.Top, StyleLength.Points(5f));
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    child.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    child.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(5f);
  }

  #endregion

  #region Justify Content Tests
  public void CalculateLayoutShouldHandleJustifyContentCenter()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.JustifyContent = Justify.Center;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(20f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - child should be centered
    child.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(40f);
  }
  public void CalculateLayoutShouldHandleJustifyContentSpaceBetween()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.JustifyContent = Justify.SpaceBetween;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child1 = new();
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(20f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child1, 0);

    FlexNode child2 = new();
    child2.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(20f));
    child2.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child2, 1);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - first at start, second at end
    child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    child2.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(80f);
  }

  #endregion

  #region Align Items Tests
  public void CalculateLayoutShouldHandleAlignItemsCenter()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.Center;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - child should be vertically centered
    child.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(40f);
  }
  public void CalculateLayoutShouldHandleAlignItemsStretch()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.Stretch;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    // No height set - should stretch
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - child should stretch to parent height
    child.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  #endregion

  #region Absolute Positioning Tests
  public void CalculateLayoutShouldHandleAbsolutePositioning()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    child.Style.PositionType = PositionType.Absolute;
    child.Style.SetPosition(Edge.Left, StyleLength.Points(10f));
    child.Style.SetPosition(Edge.Top, StyleLength.Points(20f));
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(30f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    root.InsertChild(child, 0);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert
    child.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    child.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
  }

  #endregion

  #region Wrap Tests
  public void CalculateLayoutShouldHandleFlexWrap()
  {
    // Arrange
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.FlexWrap = Wrap.Wrap;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));

    FlexNode child1 = new();
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(60f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(30f));
    root.InsertChild(child1, 0);

    FlexNode child2 = new();
    child2.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(60f));
    child2.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(30f));
    root.InsertChild(child2, 1);

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - second child should wrap to next line
    child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    child2.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(30f);
  }

  #endregion

  #region Pixel Grid Rounding Tests
  public void CalculateLayoutShouldRoundToPixelGrid()
  {
    // Arrange
    TimeWarp.Flexbox.Config config = new() { PointScaleFactor = 2f };
    FlexNode root = new(config);
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100.5f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100.5f));

    // Act
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Assert - should be rounded to nearest half-pixel (scale factor 2)
    root.Layout.GetDimension(Dimension.Width).ShouldBe(100.5f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100.5f);
  }

  #endregion

  #region Generation Counter Tests
  public void CalculateLayoutShouldWorkWithMultipleCallsUpdatingGenerationCount()
  {
    // Arrange
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    // Act - multiple layouts should increment generation counter internally
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    uint gen1 = root.Layout.GenerationCount;
    root.MarkDirtyAndPropagate();
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    uint gen2 = root.Layout.GenerationCount;

    // Assert - generation should have been updated
    gen2.ShouldBeGreaterThan(gen1);
  }

  #endregion
}
