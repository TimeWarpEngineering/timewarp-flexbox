namespace TimeWarp.Flexbox.Tests.Values;

/// <summary>
/// Tests for EdgeValues struct indexer operations.
/// </summary>
public class EdgeValuesIndexerTests
{
  private static readonly FlexValue DefaultValue = FlexValue.Undefined;

  #region Individual Edges

  public void ShouldSetAndGetLeftEdge()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Left] = FlexValue.Point(10);

    edges[Edge.Left].ShouldBe(FlexValue.Point(10));
    edges.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(10));
  }

  public void ShouldSetAndGetTopEdge()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Top] = FlexValue.Point(20);

    edges[Edge.Top].ShouldBe(FlexValue.Point(20));
    edges.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(20));
  }

  public void ShouldSetAndGetRightEdge()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Right] = FlexValue.Point(30);

    edges[Edge.Right].ShouldBe(FlexValue.Point(30));
    edges.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(30));
  }

  public void ShouldSetAndGetBottomEdge()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Bottom] = FlexValue.Point(40);

    edges[Edge.Bottom].ShouldBe(FlexValue.Point(40));
    edges.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(40));
  }

  #endregion

  #region Group Edges

  public void ShouldSetAllEdgesViaEdgeAll()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);

    edges.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(10));
  }

  public void ShouldSetHorizontalEdgesViaEdgeHorizontal()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Horizontal] = FlexValue.Point(15);

    edges.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(15));
    edges.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(15));
    edges.ComputedTop(DefaultValue).ShouldBe(DefaultValue);
    edges.ComputedBottom(DefaultValue).ShouldBe(DefaultValue);
  }

  public void ShouldSetVerticalEdgesViaEdgeVertical()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Vertical] = FlexValue.Point(25);

    edges.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(25));
    edges.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(25));
    edges.ComputedLeft(DefaultValue).ShouldBe(DefaultValue);
    edges.ComputedRight(DefaultValue).ShouldBe(DefaultValue);
  }

  #endregion

  #region Logical Edges (Start/End)

  public void ShouldSetStartEdge()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Start] = FlexValue.Point(10);

    edges[Edge.Start].ShouldBe(FlexValue.Point(10));
  }

  public void ShouldSetEndEdge()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.End] = FlexValue.Point(20);

    edges[Edge.End].ShouldBe(FlexValue.Point(20));
  }

  #endregion
}

/// <summary>
/// Tests for EdgeValues cascade resolution.
/// </summary>
public class EdgeValuesCascadeTests
{
  private static readonly FlexValue DefaultValue = FlexValue.Undefined;

  #region Cascade Precedence

  public void ShouldAllowSpecificEdgeToOverrideAll()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    edges[Edge.Left] = FlexValue.Point(20);

