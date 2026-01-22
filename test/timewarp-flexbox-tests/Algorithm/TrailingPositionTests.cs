/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for TrailingPosition utilities
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for TrailingPosition utilities.
/// </summary>
public class TrailingPositionTests
{
    #region GetPositionOfOppositeEdge - Row Axis

    public void GetPositionOfOppositeEdgeShouldCalculateCorrectlyForRowAxis()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Width, 200f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Width, 50f);

        float startPosition = 20f; // Child is at left=20

        // Act
        float oppositePosition = TrailingPosition.GetPositionOfOppositeEdge(
            startPosition,
            FlexDirection.Row,
            parent,
            child);

        // Assert - parent width (200) - child width (50) - start position (20) = 130
        oppositePosition.ShouldBe(130f);
    }

    public void GetPositionOfOppositeEdgeShouldCalculateCorrectlyForRowReverseAxis()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Width, 300f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Width, 100f);

        float startPosition = 50f;

        // Act
        float oppositePosition = TrailingPosition.GetPositionOfOppositeEdge(
            startPosition,
            FlexDirection.RowReverse,
            parent,
            child);

        // Assert - 300 - 100 - 50 = 150
        oppositePosition.ShouldBe(150f);
    }

    #endregion

    #region GetPositionOfOppositeEdge - Column Axis

    public void GetPositionOfOppositeEdgeShouldCalculateCorrectlyForColumnAxis()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Height, 400f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Height, 80f);

        float startPosition = 30f;

        // Act
        float oppositePosition = TrailingPosition.GetPositionOfOppositeEdge(
            startPosition,
            FlexDirection.Column,
            parent,
            child);

        // Assert - 400 - 80 - 30 = 290
        oppositePosition.ShouldBe(290f);
    }

    public void GetPositionOfOppositeEdgeShouldCalculateCorrectlyForColumnReverseAxis()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Height, 500f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Height, 150f);

        float startPosition = 100f;

        // Act
        float oppositePosition = TrailingPosition.GetPositionOfOppositeEdge(
            startPosition,
            FlexDirection.ColumnReverse,
            parent,
            child);

        // Assert - 500 - 150 - 100 = 250
        oppositePosition.ShouldBe(250f);
    }

    #endregion

    #region GetPositionOfOppositeEdge - Edge Cases

    public void GetPositionOfOppositeEdgeWithZeroStartPositionShouldWork()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Width, 100f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Width, 40f);

        // Act
        float oppositePosition = TrailingPosition.GetPositionOfOppositeEdge(
            0f,
            FlexDirection.Row,
            parent,
            child);

        // Assert - 100 - 40 - 0 = 60
        oppositePosition.ShouldBe(60f);
    }

    public void GetPositionOfOppositeEdgeWithChildFillingShouldReturnZero()
    {
        // Arrange - Child fills entire parent
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Width, 100f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Width, 100f);

        // Act
        float oppositePosition = TrailingPosition.GetPositionOfOppositeEdge(
            0f,
            FlexDirection.Row,
            parent,
            child);

        // Assert - 100 - 100 - 0 = 0
        oppositePosition.ShouldBe(0f);
    }

    #endregion

    #region SetChildTrailingPosition - Row Axis

    public void SetChildTrailingPositionShouldSetRightEdgeForRowAxis()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Width, 200f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Width, 50f);
        child.SetLayoutPosition(20f, PhysicalEdge.Left); // Left position

        // Act
        TrailingPosition.SetChildTrailingPosition(parent, child, FlexDirection.Row);

        // Assert - Right should be: 200 - 50 - 20 = 130
        child.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(130f);
    }

    public void SetChildTrailingPositionShouldSetLeftEdgeForRowReverseAxis()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Width, 200f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Width, 50f);
        child.SetLayoutPosition(30f, PhysicalEdge.Right); // Right position

        // Act
        TrailingPosition.SetChildTrailingPosition(parent, child, FlexDirection.RowReverse);

        // Assert - Left should be: 200 - 50 - 30 = 120
        child.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(120f);
    }

    #endregion

    #region SetChildTrailingPosition - Column Axis

    public void SetChildTrailingPositionShouldSetBottomEdgeForColumnAxis()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Height, 300f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Height, 60f);
        child.SetLayoutPosition(40f, PhysicalEdge.Top); // Top position

        // Act
        TrailingPosition.SetChildTrailingPosition(parent, child, FlexDirection.Column);

        // Assert - Bottom should be: 300 - 60 - 40 = 200
        child.Layout.GetPosition(PhysicalEdge.Bottom).ShouldBe(200f);
    }

    public void SetChildTrailingPositionShouldSetTopEdgeForColumnReverseAxis()
    {
        // Arrange
        FlexNode parent = new();
        parent.Layout.SetMeasuredDimension(Dimension.Height, 300f);

        FlexNode child = new();
        child.Layout.SetMeasuredDimension(Dimension.Height, 60f);
        child.SetLayoutPosition(50f, PhysicalEdge.Bottom); // Bottom position

        // Act
        TrailingPosition.SetChildTrailingPosition(parent, child, FlexDirection.ColumnReverse);

        // Assert - Top should be: 300 - 60 - 50 = 190
        child.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(190f);
    }

    #endregion

    #region NeedsTrailingPosition - Reversed Axes

    public void NeedsTrailingPositionShouldReturnTrueForRowReverse()
    {
        // Act
        bool result = TrailingPosition.NeedsTrailingPosition(FlexDirection.RowReverse);

        // Assert
        result.ShouldBeTrue();
    }

    public void NeedsTrailingPositionShouldReturnTrueForColumnReverse()
    {
        // Act
        bool result = TrailingPosition.NeedsTrailingPosition(FlexDirection.ColumnReverse);

        // Assert
        result.ShouldBeTrue();
    }

    #endregion

    #region NeedsTrailingPosition - Non-Reversed Axes

    public void NeedsTrailingPositionShouldReturnFalseForRow()
    {
        // Act
        bool result = TrailingPosition.NeedsTrailingPosition(FlexDirection.Row);

        // Assert
        result.ShouldBeFalse();
    }

    public void NeedsTrailingPositionShouldReturnFalseForColumn()
    {
        // Act
        bool result = TrailingPosition.NeedsTrailingPosition(FlexDirection.Column);

        // Assert
        result.ShouldBeFalse();
    }

    #endregion
}
