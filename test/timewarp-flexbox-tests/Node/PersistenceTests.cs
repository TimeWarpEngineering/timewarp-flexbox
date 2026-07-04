/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ tests: tests/YGPersistenceTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 *
 * Omitted tests:
 * - cloning_and_freeing: free-semantics-specific (asserts node allocation counts
 *   around YGNodeFree/YGNodeFreeRecursive, which have no equivalent under GC).
 */

namespace TimeWarp.Flexbox.Tests.Node_;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for persistent (persistent data structure style) node tree behavior.
/// Ported from C++ YGPersistenceTest.cpp.
/// </summary>
public class PersistenceTests
{
  public void cloning_shared_root()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new(config);
    root_child0.Style.FlexGrow = new FloatOptional(1f);
    root_child0.Style.FlexBasis = StyleSizeLength.Points(50f);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.FlexGrow = new FloatOptional(1f);
    root.InsertChild(root_child1, 1);
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(75f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(75f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(25f);

    FlexNode root2 = root.Clone();
    root2.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));

    root2.GetChildCount().ShouldBe(2);
    // The children should have referential equality at this point.
    root2.GetChild(0).ShouldBeSameAs(root_child0);
    root2.GetChild(1).ShouldBeSameAs(root_child1);

    CalculateLayout.Calculate(root2, float.NaN, float.NaN, Direction.LTR);

    root2.GetChildCount().ShouldBe(2);
    // Relayout with no changed input should result in referential equality.
    root2.GetChild(0).ShouldBeSameAs(root_child0);
    root2.GetChild(1).ShouldBeSameAs(root_child1);

