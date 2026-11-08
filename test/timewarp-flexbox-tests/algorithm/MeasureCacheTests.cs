/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGMeasureCacheTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for measure caching, ported from YGMeasureCacheTest.cpp.
/// </summary>
public class MeasureCacheTests
{
  private static MeasureFunc MeasureMax(Counter counter)
  {
    return (node, width, widthMode, height, heightMode) =>
    {
      counter.Count++;

      return new YGSize(
              widthMode == MeasureMode.Undefined ? 10 : width,
              heightMode == MeasureMode.Undefined ? 10 : height);
    };
  }

  private static MeasureFunc MeasureMin(Counter counter)
  {
    return (node, width, widthMode, height, heightMode) =>
    {
      counter.Count++;
      return new YGSize(
              widthMode == MeasureMode.Undefined ||
                      (widthMode == MeasureMode.AtMost && width > 10)
                  ? 10
                  : width,
              heightMode == MeasureMode.Undefined ||
                      (heightMode == MeasureMode.AtMost && height > 10)
                  ? 10
                  : height);
    };
  }

  private static MeasureFunc Measure84x49(Counter counter)
  {
    return (node, width, widthMode, height, heightMode) =>
    {
      counter.Count++;

      return new YGSize(84f, 49f);
    };
  }

  private sealed class Counter
  {
    public int Count;
  }

  public void measure_once_single_flexible_child()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    Counter measureCount = new();
    root_child0.SetMeasureFunc(MeasureMax(measureCount));
    root_child0.Style.FlexGrow = 1f;
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    measureCount.Count.ShouldBe(1);
  }

  public void remeasure_with_same_exact_width_larger_than_needed_height()
  {
    FlexNode root = new();

    FlexNode root_child0 = new();
    Counter measureCount = new();
    root_child0.SetMeasureFunc(MeasureMin(measureCount));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, 100f, 100f, Direction.LTR);
    CalculateLayout.Calculate(root, 100f, 50f, Direction.LTR);

    measureCount.Count.ShouldBe(1);
  }

  public void remeasure_with_same_atmost_width_larger_than_needed_height()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;

    FlexNode root_child0 = new();
    Counter measureCount = new();
    root_child0.SetMeasureFunc(MeasureMin(measureCount));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, 100f, 100f, Direction.LTR);
    CalculateLayout.Calculate(root, 100f, 50f, Direction.LTR);

    measureCount.Count.ShouldBe(1);
  }

  public void remeasure_with_computed_width_larger_than_needed_height()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;

    FlexNode root_child0 = new();
    Counter measureCount = new();
    root_child0.SetMeasureFunc(MeasureMin(measureCount));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, 100f, 100f, Direction.LTR);
    root.Style.AlignItems = Align.Stretch;
    CalculateLayout.Calculate(root, 10f, 50f, Direction.LTR);

    measureCount.Count.ShouldBe(1);
  }

  public void remeasure_with_atmost_computed_width_undefined_height()
  {
    FlexNode root = new();
    root.Style.AlignItems = Align.FlexStart;

    FlexNode root_child0 = new();
    Counter measureCount = new();
    root_child0.SetMeasureFunc(MeasureMin(measureCount));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, 100f, float.NaN, Direction.LTR);
    CalculateLayout.Calculate(root, 10f, float.NaN, Direction.LTR);

    measureCount.Count.ShouldBe(1);
  }

  public void remeasure_with_already_measured_value_smaller_but_still_float_equal()
  {
    Counter measureCount = new();

    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(288f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(288f));
    root.Style.FlexDirection = FlexDirection.Row;

    FlexNode root_child0 = new();
    root_child0.Style.SetPadding(Edge.All, StyleLength.Points(2.88f));
    root_child0.Style.FlexDirection = FlexDirection.Row;
    root.InsertChild(root_child0, 0);

    FlexNode root_child0_child0 = new();
    root_child0_child0.SetMeasureFunc(Measure84x49(measureCount));
    root_child0.InsertChild(root_child0_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    measureCount.Count.ShouldBe(1);
  }
}
