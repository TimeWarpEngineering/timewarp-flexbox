namespace TimeWarp.Flexbox.Tests.Layout;

/// <summary>
/// Tests for basic row layout behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class RowLayoutTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldArrangeChildrenHorizontally()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(100) };
    FlexNode child2 = new() { Width = FlexValue.Point(100) };
    FlexNode child3 = new() { Width = FlexValue.Point(100) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 300, 100);

    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(100);
    child3.Layout.Left.ShouldBe(200);
  }

  public void ShouldStretchChildrenToContainerHeight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Stretch
    };

    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 300, 100);

    child.Layout.Height.ShouldBe(100);
  }

  public void ShouldRespectExplicitChildHeight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(50)
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 300, 100);

    child.Layout.Height.ShouldBe(50);
  }
}

/// <summary>
/// Tests for basic column layout behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class ColumnLayoutTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldArrangeChildrenVertically()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(300),
      FlexDirection = FlexDirection.Column
    };

    FlexNode child1 = new() { Height = FlexValue.Point(100) };
    FlexNode child2 = new() { Height = FlexValue.Point(100) };
    FlexNode child3 = new() { Height = FlexValue.Point(100) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 300);

    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(100);
    child3.Layout.Top.ShouldBe(200);
  }

  public void ShouldStretchChildrenToContainerWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(300),
      FlexDirection = FlexDirection.Column,
      AlignItems = AlignItems.Stretch
    };

    FlexNode child = new() { Height = FlexValue.Point(100) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 300);

    child.Layout.Width.ShouldBe(100);
  }
}

/// <summary>
/// Tests for flex-grow distribution.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexGrowTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldDistributeRemainingSpaceProportionally()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { FlexGrow = 1 };
    FlexNode child2 = new() { FlexGrow = 2 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 300, 100);

    child1.Layout.Width.ShouldBe(100); // 1/3 of 300
    child2.Layout.Width.ShouldBe(200); // 2/3 of 300
  }

  public void ShouldDistributeRemainingSpaceAfterFixedWidths()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(100) };
    FlexNode child2 = new() { FlexGrow = 1 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 300, 100);

    child1.Layout.Width.ShouldBe(100);
    child2.Layout.Width.ShouldBe(200); // Remaining space
  }

  public void ShouldNotGrowWhenFlexGrowIsZero()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      FlexGrow = 0
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 300, 100);

    child.Layout.Width.ShouldBe(100);
  }
}

/// <summary>
/// Tests for flex-shrink distribution.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexShrinkTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldShrinkChildrenWhenOverflowing()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };
    FlexNode child2 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 200, 100);

    child1.Layout.Width.ShouldBe(100); // Shrunk equally
    child2.Layout.Width.ShouldBe(100);
  }

  public void ShouldShrinkProportionallyByFlexShrinkFactor()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };
    FlexNode child2 = new() { Width = FlexValue.Point(150), FlexShrink = 2 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 200, 100);

    // Total overflow: 300 - 200 = 100
    // Weighted shrink: child1 = 1*150 = 150, child2 = 2*150 = 300, total = 450
    // child1 shrinks: 100 * (150/450) = 33.33
    // child2 shrinks: 100 * (300/450) = 66.67
    child1.Layout.Width.ShouldBeInRange(116, 118); // ~116.67
    child2.Layout.Width.ShouldBeInRange(82, 84);   // ~83.33
  }

  public void ShouldNotShrinkWhenFlexShrinkIsZero()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(150), FlexShrink = 0 };
    FlexNode child2 = new() { Width = FlexValue.Point(150), FlexShrink = 1 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 200, 100);

    child1.Layout.Width.ShouldBe(150); // No shrink
    child2.Layout.Width.ShouldBe(50);  // All shrink applied here
  }
}

/// <summary>
/// Tests for justify-content alignment.
/// </summary>
[TestTag(TestTags.Fast)]
public class JustifyContentTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldPositionAtStartWithFlexStart()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.FlexStart
    };

    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Left.ShouldBe(0);
  }

  public void ShouldPositionAtEndWithFlexEnd()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.FlexEnd
    };

    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Left.ShouldBe(100);
  }

  public void ShouldPositionAtCenterWithCenter()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.Center
    };

    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Left.ShouldBe(50);
  }

  public void ShouldDistributeSpaceBetweenWithSpaceBetween()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.SpaceBetween
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };
    FlexNode child3 = new() { Width = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 300, 100);

    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(125); // (300 - 150) / 2 = 75 spacing, 50 + 75 = 125
    child3.Layout.Left.ShouldBe(250);
  }

  public void ShouldDistributeSpaceAroundWithSpaceAround()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.SpaceAround
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 300, 100);

    // Free space = 300 - 100 = 200
    // Space per item = 200 / 2 = 100
    // Half space before first = 50
    child1.Layout.Left.ShouldBe(50);
    child2.Layout.Left.ShouldBe(200); // 50 + 50 + 100 = 200
  }

  public void ShouldDistributeSpaceEvenlyWithSpaceEvenly()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.SpaceEvenly
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 300, 100);

    // Free space = 300 - 100 = 200
    // Space = 200 / (2 + 1) = 66.67
    child1.Layout.Left.ShouldBeInRange(66, 68); // ~66.67
    child2.Layout.Left.ShouldBeInRange(183, 185); // ~66.67 + 50 + 66.67 = 183.33
  }
}

/// <summary>
/// Tests for align-items alignment.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignItemsTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldPositionAtStartWithFlexStart()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.FlexStart
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 200);

    child.Layout.Top.ShouldBe(0);
  }

  public void ShouldPositionAtEndWithFlexEnd()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.FlexEnd
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 200);

    child.Layout.Top.ShouldBe(150);
  }

  public void ShouldPositionAtCenterWithCenter()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Center
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 200);

    child.Layout.Top.ShouldBe(75);
  }

  public void ShouldStretchWithStretch()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Stretch
    };

    FlexNode child = new() { Width = FlexValue.Point(50) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 200);

    child.Layout.Height.ShouldBe(200);
  }
}

/// <summary>
/// Tests for align-self override.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignSelfTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldOverrideAlignItems()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.FlexStart
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50),
      AlignSelf = AlignSelf.FlexEnd
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 200);

    child.Layout.Top.ShouldBe(150); // FlexEnd overrides FlexStart
  }

  public void ShouldUseAutoToInheritFromParent()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Center
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50),
      AlignSelf = AlignSelf.Auto
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 200);

    child.Layout.Top.ShouldBe(75); // Inherits Center from parent
  }
}

/// <summary>
/// Tests for gap property.
/// </summary>
[TestTag(TestTags.Fast)]
public class GapTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldAddGapBetweenItemsInRow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      ColumnGap = 20
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };
    FlexNode child3 = new() { Width = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 300, 100);

    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(70);  // 50 + 20
    child3.Layout.Left.ShouldBe(140); // 50 + 20 + 50 + 20
  }

  public void ShouldAddGapBetweenItemsInColumn()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(300),
      FlexDirection = FlexDirection.Column,
      RowGap = 20
    };

    FlexNode child1 = new() { Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Height = FlexValue.Point(50) };
    FlexNode child3 = new() { Height = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 300);

    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(70);  // 50 + 20
    child3.Layout.Top.ShouldBe(140); // 50 + 20 + 50 + 20
  }

  public void ShouldAccountForGapInFlexGrowCalculation()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      ColumnGap = 20
    };

    FlexNode child1 = new() { FlexGrow = 1 };
    FlexNode child2 = new() { FlexGrow = 1 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 300, 100);

    // Available: 300, Gap: 20, Remaining: 280
    // Each child: 140
    child1.Layout.Width.ShouldBe(140);
    child2.Layout.Width.ShouldBe(140);
  }
}

/// <summary>
/// Tests for absolute positioning.
/// </summary>
[TestTag(TestTags.Fast)]
public class AbsolutePositioningTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldPositionWithLeftAndTop()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      PositionType = PositionType.Absolute,
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    child.SetPosition(Edge.Left, FlexValue.Point(10));
    child.SetPosition(Edge.Top, FlexValue.Point(20));
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 200);

    child.Layout.Left.ShouldBe(10);
    child.Layout.Top.ShouldBe(20);
  }

  public void ShouldPositionWithRightAndBottom()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      PositionType = PositionType.Absolute,
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    child.SetPosition(Edge.Right, FlexValue.Point(10));
    child.SetPosition(Edge.Bottom, FlexValue.Point(20));
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 200);

    child.Layout.Left.ShouldBe(140);  // 200 - 10 - 50
    child.Layout.Top.ShouldBe(130);   // 200 - 20 - 50
  }

  public void ShouldStretchWithLeftAndRight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      PositionType = PositionType.Absolute,
      Height = FlexValue.Point(50)
    };
    child.SetPosition(Edge.Left, FlexValue.Point(10));
    child.SetPosition(Edge.Right, FlexValue.Point(10));
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 200);

    child.Layout.Width.ShouldBe(180);  // 200 - 10 - 10
    child.Layout.Left.ShouldBe(10);
  }

  public void ShouldNotAffectFlexLayout()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode absolute = new()
    {
      PositionType = PositionType.Absolute,
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    absolute.SetPosition(Edge.Left, FlexValue.Point(0));
    absolute.SetPosition(Edge.Top, FlexValue.Point(0));

    FlexNode child1 = new() { Width = FlexValue.Point(100) };
    FlexNode child2 = new() { Width = FlexValue.Point(100) };

    root.AddChild(absolute);
    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 200, 100);

    // Absolute child doesn't take space
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(100);
  }
}

/// <summary>
/// Tests for percentage dimensions.
/// </summary>
[TestTag(TestTags.Fast)]
public class PercentageDimensionTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldResolvePercentageWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new() { Width = FlexValue.Percent(50) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Width.ShouldBe(100); // 50% of 200
  }

  public void ShouldResolvePercentageHeight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Column
    };

    FlexNode child = new() { Height = FlexValue.Percent(50) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 200);

    child.Layout.Height.ShouldBe(100); // 50% of 200
  }
}

/// <summary>
/// Tests for min/max constraints.
/// </summary>
[TestTag(TestTags.Fast)]
public class MinMaxConstraintTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldRespectMinWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      FlexGrow = 1,
      MinWidth = FlexValue.Point(150)
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Width.ShouldBe(200); // Grows to fill, but >= 150
  }

  public void ShouldRespectMaxWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      FlexGrow = 1,
      MaxWidth = FlexValue.Point(100)
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Width.ShouldBe(100); // Clamped to max
  }

  public void ShouldRespectMinHeightDuringShrink()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(200),
      FlexShrink = 1,
      MinWidth = FlexValue.Point(150)
    };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Width.ShouldBe(150); // Shrinks but not below min
  }
}

