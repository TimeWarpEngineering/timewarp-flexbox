/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGRoundingFunctionTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for pixel grid rounding, ported from YGRoundingFunctionTest.cpp.
/// </summary>
public class RoundingFunctionTests
{
  public void rounding_value()
  {
    // Test that whole numbers are rounded to whole despite ceil/floor flags
    PixelGrid.RoundValueToPixelGrid(6.000001, 2.0, false, false).ShouldBe(6.0f);
    PixelGrid.RoundValueToPixelGrid(6.000001, 2.0, true, false).ShouldBe(6.0f);
    PixelGrid.RoundValueToPixelGrid(6.000001, 2.0, false, true).ShouldBe(6.0f);
    PixelGrid.RoundValueToPixelGrid(5.999999, 2.0, false, false).ShouldBe(6.0f);
    PixelGrid.RoundValueToPixelGrid(5.999999, 2.0, true, false).ShouldBe(6.0f);
    PixelGrid.RoundValueToPixelGrid(5.999999, 2.0, false, true).ShouldBe(6.0f);
    // Same tests for negative numbers
    PixelGrid.RoundValueToPixelGrid(-6.000001, 2.0, false, false).ShouldBe(-6.0f);
    PixelGrid.RoundValueToPixelGrid(-6.000001, 2.0, true, false).ShouldBe(-6.0f);
    PixelGrid.RoundValueToPixelGrid(-6.000001, 2.0, false, true).ShouldBe(-6.0f);
    PixelGrid.RoundValueToPixelGrid(-5.999999, 2.0, false, false).ShouldBe(-6.0f);
    PixelGrid.RoundValueToPixelGrid(-5.999999, 2.0, true, false).ShouldBe(-6.0f);
    PixelGrid.RoundValueToPixelGrid(-5.999999, 2.0, false, true).ShouldBe(-6.0f);

    // Test that numbers with fraction are rounded correctly accounting for
    // ceil/floor flags
    PixelGrid.RoundValueToPixelGrid(6.01, 2.0, false, false).ShouldBe(6.0f);
    PixelGrid.RoundValueToPixelGrid(6.01, 2.0, true, false).ShouldBe(6.5f);
    PixelGrid.RoundValueToPixelGrid(6.01, 2.0, false, true).ShouldBe(6.0f);
    PixelGrid.RoundValueToPixelGrid(5.99, 2.0, false, false).ShouldBe(6.0f);
    PixelGrid.RoundValueToPixelGrid(5.99, 2.0, true, false).ShouldBe(6.0f);
    PixelGrid.RoundValueToPixelGrid(5.99, 2.0, false, true).ShouldBe(5.5f);
    // Same tests for negative numbers
    PixelGrid.RoundValueToPixelGrid(-6.01, 2.0, false, false).ShouldBe(-6.0f);
    PixelGrid.RoundValueToPixelGrid(-6.01, 2.0, true, false).ShouldBe(-6.0f);
    PixelGrid.RoundValueToPixelGrid(-6.01, 2.0, false, true).ShouldBe(-6.5f);
    PixelGrid.RoundValueToPixelGrid(-5.99, 2.0, false, false).ShouldBe(-6.0f);
    PixelGrid.RoundValueToPixelGrid(-5.99, 2.0, true, false).ShouldBe(-5.5f);
    PixelGrid.RoundValueToPixelGrid(-5.99, 2.0, false, true).ShouldBe(-6.0f);

    // Rounding up/down halfway values is as expected for both positive and
    // negative numbers
    PixelGrid.RoundValueToPixelGrid(-3.5, 1.0, false, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.4, 1.0, false, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.6, 1.0, false, false).ShouldBe(-4f);
    PixelGrid.RoundValueToPixelGrid(-3.499999, 1.0, false, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.500001, 1.0, false, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.5001, 1.0, false, false).ShouldBe(-4f);

    PixelGrid.RoundValueToPixelGrid(-3.5, 1.0, true, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.4, 1.0, true, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.6, 1.0, true, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.499999, 1.0, true, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.500001, 1.0, true, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.5001, 1.0, true, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3.00001, 1.0, true, false).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3, 1.0, true, false).ShouldBe(-3f);

