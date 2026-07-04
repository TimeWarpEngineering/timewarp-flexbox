/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ tests: tests/YGCloneNodeTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Node_;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for node cloning ownership behavior.
/// Ported from C++ YGCloneNodeTest.cpp.
/// </summary>
public class CloneNodeTests
{
  private static void RecursivelyAssertProperNodeOwnership(FlexNode node)
  {
    for (int i = 0; i < node.GetChildCount(); ++i)
    {
      FlexNode child = node.GetChild(i);
      child.Owner.ShouldBeSameAs(node);
      RecursivelyAssertProperNodeOwnership(child);
    }
  }

  public void absolute_node_cloned_with_static_parent()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.PositionType = PositionType.Static;
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(10f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(10f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child0_child0 = new();
    root_child0_child0.Style.PositionType = PositionType.Absolute;
    root_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Percent(1f));
    root_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(1f));
    root_child0.InsertChild(root_child0_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    FlexNode clonedRoot = root.Clone();
    clonedRoot.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(110f));
    CalculateLayout.Calculate(clonedRoot, float.NaN, float.NaN, Direction.LTR);

    RecursivelyAssertProperNodeOwnership(clonedRoot);
  }

  public void absolute_node_cloned_through_nested_display_contents()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode wrapper = new();
    wrapper.Style.PositionType = PositionType.Static;
    wrapper.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    wrapper.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(wrapper, 0);

    FlexNode static1 = new();
    static1.Style.PositionType = PositionType.Static;
    static1.Style.FlexGrow = new FloatOptional(1f);
    wrapper.InsertChild(static1, 0);

    FlexNode contents1 = new();
    contents1.Style.Display = Display.Contents;
    static1.InsertChild(contents1, 0);

    FlexNode contents2 = new();
    contents2.Style.Display = Display.Contents;
    contents1.InsertChild(contents2, 0);

    FlexNode absolute = new();
    absolute.Style.PositionType = PositionType.Absolute;
    absolute.Style.SetDimension(Dimension.Width, StyleSizeLength.Percent(50f));
    absolute.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(1f));
    contents2.InsertChild(absolute, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    FlexNode clonedRoot = root.Clone();
    clonedRoot.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200f));
    CalculateLayout.Calculate(clonedRoot, float.NaN, float.NaN, Direction.LTR);

    RecursivelyAssertProperNodeOwnership(clonedRoot);
  }

  public void absolute_node_cloned_with_static_ancestors()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.PositionType = PositionType.Static;
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child0_child0 = new();
    root_child0_child0.Style.PositionType = PositionType.Static;
    root_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(40f));
    root_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    root_child0.InsertChild(root_child0_child0, 0);

    FlexNode root_child0_child0_child0 = new();
    root_child0_child0_child0.Style.PositionType = PositionType.Static;
    root_child0_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(30f));
    root_child0_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(30f));
    root_child0_child0.InsertChild(root_child0_child0_child0, 0);

    FlexNode root_child0_child0_child0_child0 = new();
    root_child0_child0_child0_child0.Style.PositionType = PositionType.Absolute;
    root_child0_child0_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Percent(1f));
    root_child0_child0_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(1f));
    root_child0_child0_child0.InsertChild(root_child0_child0_child0_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    FlexNode clonedRoot = root.Clone();
    clonedRoot.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(110f));
    CalculateLayout.Calculate(clonedRoot, float.NaN, float.NaN, Direction.LTR);

    RecursivelyAssertProperNodeOwnership(clonedRoot);
  }
}