/// <summary>
/// Tests for nested flex containers.
/// </summary>
[TestTag(TestTags.Fast)]
public class NestedContainerTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldLayoutNestedContainerCorrectly()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Column
    };

    FlexNode container = new()
    {
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { FlexGrow = 1 };
    FlexNode child2 = new() { FlexGrow = 1 };

    container.AddChild(child1);
    container.AddChild(child2);
    root.AddChild(container);

    Engine.CalculateLayout(root, 200, 200);

    container.Layout.Width.ShouldBe(200);
    container.Layout.Height.ShouldBe(100);
    child1.Layout.Width.ShouldBe(100);
    child2.Layout.Width.ShouldBe(100);
  }
}

/// <summary>
/// Tests for PixelGrid.RoundToPixelGrid static method.
/// </summary>
[TestTag(TestTags.Fast)]
public class PixelGridRoundToPixelGridTests
{
  public void ShouldReturnValueUnchangedWhenScaleFactorIsZero()
  {
    float result = PixelGrid.RoundToPixelGrid(3.7f, 0);

    result.ShouldBe(3.7f);
  }

  public void ShouldReturnValueUnchangedWhenScaleFactorIsNegative()
  {
    float result = PixelGrid.RoundToPixelGrid(3.7f, -1);

    result.ShouldBe(3.7f);
  }

  public void ShouldRoundToNearestPixelAtScale1()
  {
    PixelGrid.RoundToPixelGrid(3.3f, 1).ShouldBe(3);
    PixelGrid.RoundToPixelGrid(3.5f, 1).ShouldBe(4);
    PixelGrid.RoundToPixelGrid(3.7f, 1).ShouldBe(4);
  }

  public void ShouldRoundToHalfPixelAtScale2()
  {
    // At scale 2, grid is 0.5 units
    PixelGrid.RoundToPixelGrid(3.1f, 2).ShouldBe(3.0f);
    PixelGrid.RoundToPixelGrid(3.3f, 2).ShouldBe(3.5f);
    PixelGrid.RoundToPixelGrid(3.6f, 2).ShouldBe(3.5f);
    PixelGrid.RoundToPixelGrid(3.8f, 2).ShouldBe(4.0f);
  }

  public void ShouldForceCeilWhenRequested()
  {
    PixelGrid.RoundToPixelGrid(3.1f, 1, forceCeil: true).ShouldBe(4);
    PixelGrid.RoundToPixelGrid(3.9f, 1, forceCeil: true).ShouldBe(4);
    PixelGrid.RoundToPixelGrid(3.0f, 1, forceCeil: true).ShouldBe(3);
  }

  public void ShouldForceFloorWhenRequested()
  {
    PixelGrid.RoundToPixelGrid(3.1f, 1, forceFloor: true).ShouldBe(3);
    PixelGrid.RoundToPixelGrid(3.9f, 1, forceFloor: true).ShouldBe(3);
    PixelGrid.RoundToPixelGrid(4.0f, 1, forceFloor: true).ShouldBe(4);
  }

  public void ShouldHandleNegativeValues()
  {
    PixelGrid.RoundToPixelGrid(-3.3f, 1).ShouldBe(-3);
    PixelGrid.RoundToPixelGrid(-3.7f, 1).ShouldBe(-4);
  }
}

/// <summary>
/// Tests for PixelGrid.RoundLayoutToPixelGrid recursive rounding.
/// </summary>
[TestTag(TestTags.Fast)]
public class PixelGridRoundLayoutTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldThrowWhenNodeIsNull()
  {
    Should.Throw<ArgumentNullException>(() => PixelGrid.RoundLayoutToPixelGrid(null!, 2));
  }

  public void ShouldNotModifyLayoutWhenScaleFactorIsOne()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100.3f),
      Height = FlexValue.Point(100.7f)
    };

    Engine.CalculateLayout(root, 100.3f, 100.7f);

    float originalWidth = root.Layout.Width;
    float originalHeight = root.Layout.Height;

    PixelGrid.RoundLayoutToPixelGrid(root, 1.0f);

    root.Layout.Width.ShouldBe(originalWidth);
    root.Layout.Height.ShouldBe(originalHeight);
  }

  public void ShouldNotModifyLayoutWhenScaleFactorIsZero()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100.3f),
      Height = FlexValue.Point(100.7f)
    };

    Engine.CalculateLayout(root, 100.3f, 100.7f);

    float originalWidth = root.Layout.Width;
    float originalHeight = root.Layout.Height;

    PixelGrid.RoundLayoutToPixelGrid(root, 0);

    root.Layout.Width.ShouldBe(originalWidth);
    root.Layout.Height.ShouldBe(originalHeight);
  }

  public void ShouldRoundRootLayoutToPixelGrid()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(33.3f),
      Height = FlexValue.Point(33.3f)
    };

    root.AddChild(child);
    Engine.CalculateLayout(root, 100, 100);

    PixelGrid.RoundLayoutToPixelGrid(root, 2);

    // At scale 2, 33.3 * 2 = 66.6 -> rounds to 67 -> 67 / 2 = 33.5
    child.Layout.Width.ShouldBe(33.5f);
    child.Layout.Height.ShouldBe(33.5f);
  }

  public void ShouldRoundChildPositionsToPixelGrid()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(33.3f) };
    FlexNode child2 = new() { Width = FlexValue.Point(33.3f) };
    FlexNode child3 = new() { Width = FlexValue.Point(33.3f) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    PixelGrid.RoundLayoutToPixelGrid(root, 2);

    // At scale 2, grid is 0.5 units
    // Positions should be on half-pixel boundaries
    child1.Layout.Left.ShouldBe(0);
    // child2's absolute left is 33.3, * 2 = 66.6, rounds to 67, / 2 = 33.5
    child2.Layout.Left.ShouldBe(33.5f);
    // child3's absolute left is 66.6, * 2 = 133.2, rounds to 133, / 2 = 66.5
    // Relative to parent (0), so still 66.5
    child3.Layout.Left.ShouldBe(66.5f);
  }

  public void ShouldRoundNestedChildrenCorrectly()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode container = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(25.3f),
      Height = FlexValue.Point(25.3f)
    };

    container.AddChild(child);
    root.AddChild(container);

    Engine.CalculateLayout(root, 100, 100);

    PixelGrid.RoundLayoutToPixelGrid(root, 2);

    // 25.3 * 2 = 50.6 -> rounds to 51 -> 51 / 2 = 25.5
    child.Layout.Width.ShouldBe(25.5f);
    child.Layout.Height.ShouldBe(25.5f);
  }
}

/// <summary>
/// Tests for FlexLayoutEngine.CalculateLayout with pixel grid rounding.
/// </summary>
[TestTag(TestTags.Fast)]
public class LayoutWithPixelGridRoundingTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldRoundLayoutWhenRoundToPixelGridIsTrue()
  {
    FlexConfig config = new() { PointScaleFactor = 2.0f };

    FlexNode root = new()
    {
      Config = config,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new() { Width = FlexValue.Point(33.3f) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr, roundToPixelGrid: true);

    // Width should be rounded to pixel grid
    child.Layout.Width.ShouldBe(33.5f);
  }

  public void ShouldNotRoundLayoutWhenRoundToPixelGridIsFalse()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new() { Width = FlexValue.Point(33.3f) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr, roundToPixelGrid: false);

    // Width should remain as calculated (33.3)
    child.Layout.Width.ShouldBe(33.3f);
  }

  public void ShouldSkipRoundingWhenScaleFactorIsOne()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new() { Width = FlexValue.Point(33.3f) };
    root.AddChild(child);

    // Default scale factor is 1.0, which skips rounding (intentional optimization)
    Engine.CalculateLayout(root, 100, 100, Direction.Ltr, roundToPixelGrid: true);

    // At scale 1, rounding is skipped - values remain unchanged
    child.Layout.Width.ShouldBe(33.3f);
  }
}

/// <summary>
/// Tests for RTL (right-to-left) layout direction support.
/// </summary>
[TestTag(TestTags.Fast)]
public class RtlLayoutTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldReverseRowDirectionInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(100) };
    FlexNode child2 = new() { Width = FlexValue.Point(100) };
    FlexNode child3 = new() { Width = FlexValue.Point(100) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 300, 100, Direction.Rtl);

    // In RTL, children should be positioned from right to left
    child1.Layout.Left.ShouldBe(200);
    child2.Layout.Left.ShouldBe(100);
    child3.Layout.Left.ShouldBe(0);
  }

  public void ShouldNotAffectColumnDirectionInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(300),
      FlexDirection = FlexDirection.Column
    };

    FlexNode child1 = new() { Height = FlexValue.Point(100) };
    FlexNode child2 = new() { Height = FlexValue.Point(100) };
    FlexNode child3 = new() { Height = FlexValue.Point(100) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 300, Direction.Rtl);

    // Column direction is not affected by RTL
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(100);
    child3.Layout.Top.ShouldBe(200);
  }

  public void ShouldReverseRowReverseToRowInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.RowReverse
    };

    FlexNode child1 = new() { Width = FlexValue.Point(100) };
    FlexNode child2 = new() { Width = FlexValue.Point(100) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 300, 100, Direction.Rtl);

    // RowReverse in RTL becomes Row (double reversal)
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(100);
  }

  public void ShouldPositionAbsoluteChildWithStartEdgeInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      PositionType = PositionType.Absolute,
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    child.SetPosition(Edge.Start, FlexValue.Point(10));

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 200, Direction.Rtl);

    // In RTL, Start edge maps to Right, so child is 10px from right edge
    // Child is at right=10, so left = 200 - 50 - 10 = 140
    child.Layout.Left.ShouldBe(140);
  }

  public void ShouldPositionAbsoluteChildWithEndEdgeInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      PositionType = PositionType.Absolute,
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    child.SetPosition(Edge.End, FlexValue.Point(10));

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 200, Direction.Rtl);

    // In RTL, End edge maps to Left, so child is 10px from left edge
    child.Layout.Left.ShouldBe(10);
  }

  public void ShouldInheritDirectionFromParent()
  {
    FlexNode root = new()
    {
      Direction = Direction.Rtl,
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);

    // Child inherits RTL from parent
    child.ResolvedDirection.ShouldBe(Direction.Rtl);
  }

  public void ShouldUseConfigDirectionForRootWithInherit()
  {
    FlexConfig config = new() { Direction = Direction.Rtl };

    FlexNode root = new()
    {
      Config = config,
      Direction = Direction.Inherit, // Explicit inherit (same as default)
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    root.ResolvedDirection.ShouldBe(Direction.Rtl);
  }

  public void ShouldOverrideParentDirection()
  {
    FlexNode root = new()
    {
      Direction = Direction.Rtl,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Direction = Direction.Ltr
    };
    root.AddChild(child);

    root.ResolvedDirection.ShouldBe(Direction.Rtl);
    child.ResolvedDirection.ShouldBe(Direction.Ltr);
  }

  public void ShouldApplyJustifyContentFlexEndInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.FlexEnd
    };

    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 300, 100, Direction.Rtl);

    // In RTL, Row becomes RowReverse, so:
    // - FlexStart is the right side (visual start in RTL)
    // - FlexEnd is the left side (visual end in RTL)
    // FlexEnd pushes items to the left side, so Left = 0
    child.Layout.Left.ShouldBe(0);
  }

  public void ShouldApplyJustifyContentCenterInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      JustifyContent = JustifyContent.Center
    };

    FlexNode child = new() { Width = FlexValue.Point(100) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 300, 100, Direction.Rtl);

    // Center should still center the child
    child.Layout.Left.ShouldBe(100);
  }

  public void ShouldResolvePaddingStartInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    root.SetPadding(Edge.Start, FlexValue.Point(20));

    FlexNode child = new() { Width = FlexValue.Point(50) };
    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100, Direction.Rtl);

    // In RTL, Start padding is on the right side
    // Row becomes RowReverse, child starts from right after padding
    // Child width 50, container 200, padding-right 20
    // Position: 200 - 20 - 50 = 130
    child.Layout.Left.ShouldBe(130);
  }
}

