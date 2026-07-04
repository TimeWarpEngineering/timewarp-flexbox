/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGHadOverflowTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for the HadOverflow layout flag, ported from YGHadOverflowTest.cpp.
/// </summary>
public class HadOverflowTests
{
  // Equivalent of the YogaTest_HadOverflowTests fixture setup.
  private static (FlexConfig Config, FlexNode Root) CreateFixture()
  {
    FlexConfig config = new();
    FlexNode root = new(config);
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root.Style.FlexDirection = FlexDirection.Column;
    root.Style.FlexWrap = Wrap.NoWrap;
    return (config, root);
  }

  public void children_overflow_no_wrap_and_no_flex_children()
  {
    (FlexConfig config, FlexNode root) = CreateFixture();

    FlexNode child0 = new(config);
    child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child0.Style.SetMargin(Edge.Top, StyleLength.Points(10f));
    child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(15f));
    root.InsertChild(child0, 0);
    FlexNode child1 = new(config);
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child1.Style.SetMargin(Edge.Bottom, StyleLength.Points(5f));
    root.InsertChild(child1, 1);

    CalculateLayout.Calculate(root, 200, 100, Direction.LTR);

    root.Layout.HadOverflow.ShouldBeTrue();
  }

  public void spacing_overflow_no_wrap_and_no_flex_children()
  {
    (FlexConfig config, FlexNode root) = CreateFixture();

    FlexNode child0 = new(config);
    child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child0.Style.SetMargin(Edge.Top, StyleLength.Points(10f));
    child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(10f));
    root.InsertChild(child0, 0);
    FlexNode child1 = new(config);
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child1.Style.SetMargin(Edge.Bottom, StyleLength.Points(5f));
    root.InsertChild(child1, 1);

    CalculateLayout.Calculate(root, 200, 100, Direction.LTR);

    root.Layout.HadOverflow.ShouldBeTrue();
  }

  public void no_overflow_no_wrap_and_flex_children()
  {
    (FlexConfig config, FlexNode root) = CreateFixture();

    FlexNode child0 = new(config);
    child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child0.Style.SetMargin(Edge.Top, StyleLength.Points(10f));
    child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(10f));
    root.InsertChild(child0, 0);
    FlexNode child1 = new(config);
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child1.Style.SetMargin(Edge.Bottom, StyleLength.Points(5f));
    child1.Style.FlexShrink = 1f;
    root.InsertChild(child1, 1);

    CalculateLayout.Calculate(root, 200, 100, Direction.LTR);

    root.Layout.HadOverflow.ShouldBeFalse();
  }

  public void hadOverflow_gets_reset_if_not_logger_valid()
  {
    (FlexConfig config, FlexNode root) = CreateFixture();

    FlexNode child0 = new(config);
    child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child0.Style.SetMargin(Edge.Top, StyleLength.Points(10f));
    child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(10f));
    root.InsertChild(child0, 0);
    FlexNode child1 = new(config);
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child1.Style.SetMargin(Edge.Bottom, StyleLength.Points(5f));
    root.InsertChild(child1, 1);

    CalculateLayout.Calculate(root, 200, 100, Direction.LTR);

    root.Layout.HadOverflow.ShouldBeTrue();

    child1.Style.FlexShrink = 1f;

    CalculateLayout.Calculate(root, 200, 100, Direction.LTR);

    root.Layout.HadOverflow.ShouldBeFalse();
  }

  public void spacing_overflow_in_nested_nodes()
  {
    (FlexConfig config, FlexNode root) = CreateFixture();

    FlexNode child0 = new(config);
    child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child0.Style.SetMargin(Edge.Top, StyleLength.Points(10f));
    child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(10f));
    root.InsertChild(child0, 0);
    FlexNode child1 = new(config);
    child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    root.InsertChild(child1, 1);
    FlexNode child1_1 = new(config);
    child1_1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(80f));
    child1_1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    child1_1.Style.SetMargin(Edge.Bottom, StyleLength.Points(5f));
    child1.InsertChild(child1_1, 0);

    CalculateLayout.Calculate(root, 200, 100, Direction.LTR);

    root.Layout.HadOverflow.ShouldBeTrue();
  }
}
