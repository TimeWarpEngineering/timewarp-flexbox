/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ tests: tests/YGDefaultValuesTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 *
 * Skipped tests:
 * - assert_box_sizing_border_box: already covered by NodeTests.Assert_box_sizing_border_box.
 */

namespace TimeWarp.Flexbox.Tests.Node_;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;
using FlexStyle = TimeWarp.Flexbox.Style;

/// <summary>
/// Tests for default style and layout values.
/// Ported from C++ YGDefaultValuesTest.cpp.
/// </summary>
public class DefaultValuesTests
{
  public void assert_default_values()
  {
    FlexNode root = new();

    root.GetChildCount().ShouldBe(0);
    // Note: C++ asserts YGNodeGetChild(root, 1) == nullptr for out-of-range access.
    // C# GetChild throws ArgumentOutOfRangeException instead, so that assertion is omitted.

    root.Style.Direction.ShouldBe(Direction.Inherit);
    root.Style.FlexDirection.ShouldBe(FlexDirection.Column);
    root.Style.JustifyContent.ShouldBe(Justify.FlexStart);
    root.Style.AlignContent.ShouldBe(Align.FlexStart);
    root.Style.AlignItems.ShouldBe(Align.Stretch);
    root.Style.AlignSelf.ShouldBe(Align.Auto);
    root.Style.PositionType.ShouldBe(PositionType.Relative);
    root.Style.FlexWrap.ShouldBe(Wrap.NoWrap);
    root.Style.Overflow.ShouldBe(Overflow.Visible);
    root.Style.FlexGrow.UnwrapOrDefault(FlexStyle.DefaultFlexGrow).ShouldBe(0f);
    root.Style.FlexShrink.UnwrapOrDefault(FlexStyle.DefaultFlexShrink).ShouldBe(0f);
    root.Style.FlexBasis.IsAuto.ShouldBeTrue();

    root.Style.GetPosition(Edge.Left).IsUndefined.ShouldBeTrue();
    root.Style.GetPosition(Edge.Top).IsUndefined.ShouldBeTrue();
    root.Style.GetPosition(Edge.Right).IsUndefined.ShouldBeTrue();
    root.Style.GetPosition(Edge.Bottom).IsUndefined.ShouldBeTrue();
    root.Style.GetPosition(Edge.Start).IsUndefined.ShouldBeTrue();
    root.Style.GetPosition(Edge.End).IsUndefined.ShouldBeTrue();

    root.Style.GetMargin(Edge.Left).IsUndefined.ShouldBeTrue();
    root.Style.GetMargin(Edge.Top).IsUndefined.ShouldBeTrue();
    root.Style.GetMargin(Edge.Right).IsUndefined.ShouldBeTrue();
    root.Style.GetMargin(Edge.Bottom).IsUndefined.ShouldBeTrue();
    root.Style.GetMargin(Edge.Start).IsUndefined.ShouldBeTrue();
    root.Style.GetMargin(Edge.End).IsUndefined.ShouldBeTrue();

    root.Style.GetPadding(Edge.Left).IsUndefined.ShouldBeTrue();
    root.Style.GetPadding(Edge.Top).IsUndefined.ShouldBeTrue();
    root.Style.GetPadding(Edge.Right).IsUndefined.ShouldBeTrue();
    root.Style.GetPadding(Edge.Bottom).IsUndefined.ShouldBeTrue();
    root.Style.GetPadding(Edge.Start).IsUndefined.ShouldBeTrue();
    root.Style.GetPadding(Edge.End).IsUndefined.ShouldBeTrue();

    root.Style.GetBorder(Edge.Left).IsUndefined.ShouldBeTrue();
    root.Style.GetBorder(Edge.Top).IsUndefined.ShouldBeTrue();
    root.Style.GetBorder(Edge.Right).IsUndefined.ShouldBeTrue();
    root.Style.GetBorder(Edge.Bottom).IsUndefined.ShouldBeTrue();
    root.Style.GetBorder(Edge.Start).IsUndefined.ShouldBeTrue();
    root.Style.GetBorder(Edge.End).IsUndefined.ShouldBeTrue();

    root.Style.GetDimension(Dimension.Width).IsAuto.ShouldBeTrue();
    root.Style.GetDimension(Dimension.Height).IsAuto.ShouldBeTrue();
    root.Style.GetMinDimension(Dimension.Width).IsUndefined.ShouldBeTrue();
    root.Style.GetMinDimension(Dimension.Height).IsUndefined.ShouldBeTrue();
    root.Style.GetMaxDimension(Dimension.Width).IsUndefined.ShouldBeTrue();
    root.Style.GetMaxDimension(Dimension.Height).IsUndefined.ShouldBeTrue();

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Bottom).ShouldBe(0f);