/// <summary>
/// Tests for align-content: flex-start behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignContentFlexStartTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldPositionLinesAtContainerStart()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.FlexStart
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Line 1: child1, child2 (top = 0)
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    // Line 2: child3 (top = 20)
    child3.Layout.Top.ShouldBe(20);
  }

  public void ShouldNotAffectSingleLine()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.FlexStart
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 200, 100);

    // Single line, all at top
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
  }
}

/// <summary>
/// Tests for align-content: flex-end behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignContentFlexEndTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldPositionLinesAtContainerEnd()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.FlexEnd
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Two lines of 20px each = 40px total, container = 100px
    // Lines pushed to bottom: start at 100 - 40 = 60
    child1.Layout.Top.ShouldBe(60);
    child2.Layout.Top.ShouldBe(60);
    child3.Layout.Top.ShouldBe(80);
  }
}

/// <summary>
/// Tests for align-content: center behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignContentCenterTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldPositionLinesInCenter()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.Center
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Two lines of 20px each = 40px total, container = 100px
    // Free space = 60px, centered = start at 30
    child1.Layout.Top.ShouldBe(30);
    child2.Layout.Top.ShouldBe(30);
    child3.Layout.Top.ShouldBe(50);
  }
}

/// <summary>
/// Tests for align-content: space-between behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignContentSpaceBetweenTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldDistributeLinesWithSpaceBetween()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.SpaceBetween
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Line 1 at top (0), Line 2 at bottom (100 - 20 = 80)
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    child3.Layout.Top.ShouldBe(80);
  }

  public void ShouldHandleThreeLines()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.SpaceBetween
    };

    // 5 children of 50px width each, container 100px wide = 2 per line, 3 lines
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child4 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child5 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    root.AddChild(child4);
    root.AddChild(child5);

    Engine.CalculateLayout(root, 100, 100);

    // 3 lines of 20px each = 60px, free space = 40px, gaps = 2, gap size = 20px
    // Line 1: top = 0
    // Line 2: top = 20 + 20 = 40
    // Line 3: top = 40 + 20 + 20 = 80
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    child3.Layout.Top.ShouldBe(40);
    child4.Layout.Top.ShouldBe(40);
    child5.Layout.Top.ShouldBe(80);
  }
}

/// <summary>
/// Tests for align-content: space-around behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignContentSpaceAroundTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldDistributeLinesWithSpaceAround()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.SpaceAround
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // 2 lines of 20px each = 40px, free space = 60px
    // Space-around: each line gets equal margin on both sides
    // 60px / (2 lines * 2 sides) = 15px per side
    // Line 1: top = 15
    // Line 2: top = 15 + 20 + 30 = 65
    child1.Layout.Top.ShouldBe(15);
    child2.Layout.Top.ShouldBe(15);
    child3.Layout.Top.ShouldBe(65);
  }
}

/// <summary>
/// Tests for align-content: space-evenly behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignContentSpaceEvenlyTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldDistributeLinesWithSpaceEvenly()
  {
    // Yoga test: align_content_space_evenly_wrap
    // Container 140x120, 5 children (50x10), 2 per row = 3 lines
    // 3 lines × 10px = 30px, freeSpace = 90px
    // SpaceEvenly: 90 / (3+1) = 22.5px gaps
    FlexNode root = new()
    {
      Width = FlexValue.Point(140),
      Height = FlexValue.Point(120),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.SpaceEvenly
    };

    FlexNode child0 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };
    FlexNode child4 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };

    root.AddChild(child0);
    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    root.AddChild(child4);

    Engine.CalculateLayout(root, 140, 120);

    // Line 1: top = 22.5 (Yoga rounds to 23)
    child0.Layout.Top.ShouldBe(22.5f);
    child1.Layout.Top.ShouldBe(22.5f);
    // Line 2: top = 22.5 + 10 + 22.5 = 55
    child2.Layout.Top.ShouldBe(55);
    child3.Layout.Top.ShouldBe(55);
    // Line 3: top = 55 + 10 + 22.5 = 87.5
    child4.Layout.Top.ShouldBe(87.5f);
  }

  public void ShouldCenterSingleLineWithSpaceEvenly()
  {
    // Yoga test: align_content_space_evenly_wrap_singleline
    // Single line: freeSpace / (1+1) = freeSpace / 2 = same as center
    // BUG: Single-line align-content doesn't work - see kanban task 081
    FlexNode root = new()
    {
      Width = FlexValue.Point(140),
      Height = FlexValue.Point(120),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.SpaceEvenly
    };

    FlexNode child0 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };

    root.AddChild(child0);
    root.AddChild(child1);

    Engine.CalculateLayout(root, 140, 120);

    // 1 line of 10px, freeSpace = 110px
    // SpaceEvenly: 110 / (1+1) = 55px offset
    child0.Layout.Top.ShouldBe(55);
    child1.Layout.Top.ShouldBe(55);
  }
}

/// <summary>
/// Tests for align-content: stretch behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignContentStretchTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldExpandLinesToFillContainer()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.Stretch
    };

    // Need explicit heights for children to establish line heights
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Children keep their explicit heights
    child1.Layout.Height.ShouldBe(20);
    child2.Layout.Height.ShouldBe(20);
    child3.Layout.Height.ShouldBe(20);

    // With stretch, lines are evenly distributed
    // Two lines of 20px each, container 100px
    // Line 1: top = 0, Line 2: top = 50
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    child3.Layout.Top.ShouldBe(50);
  }

  public void ShouldStretchWithVaryingLineHeights()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.Stretch
    };

    // Line 1: one tall child (30px)
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };
    // Line 2: shorter children
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Stretch distributes extra space proportionally
    // Line heights: 30px and 10px = 40px total, extra = 60px
    // Each line gets proportional stretch
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
  }
}

/// <summary>
/// Tests for align-content combined with other properties.
/// </summary>
[TestTag(TestTags.Fast)]
public class AlignContentCombinedTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldWorkWithAlignItems()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.Center,
      AlignItems = AlignItems.Center
    };

    // Force wrapping with 3 children of 50px on 100px container
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(10) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Line 1: child1 (10px), child2 (20px) - line height = 20
    // Line 2: child3 (10px) - line height = 10
    // Total lines height = 30px, container = 100px
    // With AlignContent.Center: start at (100-30)/2 = 35

    // child1 should be centered within 20px line at top 35
    // child1 center offset = (20-10)/2 = 5
    child1.Layout.Top.ShouldBe(40); // 35 + 5
    child2.Layout.Top.ShouldBe(35); // 35 + 0 (already 20px tall)
    // child3 on second line at 35 + 20 = 55
    child3.Layout.Top.ShouldBe(55);
  }

  public void ShouldWorkInColumnDirection()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Column,
      FlexWrap = FlexWrap.Wrap,
      AlignContent = AlignContent.FlexEnd
    };

    FlexNode child1 = new() { Width = FlexValue.Point(20), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(20), Height = FlexValue.Point(50) };
    FlexNode child3 = new() { Width = FlexValue.Point(20), Height = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Column wrap: 2 children per column (50+50=100), third wraps
    // 2 columns of 20px each = 40px, container = 100px
    // FlexEnd: columns pushed to right, start at 100 - 40 = 60
    child1.Layout.Left.ShouldBe(60);
    child2.Layout.Left.ShouldBe(60);
    child3.Layout.Left.ShouldBe(80);
  }
}

/// <summary>
/// Tests for uniform border on all edges.
/// </summary>
[TestTag(TestTags.Fast)]
public class BorderUniformTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldOffsetChildContent()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.All, 10);

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Left.ShouldBe(10);
    child.Layout.Top.ShouldBe(10);
  }

  public void ShouldReduceAvailableSpaceForChildren()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.All, 10);

    FlexNode child = new()
    {
      Width = FlexValue.Percent(100),
      Height = FlexValue.Percent(100)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Available space is 100 - 10 - 10 = 80
    child.Layout.Width.ShouldBe(80);
    child.Layout.Height.ShouldBe(80);
  }
}

/// <summary>
/// Tests for individual edge borders.
/// </summary>
[TestTag(TestTags.Fast)]
public class BorderIndividualTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldApplyOnlyToLeftEdge()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.Left, 20);

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Left.ShouldBe(20);
    child.Layout.Top.ShouldBe(0);
  }

  public void ShouldApplyOnlyToTopEdge()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.Top, 15);

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Left.ShouldBe(0);
    child.Layout.Top.ShouldBe(15);
  }

  public void ShouldApplyDifferentBordersToEachEdge()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.Top, 5);
    root.SetBorder(Edge.Right, 10);
    root.SetBorder(Edge.Bottom, 15);
    root.SetBorder(Edge.Left, 20);

    FlexNode child = new()
    {
      Width = FlexValue.Percent(100),
      Height = FlexValue.Percent(100)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Left.ShouldBe(20);
    child.Layout.Top.ShouldBe(5);
    child.Layout.Width.ShouldBe(70); // 100 - 20 - 10
    child.Layout.Height.ShouldBe(80); // 100 - 5 - 15
  }
}

/// <summary>
/// Tests for border combined with padding.
/// </summary>
[TestTag(TestTags.Fast)]
public class BorderWithPaddingTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldStackCorrectly()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.Top, 10);
    root.SetBorder(Edge.Left, 10);
    root.SetPadding(Edge.Top, FlexValue.Point(5));
    root.SetPadding(Edge.Left, FlexValue.Point(5));

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Child offset by border + padding
    child.Layout.Left.ShouldBe(15); // 10 + 5
    child.Layout.Top.ShouldBe(15); // 10 + 5
  }

  public void ShouldBothReduceAvailableSpace()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.All, 5);
    root.SetPadding(Edge.All, FlexValue.Point(10));

    FlexNode child = new()
    {
      Width = FlexValue.Percent(100),
      Height = FlexValue.Percent(100)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Available: 100 - 5 - 5 - 10 - 10 = 70
    child.Layout.Width.ShouldBe(70);
    child.Layout.Height.ShouldBe(70);
  }
}

