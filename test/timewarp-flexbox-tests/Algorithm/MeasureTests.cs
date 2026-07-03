/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGMeasureTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for measure functions, ported from YGMeasureTest.cpp.
/// </summary>
public class MeasureTests
{
    private static YGSize SimulateWrappingText(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
    {
        if (widthMode == MeasureMode.Undefined || width >= 68)
        {
            return new YGSize(68, 16);
        }

        return new YGSize(50, 32);
    }

    private static YGSize MeasureAssertNegative(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
    {
        width.ShouldBeGreaterThanOrEqualTo(0f);
        height.ShouldBeGreaterThanOrEqualTo(0f);

        return new YGSize(0, 0);
    }

    private static YGSize Measure90x10(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
    {
        return new YGSize(90, 10);
    }

    private static YGSize Measure100x100(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
    {
        return new YGSize(100, 100);
    }

    public void dont_measure_single_grow_shrink_child()
    {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        int measureCount = 0;

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
        {
            measureCount++;
            return new YGSize(10, 10);
        });
        root_child0.Style.FlexGrow = 1f;
        root_child0.Style.FlexShrink = 1f;
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        measureCount.ShouldBe(0);
    }

    public void measure_absolute_child_with_no_constraints()
    {
        FlexNode root = new();

        FlexNode root_child0 = new();
        root.InsertChild(root_child0, 0);

        int measureCount = 0;

        FlexNode root_child0_child0 = new();
        root_child0_child0.Style.PositionType = PositionType.Absolute;
        root_child0_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
        {
            measureCount++;
            return new YGSize(10, 10);
        });
        root_child0.InsertChild(root_child0_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        measureCount.ShouldBe(1);
    }

    public void dont_measure_when_min_equals_max()
    {
        FlexNode root = new();
        root.Style.AlignItems = Align.FlexStart;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        int measureCount = 0;

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
        {
            measureCount++;
            return new YGSize(10, 10);
        });
        root_child0.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(10f));
        root_child0.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(10f));
        root_child0.Style.SetMinDimension(Dimension.Height, StyleSizeLength.Points(10f));
        root_child0.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(10f));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        measureCount.ShouldBe(0);
        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(10f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10f);
    }

    public void dont_measure_when_min_equals_max_percentages()
    {
        FlexNode root = new();
        root.Style.AlignItems = Align.FlexStart;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        int measureCount = 0;

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
        {
            measureCount++;
            return new YGSize(10, 10);
        });
        root_child0.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Percent(10f));
        root_child0.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Percent(10f));
        root_child0.Style.SetMinDimension(Dimension.Height, StyleSizeLength.Percent(10f));
        root_child0.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Percent(10f));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        measureCount.ShouldBe(0);
        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(10f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10f);
    }

    public void measure_nodes_with_margin_auto_and_stretch()
    {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(500f));

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) => new YGSize(10, 10));
        root_child0.Style.SetMargin(Edge.Left, StyleLength.Auto);
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(490f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(10f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10f);
    }

    public void dont_measure_when_min_equals_max_mixed_width_percent()
    {
        FlexNode root = new();
        root.Style.AlignItems = Align.FlexStart;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        int measureCount = 0;

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
        {
            measureCount++;
            return new YGSize(10, 10);
        });
        root_child0.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Percent(10f));
        root_child0.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Percent(10f));
        root_child0.Style.SetMinDimension(Dimension.Height, StyleSizeLength.Points(10f));
        root_child0.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(10f));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        measureCount.ShouldBe(0);
        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(10f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10f);
    }

    public void dont_measure_when_min_equals_max_mixed_height_percent()
    {
        FlexNode root = new();
        root.Style.AlignItems = Align.FlexStart;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

        int measureCount = 0;

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
        {
            measureCount++;
            return new YGSize(10, 10);
        });
        root_child0.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(10f));
        root_child0.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(10f));
        root_child0.Style.SetMinDimension(Dimension.Height, StyleSizeLength.Percent(10f));
        root_child0.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Percent(10f));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        measureCount.ShouldBe(0);
        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(10f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10f);
    }

    public void measure_enough_size_should_be_in_single_line()
    {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));

        FlexNode root_child0 = new();
        root_child0.Style.AlignSelf = Align.FlexStart;
        root_child0.SetMeasureFunc(SimulateWrappingText);

        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(68f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(16f);
    }

    public void measure_not_enough_size_should_wrap()
    {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(55f));

        FlexNode root_child0 = new();
        root_child0.Style.AlignSelf = Align.FlexStart;
        root_child0.SetMeasureFunc(SimulateWrappingText);
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(32f);
    }

    public void measure_zero_space_should_grow()
    {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));
        root.Style.FlexDirection = FlexDirection.Column;
        root.Style.FlexGrow = 0f;

        int measureCount = 0;

        FlexNode root_child0 = new();
        root_child0.Style.FlexDirection = FlexDirection.Column;
        root_child0.Style.SetPadding(Edge.All, StyleLength.Points(100f));
        root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
        {
            measureCount++;
            return new YGSize(10, 10);
        });

        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, 282f, float.NaN, Direction.LTR);

        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(282f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    }

    public void measure_flex_direction_row_and_padding()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.SetPadding(Edge.Left, StyleLength.Points(25f));
        root.Style.SetPadding(Edge.Top, StyleLength.Points(25f));
        root.Style.SetPadding(Edge.Right, StyleLength.Points(25f));
        root.Style.SetPadding(Edge.Bottom, StyleLength.Points(25f));
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

        FlexNode root_child0 = new(config);
        root_child0.SetMeasureFunc(SimulateWrappingText);
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(5f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(5f));
        root.InsertChild(root_child1, 1);
        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(25f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(75f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(25f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(5f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(5f);
    }

    public void measure_flex_direction_column_and_padding()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.SetMargin(Edge.Top, StyleLength.Points(20f));
        root.Style.SetPadding(Edge.All, StyleLength.Points(25f));
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

        FlexNode root_child0 = new(config);
        root_child0.SetMeasureFunc(SimulateWrappingText);
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(5f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(5f));
        root.InsertChild(root_child1, 1);
        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(25f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(32f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(57f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(5f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(5f);
    }

    public void measure_flex_direction_row_no_padding()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.SetMargin(Edge.Top, StyleLength.Points(20f));
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

        FlexNode root_child0 = new(config);
        root_child0.SetMeasureFunc(SimulateWrappingText);
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(5f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(5f));
        root.InsertChild(root_child1, 1);
        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(50f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(5f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(5f);
    }

    public void measure_flex_direction_row_no_padding_align_items_flexstart()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.SetMargin(Edge.Top, StyleLength.Points(20f));
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
        root.Style.AlignItems = Align.FlexStart;

        FlexNode root_child0 = new(config);
        root_child0.SetMeasureFunc(SimulateWrappingText);
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(5f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(5f));
        root.InsertChild(root_child1, 1);
        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(32f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(50f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(5f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(5f);
    }

    public void measure_with_fixed_size()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.SetMargin(Edge.Top, StyleLength.Points(20f));
        root.Style.SetPadding(Edge.All, StyleLength.Points(25f));
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

        FlexNode root_child0 = new(config);
        root_child0.SetMeasureFunc(SimulateWrappingText);
        root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(10f));
        root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(10f));
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(5f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(5f));
        root.InsertChild(root_child1, 1);
        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(25f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(10f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(35f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(5f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(5f);
    }

    public void measure_with_flex_shrink()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.SetMargin(Edge.Top, StyleLength.Points(20f));
        root.Style.SetPadding(Edge.All, StyleLength.Points(25f));
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

        FlexNode root_child0 = new(config);
        root_child0.SetMeasureFunc(SimulateWrappingText);
        root_child0.Style.FlexShrink = 1f;
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(5f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(5f));
        root.InsertChild(root_child1, 1);
        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(25f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(25f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(5f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(5f);
    }

    public void measure_no_padding()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.SetMargin(Edge.Top, StyleLength.Points(20f));
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

        FlexNode root_child0 = new(config);
        root_child0.SetMeasureFunc(SimulateWrappingText);
        root_child0.Style.FlexShrink = 1f;
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(5f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(5f));
        root.InsertChild(root_child1, 1);
        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(32f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(32f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(5f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(5f);
    }

    public void cannot_add_child_to_node_with_measure_func()
    {
        FlexNode root = new();
        root.SetMeasureFunc((node, width, widthMode, height, heightMode) => new YGSize(10, 10));

        FlexNode root_child0 = new();
        Should.Throw<Exception>(() => root.InsertChild(root_child0, 0));
    }

    public void cannot_add_nonnull_measure_func_to_non_leaf_node()
    {
        FlexNode root = new();
        FlexNode root_child0 = new();
        root.InsertChild(root_child0, 0);
        Should.Throw<Exception>(() =>
            root.SetMeasureFunc((node, width, widthMode, height, heightMode) => new YGSize(10, 10)));
    }

    public void can_nullify_measure_func_on_any_node()
    {
        FlexNode root = new();
        root.InsertChild(new FlexNode(), 0);
        root.SetMeasureFunc(null);
        root.HasMeasureFunc.ShouldBeFalse();
    }

    public void cant_call_negative_measure()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.FlexDirection = FlexDirection.Column;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(10f));

        FlexNode root_child0 = new(config);
        root_child0.SetMeasureFunc(MeasureAssertNegative);
        root_child0.Style.SetMargin(Edge.Top, StyleLength.Points(20f));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    }

    public void cant_call_negative_measure_horizontal()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(10f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));

        FlexNode root_child0 = new(config);
        root_child0.SetMeasureFunc(MeasureAssertNegative);
        root_child0.Style.SetMargin(Edge.Start, StyleLength.Points(20f));
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    }

    public void percent_with_text_node()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.JustifyContent = Justify.SpaceBetween;
        root.Style.AlignItems = Align.Center;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(80f));

        FlexNode root_child0 = new(config);
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.SetMeasureFunc(Measure90x10);
        root_child1.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Percent(50f));
        root_child1.Style.SetPadding(Edge.Top, StyleLength.Percent(50f));
        root.InsertChild(root_child1, 1);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(80f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(40f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(50f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(10f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(60f);
    }

    public void percent_margin_with_measure_func()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(500f));

        FlexNode root_child0 = new(config);
        root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root_child0.Style.SetMargin(Edge.Top, StyleLength.Points(0f));
        root_child0.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root_child1.Style.SetMargin(Edge.Top, StyleLength.Points(100f));
        root_child1.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child1, 1);

        FlexNode root_child2 = new(config);
        root_child2.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root_child2.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root_child2.Style.SetMargin(Edge.Top, StyleLength.Percent(10f));
        root_child2.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child2, 2);

        FlexNode root_child3 = new(config);
        root_child3.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root_child3.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root_child3.Style.SetMargin(Edge.Top, StyleLength.Percent(20f));
        root_child3.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child3, 3);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(500f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(100f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

        root_child2.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(200f);
        root_child2.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(50f);
        root_child2.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child2.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

        root_child3.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(300f);
        root_child3.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);
        root_child3.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child3.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
    }

    public void percent_padding_with_measure_func()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.AlignItems = Align.FlexStart;
        root.Style.AlignContent = Align.FlexStart;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(500f));

        FlexNode root_child0 = new(config);
        root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root_child0.Style.SetPadding(Edge.Top, StyleLength.Points(0f));
        root_child0.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root_child1.Style.SetPadding(Edge.Top, StyleLength.Points(100f));
        root_child1.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child1, 1);

        FlexNode root_child2 = new(config);
        root_child2.Style.SetPadding(Edge.Top, StyleLength.Percent(10f));
        root_child2.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child2, 2);

        FlexNode root_child3 = new(config);
        root_child3.Style.SetPadding(Edge.Top, StyleLength.Percent(20f));
        root_child3.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child3, 3);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(500f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(100f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

        root_child2.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(200f);
        root_child2.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child2.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child2.Layout.GetDimension(Dimension.Height).ShouldBe(150f);

        root_child3.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(300f);
        root_child3.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child3.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child3.Layout.GetDimension(Dimension.Height).ShouldBe(200f);
    }

    public void percent_padding_and_percent_margin_with_measure_func()
    {
        FlexConfig config = new();

        FlexNode root = new(config);
        root.Style.FlexDirection = FlexDirection.Row;
        root.Style.AlignItems = Align.FlexStart;
        root.Style.AlignContent = Align.FlexStart;
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(500f));

        FlexNode root_child0 = new(config);
        root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root_child0.Style.SetPadding(Edge.Top, StyleLength.Points(0f));
        root_child0.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child0, 0);

        FlexNode root_child1 = new(config);
        root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root_child1.Style.SetPadding(Edge.Top, StyleLength.Points(100f));
        root_child1.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child1, 1);

        FlexNode root_child2 = new(config);
        root_child2.Style.SetPadding(Edge.Top, StyleLength.Percent(10f));
        root_child2.Style.SetMargin(Edge.Top, StyleLength.Percent(10f));
        root_child2.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child2, 2);

        FlexNode root_child3 = new(config);
        root_child3.Style.SetPadding(Edge.Top, StyleLength.Percent(20f));
        root_child3.Style.SetMargin(Edge.Top, StyleLength.Percent(20f));
        root_child3.SetMeasureFunc(Measure100x100);
        root.InsertChild(root_child3, 3);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(500f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

        root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(100f);
        root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

        root_child2.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(200f);
        root_child2.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(50f);
        root_child2.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child2.Layout.GetDimension(Dimension.Height).ShouldBe(150f);

        root_child3.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(300f);
        root_child3.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);
        root_child3.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child3.Layout.GetDimension(Dimension.Height).ShouldBe(200f);
    }

    public void measure_content_box()
    {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));
        root.Style.BoxSizing = BoxSizing.ContentBox;
        root.Style.SetPadding(Edge.All, StyleLength.Points(5f));
        root.Style.SetBorder(Edge.All, StyleLength.Points(10f));

        int measureCount = 0;

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
        {
            measureCount++;
            return new YGSize(0.5f * width, 0.5f * height);
        });
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        measureCount.ShouldBe(1);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(130f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(230f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(15f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(15f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
    }

    public void measure_border_box()
    {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));
        root.Style.BoxSizing = BoxSizing.BorderBox;
        root.Style.SetPadding(Edge.All, StyleLength.Points(5f));
        root.Style.SetBorder(Edge.All, StyleLength.Points(10f));

        int measureCount = 0;

        FlexNode root_child0 = new();
        root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
        {
            measureCount++;
            return new YGSize(0.5f * width, 0.5f * height);
        });
        root.InsertChild(root_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        measureCount.ShouldBe(1);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(15f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(15f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(70f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(85f);
    }

    public void min_width_larger_than_width_propagates_to_auto_parent()
    {
        FlexNode root = new();

        FlexNode root_child0 = new();
        root_child0.Style.FlexDirection = FlexDirection.Row;
        root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
        root.InsertChild(root_child0, 0);

        FlexNode root_child0_child0 = new();
        root_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
        root_child0_child0.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
        root_child0.InsertChild(root_child0_child0, 0);

        CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

        root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

        root_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
        root_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
        root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
        root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
    }
}