    edges.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(20));
    edges.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(10));
  }

  public void ShouldResolveHorizontalOverAllButNotSpecific()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    edges[Edge.Horizontal] = FlexValue.Point(20);
    edges[Edge.Left] = FlexValue.Point(30);

    edges.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(30));   // Specific wins
    edges.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(20));  // Horizontal wins over All
    edges.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(10));    // All
    edges.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(10)); // All
  }

  public void ShouldResolveVerticalOverAll()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    edges[Edge.Vertical] = FlexValue.Point(20);

    edges.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(20));
    edges.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(20));
    edges.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(10));
  }

  public void ShouldAllowSpecificToOverrideVertical()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Vertical] = FlexValue.Point(10);
    edges[Edge.Top] = FlexValue.Point(20);

    edges.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(20));
    edges.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(10));
  }

  public void ShouldAllowSpecificToOverrideHorizontal()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Horizontal] = FlexValue.Point(10);
    edges[Edge.Right] = FlexValue.Point(20);

    edges.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(20));
  }

  #endregion

  #region RTL Support

  public void ShouldMapStartToLeftInLtr()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Start] = FlexValue.Point(10);

    edges.ComputedLeft(DefaultValue, isRtl: false).ShouldBe(FlexValue.Point(10));
    edges.ComputedRight(DefaultValue, isRtl: false).ShouldBe(DefaultValue);
  }

  public void ShouldMapEndToRightInLtr()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.End] = FlexValue.Point(10);

    edges.ComputedRight(DefaultValue, isRtl: false).ShouldBe(FlexValue.Point(10));
    edges.ComputedLeft(DefaultValue, isRtl: false).ShouldBe(DefaultValue);
  }

  public void ShouldMapStartToRightInRtl()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Start] = FlexValue.Point(10);

    edges.ComputedRight(DefaultValue, isRtl: true).ShouldBe(FlexValue.Point(10));
    edges.ComputedLeft(DefaultValue, isRtl: true).ShouldBe(DefaultValue);
  }

  public void ShouldMapEndToLeftInRtl()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.End] = FlexValue.Point(10);

    edges.ComputedLeft(DefaultValue, isRtl: true).ShouldBe(FlexValue.Point(10));
    edges.ComputedRight(DefaultValue, isRtl: true).ShouldBe(DefaultValue);
  }

  public void ShouldAllowSpecificToOverrideLogicalInLtr()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Start] = FlexValue.Point(10);
    edges[Edge.Left] = FlexValue.Point(20);

    edges.ComputedLeft(DefaultValue, isRtl: false).ShouldBe(FlexValue.Point(20));
  }

  public void ShouldAllowSpecificToOverrideLogicalInRtl()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.End] = FlexValue.Point(10);
    edges[Edge.Left] = FlexValue.Point(20);

    edges.ComputedLeft(DefaultValue, isRtl: true).ShouldBe(FlexValue.Point(20));
  }

  public void ShouldResolveLogicalOverHorizontal()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.Horizontal] = FlexValue.Point(10);
    edges[Edge.Start] = FlexValue.Point(20);

    edges.ComputedLeft(DefaultValue, isRtl: false).ShouldBe(FlexValue.Point(20));
    edges.ComputedRight(DefaultValue, isRtl: false).ShouldBe(FlexValue.Point(10));
  }

  #endregion

  #region Default Values

  public void ShouldReturnDefaultWhenNoEdgeSet()
  {
    EdgeValues<FlexValue> edges = new();
    FlexValue defaultValue = FlexValue.Point(99);

    edges.ComputedLeft(defaultValue).ShouldBe(defaultValue);
    edges.ComputedRight(defaultValue).ShouldBe(defaultValue);
    edges.ComputedTop(defaultValue).ShouldBe(defaultValue);
    edges.ComputedBottom(defaultValue).ShouldBe(defaultValue);
  }

  public void ShouldReturnDefaultForUnsetEdgesWhenOthersAreSet()
  {
    EdgeValues<FlexValue> edges = new();
    FlexValue defaultValue = FlexValue.Point(99);
    edges[Edge.Left] = FlexValue.Point(10);

    edges.ComputedLeft(defaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedRight(defaultValue).ShouldBe(defaultValue);
    edges.ComputedTop(defaultValue).ShouldBe(defaultValue);
    edges.ComputedBottom(defaultValue).ShouldBe(defaultValue);
  }

  #endregion
}

/// <summary>
/// Tests for EdgeValues helper methods.
/// </summary>
public class EdgeValuesMethodTests
{
  private static readonly FlexValue DefaultValue = FlexValue.Undefined;

  public void ShouldSetAllViaSetAllMethod()
  {
    EdgeValues<FlexValue> edges = new();
    edges.SetAll(FlexValue.Point(10));

    edges.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(10));
    edges.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(10));
  }

  public void ShouldSetHorizontalViaSetHorizontalMethod()
  {
    EdgeValues<FlexValue> edges = new();
    edges.SetHorizontal(FlexValue.Point(15));

    edges.ComputedLeft(DefaultValue).ShouldBe(FlexValue.Point(15));
    edges.ComputedRight(DefaultValue).ShouldBe(FlexValue.Point(15));
    edges.ComputedTop(DefaultValue).ShouldBe(DefaultValue);
    edges.ComputedBottom(DefaultValue).ShouldBe(DefaultValue);
  }

  public void ShouldSetVerticalViaSetVerticalMethod()
  {
    EdgeValues<FlexValue> edges = new();
    edges.SetVertical(FlexValue.Point(25));

    edges.ComputedTop(DefaultValue).ShouldBe(FlexValue.Point(25));
    edges.ComputedBottom(DefaultValue).ShouldBe(FlexValue.Point(25));
    edges.ComputedLeft(DefaultValue).ShouldBe(DefaultValue);
    edges.ComputedRight(DefaultValue).ShouldBe(DefaultValue);
  }

  public void ShouldResetAllEdges()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);
    edges[Edge.Left] = FlexValue.Point(20);
    edges[Edge.Start] = FlexValue.Point(30);

    edges.Reset();

    edges[Edge.All].ShouldBeNull();
    edges[Edge.Left].ShouldBeNull();
    edges[Edge.Start].ShouldBeNull();
    edges.ComputedLeft(DefaultValue).ShouldBe(DefaultValue);
  }
}

