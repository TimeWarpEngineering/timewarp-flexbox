/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGRoundingMeasureFuncTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for rounding of measure function results, ported from YGRoundingMeasureFuncTest.cpp.
/// </summary>
public class RoundingMeasureFuncTests
{
  private static YGSize MeasureFloor(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
  {
    return new YGSize(10.2f, 10.2f);
  }

  private static YGSize MeasureCeil(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
  {
    return new YGSize(10.5f, 10.5f);
  }

  private static YGSize MeasureFractial(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
  {
    return new YGSize(0.5f, 0.5f);
  }

  public void rounding_feature_with_custom_measure_func_floor()
  {
    FlexConfig config = new();
    FlexNode root = new(config);

    FlexNode root_child0 = new(config);
    root_child0.SetMeasureFunc(MeasureFloor);
    root.InsertChild(root_child0, 0);

    config.PointScaleFactor = 0.0f;

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(10.2f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10.2f);

    config.PointScaleFactor = 1.0f;

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(11f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(11f);

    config.PointScaleFactor = 2.0f;

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(10.5f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10.5f);

    config.PointScaleFactor = 4.0f;

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(10.25f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10.25f);

    config.PointScaleFactor = 1.0f / 3.0f;

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(12.0f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(12.0f);
  }

  public void rounding_feature_with_custom_measure_func_ceil()
  {
    FlexConfig config = new();
    FlexNode root = new(config);

    FlexNode root_child0 = new(config);
    root_child0.SetMeasureFunc(MeasureCeil);
    root.InsertChild(root_child0, 0);

    config.PointScaleFactor = 1.0f;

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(11f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(11f);
  }

  public void rounding_feature_with_custom_measure_and_fractial_matching_scale()
  {
    FlexConfig config = new();
    FlexNode root = new(config);
    root.Style.PositionType = PositionType.Absolute;

    FlexNode root_child0 = new(config);
    root_child0.Style.SetPosition(Edge.Left, StyleLength.Points(73.625f));
    root_child0.Style.PositionType = PositionType.Relative;
    root_child0.SetMeasureFunc(MeasureFractial);
    root.InsertChild(root_child0, 0);

    config.PointScaleFactor = 2.0f;

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0.5f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0.5f);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(73.5f);
  }
}