    PixelGrid.RoundValueToPixelGrid(-3.5, 1.0, false, true).ShouldBe(-4f);
    PixelGrid.RoundValueToPixelGrid(-3.4, 1.0, false, true).ShouldBe(-4f);
    PixelGrid.RoundValueToPixelGrid(-3.6, 1.0, false, true).ShouldBe(-4f);
    PixelGrid.RoundValueToPixelGrid(-3.499999, 1.0, false, true).ShouldBe(-4f);
    PixelGrid.RoundValueToPixelGrid(-3.500001, 1.0, false, true).ShouldBe(-4f);
    PixelGrid.RoundValueToPixelGrid(-3.5001, 1.0, false, true).ShouldBe(-4f);
    PixelGrid.RoundValueToPixelGrid(-3.00001, 1.0, false, true).ShouldBe(-3f);
    PixelGrid.RoundValueToPixelGrid(-3, 1.0, false, true).ShouldBe(-3f);

    // NAN is treated as expected:
    float.IsNaN(PixelGrid.RoundValueToPixelGrid(double.NaN, 1.5, false, false)).ShouldBeTrue();
    float.IsNaN(PixelGrid.RoundValueToPixelGrid(1.5, double.NaN, false, false)).ShouldBeTrue();
    float.IsNaN(PixelGrid.RoundValueToPixelGrid(double.NaN, double.NaN, false, false)).ShouldBeTrue();
  }

  // Regression test for https://github.com/facebook/yoga/issues/824
  public void consistent_rounding_during_repeated_layouts()
  {
    FlexConfig config = new();
    config.PointScaleFactor = 2f;

    FlexNode root = new(config);
    root.Style.SetMargin(Edge.Top, StyleLength.Points(-1.49f));
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(500f));

    FlexNode node0 = new(config);
    root.InsertChild(node0, 0);

    FlexNode node1 = new(config);
    node1.SetMeasureFunc((node, width, widthMode, height, heightMode) => new YGSize(10, 10));
    node0.InsertChild(node1, 0);

    for (int i = 0; i < 5; i++)
    {
      // Dirty the tree so RoundLayoutResultsToPixelGrid runs again
      root.Style.SetMargin(Edge.Left, StyleLength.Points(i + 1));

      CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
      node1.Layout.GetDimension(Dimension.Height).ShouldBe(10f);
    }
  }

  public void per_node_point_scale_factor()
  {
    FlexConfig config1 = new();
    config1.PointScaleFactor = 2f;

    FlexConfig config2 = new();
    config2.PointScaleFactor = 1f;

    FlexConfig config3 = new();
    config3.PointScaleFactor = 0.5f;

    FlexNode root = new(config1);
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(11.5f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(11.5f));

    FlexNode node0 = new(config2);
    node0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(9.5f));
    node0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(9.5f));
    root.InsertChild(node0, 0);

    FlexNode node1 = new(config3);
    node1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(7f));
    node1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(7f));
    node0.InsertChild(node1, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Width).ShouldBe(11.5f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(11.5f);

    node0.Layout.GetDimension(Dimension.Width).ShouldBe(10f);
    node0.Layout.GetDimension(Dimension.Height).ShouldBe(10f);

    node1.Layout.GetDimension(Dimension.Width).ShouldBe(8f);
    node1.Layout.GetDimension(Dimension.Height).ShouldBe(8f);
  }

  public void raw_layout_dimensions()
  {
    FlexConfig config = new();
    config.PointScaleFactor = 0.5f;

    FlexNode root = new(config);
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(11.5f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(9.5f));

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Width).ShouldBe(12.0f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(10.0f);
    root.Layout.GetRawDimension(Dimension.Width).ShouldBe(11.5f);
    root.Layout.GetRawDimension(Dimension.Height).ShouldBe(9.5f);
  }
}
