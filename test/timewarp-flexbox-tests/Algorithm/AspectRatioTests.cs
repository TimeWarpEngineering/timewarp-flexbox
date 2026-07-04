/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGAspectRatioTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Hand-written aspect ratio tests, ported from YGAspectRatioTest.cpp.
/// </summary>
public class AspectRatioTests
{
  private static YGSize Measure(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
  {
    return new YGSize(
        widthMode == MeasureMode.Exactly ? width : 50,
        heightMode == MeasureMode.Exactly ? height : 50);
  }

  public void aspect_ratio_cross_defined()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_main_defined()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_both_dimensions_defined_row()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_both_dimensions_defined_column()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_align_stretch()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_flex_grow()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_flex_shrink()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(150f));
    root_child0.Style.FlexShrink = 1f;
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_flex_shrink_2()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Percent(100f));
    root_child0.Style.FlexShrink = 1f;
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new();
    root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Percent(100f));
    root_child1.Style.FlexShrink = 1f;
    root_child1.Style.AspectRatio = 1f;
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(50f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_basis()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.FlexBasis = StyleSizeLength.Points(50f);
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_absolute_layout_width_defined()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.PositionType = PositionType.Absolute;
    root_child0.Style.SetPosition(Edge.Left, StyleLength.Points(0f));
    root_child0.Style.SetPosition(Edge.Top, StyleLength.Points(0f));
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_absolute_layout_height_defined()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.PositionType = PositionType.Absolute;
    root_child0.Style.SetPosition(Edge.Left, StyleLength.Points(0f));
    root_child0.Style.SetPosition(Edge.Top, StyleLength.Points(0f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_with_max_cross_defined()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(40f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(40f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_with_max_main_defined()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(40f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(40f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(40f);
  }

  public void aspect_ratio_with_min_cross_defined()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(30f));
    root_child0.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(40f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(40f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(30f);
  }

  public void aspect_ratio_with_min_main_defined()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(30f));
    root_child0.Style.SetMinDimension(Dimension.Height, StyleSizeLength.Points(40f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(40f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(40f);
  }

  public void aspect_ratio_double_cross()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 2f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_half_cross()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root_child0.Style.AspectRatio = 0.5f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_double_main()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 0.5f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_half_main()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root_child0.Style.AspectRatio = 2f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_with_measure_func()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.SetMeasureFunc(Measure);
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_width_height_flex_grow_row()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_width_height_flex_grow_column()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_height_as_flex_basis()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new();
    root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root_child1.Style.FlexGrow = 1f;
    root_child1.Style.AspectRatio = 1f;
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(75f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(75f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(75f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(125f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(125f);
  }

  public void aspect_ratio_width_as_flex_basis()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new();
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root_child1.Style.FlexGrow = 1f;
    root_child1.Style.AspectRatio = 1f;
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(75f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(75f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(75f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(125f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(125f);
  }

  public void aspect_ratio_overrides_flex_grow_row()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.AspectRatio = 0.5f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(200f);
  }

  public void aspect_ratio_overrides_flex_grow_column()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.AspectRatio = 2f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_left_right_absolute()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.PositionType = PositionType.Absolute;
    root_child0.Style.SetPosition(Edge.Left, StyleLength.Points(10f));
    root_child0.Style.SetPosition(Edge.Top, StyleLength.Points(10f));
    root_child0.Style.SetPosition(Edge.Right, StyleLength.Points(10f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(10f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(80f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(80f);
  }

  public void aspect_ratio_top_bottom_absolute()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.PositionType = PositionType.Absolute;
    root_child0.Style.SetPosition(Edge.Left, StyleLength.Points(10f));
    root_child0.Style.SetPosition(Edge.Top, StyleLength.Points(10f));
    root_child0.Style.SetPosition(Edge.Bottom, StyleLength.Points(10f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(10f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(80f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(80f);
  }

  public void aspect_ratio_width_overrides_align_stretch_row()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_height_overrides_align_stretch_column()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_allow_child_overflow_parent_size()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 4f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_defined_main_with_margin()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.Center;
    root.Style.JustifyContent = Justify.Center;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root_child0.Style.SetMargin(Edge.Left, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Right, StyleLength.Points(10f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_defined_cross_with_margin()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.Center;
    root.Style.JustifyContent = Justify.Center;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root_child0.Style.SetMargin(Edge.Left, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Right, StyleLength.Points(10f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_defined_cross_with_main_margin()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.Center;
    root.Style.JustifyContent = Justify.Center;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.AspectRatio = 1f;
    root_child0.Style.SetMargin(Edge.Top, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(10f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void aspect_ratio_should_prefer_explicit_height()
  {
    FlexConfig config = new() { UseWebDefaults = true };

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Column;

    FlexNode root_child0 = new(config);
    root_child0.Style.FlexDirection = FlexDirection.Column;
    root.InsertChild(root_child0, 0);

    FlexNode root_child0_child0 = new(config);
    root_child0_child0.Style.FlexDirection = FlexDirection.Column;
    root_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root_child0_child0.Style.AspectRatio = 2f;
    root_child0.InsertChild(root_child0_child0, 0);

    CalculateLayout.Calculate(root, 100, 200, Direction.LTR);

    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void aspect_ratio_should_prefer_explicit_width()
  {
    FlexConfig config = new() { UseWebDefaults = true };

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;

    FlexNode root_child0 = new(config);
    root_child0.Style.FlexDirection = FlexDirection.Row;
    root.InsertChild(root_child0, 0);

    FlexNode root_child0_child0 = new(config);
    root_child0_child0.Style.FlexDirection = FlexDirection.Row;
    root_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root_child0_child0.Style.AspectRatio = 0.5f;
    root_child0.InsertChild(root_child0_child0, 0);

    CalculateLayout.Calculate(root, 200, 100, Direction.LTR);

    root.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(200f);
  }

  public void aspect_ratio_should_prefer_flexed_dimension()
  {
    FlexConfig config = new() { UseWebDefaults = true };

    FlexNode root = new(config);

    FlexNode root_child0 = new(config);
    root_child0.Style.FlexDirection = FlexDirection.Column;
    root_child0.Style.AspectRatio = 2f;
    root_child0.Style.FlexGrow = 1f;
    root.InsertChild(root_child0, 0);

    FlexNode root_child0_child0 = new(config);
    root_child0_child0.Style.AspectRatio = 4f;
    root_child0_child0.Style.FlexGrow = 1f;
    root_child0.InsertChild(root_child0_child0, 0);

    CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

    root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }
}
