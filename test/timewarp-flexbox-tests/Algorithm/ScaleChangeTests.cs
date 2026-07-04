/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGScaleChangeTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for layout invalidation on config changes, ported from YGScaleChangeTest.cpp.
/// </summary>
public class ScaleChangeTests
{
  public void scale_change_invalidates_layout()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    config.PointScaleFactor = 1.0f;

    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

    FlexNode root_child0 = new(config);
    root_child0.Style.FlexGrow = 1f;
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.FlexGrow = 1f;
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);

    config.PointScaleFactor = 1.5f;
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    // Left should change due to pixel alignment of new scale factor
    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25.333334f);
  }

  public void errata_config_change_relayout()
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
    root_child0_child0.Style.FlexGrow = 1f;
    root_child0_child0.Style.FlexShrink = 1f;
    root_child0.InsertChild(root_child0_child0, 0);

    FlexNode root_child0_child0_child0 = new(config);
    root_child0_child0_child0.Style.FlexGrow = 1f;
    root_child0_child0_child0.Style.FlexShrink = 1f;
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

    config.SetErrata(Errata.None);
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(500f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(500f);
    // This should be modified by the lack of the errata
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0f);

    root_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    // This should be modified by the lack of the errata
    root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0f);

    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(0f);
    // This should be modified by the lack of the errata
    root_child0_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0f);
  }

  public void setting_compatible_config_maintains_layout_cache()
  {
    int measureCallCount = 0;

    FlexConfig config = new();

    FlexNode root = new(config);
    config.PointScaleFactor = 1.0f;

    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));

    FlexNode root_child0 = new(config);
    measureCallCount.ShouldBe(0);

    root_child0.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
    {
      measureCallCount++;
      return new YGSize(25.0f, 25.0f);
    });
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.FlexGrow = 1f;
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    measureCallCount.ShouldBe(1);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);

    FlexConfig config2 = new();
    // Calling the PointScaleFactor setter multiple times ensures that config2
    // gets a different config version than config1
    config2.PointScaleFactor = 1.0f;
    config2.PointScaleFactor = 1.5f;
    config2.PointScaleFactor = 1.0f;

    root.SetConfig(config2);
    root_child0.SetConfig(config2);
    root_child1.SetConfig(config2);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // Measure should not be called again, as layout should have been cached since
    // config is functionally the same as before
    measureCallCount.ShouldBe(1);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(25f);
  }
}
