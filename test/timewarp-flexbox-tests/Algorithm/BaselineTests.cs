/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for Baseline utilities
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for Baseline utilities.
/// </summary>
public class BaselineTests
{
    #region IsBaselineLayout - Row Direction

    public void IsBaselineLayoutShouldReturnTrueWhenAlignItemsIsBaseline()
    {
        // Arrange
        FlexNode node = new()
        {
            Style =
            {
                FlexDirection = FlexDirection.Row,
                AlignItems = Align.Baseline
            }
        };

        // Act
        bool result = Baseline.IsBaselineLayout(node);

        // Assert
        result.ShouldBeTrue();
    }

    public void IsBaselineLayoutShouldReturnTrueWhenChildHasAlignSelfBaseline()
    {
        // Arrange
        FlexNode parent = new()
        {
            Style =
            {
                FlexDirection = FlexDirection.Row,
                AlignItems = Align.FlexStart
            }
        };

        FlexNode child = new()
        {
            Style =
            {
                AlignSelf = Align.Baseline,
                PositionType = PositionType.Relative // Not absolute
            }
        };
        parent.InsertChild(child, parent.GetChildCount());

        // Act
        bool result = Baseline.IsBaselineLayout(parent);

        // Assert
        result.ShouldBeTrue();
    }

    public void IsBaselineLayoutShouldReturnFalseWhenNoBaselineAlignment()
    {
        // Arrange
        FlexNode node = new()
        {
            Style =
            {
                FlexDirection = FlexDirection.Row,
                AlignItems = Align.FlexStart
            }
        };

        // Act
        bool result = Baseline.IsBaselineLayout(node);

        // Assert
        result.ShouldBeFalse();
    }

    #endregion

    #region IsBaselineLayout - Column Direction

    public void IsBaselineLayoutShouldReturnFalseForColumnDirection()
    {
        // Arrange - Baseline doesn't apply in column direction
        FlexNode node = new()
        {
            Style =
            {
                FlexDirection = FlexDirection.Column,
                AlignItems = Align.Baseline
            }
        };

        // Act
        bool result = Baseline.IsBaselineLayout(node);

        // Assert - Always false for column
        result.ShouldBeFalse();
    }

    public void IsBaselineLayoutShouldReturnFalseForColumnReverseDirection()
    {
        // Arrange
        FlexNode node = new()
        {
            Style =
            {
                FlexDirection = FlexDirection.ColumnReverse,
                AlignItems = Align.Baseline
            }
        };

        // Act
        bool result = Baseline.IsBaselineLayout(node);

        // Assert - Always false for column-reverse
        result.ShouldBeFalse();
    }

    #endregion

    #region IsBaselineLayout - Absolute Children Ignored

    public void IsBaselineLayoutShouldIgnoreAbsolutePositionedChildren()
    {
        // Arrange
        FlexNode parent = new()
        {
            Style =
            {
                FlexDirection = FlexDirection.Row,
                AlignItems = Align.FlexStart
            }
        };

        // Absolute child with baseline alignment should be ignored
        FlexNode absoluteChild = new()
        {
            Style =
            {
                AlignSelf = Align.Baseline,
                PositionType = PositionType.Absolute
            }
        };
        parent.InsertChild(absoluteChild, parent.GetChildCount());

        // Act
        bool result = Baseline.IsBaselineLayout(parent);

        // Assert - Should be false because absolute children are ignored
        result.ShouldBeFalse();
    }

    #endregion

    #region CalculateBaseline - No Children

    public void CalculateBaselineShouldReturnHeightForNodeWithNoChildren()
    {
        // Arrange
        FlexNode node = new();
        node.Layout.SetMeasuredDimension(Dimension.Height, 100f);

        // Act
        float baseline = Baseline.CalculateBaseline(node);

        // Assert - Returns height when no children
        baseline.ShouldBe(100f);
    }

    #endregion

    #region CalculateBaseline - With Custom Baseline Function

    public void CalculateBaselineShouldUseCustomBaselineFunc()
    {
        // Arrange
        FlexNode node = new();
        node.Layout.SetMeasuredDimension(Dimension.Width, 100f);
        node.Layout.SetMeasuredDimension(Dimension.Height, 50f);

        // Set a custom baseline function
        node.BaselineFunc = (_, _, height) => height * 0.8f; // 80% of height

        // Act
        float baseline = Baseline.CalculateBaseline(node);

        // Assert - Should use custom baseline func: 50 * 0.8 = 40
        baseline.ShouldBe(40f);
    }

    #endregion

    #region CalculateBaseline - First Child Without Baseline Alignment

