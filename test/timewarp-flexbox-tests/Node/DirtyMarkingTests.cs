/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ tests: tests/YGDirtyMarkingTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 *
 * Omitted tests (free-semantics-specific, no equivalent in a garbage-collected port):
 * - dirty_parent_when_child_freed (relies on YGNodeFree detaching the child)
 * - dirty_parent_when_subtree_freed_recursive (relies on YGNodeFreeRecursive detaching the subtree)
 */

namespace TimeWarp.Flexbox.Tests.Node_;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for dirty marking behavior.
/// Ported from C++ YGDirtyMarkingTest.cpp.
/// </summary>
public class DirtyMarkingTests
{
  public void dirty_propagation()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new();
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(20f));

    root_child0.IsDirty.ShouldBeTrue();
    root_child1.IsDirty.ShouldBeFalse();
    root.IsDirty.ShouldBeTrue();

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.IsDirty.ShouldBeFalse();
    root_child1.IsDirty.ShouldBeFalse();
    root.IsDirty.ShouldBeFalse();
  }

  public void dirty_propagation_only_if_prop_changed()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new();
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));

    root_child0.IsDirty.ShouldBeFalse();
    root_child1.IsDirty.ShouldBeFalse();
    root.IsDirty.ShouldBeFalse();
  }

  public void dirty_propagation_changing_layout_config()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new();
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child1, 1);

    FlexNode root_child0_child0 = new();
    root_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(25f));
    root_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child0_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.IsDirty.ShouldBeFalse();
    root_child0.IsDirty.ShouldBeFalse();
    root_child1.IsDirty.ShouldBeFalse();
    root_child0_child0.IsDirty.ShouldBeFalse();

    FlexConfig newConfig = new();
    newConfig.SetErrata(Errata.StretchFlexBasis);
    root_child0.SetConfig(newConfig);

    root.IsDirty.ShouldBeTrue();
    root_child0.IsDirty.ShouldBeTrue();
    root_child1.IsDirty.ShouldBeFalse();
    root_child0_child0.IsDirty.ShouldBeFalse();

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.IsDirty.ShouldBeFalse();
    root_child0.IsDirty.ShouldBeFalse();
    root_child1.IsDirty.ShouldBeFalse();
    root_child0_child0.IsDirty.ShouldBeFalse();
  }

  public void dirty_propagation_changing_benign_config()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new();
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child1, 1);

    FlexNode root_child0_child0 = new();
    root_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(25f));
    root_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child0_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.IsDirty.ShouldBeFalse();
    root_child0.IsDirty.ShouldBeFalse();
    root_child1.IsDirty.ShouldBeFalse();
    root_child0_child0.IsDirty.ShouldBeFalse();

    FlexConfig newConfig = new();
    newConfig.SetLogger((context, level, message) => { });
    root_child0.SetConfig(newConfig);

    root.IsDirty.ShouldBeFalse();
    root_child0.IsDirty.ShouldBeFalse();
    root_child1.IsDirty.ShouldBeFalse();
    root_child0_child0.IsDirty.ShouldBeFalse();
  }

  public void dirty_mark_all_children_as_dirty_when_display_changes()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child0 = new();
    child0.Style.FlexGrow = new FloatOptional(1f);
    FlexNode child1 = new();
    child1.Style.FlexGrow = new FloatOptional(1f);

    FlexNode child1_child0 = new();
    FlexNode child1_child0_child0 = new();
    child1_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(8f));
    child1_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(16f));

    child1_child0.InsertChild(child1_child0_child0, 0);

    child1.InsertChild(child1_child0, 0);
    root.InsertChild(child0, 0);
    root.InsertChild(child1, 0);

    child0.Style.Display = Display.Flex;
    child1.Style.Display = Display.None;
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    child1_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    child1_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0f);

    child0.Style.Display = Display.None;
    child1.Style.Display = Display.Flex;
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    child1_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(8f);
    child1_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(16f);

    child0.Style.Display = Display.Flex;
    child1.Style.Display = Display.None;
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    child1_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    child1_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0f);

    child0.Style.Display = Display.None;
    child1.Style.Display = Display.Flex;
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    child1_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(8f);
    child1_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(16f);
  }

  public void dirty_node_only_if_children_are_actually_removed()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

    FlexNode child0 = new();
    child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(25f));
    root.InsertChild(child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    FlexNode child1 = new();
    root.RemoveChild(child1);
    root.IsDirty.ShouldBeFalse();

    root.RemoveChild(child0);
    root.IsDirty.ShouldBeTrue();
  }

  public void dirty_node_only_if_undefined_values_gets_set_to_undefined()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(float.NaN));

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root.IsDirty.ShouldBeFalse();

    root.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(float.NaN));

    root.IsDirty.ShouldBeFalse();
  }

  public void dirty_removed_child_node()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child = new();
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(child, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    child.IsDirty.ShouldBeFalse();

    root.RemoveChild(child);

    // Child should be marked dirty after removal so layout is recalculated
    // when the child is reused (e.g., in a recycling view system)
    child.IsDirty.ShouldBeTrue();
  }

  public void dirty_removed_child_nodes_when_removing_all()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode child0 = new();
    child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(25f));
    root.InsertChild(child0, 0);

    FlexNode child1 = new();
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(25f));
    root.InsertChild(child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    child0.IsDirty.ShouldBeFalse();
    child1.IsDirty.ShouldBeFalse();

    root.ClearChildren();

    // All children should be marked dirty after removal
    child0.IsDirty.ShouldBeTrue();
    child1.IsDirty.ShouldBeTrue();
  }
}