/// <summary>
/// Tests for EdgeValues equality operations.
/// </summary>
public class EdgeValuesEqualityTests
{
  public void ShouldBeEqualWhenAllEdgesMatch()
  {
    EdgeValues<FlexValue> edges1 = new();
    edges1[Edge.All] = FlexValue.Point(10);

    EdgeValues<FlexValue> edges2 = new();
    edges2[Edge.All] = FlexValue.Point(10);

    edges1.ShouldBe(edges2);
    (edges1 == edges2).ShouldBeTrue();
    (edges1 != edges2).ShouldBeFalse();
    edges1.Equals(edges2).ShouldBeTrue();
    edges1.Equals((object)edges2).ShouldBeTrue();
  }

  public void ShouldNotBeEqualWhenEdgesDiffer()
  {
    EdgeValues<FlexValue> edges1 = new();
    edges1[Edge.Left] = FlexValue.Point(10);

    EdgeValues<FlexValue> edges2 = new();
    edges2[Edge.Left] = FlexValue.Point(20);

    (edges1 != edges2).ShouldBeTrue();
    (edges1 == edges2).ShouldBeFalse();
    edges1.Equals(edges2).ShouldBeFalse();
  }

  public void ShouldNotBeEqualWhenDifferentEdgesAreSet()
  {
    EdgeValues<FlexValue> edges1 = new();
    edges1[Edge.Left] = FlexValue.Point(10);

    EdgeValues<FlexValue> edges2 = new();
    edges2[Edge.Right] = FlexValue.Point(10);

    (edges1 != edges2).ShouldBeTrue();
  }

  public void ShouldHaveConsistentHashCodeForEqualValues()
  {
    EdgeValues<FlexValue> edges1 = new();
    edges1[Edge.All] = FlexValue.Point(10);
    edges1[Edge.Left] = FlexValue.Point(20);

    EdgeValues<FlexValue> edges2 = new();
    edges2[Edge.All] = FlexValue.Point(10);
    edges2[Edge.Left] = FlexValue.Point(20);

    edges1.GetHashCode().ShouldBe(edges2.GetHashCode());
  }

  public void ShouldNotEqualNull()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);

    edges.Equals(null).ShouldBeFalse();
  }

  public void ShouldNotEqualDifferentType()
  {
    EdgeValues<FlexValue> edges = new();
    edges[Edge.All] = FlexValue.Point(10);

    edges.Equals("test").ShouldBeFalse();
    edges.Equals(10).ShouldBeFalse();
  }
}

/// <summary>
/// Tests for EdgeValues with float type parameter.
/// </summary>
public class EdgeValuesFloatTests
{
  public void ShouldWorkWithFloatType()
  {
    EdgeValues<float> edges = new();
    edges[Edge.All] = 10.5f;

    edges.ComputedLeft(0f).ShouldBe(10.5f);
    edges.ComputedRight(0f).ShouldBe(10.5f);
    edges.ComputedTop(0f).ShouldBe(10.5f);
    edges.ComputedBottom(0f).ShouldBe(10.5f);
  }

  public void ShouldApplyCascadeWithFloatType()
  {
    EdgeValues<float> edges = new();
    edges[Edge.All] = 10f;
    edges[Edge.Horizontal] = 20f;
    edges[Edge.Left] = 30f;

    edges.ComputedLeft(0f).ShouldBe(30f);
    edges.ComputedRight(0f).ShouldBe(20f);
    edges.ComputedTop(0f).ShouldBe(10f);
    edges.ComputedBottom(0f).ShouldBe(10f);
  }

  public void ShouldSupportRtlWithFloatType()
  {
    EdgeValues<float> edges = new();
    edges[Edge.Start] = 10f;

    edges.ComputedLeft(0f, isRtl: false).ShouldBe(10f);
    edges.ComputedRight(0f, isRtl: true).ShouldBe(10f);
  }
}
