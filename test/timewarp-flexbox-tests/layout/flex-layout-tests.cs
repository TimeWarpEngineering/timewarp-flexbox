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
