/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Port of C++ tests/StyleTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.StyleNs;

using Style = TimeWarp.Flexbox.Style;

/// <summary>
/// Tests for the Style class.
/// Ported from C++ StyleTest.cpp.
/// </summary>
public class StyleTests
{
    // C++ test: TEST(Style, computed_padding_is_floored)
    public void computed_padding_is_floored()
    {
        Style style = new();
        style.SetPadding(Edge.All, StyleLength.Points(-1.0f));
        float paddingStart = style.ComputeInlineStartPadding(
            FlexDirection.Row, Direction.LTR, 0.0f /*widthSize*/);
        paddingStart.ShouldBe(0.0f);
    }

    // C++ test: TEST(Style, computed_border_is_floored)
    public void computed_border_is_floored()
    {
        Style style = new();
        style.SetBorder(Edge.All, StyleLength.Points(-1.0f));
        float borderStart = style.ComputeInlineStartBorder(FlexDirection.Row, Direction.LTR);
        borderStart.ShouldBe(0.0f);
    }

    // C++ test: TEST(Style, computed_gap_is_floored)
    public void computed_gap_is_floored()
    {
        Style style = new();
        style.SetGap(Gutter.Column, StyleLength.Points(-1.0f));
        float gapBetweenColumns = style.ComputeGapForAxis(FlexDirection.Row, 0.0f);
        gapBetweenColumns.ShouldBe(0.0f);
    }

    // C++ test: TEST(Style, computed_margin_is_not_floored)
    public void computed_margin_is_not_floored()
    {
        Style style = new();
        style.SetMargin(Edge.All, StyleLength.Points(-1.0f));
        float marginStart = style.ComputeInlineStartMargin(
            FlexDirection.Row, Direction.LTR, 0.0f /*widthSize*/);
        marginStart.ShouldBe(-1.0f);
    }

    #region Additional Tests for Style Properties

    public void default_style_has_expected_values()
    {
        Style style = new();

        // Enum defaults
        style.Direction.ShouldBe(Direction.Inherit);
        style.FlexDirection.ShouldBe(FlexDirection.Column);
        style.JustifyContent.ShouldBe(Justify.FlexStart);
        style.AlignContent.ShouldBe(Align.FlexStart);
        style.AlignItems.ShouldBe(Align.Stretch);
        style.AlignSelf.ShouldBe(Align.Auto);
        style.PositionType.ShouldBe(PositionType.Relative);
        style.FlexWrap.ShouldBe(Wrap.NoWrap);
        style.Overflow.ShouldBe(Overflow.Visible);
        style.Display.ShouldBe(Display.Flex);
        style.BoxSizing.ShouldBe(BoxSizing.BorderBox);

        // Flex defaults
        style.Flex.IsUndefined.ShouldBeTrue();
        style.FlexGrow.IsUndefined.ShouldBeTrue();
        style.FlexShrink.IsUndefined.ShouldBeTrue();
        style.FlexBasis.IsAuto.ShouldBeTrue();

        // Dimension defaults
        style.GetDimension(Dimension.Width).IsAuto.ShouldBeTrue();
        style.GetDimension(Dimension.Height).IsAuto.ShouldBeTrue();
        style.GetMinDimension(Dimension.Width).IsUndefined.ShouldBeTrue();
        style.GetMinDimension(Dimension.Height).IsUndefined.ShouldBeTrue();
        style.GetMaxDimension(Dimension.Width).IsUndefined.ShouldBeTrue();
        style.GetMaxDimension(Dimension.Height).IsUndefined.ShouldBeTrue();

        // AspectRatio default
        style.AspectRatio.IsUndefined.ShouldBeTrue();
    }

    public void style_flex_properties_can_be_set()
    {
        Style style = new();

        style.FlexGrow = new FloatOptional(2.0f);
        style.FlexShrink = new FloatOptional(0.5f);
        style.FlexBasis = StyleSizeLength.Points(100.0f);

        style.FlexGrow.Unwrap().ShouldBe(2.0f);
        style.FlexShrink.Unwrap().ShouldBe(0.5f);
        style.FlexBasis.IsPoints.ShouldBeTrue();
        style.FlexBasis.Resolve(1000.0f).Unwrap().ShouldBe(100.0f);
    }