/// <summary>
/// Tests for border with flex-grow children.
/// </summary>
[TestTag(TestTags.Fast)]
public class BorderWithFlexGrowTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldAccountForBorderInFlexCalculation()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    root.SetBorder(Edge.Left, 10);
    root.SetBorder(Edge.Right, 10);

    FlexNode child = new() { FlexGrow = 1 };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Child grows to fill available space minus borders
    child.Layout.Width.ShouldBe(80); // 100 - 10 - 10
  }

  public void ShouldDistributeFlexGrowCorrectlyWithBorder()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    root.SetBorder(Edge.Left, 10);
    root.SetBorder(Edge.Right, 10);

    FlexNode child1 = new() { FlexGrow = 1 };
    FlexNode child2 = new() { FlexGrow = 1 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100);

    // Available: 80, split evenly
    child1.Layout.Width.ShouldBe(40);
    child2.Layout.Width.ShouldBe(40);
  }
}

/// <summary>
/// Tests for border with RTL direction.
/// </summary>
[TestTag(TestTags.Fast)]
public class BorderWithRtlTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldResolveStartBorderInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    root.SetBorder(Edge.Start, 20);

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Rtl);

    // In RTL, Start is on the right side
    // Child positioned from right edge minus border
    child.Layout.Left.ShouldBe(30); // 100 - 20 - 50 = 30
  }

  public void ShouldResolveEndBorderInRtl()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    root.SetBorder(Edge.End, 20);

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Rtl);

    // In RTL, End is on the left side
    // Child positioned from right edge, border on left
    child.Layout.Left.ShouldBe(50); // 100 - 50 = 50
  }
}

/// <summary>
/// Tests for border with absolute positioned children.
/// </summary>
[TestTag(TestTags.Fast)]
public class BorderWithAbsoluteTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldOffsetAbsoluteChildByBorder()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.All, 10);

    FlexNode child = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30),
      PositionType = PositionType.Absolute
    };
    child.SetPosition(Edge.Left, FlexValue.Point(0));
    child.SetPosition(Edge.Top, FlexValue.Point(0));

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Absolute child at 0,0 is relative to border box
    child.Layout.Left.ShouldBe(10);
    child.Layout.Top.ShouldBe(10);
  }
}

/// <summary>
/// Tests for zero border values.
/// </summary>
[TestTag(TestTags.Fast)]
public class BorderZeroTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldHaveNoEffectWithZeroBorder()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.All, 0);

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Left.ShouldBe(0);
    child.Layout.Top.ShouldBe(0);
  }
}

// =============================================================================
// Dimension Tests (Task 035)
// =============================================================================

/// <summary>
/// Tests for explicit width and height dimensions.
/// </summary>
[TestTag(TestTags.Fast)]
public class ExplicitDimensionTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldSetExactWidthAndHeight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200)
    };

    Engine.CalculateLayout(root, 100, 200);

    root.Layout.Width.ShouldBe(100);
    root.Layout.Height.ShouldBe(200);
  }

  public void ShouldUseAvailableWidthWhenNotSpecified()
  {
    FlexNode root = new()
    {
      Height = FlexValue.Point(100)
    };

    Engine.CalculateLayout(root, 200, 100);

    root.Layout.Width.ShouldBe(200);
    root.Layout.Height.ShouldBe(100);
  }

  public void ShouldUseAvailableHeightWhenNotSpecified()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100)
    };

    Engine.CalculateLayout(root, 100, 200);

    root.Layout.Width.ShouldBe(100);
    // Root uses available height
    root.Layout.Height.ShouldBe(200);
  }
}

/// <summary>
/// Tests for auto sizing to content.
/// </summary>
[TestTag(TestTags.Fast)]
public class AutoDimensionTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldSizeChildToContentInRow()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    // Child respects its explicit dimensions
    child.Layout.Width.ShouldBe(50);
    child.Layout.Height.ShouldBe(50);
  }

  public void ShouldSizeChildToContentInColumn()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(100)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 200);

    child.Layout.Height.ShouldBe(100);
  }

  public void ShouldLayoutMultipleChildrenInRow()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 200, 100);

    // Children are positioned correctly
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(50);
  }

  public void ShouldLayoutMultipleChildrenInColumn()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(40)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(60)
    };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 200);

    // Children are positioned correctly
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(40);
  }
}

/// <summary>
/// Tests for percentage dimensions with edge cases.
/// </summary>
[TestTag(TestTags.Fast)]
public class PercentageDimensionEdgeCaseTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldResolve100PercentToParentSize()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(150)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Percent(100),
      Height = FlexValue.Percent(100)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 150);

    child.Layout.Width.ShouldBe(200);
    child.Layout.Height.ShouldBe(150);
  }

  public void ShouldResolve25PercentWidth()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Percent(25),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Width.ShouldBe(50); // 25% of 200
  }
}

/// <summary>
/// Tests for min-width and max-width constraints.
/// </summary>
[TestTag(TestTags.Fast)]
public class MinMaxWidthTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldEnforceMinWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(10),
      Height = FlexValue.Point(50),
      MinWidth = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Width.ShouldBe(50); // Clamped to min
  }

  public void ShouldEnforceMaxWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(50),
      MaxWidth = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Width.ShouldBe(50); // Clamped to max
  }

  public void ShouldEnforceMinWidthOnStretchedChild()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Height = FlexValue.Point(50),
      MinWidth = FlexValue.Point(80)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 50, 100);

    child.Layout.Width.ShouldBe(80); // Min overrides stretch
  }

  public void ShouldEnforceMaxWidthOnExplicitWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(80),
      Height = FlexValue.Point(50),
      MaxWidth = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Width.ShouldBe(50); // Max limits explicit width
  }
}

/// <summary>
/// Tests for min-height and max-height constraints.
/// </summary>
[TestTag(TestTags.Fast)]
public class MinMaxHeightTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldEnforceMinHeight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(10),
      MinHeight = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Height.ShouldBe(50); // Clamped to min
  }

  public void ShouldEnforceMaxHeight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(100),
      MaxHeight = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Height.ShouldBe(50); // Clamped to max
  }

  public void ShouldEnforceMinHeightOnStretchedChild()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(50)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      MinHeight = FlexValue.Point(80)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 50);

    child.Layout.Height.ShouldBe(80); // Min overrides stretch
  }

  public void ShouldEnforceMaxHeightOnStretchedChild()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      MaxHeight = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Height.ShouldBe(50); // Max limits stretch
  }
}

/// <summary>
/// Tests for min/max percentage constraints.
/// </summary>
[TestTag(TestTags.Fast)]
public class MinMaxPercentageTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldResolveMinWidthPercentage()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(10),
      Height = FlexValue.Point(50),
      MinWidth = FlexValue.Percent(25)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Width.ShouldBe(50); // 25% of 200
  }

  public void ShouldResolveMaxWidthPercentage()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(150),
      Height = FlexValue.Point(50),
      MaxWidth = FlexValue.Percent(25)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Width.ShouldBe(50); // 25% of 200
  }
}

/// <summary>
/// Tests for min/max constraints working together.
/// </summary>
[TestTag(TestTags.Fast)]
public class MinMaxCombinedTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldApplyBothMinAndMaxWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50),
      MinWidth = FlexValue.Point(30),
      MaxWidth = FlexValue.Point(70)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Width 50 is within min 30 and max 70
    child.Layout.Width.ShouldBe(50);
  }

  public void ShouldApplyBothMinAndMaxHeight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50),
      MinHeight = FlexValue.Point(30),
      MaxHeight = FlexValue.Point(70)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Height 50 is within min 30 and max 70
    child.Layout.Height.ShouldBe(50);
  }
}

/// <summary>
/// Tests for dimension interaction with flex-basis.
/// </summary>
[TestTag(TestTags.Fast)]
public class DimensionFlexBasisTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldUseFlexBasisOverWidthInRow()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      FlexBasis = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Width.ShouldBe(50); // FlexBasis overrides width in row
  }

  public void ShouldUseFlexBasisOverHeightInColumn()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(100),
      FlexBasis = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 200);

    child.Layout.Height.ShouldBe(50); // FlexBasis overrides height in column
  }

  public void ShouldUseWidthWhenFlexBasisIsAuto()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(80),
      FlexBasis = FlexValue.Auto,
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100);

    child.Layout.Width.ShouldBe(80); // Width used when FlexBasis is auto
  }
}

// =============================================================================
// Display Tests (Task 036)
// =============================================================================

/// <summary>
/// Tests for Display.None behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class DisplayNoneTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldExcludeElementFromLayout()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(50)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(50),
      Display = Display.None
    };

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    child1.Layout.Left.ShouldBe(0);
    // child2 is skipped, child3 comes right after child1
    child3.Layout.Left.ShouldBe(30);
  }

  public void ShouldHaveZeroDimensions()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50),
      Display = Display.None
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Width.ShouldBe(0);
    child.Layout.Height.ShouldBe(0);
  }

  public void ShouldNotAffectSiblingPositions()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(200)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(40)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(40),
      Display = Display.None
    };

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(40)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 200);

    child1.Layout.Top.ShouldBe(0);
    // child2 is skipped
    child3.Layout.Top.ShouldBe(40);
  }
}

/// <summary>
/// Tests for Display.Flex behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class DisplayFlexTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldIncludeElementInLayout()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(50),
      Display = Display.Flex
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(50),
      Display = Display.Flex
    };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100);

    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(30);
  }

  public void ShouldBeDefaultDisplayValue()
  {
    FlexNode node = new();
    node.Display.ShouldBe(Display.Flex);
  }
}

/// <summary>
/// Tests for Display.None with flex-grow siblings.
/// </summary>
[TestTag(TestTags.Fast)]
public class DisplayNoneWithFlexGrowTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldRedistributeSpaceToVisibleSiblings()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      FlexGrow = 1,
      Height = FlexValue.Point(50)
    };

    FlexNode child2 = new()
    {
      FlexGrow = 1,
      Height = FlexValue.Point(50),
      Display = Display.None
    };

    FlexNode child3 = new()
    {
      FlexGrow = 1,
      Height = FlexValue.Point(50)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Space split between two visible children
    child1.Layout.Width.ShouldBe(50);
    child2.Layout.Width.ShouldBe(0);
    child3.Layout.Width.ShouldBe(50);
  }
}

/// <summary>
/// Tests for nested Display.None behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class DisplayNoneNestedTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldHideEntireSubtree()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode parent = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50),
      Display = Display.None
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    parent.AddChild(child);
    root.AddChild(parent);

    Engine.CalculateLayout(root, 100, 100);

    parent.Layout.Width.ShouldBe(0);
    parent.Layout.Height.ShouldBe(0);
    child.Layout.Width.ShouldBe(0);
    child.Layout.Height.ShouldBe(0);
  }

  public void ShouldNotMeasureHiddenChildren()
  {
    bool measureCalled = false;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Display = Display.None
    };

    child.MeasureFunc = (_, _, _, _, _) =>
    {
      measureCalled = true;
      return new Size(50, 50);
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    measureCalled.ShouldBeFalse();
  }
}

/// <summary>
/// Tests for Display.None with absolute positioning.
/// </summary>
[TestTag(TestTags.Fast)]
public class DisplayNoneAbsoluteTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldHideAbsolutePositionedElements()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50),
      PositionType = PositionType.Absolute,
      Display = Display.None
    };
    child.SetPosition(Edge.Left, FlexValue.Point(10));
    child.SetPosition(Edge.Top, FlexValue.Point(10));

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Width.ShouldBe(0);
    child.Layout.Height.ShouldBe(0);
  }
}

