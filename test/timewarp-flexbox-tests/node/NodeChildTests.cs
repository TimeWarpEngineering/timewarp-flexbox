/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ tests: tests/YGNodeChildTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Node_;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for child removal layout behavior.
/// Ported from C++ YGNodeChildTest.cpp.
/// </summary>
public class NodeChildTests
{
  public void reset_layout_when_child_removed()
  {
    FlexNode root = new();

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root.RemoveChild(root_child0);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    float.IsNaN(root_child0.Layout.GetDimension(Dimension.Width)).ShouldBeTrue();
    float.IsNaN(root_child0.Layout.GetDimension(Dimension.Height)).ShouldBeTrue();
  }

  public void removed_child_can_be_reused_with_valid_layout()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root.InsertChild(child, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    child.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    child.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    // Remove child - layout should be cleared and child marked dirty
    root.RemoveChild(child);

    float.IsNaN(child.Layout.GetDimension(Dimension.Width)).ShouldBeTrue();
    float.IsNaN(child.Layout.GetDimension(Dimension.Height)).ShouldBeTrue();
    child.IsDirty.ShouldBeTrue();

    // Reinsert the child and recalculate - layout should be valid again
    root.InsertChild(child, 0);
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    child.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    child.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
    child.IsDirty.ShouldBeFalse();
  }
}