    public void style_dimensions_can_be_set()
    {
        Style style = new();

        style.SetDimension(Dimension.Width, StyleSizeLength.Points(200.0f));
        style.SetDimension(Dimension.Height, StyleSizeLength.Percent(50.0f));
        style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(100.0f));
        style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(400.0f));

        style.GetDimension(Dimension.Width).Resolve(1000.0f).Unwrap().ShouldBe(200.0f);
        style.GetDimension(Dimension.Height).Resolve(1000.0f).Unwrap().ShouldBe(500.0f);
        style.GetMinDimension(Dimension.Width).Resolve(1000.0f).Unwrap().ShouldBe(100.0f);
        style.GetMaxDimension(Dimension.Width).Resolve(1000.0f).Unwrap().ShouldBe(400.0f);
    }

    public void style_margin_can_be_set_per_edge()
    {
        Style style = new();

        style.SetMargin(Edge.Left, StyleLength.Points(10.0f));
        style.SetMargin(Edge.Top, StyleLength.Points(20.0f));
        style.SetMargin(Edge.Right, StyleLength.Points(30.0f));
        style.SetMargin(Edge.Bottom, StyleLength.Points(40.0f));

        style.GetMargin(Edge.Left).Resolve(1000.0f).Unwrap().ShouldBe(10.0f);
        style.GetMargin(Edge.Top).Resolve(1000.0f).Unwrap().ShouldBe(20.0f);
        style.GetMargin(Edge.Right).Resolve(1000.0f).Unwrap().ShouldBe(30.0f);
        style.GetMargin(Edge.Bottom).Resolve(1000.0f).Unwrap().ShouldBe(40.0f);
    }

    public void style_margin_all_sets_all_edges()
    {
        Style style = new();

        style.SetMargin(Edge.All, StyleLength.Points(15.0f));

        // When All is set and specific edges are not, the All value is used
        float marginLeft = style.ComputeInlineStartMargin(FlexDirection.Row, Direction.LTR, 0.0f);
        marginLeft.ShouldBe(15.0f);
    }

    public void style_gap_can_be_set()
    {
        Style style = new();

        style.SetGap(Gutter.Column, StyleLength.Points(10.0f));
        style.SetGap(Gutter.Row, StyleLength.Points(20.0f));

        style.ComputeGapForAxis(FlexDirection.Row, 0.0f).ShouldBe(10.0f);
        style.ComputeGapForAxis(FlexDirection.Column, 0.0f).ShouldBe(20.0f);
    }

    public void style_gap_all_applies_to_both_axes()
    {
        Style style = new();

        style.SetGap(Gutter.All, StyleLength.Points(25.0f));

        style.ComputeGapForAxis(FlexDirection.Row, 0.0f).ShouldBe(25.0f);
        style.ComputeGapForAxis(FlexDirection.Column, 0.0f).ShouldBe(25.0f);
    }

    public void style_position_insets_query()
    {
        Style style = new();

        style.HorizontalInsetsDefined().ShouldBeFalse();
        style.VerticalInsetsDefined().ShouldBeFalse();

        style.SetPosition(Edge.Left, StyleLength.Points(10.0f));
        style.HorizontalInsetsDefined().ShouldBeTrue();
        style.VerticalInsetsDefined().ShouldBeFalse();

        style.SetPosition(Edge.Top, StyleLength.Points(20.0f));
        style.VerticalInsetsDefined().ShouldBeTrue();
    }

    public void style_aspect_ratio_rejects_zero()
    {
        Style style = new();

        style.AspectRatio = new FloatOptional(0.0f);
        style.AspectRatio.IsUndefined.ShouldBeTrue();
    }

    public void style_aspect_ratio_rejects_infinity()
    {
        Style style = new();

        style.AspectRatio = new FloatOptional(float.PositiveInfinity);
        style.AspectRatio.IsUndefined.ShouldBeTrue();

        style.AspectRatio = new FloatOptional(float.NegativeInfinity);
        style.AspectRatio.IsUndefined.ShouldBeTrue();
    }

    public void style_aspect_ratio_accepts_valid_values()
    {
        Style style = new();

        style.AspectRatio = new FloatOptional(1.5f);
        style.AspectRatio.Unwrap().ShouldBe(1.5f);
    }

    public void style_equality_identical_styles()
    {
        Style style1 = new();
        Style style2 = new();

        (style1 == style2).ShouldBeTrue();
        style1.Equals(style2).ShouldBeTrue();
    }

    public void style_equality_different_enum_values()
    {
        Style style1 = new();
        Style style2 = new();

        style2.FlexDirection = FlexDirection.Row;

        (style1 == style2).ShouldBeFalse();
        (style1 != style2).ShouldBeTrue();
    }

    public void style_equality_different_flex_values()
    {
        Style style1 = new();
        Style style2 = new();

        style2.FlexGrow = new FloatOptional(1.0f);

        (style1 == style2).ShouldBeFalse();
    }

    public void style_equality_different_dimensions()
    {
        Style style1 = new();
        Style style2 = new();

        style2.SetDimension(Dimension.Width, StyleSizeLength.Points(100.0f));

        (style1 == style2).ShouldBeFalse();
    }

    public void style_compute_margin_for_axis()
    {
        Style style = new();

        style.SetMargin(Edge.Left, StyleLength.Points(10.0f));
        style.SetMargin(Edge.Right, StyleLength.Points(20.0f));

        float totalMargin = style.ComputeMarginForAxis(FlexDirection.Row, 0.0f);
        totalMargin.ShouldBe(30.0f);
    }

    public void style_compute_border_for_axis()
    {
        Style style = new();

        style.SetBorder(Edge.Left, StyleLength.Points(5.0f));
        style.SetBorder(Edge.Right, StyleLength.Points(10.0f));

        float totalBorder = style.ComputeBorderForAxis(FlexDirection.Row);
        totalBorder.ShouldBe(15.0f);
    }

    public void style_resolved_min_dimension_with_border_box()
    {
        Style style = new();

        style.BoxSizing = BoxSizing.BorderBox;
        style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(100.0f));
        style.SetPadding(Edge.Left, StyleLength.Points(10.0f));
        style.SetPadding(Edge.Right, StyleLength.Points(10.0f));

        // With border-box, the resolved value is the style value (padding is included in the 100)
        FloatOptional resolved = style.ResolvedMinDimension(Direction.LTR, Dimension.Width, 100.0f, 0.0f);
        resolved.Unwrap().ShouldBe(100.0f);
    }

    public void style_resolved_min_dimension_with_content_box()
    {
        Style style = new();

        style.BoxSizing = BoxSizing.ContentBox;
        style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(100.0f));
        style.SetPadding(Edge.Left, StyleLength.Points(10.0f));
        style.SetPadding(Edge.Right, StyleLength.Points(10.0f));
        style.SetBorder(Edge.Left, StyleLength.Points(5.0f));
        style.SetBorder(Edge.Right, StyleLength.Points(5.0f));

        // With content-box, padding and border are added to the style value
        FloatOptional resolved = style.ResolvedMinDimension(Direction.LTR, Dimension.Width, 100.0f, 0.0f);
        resolved.Unwrap().ShouldBe(130.0f); // 100 + 10 + 10 + 5 + 5
    }

    #endregion
}