// =============================================================================
// Flex Wrap Tests (Task 037)
// =============================================================================

/// <summary>
/// Tests for FlexWrap.NoWrap behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexWrapNoWrapTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldKeepAllItemsOnSingleLine()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.NoWrap,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // All on same line (top = 0), items may shrink to fit
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    child3.Layout.Top.ShouldBe(0);
  }

  public void ShouldShrinkItemsToFitOnSingleLine()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.NoWrap,
      Width = FlexValue.Point(90),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 90, 100);

    // Items shrink proportionally to fit 90px (each is 30)
    child1.Layout.Width.ShouldBe(30);
    child2.Layout.Width.ShouldBe(30);
    child3.Layout.Width.ShouldBe(30);
  }
}

/// <summary>
/// Tests for FlexWrap.Wrap behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexWrapWrapTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldCreateMultipleLinesWhenNeeded()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(20)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(20)
    };

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(20)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Line 1: child1, child2
    child1.Layout.Top.ShouldBe(0);
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Top.ShouldBe(0);
    child2.Layout.Left.ShouldBe(50);
    // Line 2: child3
    child3.Layout.Top.ShouldBe(20);
    child3.Layout.Left.ShouldBe(0);
  }

  public void ShouldPreserveItemOrderWithinLines()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      Width = FlexValue.Point(60),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    FlexNode child4 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);
    root.AddChild(child4);

    Engine.CalculateLayout(root, 60, 100);

    // Line 1: 1, 2 - Line 2: 3, 4
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(30);
    child3.Layout.Left.ShouldBe(0);
    child4.Layout.Left.ShouldBe(30);
  }

  public void ShouldWrapSingleOversizedItem()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(20)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(20)
    };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 50, 100);

    // Each child on its own line
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(20);
  }
}

/// <summary>
/// Tests for FlexWrap.WrapReverse behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexWrapReverseTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldReverseCrossAxisLineOrder()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.WrapReverse,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(20)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(20)
    };

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(20)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // With wrap-reverse, lines go from bottom to top
    // Line 2 (child3) starts at a higher position than Line 1
    child1.Layout.Top.ShouldBeGreaterThan(child3.Layout.Top);
  }
}

/// <summary>
/// Tests for FlexWrap with gap.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexWrapWithGapTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldApplyRowGapBetweenLines()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      RowGap = 10
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(20)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(20)
    };

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(20)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100);

    // Line 2 offset by line height + row gap
    child3.Layout.Top.ShouldBe(30); // 20 + 10 gap
  }

  public void ShouldApplyColumnGapWithinLines()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      ColumnGap = 10
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(20)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(20)
    };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100);

    // child2 offset by child1 width + column gap
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(40); // 30 + 10 gap
  }
}

/// <summary>
/// Tests for FlexWrap in column direction.
/// </summary>
[TestTag(TestTags.Fast)]
public class FlexWrapColumnTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldWrapInColumnDirection()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column,
      FlexWrap = FlexWrap.Wrap,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(60)
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 60);

    // Column 1: child1, child2 - Column 2: child3
    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(0);
    child3.Layout.Left.ShouldBe(30);
  }
}

// =============================================================================
// Padding Tests (Task 040)
// =============================================================================

/// <summary>
/// Tests for basic padding behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class PaddingBasicTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldOffsetChildByPaddingTop()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.Top, FlexValue.Point(10));

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Top.ShouldBe(10);
  }

  public void ShouldOffsetChildByPaddingLeft()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.Left, FlexValue.Point(15));

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Left.ShouldBe(15);
  }

  public void ShouldOffsetChildByPaddingAll()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.All, FlexValue.Point(10));

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    child.Layout.Top.ShouldBe(10);
    child.Layout.Left.ShouldBe(10);
  }
}

/// <summary>
/// Tests for padding reducing available content space.
/// </summary>
[TestTag(TestTags.Fast)]
public class PaddingContentSpaceTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldReduceContentWidthByHorizontalPadding()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.Left, FlexValue.Point(10));
    root.SetPadding(Edge.Right, FlexValue.Point(10));

    FlexNode child = new()
    {
      FlexGrow = 1,
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Child fills remaining 80px (100 - 10 - 10)
    child.Layout.Width.ShouldBe(80);
  }

  public void ShouldReduceContentHeightByVerticalPadding()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.Top, FlexValue.Point(15));
    root.SetPadding(Edge.Bottom, FlexValue.Point(15));

    FlexNode child = new()
    {
      FlexGrow = 1,
      Width = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Child fills remaining 70px (100 - 15 - 15)
    child.Layout.Height.ShouldBe(70);
  }
}

/// <summary>
/// Tests for multiple children with padding.
/// </summary>
[TestTag(TestTags.Fast)]
public class PaddingMultipleChildrenTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldPositionMultipleChildrenWithPadding()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.Left, FlexValue.Point(10));

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(50)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100);

    // Both children offset by padding
    child1.Layout.Left.ShouldBe(10);
    child2.Layout.Left.ShouldBe(40); // 10 + 30
  }
}

/// <summary>
/// Tests for padding with logical edges (Start/End).
/// </summary>
[TestTag(TestTags.Fast)]
public class PaddingLogicalEdgeTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldApplyStartPaddingInLtr()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.Start, FlexValue.Point(15));

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // Start = Left in LTR
    child.Layout.Left.ShouldBe(15);
  }

  public void ShouldApplyStartPaddingInRtl()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.Start, FlexValue.Point(15));

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Rtl);

    // Start = Right in RTL, so child is at left edge with right padding
    // Child positioned at: 100 - 50 - 15 = 35
    child.Layout.Left.ShouldBe(35);
  }
}

/// <summary>
/// Tests for padding combined with border.
/// </summary>
[TestTag(TestTags.Fast)]
public class PaddingWithBorderTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldCombinePaddingAndBorderForChildOffset()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.All, FlexValue.Point(10));
    root.SetBorder(Edge.All, 5);

    FlexNode child = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    // Child offset by padding + border
    child.Layout.Left.ShouldBe(15); // 10 + 5
    child.Layout.Top.ShouldBe(15);  // 10 + 5
  }
}

// =============================================================================
// Rounding Tests (Task 041)
// =============================================================================

/// <summary>
/// Tests for basic pixel rounding behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class PixelRoundingBasicTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldRoundFractionalValuesToPixels()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    // 100 / 3 = 33.333... each
    FlexNode child1 = new() { FlexGrow = 1, Height = FlexValue.Point(50) };
    FlexNode child2 = new() { FlexGrow = 1, Height = FlexValue.Point(50) };
    FlexNode child3 = new() { FlexGrow = 1, Height = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr, roundToPixelGrid: true);

    // Rounding should preserve total: 33 + 33 + 34 = 100 or similar
    float totalWidth = child1.Layout.Width + child2.Layout.Width + child3.Layout.Width;
    totalWidth.ShouldBe(100, tolerance: 0.001f);
  }

  public void ShouldRoundExplicitDimensionsToWholePixels()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100.5f),
      Height = FlexValue.Point(50.7f)
    };

    Engine.CalculateLayout(root, 200, 200, Direction.Ltr, roundToPixelGrid: true);

    // Explicit dimensions should be rounded to whole pixels
    (root.Layout.Width % 1).ShouldBe(0);
    (root.Layout.Height % 1).ShouldBe(0);
  }
}

/// <summary>
/// Tests for cumulative rounding across multiple children.
/// </summary>
[TestTag(TestTags.Fast)]
public class CumulativeRoundingTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldDistributeRoundingErrorAcrossSevenChildren()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    for (int i = 0; i < 7; i++)
    {
      root.AddChild(new FlexNode { FlexGrow = 1, Height = FlexValue.Point(50) });
    }

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr, roundToPixelGrid: true);

    // Total should still be exactly 100
    float totalWidth = 0;
    for (int i = 0; i < 7; i++)
    {
      totalWidth += root.Children[i].Layout.Width;
    }

    totalWidth.ShouldBe(100);
  }

  public void ShouldAccumulatePositionsCorrectly()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    for (int i = 0; i < 7; i++)
    {
      root.AddChild(new FlexNode { FlexGrow = 1, Height = FlexValue.Point(50) });
    }

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr, roundToPixelGrid: true);

    // Last child should end exactly at 100
    FlexNode lastChild = root.Children[6];
    float rightEdge = lastChild.Layout.Left + lastChild.Layout.Width;
    rightEdge.ShouldBe(100);
  }
}

/// <summary>
/// Tests for rounding without pixel grid enabled.
/// </summary>
[TestTag(TestTags.Fast)]
public class NoRoundingTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldProduceFractionalValuesWithoutRounding()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new() { FlexGrow = 1, Height = FlexValue.Point(50) };
    FlexNode child2 = new() { FlexGrow = 1, Height = FlexValue.Point(50) };
    FlexNode child3 = new() { FlexGrow = 1, Height = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    // roundToPixelGrid defaults to false
    Engine.CalculateLayout(root, 100, 100);

    // Without rounding, we expect fractional values (33.333...)
    // Check that at least one is fractional
    bool hasFractional = (child1.Layout.Width % 1) != 0 ||
                         (child2.Layout.Width % 1) != 0 ||
                         (child3.Layout.Width % 1) != 0;
    hasFractional.ShouldBeTrue();
  }
}

/// <summary>
/// Tests for rounding with nested layouts.
/// </summary>
[TestTag(TestTags.Fast)]
public class NestedRoundingTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldRoundNestedLayoutsCorrectly()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode parent = new()
    {
      FlexDirection = FlexDirection.Column,
      FlexGrow = 1,
      Height = FlexValue.Point(100)
    };

    FlexNode child1 = new() { FlexGrow = 1, Width = FlexValue.Point(30) };
    FlexNode child2 = new() { FlexGrow = 1, Width = FlexValue.Point(30) };

    parent.AddChild(child1);
    parent.AddChild(child2);
    root.AddChild(parent);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr, roundToPixelGrid: true);

    // Nested children should also be rounded
    (child1.Layout.Height % 1).ShouldBe(0);
    (child2.Layout.Height % 1).ShouldBe(0);

    // Their heights should sum to parent height
    float totalHeight = child1.Layout.Height + child2.Layout.Height;
    totalHeight.ShouldBe(100);
  }
}

// =============================================================================
// Measure Tests (Task 045)
// =============================================================================

/// <summary>
/// Tests for MeasureFunc invocation.
/// </summary>
[TestTag(TestTags.Fast)]
public class MeasureFuncInvocationTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldCallMeasureFuncDuringLayout()
  {
    bool measureCalled = false;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new();
    child.MeasureFunc = (_, _, _, _, _) =>
    {
      measureCalled = true;
      return new Size(50, 50);
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    measureCalled.ShouldBeTrue();
  }

  public void ShouldNotCallMeasureFuncForHiddenElements()
  {
    bool measureCalled = false;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Display = Display.None
    };
    child.MeasureFunc = (_, _, _, _, _) =>
    {
      measureCalled = true;
      return new Size(50, 50);
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    measureCalled.ShouldBeFalse();
  }
}

