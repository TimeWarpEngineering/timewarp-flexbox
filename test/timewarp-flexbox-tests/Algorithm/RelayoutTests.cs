/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGRelayoutTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for relayout behavior, ported from YGRelayoutTest.cpp.
/// </summary>
public class RelayoutTests
{
  public void dont_cache_computed_flex_basis_between_layouts()
  {
    FlexConfig config = new();
    config.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, true);

    FlexNode root = new(config);
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Percent(100f));
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Percent(100f));

    FlexNode root_child0 = new(config);
    root_child0.Style.FlexBasis = StyleSizeLength.Percent(100f);
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, 100f, float.NaN, Direction.LTR);
    CalculateLayout.Calculate(root, 100f, 100f, Direction.LTR);

    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(100f);
  }

  public void recalculate_resolvedDimonsion_onchange()
  {
    FlexNode root = new();

    FlexNode root_child0 = new();
    root_child0.Style.SetMinDimension(Dimension.Height, StyleSizeLength.Points(10f));
    root_child0.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(10f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(10f);

    root_child0.Style.SetMinDimension(Dimension.Height, StyleSizeLength.Undefined);
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(0f);
  }

  public void relayout_containing_block_size_changes()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.PositionType = PositionType.Absolute;

    FlexNode root_child0 = new(config);
    root_child0.Style.PositionType = PositionType.Relative;
    root_child0.Style.SetMargin(Edge.Left, StyleLength.Points(4f));
    root_child0.Style.SetMargin(Edge.Top, StyleLength.Points(5f));
    root_child0.Style.SetMargin(Edge.Right, StyleLength.Points(9f));
    root_child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(1f));
    root_child0.Style.SetPadding(Edge.Left, StyleLength.Points(2f));
    root_child0.Style.SetPadding(Edge.Top, StyleLength.Points(9f));
    root_child0.Style.SetPadding(Edge.Right, StyleLength.Points(11f));
    root_child0.Style.SetPadding(Edge.Bottom, StyleLength.Points(13f));
    root_child0.Style.SetBorder(Edge.Left, StyleLength.Points(5f));
    root_child0.Style.SetBorder(Edge.Top, StyleLength.Points(6f));
    root_child0.Style.SetBorder(Edge.Right, StyleLength.Points(7f));
    root_child0.Style.SetBorder(Edge.Bottom, StyleLength.Points(8f));
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(500f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child0_child0 = new(config);
    root_child0_child0.Style.PositionType = PositionType.Static;
    root_child0_child0.Style.SetMargin(Edge.Left, StyleLength.Points(8f));
    root_child0_child0.Style.SetMargin(Edge.Top, StyleLength.Points(6f));
    root_child0_child0.Style.SetMargin(Edge.Right, StyleLength.Points(3f));
    root_child0_child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(9f));
    root_child0_child0.Style.SetPadding(Edge.Left, StyleLength.Points(1f));
    root_child0_child0.Style.SetPadding(Edge.Top, StyleLength.Points(7f));
    root_child0_child0.Style.SetPadding(Edge.Right, StyleLength.Points(9f));
    root_child0_child0.Style.SetPadding(Edge.Bottom, StyleLength.Points(4f));
    root_child0_child0.Style.SetBorder(Edge.Left, StyleLength.Points(8f));
    root_child0_child0.Style.SetBorder(Edge.Top, StyleLength.Points(10f));
    root_child0_child0.Style.SetBorder(Edge.Right, StyleLength.Points(2f));
    root_child0_child0.Style.SetBorder(Edge.Bottom, StyleLength.Points(1f));
    root_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200f));
    root_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));
    root_child0.InsertChild(root_child0_child0, 0);

    FlexNode root_child0_child0_child0 = new(config);
    root_child0_child0_child0.Style.PositionType = PositionType.Absolute;
    root_child0_child0_child0.Style.SetPosition(Edge.Left, StyleLength.Points(2f));
    root_child0_child0_child0.Style.SetPosition(Edge.Right, StyleLength.Points(12f));
    root_child0_child0_child0.Style.SetMargin(Edge.Left, StyleLength.Points(9f));
    root_child0_child0_child0.Style.SetMargin(Edge.Top, StyleLength.Points(12f));
    root_child0_child0_child0.Style.SetMargin(Edge.Right, StyleLength.Points(4f));
    root_child0_child0_child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(7f));
    root_child0_child0_child0.Style.SetPadding(Edge.Left, StyleLength.Points(5f));
    root_child0_child0_child0.Style.SetPadding(Edge.Top, StyleLength.Points(3f));
    root_child0_child0_child0.Style.SetPadding(Edge.Right, StyleLength.Points(8f));
    root_child0_child0_child0.Style.SetPadding(Edge.Bottom, StyleLength.Points(10f));
    root_child0_child0_child0.Style.SetBorder(Edge.Left, StyleLength.Points(2f));
    root_child0_child0_child0.Style.SetBorder(Edge.Top, StyleLength.Points(1f));
    root_child0_child0_child0.Style.SetBorder(Edge.Right, StyleLength.Points(5f));
    root_child0_child0_child0.Style.SetBorder(Edge.Bottom, StyleLength.Points(9f));
    root_child0_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Percent(41f));
    root_child0_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Percent(63f));
    root_child0_child0.InsertChild(root_child0_child0_child0, 0);
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(513f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(506f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(4f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(5f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(500f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

    root_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(15f);
    root_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(21f);
    root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(1f);
    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(29f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(306f);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(513f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(506f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(4f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(5f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(500f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

    root_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(279f);
    root_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(21f);
    root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(-2f);
    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(29f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(306f);

    // Relayout starts here
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(456f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(432f));

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(469f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(438f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(4f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(5f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(456f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(432f);

    root_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(15f);
    root_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(21f);
    root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(1f);
    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(29f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(182f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(263f);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(469f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(438f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(4f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(5f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(456f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(432f);

    root_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(235f);
    root_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(21f);
    root_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(200f);
    root_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(16f);
    root_child0_child0_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(29f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Width).ShouldBe(182f);
    root_child0_child0_child0.Layout.GetDimension(Dimension.Height).ShouldBe(263f);
  }

  public void has_new_layout_flag_set_static()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.PositionType = PositionType.Static;
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(10f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(10f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child0_child1 = new();
    root_child0_child1.Style.PositionType = PositionType.Absolute;
    root_child0_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(5f));
    root_child0_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(5f));
    root_child0.InsertChild(root_child0_child1, 0);

    FlexNode root_child0_child0 = new();
    root_child0_child0.Style.PositionType = PositionType.Static;
    root_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(5f));
    root_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(5f));
    root_child0.InsertChild(root_child0_child0, 1);

    FlexNode root_child0_child0_child0 = new();
    root_child0_child0_child0.Style.PositionType = PositionType.Absolute;
    root_child0_child0_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Percent(1f));
    root_child0_child0_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(1f));
    root_child0_child0.InsertChild(root_child0_child0_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root.HasNewLayout = false;
    root_child0.HasNewLayout = false;
    root_child0_child0.HasNewLayout = false;
    root_child0_child0_child0.HasNewLayout = false;

    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(110f));
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.HasNewLayout.ShouldBeTrue();
    root_child0.HasNewLayout.ShouldBeTrue();
    root_child0_child0.HasNewLayout.ShouldBeTrue();
    root_child0_child0_child0.HasNewLayout.ShouldBeTrue();
  }
}
