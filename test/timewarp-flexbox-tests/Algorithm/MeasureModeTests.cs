/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGMeasureModeTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using System.Collections.Generic;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for measure modes passed to measure functions, ported from YGMeasureModeTest.cpp.
/// </summary>
public class MeasureModeTests
{
    private readonly record struct MeasureConstraint(
        float Width,
        MeasureMode WidthMode,
        float Height,
        MeasureMode HeightMode);

    private static MeasureFunc Measure(List<MeasureConstraint> constraintList)
    {
        return (node, width, widthMode, height, heightMode) =>
        {
            constraintList.Add(new MeasureConstraint(width, widthMode, height, heightMode));

            // Note: the C++ test intentionally returns `width` for both
            // dimensions when the mode is not Undefined.
            return new YGSize(
                widthMode == MeasureMode.Undefined ? 10 : width,
                heightMode == MeasureMode.Undefined ? 10 : width);
        };
    }

    public void exactly_measure_stretched_child_column()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(1);

        constraintList[0].Width.ShouldBe(100f);
        constraintList[0].WidthMode.ShouldBe(MeasureMode.Exactly);
    }

    public void exactly_measure_stretched_child_row()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(1);

        constraintList[0].Height.ShouldBe(100f);
        constraintList[0].HeightMode.ShouldBe(MeasureMode.Exactly);
    }

    public void at_most_main_axis_column()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(1);

        constraintList[0].Height.ShouldBe(100f);
        constraintList[0].HeightMode.ShouldBe(MeasureMode.AtMost);
    }

    public void at_most_cross_axis_column()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.AlignItems = Align.FlexStart;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(1);

        constraintList[0].Width.ShouldBe(100f);
        constraintList[0].WidthMode.ShouldBe(MeasureMode.AtMost);
    }

    public void at_most_main_axis_row()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(1);

        constraintList[0].Width.ShouldBe(100f);
        constraintList[0].WidthMode.ShouldBe(MeasureMode.AtMost);
    }

    public void at_most_cross_axis_row()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.AlignItems = Align.FlexStart;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(1);

        constraintList[0].Height.ShouldBe(100f);
        constraintList[0].HeightMode.ShouldBe(MeasureMode.AtMost);
    }

    public void flex_child()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.Style.FlexGrow = 1f;
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(2);

        constraintList[0].Height.ShouldBe(100f);
        constraintList[0].HeightMode.ShouldBe(MeasureMode.AtMost);

        constraintList[1].Height.ShouldBe(100f);
        constraintList[1].HeightMode.ShouldBe(MeasureMode.Exactly);
    }

    public void flex_child_with_flex_basis()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.Style.FlexGrow = 1f;
        root_child0.Style.FlexBasis = StyleSizeLength.Points(0f);
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(1);

        constraintList[0].Height.ShouldBe(100f);
        constraintList[0].HeightMode.ShouldBe(MeasureMode.Exactly);
    }

    public void overflow_scroll_column()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.AlignItems = Align.FlexStart;
        root.Style.Overflow = Overflow.Scroll;
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(1);

        constraintList[0].Width.ShouldBe(100f);
        constraintList[0].WidthMode.ShouldBe(MeasureMode.AtMost);

        float.IsNaN(constraintList[0].Height).ShouldBeTrue();
        constraintList[0].HeightMode.ShouldBe(MeasureMode.Undefined);
    }

    public void overflow_scroll_row()
    {
        List<MeasureConstraint> constraintList = [];

        FlexNode root = new();
        root.Style.AlignItems = Align.FlexStart;
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.Overflow = Overflow.Scroll;
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc(Measure(constraintList));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        constraintList.Count.ShouldBe(1);

        float.IsNaN(constraintList[0].Width).ShouldBeTrue();
        constraintList[0].WidthMode.ShouldBe(MeasureMode.Undefined);

        constraintList[0].Height.ShouldBe(100f);
        constraintList[0].HeightMode.ShouldBe(MeasureMode.AtMost);
    }
}