/// <summary>
/// Tests for MeasureFunc result determining node size.
/// </summary>
[TestTag(TestTags.Fast)]
public class MeasureResultTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldUseMeasureResultForNodeSize()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new();
    child.MeasureFunc = (_, _, _, _, _) => new Size(75, 100);

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 200);

    child.Layout.Width.ShouldBe(75);
    child.Layout.Height.ShouldBe(100);
  }

  public void ShouldConstrainMeasureResultByMaxWidth()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      MaxWidth = FlexValue.Point(50)
    };
    child.MeasureFunc = (_, _, _, _, _) => new Size(100, 50); // Wants 100, max is 50

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 200);

    child.Layout.Width.ShouldBe(50); // Clamped to max
  }

  public void ShouldConstrainMeasureResultByMaxHeight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      MaxHeight = FlexValue.Point(30)
    };
    child.MeasureFunc = (_, _, _, _, _) => new Size(50, 100); // Wants 100, max is 30

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 200);

    child.Layout.Height.ShouldBe(30); // Clamped to max
  }

  public void ShouldEnforceMinWidthOnMeasureResult()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      MinWidth = FlexValue.Point(80)
    };
    child.MeasureFunc = (_, _, _, _, _) => new Size(50, 50); // Wants 50, min is 80

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 200);

    child.Layout.Width.ShouldBe(80); // Clamped to min
  }
}

/// <summary>
/// Tests for MeasureFunc receiving correct parameters.
/// </summary>
[TestTag(TestTags.Fast)]
public class MeasureParameterTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldReceiveNodeReference()
  {
    FlexNode? receivedNode = null;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new();
    child.MeasureFunc = (node, _, _, _, _) =>
    {
      receivedNode = node;
      return new Size(50, 50);
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    receivedNode.ShouldBe(child);
  }

  public void ShouldReceiveWidthModeExactlyForFixedWidth()
  {
    MeasureMode receivedWidthMode = MeasureMode.Undefined;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50)
    };
    child.MeasureFunc = (_, _, widthMode, _, _) =>
    {
      receivedWidthMode = widthMode;
      return new Size(50, 50);
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100);

    receivedWidthMode.ShouldBe(MeasureMode.Exactly);
  }
}

/// <summary>
/// Tests for measured node behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class MeasuredNodeBehaviorTests
{
  public void ShouldMarkNodeAsLeafWithMeasureFunc()
  {
    FlexNode node = new();
    node.MeasureFunc = (_, _, _, _, _) => new Size(50, 50);

    node.IsLeaf.ShouldBeTrue();
  }

  public void ShouldRemoveMeasureFuncBySettingNull()
  {
    FlexNode node = new();
    node.MeasureFunc = (_, _, _, _, _) => new Size(50, 50);

    node.HasMeasureFunc.ShouldBeTrue();

    node.MeasureFunc = null;

    node.HasMeasureFunc.ShouldBeFalse();
  }
}

/// <summary>
/// Tests for box-sizing behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class BoxSizingTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldDefaultToBorderBox()
  {
    FlexNode node = new();

    node.BoxSizing.ShouldBe(BoxSizing.BorderBox);
  }

  public void ShouldIncludePaddingInWidthWithBorderBox()
  {
    // Test with a child node since root nodes use available dimensions
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.BorderBox
    };
    child.SetPadding(Edge.Left, FlexValue.Point(10));
    child.SetPadding(Edge.Right, FlexValue.Point(10));

    root.AddChild(child);
    Engine.CalculateLayout(root, 200, 200, Direction.Ltr);

    // Total width is 100, content area is 80
    child.Layout.Width.ShouldBe(100);
  }

  public void ShouldIncludeBorderInWidthWithBorderBox()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.BorderBox
    };
    child.SetBorder(Edge.Left, 10);
    child.SetBorder(Edge.Right, 10);

    root.AddChild(child);
    Engine.CalculateLayout(root, 200, 200, Direction.Ltr);

    // Total width is 100 including borders
    child.Layout.Width.ShouldBe(100);
  }

  public void ShouldAddPaddingToWidthWithContentBox()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.ContentBox
    };
    child.SetPadding(Edge.Left, FlexValue.Point(10));
    child.SetPadding(Edge.Right, FlexValue.Point(10));

    root.AddChild(child);
    Engine.CalculateLayout(root, 200, 200, Direction.Ltr);

    // Content is 100, total is 120 with padding
    child.Layout.Width.ShouldBe(120);
  }

  public void ShouldAddBorderToWidthWithContentBox()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.ContentBox
    };
    child.SetBorder(Edge.Left, 10);
    child.SetBorder(Edge.Right, 10);

    root.AddChild(child);
    Engine.CalculateLayout(root, 200, 200, Direction.Ltr);

    // Content is 100, total is 120 with borders
    child.Layout.Width.ShouldBe(120);
  }

  public void ShouldAddPaddingAndBorderToWidthWithContentBox()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.ContentBox
    };
    child.SetPadding(Edge.Left, FlexValue.Point(10));
    child.SetPadding(Edge.Right, FlexValue.Point(10));
    child.SetBorder(Edge.Left, 5);
    child.SetBorder(Edge.Right, 5);

    root.AddChild(child);
    Engine.CalculateLayout(root, 200, 200, Direction.Ltr);

    // Content is 100, total is 130 with padding (20) and border (10)
    child.Layout.Width.ShouldBe(130);
  }

  public void ShouldApplyContentBoxToHeight()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      BoxSizing = BoxSizing.ContentBox
    };
    child.SetPadding(Edge.Top, FlexValue.Point(15));
    child.SetPadding(Edge.Bottom, FlexValue.Point(15));

    root.AddChild(child);
    Engine.CalculateLayout(root, 200, 200, Direction.Ltr);

    // Content height is 100, total is 130 with padding
    child.Layout.Height.ShouldBe(130);
  }

  public void ShouldWorkWithFlexGrowAndBorderBox()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      FlexGrow = 1,
      BoxSizing = BoxSizing.BorderBox
    };
    child.SetPadding(Edge.Left, FlexValue.Point(10));
    child.SetPadding(Edge.Right, FlexValue.Point(10));

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100, Direction.Ltr);

    // Child fills 200, padding is included in that
    child.Layout.Width.ShouldBe(200);
  }

  public void ShouldWorkWithFlexGrowAndContentBox()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      FlexGrow = 1,
      BoxSizing = BoxSizing.ContentBox
    };
    child.SetPadding(Edge.Left, FlexValue.Point(10));
    child.SetPadding(Edge.Right, FlexValue.Point(10));

    root.AddChild(child);

    Engine.CalculateLayout(root, 200, 100, Direction.Ltr);

    // Child content fills available space (200 - padding consumed by flex grow)
    // With content-box, the flex-grow should give content size, then padding added
    // This is complex - flex grow distributes to content, then padding adds
    // The child ends up with width 200 (content) but we need to check behavior
    child.Layout.Width.ShouldBe(200);
  }
}

/// <summary>
/// Tests for baseline alignment behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class BaselineAlignmentTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldAlignChildrenToMaxBaseline()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Baseline
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };
    child1.BaselineFunc = (_, _, _) => 25; // Baseline at 25px from top

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(20)
    };
    child2.BaselineFunc = (_, _, _) => 15; // Baseline at 15px from top

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // child1 has baseline at 25, child2 has baseline at 15
    // Max baseline is 25, so:
    // child1 is at top 0 (its baseline 25 aligns with line baseline 25)
    // child2 is at top 10 (its baseline 15 + offset 10 = 25)
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(10);
  }

  public void ShouldCallBaselineFuncDuringLayout()
  {
    bool baselineCalled = false;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Baseline
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    child.BaselineFunc = (_, _, _) =>
    {
      baselineCalled = true;
      return 40;
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    baselineCalled.ShouldBeTrue();
  }

  public void ShouldPassCorrectDimensionsToBaselineFunc()
  {
    float receivedWidth = 0;
    float receivedHeight = 0;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Baseline
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(40)
    };
    child.BaselineFunc = (_, width, height) =>
    {
      receivedWidth = width;
      receivedHeight = height;
      return height;
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    receivedWidth.ShouldBe(50);
    receivedHeight.ShouldBe(40);
  }

  public void ShouldDefaultBaselineToBottomWhenNoFunc()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.Baseline
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };
    // No BaselineFunc - defaults to bottom (height = 30)

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(20)
    };
    // No BaselineFunc - defaults to bottom (height = 20)

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // child1 baseline is 30, child2 baseline is 20
    // Max baseline is 30, so:
    // child1 is at top 0
    // child2 is at top 10 (baseline 20 + 10 = 30)
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(10);
  }

  public void ShouldHandleAlignSelfBaseline()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.FlexStart // Default is FlexStart
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30),
      AlignSelf = AlignSelf.Baseline
    };
    child1.BaselineFunc = (_, _, _) => 25;

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(20),
      AlignSelf = AlignSelf.Baseline
    };
    child2.BaselineFunc = (_, _, _) => 15;

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // Both use baseline via AlignSelf
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(10);
  }

  public void ShouldMixBaselineWithOtherAlignments()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      AlignItems = AlignItems.FlexStart
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };
    // Uses FlexStart (default) - top = 0

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(20),
      AlignSelf = AlignSelf.Baseline
    };
    child2.BaselineFunc = (_, _, _) => 15;

    FlexNode child3 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(40),
      AlignSelf = AlignSelf.Baseline
    };
    child3.BaselineFunc = (_, _, _) => 30;

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // child1: FlexStart, top = 0
    // child2 & child3 use baseline
    // child2 baseline is 15, child3 baseline is 30
    // Line baseline is 30 (max of baseline items)
    // child2 top = 30 - 15 = 15
    // child3 top = 30 - 30 = 0
    child1.Layout.Top.ShouldBe(0);
    child2.Layout.Top.ShouldBe(15);
    child3.Layout.Top.ShouldBe(0);
  }
}