    root2.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(150f));
    root2.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));
    CalculateLayout.Calculate(root2, float.NaN, float.NaN, Direction.LTR);

    root2.GetChildCount().ShouldBe(2);
    // Relayout with changed input should result in cloned children.
    FlexNode root2_child0 = root2.GetChild(0);
    FlexNode root2_child1 = root2.GetChild(1);
    root2_child0.ShouldNotBeSameAs(root_child0);
    root2_child1.ShouldNotBeSameAs(root_child1);

    // Everything in the root should remain unchanged.
    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(75f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(75f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(25f);

    // The new root now has new layout.
    root2.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root2.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root2.Layout.GetDimension(Dimension.Width).ShouldBe(150f);
    root2.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root2_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root2_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root2_child0.Layout.GetDimension(Dimension.Width).ShouldBe(150f);
    root2_child0.Layout.GetDimension(Dimension.Height).ShouldBe(125f);

    root2_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root2_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(125f);
    root2_child1.Layout.GetDimension(Dimension.Width).ShouldBe(150f);
    root2_child1.Layout.GetDimension(Dimension.Height).ShouldBe(75f);
  }

  public void mutating_children_of_a_clone_clones_only_after_layout()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.GetChildCount().ShouldBe(0);

    FlexNode root2 = root.Clone();
    root2.GetChildCount().ShouldBe(0);

    FlexNode root2_child0 = new(config);
    root2.InsertChild(root2_child0, 0);

    root.GetChildCount().ShouldBe(0);
    root2.GetChildCount().ShouldBe(1);

    FlexNode root3 = root2.Clone();
    root2.GetChildCount().ShouldBe(1);
    root3.GetChildCount().ShouldBe(1);
    root3.GetChild(0).ShouldBeSameAs(root2.GetChild(0));

    FlexNode root3_child1 = new(config);
    root3.InsertChild(root3_child1, 1);
    root2.GetChildCount().ShouldBe(1);
    root3.GetChildCount().ShouldBe(2);
    root3.GetChild(1).ShouldBeSameAs(root3_child1);
    root3.GetChild(0).ShouldBeSameAs(root2.GetChild(0));

    FlexNode root4 = root3.Clone();
    root4.GetChild(1).ShouldBeSameAs(root3_child1);

    root4.RemoveChild(root3_child1);
    root3.GetChildCount().ShouldBe(2);
    root4.GetChildCount().ShouldBe(1);
    root4.GetChild(0).ShouldBeSameAs(root3.GetChild(0));

    CalculateLayout.Calculate(root4, float.NaN, float.NaN, Direction.LTR);
    root4.GetChild(0).ShouldNotBeSameAs(root3.GetChild(0));
    CalculateLayout.Calculate(root3, float.NaN, float.NaN, Direction.LTR);
    root3.GetChild(0).ShouldNotBeSameAs(root2.GetChild(0));
  }

  public void cloning_two_levels()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new(config);
    root_child0.Style.FlexGrow = new FloatOptional(1f);
    root_child0.Style.FlexBasis = StyleSizeLength.Points(15f);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.FlexGrow = new FloatOptional(1f);
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_0 = new(config);
    root_child1_0.Style.FlexBasis = StyleSizeLength.Points(10f);
    root_child1_0.Style.FlexGrow = new FloatOptional(1f);
    root_child1.InsertChild(root_child1_0, 0);

    FlexNode root_child1_1 = new(config);
    root_child1_1.Style.FlexBasis = StyleSizeLength.Points(25f);
    root_child1.InsertChild(root_child1_1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(40f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(60f);
    root_child1_0.Layout.GetDimension(Dimension.Height).ShouldBe(35f);
    root_child1_1.Layout.GetDimension(Dimension.Height).ShouldBe(25f);

    FlexNode root2_child0 = root_child0.Clone();
    FlexNode root2_child1 = root_child1.Clone();
    FlexNode root2 = root.Clone();

    root2_child0.Style.FlexGrow = new FloatOptional(0f);
    root2_child0.Style.FlexBasis = StyleSizeLength.Points(40f);

    root2.ClearChildren();
    root2.InsertChild(root2_child0, 0);
    root2.InsertChild(root2_child1, 1);
    root2.GetChildCount().ShouldBe(2);

    CalculateLayout.Calculate(root2, float.NaN, float.NaN, Direction.LTR);

    // Original root is unchanged
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(40f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(60f);
    root_child1_0.Layout.GetDimension(Dimension.Height).ShouldBe(35f);
    root_child1_1.Layout.GetDimension(Dimension.Height).ShouldBe(25f);

    // New root has new layout at the top
    root2_child0.Layout.GetDimension(Dimension.Height).ShouldBe(40f);
    root2_child1.Layout.GetDimension(Dimension.Height).ShouldBe(60f);

    // The deeper children are untouched.
    root2_child1.GetChild(0).ShouldBeSameAs(root_child1_0);
    root2_child1.GetChild(1).ShouldBeSameAs(root_child1_1);
  }

  public void mixed_shared_and_owned_children()
  {
    // Don't try this at home!

    FlexNode root0 = new();
    FlexNode root1 = new();

    FlexNode root0_child0 = new();
    FlexNode root0_child0_0 = new();
    root0.InsertChild(root0_child0, 0);
    root0_child0.InsertChild(root0_child0_0, 0);

    FlexNode root1_child0 = new();
    FlexNode root1_child2 = new();
    root1.InsertChild(root1_child0, 0);
    root1.InsertChild(root1_child2, 1);

    List<FlexNode> children = [.. root1.Children];
    children.Insert(1, root0_child0);
    root1.SetChildren(children);

    FlexNode secondChild = root1.GetChild(1);
    secondChild.ShouldBeSameAs(root0.GetChild(0));
    secondChild.GetChild(0).ShouldBeSameAs(root0_child0.GetChild(0));

    CalculateLayout.Calculate(root1, float.NaN, float.NaN, Direction.LTR);
    secondChild = root1.GetChild(1);
    secondChild.ShouldNotBeSameAs(root0.GetChild(0));
    secondChild.Owner.ShouldBeSameAs(root1);
    secondChild.GetChild(0).ShouldNotBeSameAs(root0_child0.GetChild(0));
    secondChild.GetChild(0).Owner.ShouldBeSameAs(secondChild);
  }
}