    public void CalculateBaselineShouldUseFirstNonAbsoluteChildAsDefault()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Height, 200f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Height, 50f);
        child.SetLayoutPosition(10f, PhysicalEdge.Top);
        parent.InsertChild(child, parent.GetChildCount());

        // Act
        float baseline = Baseline.CalculateBaseline(parent);

        // Assert - Child baseline (50, its height) + top position (10) = 60
        baseline.ShouldBe(60f);
    }

    #endregion

    #region CalculateBaseline - Child With Baseline Alignment

    public void CalculateBaselineShouldPreferChildWithBaselineAlignment()
    {
        // Arrange
        FlexNode parent = new()
        {
            Style =
            {
                FlexDirection = FlexDirection.Row,
                AlignItems = Align.FlexStart
            }
        };
        parent.Layout.SetMeasuredDimension(Dimension.Height, 200f);

        // First child without baseline alignment
        FlexNode child1 = new();
        child1.Layout.SetMeasuredDimension(Dimension.Height, 30f);
        child1.SetLayoutPosition(0f, PhysicalEdge.Top);
        parent.InsertChild(child1, parent.GetChildCount());

        // Second child with baseline alignment (should be preferred)
        FlexNode child2 = new()
        {
            Style = { AlignSelf = Align.Baseline }
        };
        child2.Layout.SetMeasuredDimension(Dimension.Height, 40f);
        child2.SetLayoutPosition(20f, PhysicalEdge.Top);
        parent.InsertChild(child2, parent.GetChildCount());

        // Act
        float baseline = Baseline.CalculateBaseline(parent);

        // Assert - Should use child2: height (40) + top (20) = 60
        baseline.ShouldBe(60f);
    }

    #endregion

    #region CalculateBaseline - Skips Absolute Children

    public void CalculateBaselineShouldSkipAbsolutePositionedChildren()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Height, 200f);

        // Absolute child should be skipped
        FlexNode absoluteChild = new()
        {
            Style = { PositionType = PositionType.Absolute }
        };
        absoluteChild.Layout.SetMeasuredDimension(Dimension.Height, 100f);
        absoluteChild.SetLayoutPosition(0f, PhysicalEdge.Top);
        parent.InsertChild(absoluteChild, parent.GetChildCount());

        // Relative child should be used
        FlexNode relativeChild = new()
        {
            Style = { PositionType = PositionType.Relative }
        };
        relativeChild.Layout.SetMeasuredDimension(Dimension.Height, 50f);
        relativeChild.SetLayoutPosition(10f, PhysicalEdge.Top);
        parent.InsertChild(relativeChild, parent.GetChildCount());

        // Act
        float baseline = Baseline.CalculateBaseline(parent);

        // Assert - Should use relative child: height (50) + top (10) = 60
        baseline.ShouldBe(60f);
    }

    #endregion

    #region CalculateBaseline - Reference Baseline Child

    public void CalculateBaselineShouldPreferReferenceBaselineChild()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Height, 200f);

        // First child
        FlexNode child1 = new();
        child1.Layout.SetMeasuredDimension(Dimension.Height, 30f);
        child1.SetLayoutPosition(0f, PhysicalEdge.Top);
        parent.InsertChild(child1, parent.GetChildCount());

        // Second child marked as reference baseline
        FlexNode child2 = new();
        child2.IsReferenceBaseline = true;
        child2.Layout.SetMeasuredDimension(Dimension.Height, 40f);
        child2.SetLayoutPosition(25f, PhysicalEdge.Top);
        parent.InsertChild(child2, parent.GetChildCount());

        // Act
        float baseline = Baseline.CalculateBaseline(parent);

        // Assert - Should use reference baseline child: height (40) + top (25) = 65
        baseline.ShouldBe(65f);
    }

    #endregion

    #region CalculateBaseline - Only First Line Children

    public void CalculateBaselineShouldOnlyConsiderFirstLineChildren()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Height, 200f);

        // Child on line 0
        FlexNode child1 = new();
        child1.LineIndex = 0;
        child1.Layout.SetMeasuredDimension(Dimension.Height, 30f);
        child1.SetLayoutPosition(5f, PhysicalEdge.Top);
        parent.InsertChild(child1, parent.GetChildCount());

        // Child on line 1 - should be ignored
        FlexNode child2 = new();
        child2.LineIndex = 1;
        child2.Layout.SetMeasuredDimension(Dimension.Height, 100f);
        child2.SetLayoutPosition(50f, PhysicalEdge.Top);
        parent.InsertChild(child2, parent.GetChildCount());

        // Act
        float baseline = Baseline.CalculateBaseline(parent);

        // Assert - Should use child1 (line 0): height (30) + top (5) = 35
        baseline.ShouldBe(35f);
    }

    #endregion
}