/// <summary>
/// Tests for margin layout behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class MarginTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldOffsetElementWithMarginLeft()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    child.SetMargin(Edge.Left, FlexValue.Point(10));

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child.Layout.Left.ShouldBe(10);
  }

  public void ShouldOffsetElementWithMarginTop()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Column
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    child.SetMargin(Edge.Top, FlexValue.Point(10));

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child.Layout.Top.ShouldBe(10);
  }

  public void ShouldSpaceChildrenWithMargins()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };
    child1.SetMargin(Edge.Right, FlexValue.Point(10));

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(40); // 30 + 10 margin
  }

  public void ShouldCenterWithAutoMargins()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };
    child.SetMargin(Edge.Left, FlexValue.Auto);
    child.SetMargin(Edge.Right, FlexValue.Auto);

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child.Layout.Left.ShouldBe(35); // (100 - 30) / 2
  }

  public void ShouldPushToEndWithLeadingAutoMargin()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };
    child.SetMargin(Edge.Left, FlexValue.Auto);

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child.Layout.Left.ShouldBe(70); // 100 - 30
  }

  public void ShouldCenterOnCrossAxisWithAutoMargins()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };
    child.SetMargin(Edge.Top, FlexValue.Auto);
    child.SetMargin(Edge.Bottom, FlexValue.Auto);

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child.Layout.Top.ShouldBe(35); // (100 - 30) / 2
  }

  public void ShouldResolvePercentageMargin()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(30),
      Height = FlexValue.Point(30)
    };
    child.SetMargin(Edge.Left, FlexValue.Percent(10)); // 10% of 100 = 10

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child.Layout.Left.ShouldBe(10);
  }

  public void ShouldHandleNegativeMargin()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    FlexNode child2 = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    child2.SetMargin(Edge.Left, FlexValue.Point(-10));

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child1.Layout.Left.ShouldBe(0);
    child2.Layout.Left.ShouldBe(40); // 50 - 10 = 40, overlapping
  }

  public void ShouldAccountForMarginsInFlexGrow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Height = FlexValue.Point(30),
      FlexGrow = 1
    };
    child.SetMargin(Edge.Left, FlexValue.Point(10));
    child.SetMargin(Edge.Right, FlexValue.Point(10));

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // Child should grow to fill remaining space after margins
    child.Layout.Left.ShouldBe(10);
    child.Layout.Width.ShouldBe(80); // 100 - 10 - 10
  }
}

/// <summary>
/// Tests for border handling in layout calculations.
/// </summary>
[TestTag(TestTags.Fast)]
public class BorderTests
{
  private readonly FlexLayoutEngine Engine = new();

  #region Uniform Border

  public void ShouldOffsetChildContentWithUniformBorder()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.All, 10);

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    child.Layout.Left.ShouldBe(10);
    child.Layout.Top.ShouldBe(10);
  }

  public void ShouldReduceAvailableSpaceForChildrenWithUniformBorder()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.All, 10);

    FlexNode child = new()
    {
      Width = FlexValue.Percent(100),
      Height = FlexValue.Percent(100)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // Available space is 100 - 10 - 10 = 80
    child.Layout.Width.ShouldBe(80);
    child.Layout.Height.ShouldBe(80);
  }

  #endregion
}

/// <summary>
/// Tests for HadOverflow flag behavior.
/// </summary>
[TestTag(TestTags.Fast)]
public class HadOverflowTests
{
  private FlexLayoutEngine Engine { get; } = new();

  public void ShouldBeTrueWhenChildrenOverflowHorizontally()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    // FlexShrink = 0 prevents shrinking, causing actual overflow
    FlexNode child1 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50), FlexShrink = 0 };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50), FlexShrink = 0 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // 160px of children in 100px container with no shrink = overflow
    root.Layout.HadOverflow.ShouldBeTrue();
  }

  public void ShouldBeTrueWhenChildrenOverflowVertically()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Column
    };

    // FlexShrink = 0 prevents shrinking, causing actual overflow
    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(80), FlexShrink = 0 };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(80), FlexShrink = 0 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // 160px of children in 100px container with no shrink = overflow
    root.Layout.HadOverflow.ShouldBeTrue();
  }

  public void ShouldBeFalseWhenContentFits()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(40), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(40), Height = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // 80px of children in 100px container = no overflow
    root.Layout.HadOverflow.ShouldBeFalse();
  }

  public void ShouldBeFalseWhenContentExactlyFits()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // 100px of children in 100px container = exactly fits
    root.Layout.HadOverflow.ShouldBeFalse();
  }

  public void ShouldBeFalseWhenWrapPreventsOverflow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap
    };

    FlexNode child1 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(40) };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(40) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // Wrapping prevents main-axis overflow
    root.Layout.HadOverflow.ShouldBeFalse();
  }

  public void ShouldBeTrueWhenWrapCausesCrossAxisOverflow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap
    };

    FlexNode child1 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(60) };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(60) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // Wrapping causes cross-axis overflow (120px in 100px)
    root.Layout.HadOverflow.ShouldBeTrue();
  }

  public void ShouldNotCountPaddingAsOverflow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.Top, FlexValue.Point(10));
    root.SetPadding(Edge.Bottom, FlexValue.Point(10));

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(80)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // 80 + 20 padding = 100, exactly fits
    root.Layout.HadOverflow.ShouldBeFalse();
  }

  public void ShouldDetectOverflowWithPadding()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetPadding(Edge.Top, FlexValue.Point(10));
    root.SetPadding(Edge.Bottom, FlexValue.Point(10));

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(90)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // 90 + 20 padding = 110 > 100 = overflow
    root.Layout.HadOverflow.ShouldBeTrue();
  }

  public void ShouldDetectOverflowWithBorder()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };
    root.SetBorder(Edge.Top, 10);
    root.SetBorder(Edge.Bottom, 10);

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(90)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // 90 + 20 border = 110 > 100 = overflow
    root.Layout.HadOverflow.ShouldBeTrue();
  }

  public void ShouldNotAffectHadOverflowWithAbsoluteChildren()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode absoluteChild = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200),
      PositionType = PositionType.Absolute
    };

    root.AddChild(absoluteChild);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // Absolute children don't cause HadOverflow
    root.Layout.HadOverflow.ShouldBeFalse();
  }

  public void ShouldOnlyAffectImmediateParent()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode parent = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    // FlexShrink = 0 to prevent shrinking and cause actual overflow
    FlexNode child1 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50), FlexShrink = 0 };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50), FlexShrink = 0 };

    parent.AddChild(child1);
    parent.AddChild(child2);
    root.AddChild(parent);

    Engine.CalculateLayout(root, 200, 200, Direction.Ltr);

    parent.Layout.HadOverflow.ShouldBeTrue();
    root.Layout.HadOverflow.ShouldBeFalse();
  }

  public void ShouldDetectOverflowInBothAxes()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(150),
      Height = FlexValue.Point(150)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // Child overflows both axes
    root.Layout.HadOverflow.ShouldBeTrue();
  }

  public void ShouldBeFalseWhenFlexShrinkPreventsOverflow()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50), FlexShrink = 1 };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50), FlexShrink = 1 };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // FlexShrink prevents overflow
    root.Layout.HadOverflow.ShouldBeFalse();
  }

  public void ShouldBeTrueWhenMinWidthPreventsFullShrink()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new()
    {
      Width = FlexValue.Point(80),
      Height = FlexValue.Point(50),
      FlexShrink = 1,
      MinWidth = FlexValue.Point(60)
    };
    FlexNode child2 = new()
    {
      Width = FlexValue.Point(80),
      Height = FlexValue.Point(50),
      FlexShrink = 1,
      MinWidth = FlexValue.Point(60)
    };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // MinWidth prevents full shrink, causing overflow (120 > 100)
    root.Layout.HadOverflow.ShouldBeTrue();
  }

  public void ShouldResetBetweenLayouts()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    // FlexShrink = 0 to cause actual overflow
    FlexNode child1 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50), FlexShrink = 0 };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(50), FlexShrink = 0 };

    root.AddChild(child1);
    root.AddChild(child2);

    // First layout - should overflow (160 > 100 with no shrink)
    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);
    root.Layout.HadOverflow.ShouldBeTrue();

    // Change children to fit
    child1.Width = FlexValue.Point(40);
    child2.Width = FlexValue.Point(40);

    // Second layout - should not overflow (80 <= 100)
    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);
    root.Layout.HadOverflow.ShouldBeFalse();
  }
}

/// <summary>
/// Tests for intrinsic sizing behavior - how nodes size themselves based on content.
/// </summary>
[TestTag(TestTags.Fast)]
public class IntrinsicSizeTests
{
  private readonly FlexLayoutEngine Engine = new();

  #region Intrinsic Width

  public void ShouldSizeToChildrenWidthInRow()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(80); // 50 + 30
  }

  public void ShouldUseMaxChildWidthInColumn()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(20) };
    FlexNode child2 = new() { Width = FlexValue.Point(80), Height = FlexValue.Point(20) };
    FlexNode child3 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(20) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(80); // Max of children
  }

  public void ShouldIncludeGapInIntrinsicWidth()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      ColumnGap = 10
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(110); // 50 + 10 + 50
  }

  #endregion

  #region Intrinsic Height

  public void ShouldSizeToChildrenHeightInColumn()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(40) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Height.ShouldBe(70); // 30 + 40
  }

  public void ShouldUseMaxChildHeightInRow()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(60) };
    FlexNode child3 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(40) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Height.ShouldBe(60); // Max of children
  }

  public void ShouldIncludeGapInIntrinsicHeight()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column,
      RowGap = 15
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(30) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Height.ShouldBe(75); // 30 + 15 + 30
  }

  #endregion

  #region Intrinsic Size with MeasureFunc

  public void ShouldUseMeasureFuncResultForIntrinsicSize()
  {
    FlexNode root = new();

    FlexNode child = new();
    child.MeasureFunc = (_, _, _, _, _) => new Size(100, 50);

    root.AddChild(child);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    child.Layout.Width.ShouldBe(100);
    child.Layout.Height.ShouldBe(50);
    root.Layout.Width.ShouldBe(100);
    root.Layout.Height.ShouldBe(50);
  }

  public void ShouldPassUndefinedModeForUnconstrainedLayout()
  {
    // Track the first call's measure modes (intrinsic sizing phase)
    MeasureMode? firstWidthMode = null;
    MeasureMode? firstHeightMode = null;

    FlexNode root = new();

    FlexNode child = new();
    child.MeasureFunc = (_, _, widthMode, _, heightMode) =>
    {
      // Capture only the first call (intrinsic sizing)
      firstWidthMode ??= widthMode;
      firstHeightMode ??= heightMode;
      return new Size(50, 50);
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    // First call should be with Undefined mode for intrinsic sizing
    firstWidthMode.ShouldBe(MeasureMode.Undefined);
    firstHeightMode.ShouldBe(MeasureMode.Undefined);
  }

  public void ShouldPassAtMostModeForConstrainedLayout()
  {
    // Track the first call's measure mode (intrinsic sizing phase)
    MeasureMode? firstWidthMode = null;

    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100)
    };

    FlexNode child = new();
    child.MeasureFunc = (_, _, widthMode, _, _) =>
    {
      // Capture only the first call
      firstWidthMode ??= widthMode;
      return new Size(50, 50);
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, 100, 100, Direction.Ltr);

    // First call should be with AtMost mode when parent has constraints
    firstWidthMode.ShouldBe(MeasureMode.AtMost);
  }

  public void ShouldCombineMeasureFuncResultsForMultipleChildren()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new();
    child1.MeasureFunc = (_, _, _, _, _) => new Size(40, 30);

    FlexNode child2 = new();
    child2.MeasureFunc = (_, _, _, _, _) => new Size(60, 50);

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(100); // 40 + 60
    root.Layout.Height.ShouldBe(50); // Max of 30, 50
  }

  #endregion

  #region Intrinsic Size with Constraints

  public void ShouldRespectMinWidthOnIntrinsicSize()
  {
    FlexNode root = new()
    {
      MinWidth = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(100); // Min enforced
  }

  public void ShouldRespectMaxWidthOnIntrinsicSize()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row,
      MaxWidth = FlexValue.Point(80)
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(80); // Max enforced (children want 100)
  }

  public void ShouldRespectMinHeightOnIntrinsicSize()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column,
      MinHeight = FlexValue.Point(100)
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(30)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Height.ShouldBe(100); // Min enforced
  }

  public void ShouldRespectMaxHeightOnIntrinsicSize()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column,
      MaxHeight = FlexValue.Point(50)
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(40) };
    FlexNode child2 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(40) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Height.ShouldBe(50); // Max enforced (children want 80)
  }

  #endregion

  #region Intrinsic Size with Nested Content

  public void ShouldCalculateIntrinsicSizeFromNestedChildren()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Column
    };

    FlexNode container = new()
    {
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(30), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(40), Height = FlexValue.Point(30) };

    container.AddChild(child1);
    container.AddChild(child2);
    root.AddChild(container);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    container.Layout.Width.ShouldBe(70); // 30 + 40
    root.Layout.Width.ShouldBe(70);
  }

  public void ShouldCalculateIntrinsicSizeFromDeeplyNestedChildren()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row
    };

    FlexNode level1 = new()
    {
      FlexDirection = FlexDirection.Column
    };

    FlexNode level2 = new()
    {
      FlexDirection = FlexDirection.Row
    };

    FlexNode leaf = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    level2.AddChild(leaf);
    level1.AddChild(level2);
    root.AddChild(level1);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(50);
    root.Layout.Height.ShouldBe(50);
  }

  #endregion

  #region Intrinsic Size with Padding and Border

  public void ShouldIncludePaddingInIntrinsicSize()
  {
    FlexNode root = new();
    root.SetPadding(Edge.All, FlexValue.Point(10));

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(70);  // 50 + 10 + 10
    root.Layout.Height.ShouldBe(70); // 50 + 10 + 10
  }

  public void ShouldIncludeBorderInIntrinsicSize()
  {
    FlexNode root = new();
    root.SetBorder(Edge.All, 5);

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(60);  // 50 + 5 + 5
    root.Layout.Height.ShouldBe(60); // 50 + 5 + 5
  }

  #endregion

  #region Intrinsic Size with Flex

  public void ShouldNotGrowWithoutAvailableSpace()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50),
      FlexGrow = 1
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    // No available space to grow into when parent is intrinsically sized
    child.Layout.Width.ShouldBe(50);
  }

  public void ShouldUseFlexBasisForIntrinsicSize()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row
    };

    FlexNode child = new()
    {
      FlexBasis = FlexValue.Point(80),
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    child.Layout.Width.ShouldBe(80);
    root.Layout.Width.ShouldBe(80);
  }

  #endregion

  #region Edge Cases

  public void ShouldHandleEmptyContainer()
  {
    FlexNode root = new();

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(0);
    root.Layout.Height.ShouldBe(0);
  }

  public void ShouldHandleSingleChildWithNoSize()
  {
    FlexNode root = new();
    FlexNode child = new();

    root.AddChild(child);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(0);
    root.Layout.Height.ShouldBe(0);
  }

  public void ShouldHandleZeroSizeChild()
  {
    FlexNode root = new()
    {
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50), Height = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(0), Height = FlexValue.Point(0) };

    root.AddChild(child1);
    root.AddChild(child2);

    Engine.CalculateLayout(root, float.NaN, float.NaN, Direction.Ltr);

    root.Layout.Width.ShouldBe(50);
    root.Layout.Height.ShouldBe(50);
  }

  #endregion
}

