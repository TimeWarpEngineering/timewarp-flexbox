/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ tests: tests/YGDirtiedTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Node_;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for the dirtied callback.
/// Ported from C++ YGDirtiedTest.cpp.
/// </summary>
public class DirtiedTests
{
  public void dirtied()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    int dirtiedCount = 0;
    root.DirtiedFunc = _ => dirtiedCount++;

    dirtiedCount.ShouldBe(0);

    // `dirtied` MUST be called in case of explicit dirtying.
    root.SetDirty(true);
    dirtiedCount.ShouldBe(1);

    // `dirtied` MUST be called ONCE.
    root.SetDirty(true);
    dirtiedCount.ShouldBe(1);
  }

  public void dirtied_propagation()
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

    int dirtiedCount = 0;
    root.DirtiedFunc = _ => dirtiedCount++;

    dirtiedCount.ShouldBe(0);

    // `dirtied` MUST be called for the first time.
    root_child0.MarkDirtyAndPropagate();
    dirtiedCount.ShouldBe(1);

    // `dirtied` must NOT be called for the second time.
    root_child0.MarkDirtyAndPropagate();
    dirtiedCount.ShouldBe(1);
  }

  public void dirtied_hierarchy()
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

    int dirtiedCount = 0;
    root_child0.DirtiedFunc = _ => dirtiedCount++;

    dirtiedCount.ShouldBe(0);

    // `dirtied` must NOT be called for descendants.
    root.MarkDirtyAndPropagate();
    dirtiedCount.ShouldBe(0);

    // `dirtied` must NOT be called for the sibling node.
    root_child1.MarkDirtyAndPropagate();
    dirtiedCount.ShouldBe(0);

    // `dirtied` MUST be called in case of explicit dirtying.
    root_child0.MarkDirtyAndPropagate();
    dirtiedCount.ShouldBe(1);
  }
}