    root.Layout.GetMargin(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetMargin(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetMargin(PhysicalEdge.Right).ShouldBe(0f);
    root.Layout.GetMargin(PhysicalEdge.Bottom).ShouldBe(0f);

    root.Layout.GetPadding(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPadding(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetPadding(PhysicalEdge.Right).ShouldBe(0f);
    root.Layout.GetPadding(PhysicalEdge.Bottom).ShouldBe(0f);

    root.Layout.GetBorder(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetBorder(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetBorder(PhysicalEdge.Right).ShouldBe(0f);
    root.Layout.GetBorder(PhysicalEdge.Bottom).ShouldBe(0f);

    float.IsNaN(root.Layout.GetDimension(Dimension.Width)).ShouldBeTrue();
    float.IsNaN(root.Layout.GetDimension(Dimension.Height)).ShouldBeTrue();
    root.Layout.Direction.ShouldBe(Direction.Inherit);
  }

  public void assert_webdefault_values()
  {
    FlexConfig config = new() { UseWebDefaults = true };
    FlexNode root = new(config);

    root.Style.FlexDirection.ShouldBe(FlexDirection.Row);
    root.Style.AlignContent.ShouldBe(Align.Stretch);

    // Equivalent of C++ YGNodeStyleGetFlexShrink, which falls back to the
    // web default when the style value is undefined and web defaults are enabled.
    root.Style.FlexShrink
        .UnwrapOrDefault(root.Config.UseWebDefaults ? FlexStyle.WebDefaultFlexShrink : FlexStyle.DefaultFlexShrink)
        .ShouldBe(1.0f);
  }

  public void assert_webdefault_values_reset()
  {
    FlexConfig config = new() { UseWebDefaults = true };
    FlexNode root = new(config);
    root.Reset();

    root.Style.FlexDirection.ShouldBe(FlexDirection.Row);
    root.Style.AlignContent.ShouldBe(Align.Stretch);
    root.Style.FlexShrink
        .UnwrapOrDefault(root.Config.UseWebDefaults ? FlexStyle.WebDefaultFlexShrink : FlexStyle.DefaultFlexShrink)
        .ShouldBe(1.0f);
  }

  public void assert_legacy_stretch_behaviour()
  {
    FlexConfig config = new();
    config.SetErrata(Errata.StretchFlexBasis);
    FlexNode root = new(config);
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(500f));

    FlexNode root_child0 = new(config);
    root_child0.Style.AlignItems = Align.FlexStart;
    root.InsertChild(root_child0, 0);

    FlexNode root_child0_child0 = new(config);
    root_child0_child0.Style.FlexGrow = new FloatOptional(1f);
    root_child0_child0.Style.FlexShrink = new FloatOptional(1f);
    root_child0.InsertChild(root_child0_child0, 0);

    FlexNode root_child0_child0_child0 = new(config);
    root_child0_child0_child0.Style.FlexGrow = new FloatOptional(1f);
    root_child0_child0_child0.Style.FlexShrink = new FloatOptional(1f);
    root_child0_child0.InsertChild(root_child0_child0_child0, 0);
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(500f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(500f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

    root_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(500f);
  }
}