// =============================================================================
// Layout Idempotency Tests (Task 080 - GitHub Issue #2)
// =============================================================================

/// <summary>
/// Tests for layout idempotency - calling CalculateLayout multiple times
/// should produce consistent results.
/// </summary>
[TestTag(TestTags.Fast)]
public class LayoutIdempotencyTests
{
  private readonly FlexLayoutEngine Engine = new();

  public void ShouldProduceSameResultsWhenCalledTwice()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(80),
      Height = FlexValue.Point(24),
      FlexDirection = FlexDirection.Column
    };

    FlexNode child = new()
    {
      Height = FlexValue.Point(10),
      FlexGrow = 0
    };

    root.AddChild(child);

    // First call
    Engine.CalculateLayout(root, 80, 24);
    float firstWidth = child.Layout.Width;
    float firstHeight = child.Layout.Height;
    float firstLeft = child.Layout.Left;
    float firstTop = child.Layout.Top;

    // Verify first call produced expected results
    firstWidth.ShouldBe(80);
    firstHeight.ShouldBe(10);

    // Second call - should produce identical results
    Engine.CalculateLayout(root, 80, 24);

    child.Layout.Width.ShouldBe(firstWidth);
    child.Layout.Height.ShouldBe(firstHeight);
    child.Layout.Left.ShouldBe(firstLeft);
    child.Layout.Top.ShouldBe(firstTop);
  }

  public void ShouldProduceSameResultsWhenCalledThreeTimes()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(30) };
    FlexNode child3 = new() { Width = FlexValue.Point(30) };

    root.AddChild(child1);
    root.AddChild(child2);
    root.AddChild(child3);

    // First call
    Engine.CalculateLayout(root, 100, 100);
    float c1Left = child1.Layout.Left;
    float c2Left = child2.Layout.Left;
    float c3Left = child3.Layout.Left;

    // Second call
    Engine.CalculateLayout(root, 100, 100);
    child1.Layout.Left.ShouldBe(c1Left);
    child2.Layout.Left.ShouldBe(c2Left);
    child3.Layout.Left.ShouldBe(c3Left);

    // Third call
    Engine.CalculateLayout(root, 100, 100);
    child1.Layout.Left.ShouldBe(c1Left);
    child2.Layout.Left.ShouldBe(c2Left);
    child3.Layout.Left.ShouldBe(c3Left);
  }

  public void ShouldRecalculateAfterStyleChange()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Column
    };

    FlexNode child = new()
    {
      Height = FlexValue.Point(50)
    };

    root.AddChild(child);

    // First layout
    Engine.CalculateLayout(root, 100, 100);
    child.Layout.Width.ShouldBe(100);

    // Change style
    root.Width = FlexValue.Point(200);

    // Second layout should reflect the change
    Engine.CalculateLayout(root, 200, 100);
    child.Layout.Width.ShouldBe(200);
  }

  public void ShouldHandleNestedContainersIdempotently()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200),
      FlexDirection = FlexDirection.Column
    };

    FlexNode container = new()
    {
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { FlexGrow = 1 };
    FlexNode child2 = new() { FlexGrow = 1 };

    container.AddChild(child1);
    container.AddChild(child2);
    root.AddChild(container);

    // First call
    Engine.CalculateLayout(root, 200, 200);
    float containerWidth = container.Layout.Width;
    float child1Width = child1.Layout.Width;
    float child2Width = child2.Layout.Width;

    containerWidth.ShouldBe(200);
    child1Width.ShouldBe(100);
    child2Width.ShouldBe(100);

    // Second call - should be identical
    Engine.CalculateLayout(root, 200, 200);

    container.Layout.Width.ShouldBe(containerWidth);
    child1.Layout.Width.ShouldBe(child1Width);
    child2.Layout.Width.ShouldBe(child2Width);
  }

  public void ShouldHandleFlexGrowIdempotently()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(300),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { FlexGrow = 1 };
    FlexNode child2 = new() { FlexGrow = 2 };

    root.AddChild(child1);
    root.AddChild(child2);

    // First call
    Engine.CalculateLayout(root, 300, 100);
    float firstChild1Width = child1.Layout.Width;
    float firstChild2Width = child2.Layout.Width;

    firstChild1Width.ShouldBe(100); // 1/3 of 300
    firstChild2Width.ShouldBe(200); // 2/3 of 300

    // Second call
    Engine.CalculateLayout(root, 300, 100);

    child1.Layout.Width.ShouldBe(firstChild1Width);
    child2.Layout.Width.ShouldBe(firstChild2Width);
  }

  public void ShouldHandleAbsolutePositioningIdempotently()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(200)
    };

    FlexNode absolute = new()
    {
      PositionType = PositionType.Absolute,
      Width = FlexValue.Point(50),
      Height = FlexValue.Point(50)
    };
    absolute.SetPosition(Edge.Left, FlexValue.Point(10));
    absolute.SetPosition(Edge.Top, FlexValue.Point(20));

    root.AddChild(absolute);

    // First call
    Engine.CalculateLayout(root, 200, 200);
    float firstLeft = absolute.Layout.Left;
    float firstTop = absolute.Layout.Top;

    firstLeft.ShouldBe(10);
    firstTop.ShouldBe(20);

    // Second call
    Engine.CalculateLayout(root, 200, 200);

    absolute.Layout.Left.ShouldBe(firstLeft);
    absolute.Layout.Top.ShouldBe(firstTop);
  }

  public void ShouldHandleFlexWrapIdempotently()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row,
      FlexWrap = FlexWrap.Wrap
    };

    FlexNode child1 = new() { Width = FlexValue.Point(60), Height = FlexValue.Point(30) };
    FlexNode child2 = new() { Width = FlexValue.Point(60), Height = FlexValue.Point(30) };

    root.AddChild(child1);
    root.AddChild(child2);

    // First call
    Engine.CalculateLayout(root, 100, 100);
    float c1Top = child1.Layout.Top;
    float c2Top = child2.Layout.Top;

    // Second call
    Engine.CalculateLayout(root, 100, 100);

    child1.Layout.Top.ShouldBe(c1Top);
    child2.Layout.Top.ShouldBe(c2Top);
  }

  public void ShouldHandleMeasureFuncIdempotently()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(100),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode measuredChild = new()
    {
      MeasureFunc = (_, _, _, _, _) => new Size(50, 30)
    };

    root.AddChild(measuredChild);

    // First call
    Engine.CalculateLayout(root, 100, 100);
    float firstWidth = measuredChild.Layout.Width;
    float firstHeight = measuredChild.Layout.Height;

    firstWidth.ShouldBe(50);
    firstHeight.ShouldBe(30);

    // Second call
    Engine.CalculateLayout(root, 100, 100);

    measuredChild.Layout.Width.ShouldBe(firstWidth);
    measuredChild.Layout.Height.ShouldBe(firstHeight);
  }

  public void ShouldHandleRtlDirectionIdempotently()
  {
    FlexNode root = new()
    {
      Width = FlexValue.Point(200),
      Height = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };

    FlexNode child1 = new() { Width = FlexValue.Point(50) };
    FlexNode child2 = new() { Width = FlexValue.Point(50) };

    root.AddChild(child1);
    root.AddChild(child2);

    // First RTL call
    Engine.CalculateLayout(root, 200, 100, Direction.Rtl);
    float c1Left = child1.Layout.Left;
    float c2Left = child2.Layout.Left;

    // In RTL, children positioned from right
    c1Left.ShouldBe(150);
    c2Left.ShouldBe(100);

    // Second RTL call
    Engine.CalculateLayout(root, 200, 100, Direction.Rtl);

    child1.Layout.Left.ShouldBe(c1Left);
    child2.Layout.Left.ShouldBe(c2Left);
  }
}
