/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for BoundAxis utilities
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for BoundAxis utilities.
/// </summary>
public class BoundAxisTests
{
    #region PaddingAndBorderForAxis - Row Direction

    public void PaddingAndBorderForAxisShouldReturnZeroForDefaultNode()
    {
        // Arrange
        FlexNode node = new();

        // Act
        float result = BoundAxis.PaddingAndBorderForAxis(
            node,
            FlexDirection.Row,
            Direction.LTR,
            widthSize: 100f);

        // Assert - Default node has no padding or border
        result.ShouldBe(0f);
    }

    public void PaddingAndBorderForAxisShouldIncludePaddingForRowAxis()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Left, StyleLength.Points(10f));
        node.Style.SetPadding(Edge.Right, StyleLength.Points(20f));

        // Act
        float result = BoundAxis.PaddingAndBorderForAxis(
            node,
            FlexDirection.Row,
            Direction.LTR,
            widthSize: 100f);

        // Assert - Left (10) + Right (20) = 30
        result.ShouldBe(30f);
    }

    public void PaddingAndBorderForAxisShouldIncludeBorderForRowAxis()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetBorder(Edge.Left, StyleLength.Points(5f));
        node.Style.SetBorder(Edge.Right, StyleLength.Points(15f));

        // Act
        float result = BoundAxis.PaddingAndBorderForAxis(
            node,
            FlexDirection.Row,
            Direction.LTR,
            widthSize: 100f);

        // Assert - Left (5) + Right (15) = 20
        result.ShouldBe(20f);
    }

    public void PaddingAndBorderForAxisShouldIncludeBothPaddingAndBorder()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Left, StyleLength.Points(10f));
        node.Style.SetPadding(Edge.Right, StyleLength.Points(10f));
        node.Style.SetBorder(Edge.Left, StyleLength.Points(5f));
        node.Style.SetBorder(Edge.Right, StyleLength.Points(5f));

        // Act
        float result = BoundAxis.PaddingAndBorderForAxis(
            node,
            FlexDirection.Row,
            Direction.LTR,
            widthSize: 100f);

        // Assert - Padding (10+10) + Border (5+5) = 30
        result.ShouldBe(30f);
    }

    #endregion

    #region PaddingAndBorderForAxis - Column Direction

    public void PaddingAndBorderForAxisShouldWorkForColumnAxis()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Top, StyleLength.Points(15f));
        node.Style.SetPadding(Edge.Bottom, StyleLength.Points(25f));

        // Act
        float result = BoundAxis.PaddingAndBorderForAxis(
            node,
            FlexDirection.Column,
            Direction.LTR,
            widthSize: 100f);

        // Assert - Top (15) + Bottom (25) = 40
        result.ShouldBe(40f);
    }

    #endregion

    #region PaddingAndBorderForAxis - RTL Direction

    public void PaddingAndBorderForAxisShouldRespectRTLDirection()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Start, StyleLength.Points(10f));
        node.Style.SetPadding(Edge.End, StyleLength.Points(20f));

        // Act
        float result = BoundAxis.PaddingAndBorderForAxis(
            node,
            FlexDirection.Row,
            Direction.RTL,
            widthSize: 100f);

        // Assert - Start (10) + End (20) = 30 (regardless of direction for total)
        result.ShouldBe(30f);
    }

    #endregion

    #region PaddingAndBorderForAxis - Percentage Padding

    public void PaddingAndBorderForAxisShouldResolvePercentagePadding()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Left, StyleLength.Percent(10f)); // 10% of 100 = 10
        node.Style.SetPadding(Edge.Right, StyleLength.Percent(20f)); // 20% of 100 = 20

        // Act
        float result = BoundAxis.PaddingAndBorderForAxis(
            node,
            FlexDirection.Row,
            Direction.LTR,
            widthSize: 100f);

        // Assert - 10% + 20% of 100 = 30
        result.ShouldBe(30f);
    }

    #endregion

    #region BoundAxisWithinMinAndMax - No Constraints

    public void BoundAxisWithinMinAndMaxShouldReturnValueWhenNoConstraints()
    {
        // Arrange
        FlexNode node = new();
        FloatOptional value = new(50f);

        // Act
        FloatOptional result = BoundAxis.BoundAxisWithinMinAndMax(
            node,
            Direction.LTR,
            FlexDirection.Row,
            value,
            axisSize: 100f,
            widthSize: 100f);

        // Assert
        result.Unwrap().ShouldBe(50f);
    }

    #endregion

    #region BoundAxisWithinMinAndMax - Max Constraint

    public void BoundAxisWithinMinAndMaxShouldClampToMaxWhenValueExceeds()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(40f));
        FloatOptional value = new(50f);

        // Act
        FloatOptional result = BoundAxis.BoundAxisWithinMinAndMax(
            node,
            Direction.LTR,
            FlexDirection.Row,
            value,
            axisSize: 100f,
            widthSize: 100f);

        // Assert - Clamped to max of 40
        result.Unwrap().ShouldBe(40f);
    }

    public void BoundAxisWithinMinAndMaxShouldNotClampWhenBelowMax()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(60f));
        FloatOptional value = new(50f);

        // Act
        FloatOptional result = BoundAxis.BoundAxisWithinMinAndMax(
            node,
            Direction.LTR,
            FlexDirection.Row,
            value,
            axisSize: 100f,
            widthSize: 100f);

        // Assert - Value is below max, returns value
        result.Unwrap().ShouldBe(50f);
    }

    #endregion

    #region BoundAxisWithinMinAndMax - Min Constraint

    public void BoundAxisWithinMinAndMaxShouldClampToMinWhenValueBelowMin()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(60f));
        FloatOptional value = new(50f);

        // Act
        FloatOptional result = BoundAxis.BoundAxisWithinMinAndMax(
            node,
            Direction.LTR,
            FlexDirection.Row,
            value,
            axisSize: 100f,
            widthSize: 100f);

        // Assert - Clamped to min of 60
        result.Unwrap().ShouldBe(60f);
    }

    public void BoundAxisWithinMinAndMaxShouldNotClampWhenAboveMin()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMinDimension(Dimension.Width, StyleSizeLength.Points(30f));
        FloatOptional value = new(50f);

        // Act
        FloatOptional result = BoundAxis.BoundAxisWithinMinAndMax(
            node,
            Direction.LTR,
            FlexDirection.Row,
            value,
            axisSize: 100f,
            widthSize: 100f);

        // Assert - Value is above min, returns value
        result.Unwrap().ShouldBe(50f);
    }

    #endregion

    #region BoundAxisWithinMinAndMax - Column Axis

    public void BoundAxisWithinMinAndMaxShouldWorkForColumnAxis()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(80f));
        FloatOptional value = new(100f);

        // Act
        FloatOptional result = BoundAxis.BoundAxisWithinMinAndMax(
            node,
            Direction.LTR,
            FlexDirection.Column,
            value,
            axisSize: 200f,
            widthSize: 100f);

        // Assert - Clamped to max height of 80
        result.Unwrap().ShouldBe(80f);
    }

    #endregion

    #region BoundAxisValue - Basic Usage

    public void BoundAxisValueShouldReturnValueWhenNoConstraints()
    {
        // Arrange
        FlexNode node = new();

        // Act
        float result = BoundAxis.BoundAxisValue(
            node,
            FlexDirection.Row,
            Direction.LTR,
            value: 50f,
            axisSize: 100f,
            widthSize: 100f);

        // Assert
        result.ShouldBe(50f);
    }

    #endregion

    #region BoundAxisValue - Padding Floor

    public void BoundAxisValueShouldNotGoBelowPaddingAndBorder()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Left, StyleLength.Points(20f));
        node.Style.SetPadding(Edge.Right, StyleLength.Points(20f));

        // Act - Try to set value to 10, but padding+border is 40
        float result = BoundAxis.BoundAxisValue(
            node,
            FlexDirection.Row,
            Direction.LTR,
            value: 10f,
            axisSize: 100f,
            widthSize: 100f);

        // Assert - Should be floored to padding+border = 40
        result.ShouldBe(40f);
    }

    public void BoundAxisValueShouldAllowValueAbovePaddingAndBorder()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetPadding(Edge.Left, StyleLength.Points(10f));
        node.Style.SetPadding(Edge.Right, StyleLength.Points(10f));

        // Act - Value of 50 is above padding+border (20)
        float result = BoundAxis.BoundAxisValue(
            node,
            FlexDirection.Row,
            Direction.LTR,
            value: 50f,
            axisSize: 100f,
            widthSize: 100f);

        // Assert - Should return the value
        result.ShouldBe(50f);
    }

    #endregion

    #region BoundAxisValue - Combined Min/Max and Padding Floor

    public void BoundAxisValueShouldApplyMinMaxThenPaddingFloor()
    {
        // Arrange
        FlexNode node = new();
        node.Style.SetMaxDimension(Dimension.Width, StyleSizeLength.Points(30f));
        node.Style.SetPadding(Edge.Left, StyleLength.Points(20f));
        node.Style.SetPadding(Edge.Right, StyleLength.Points(20f));

        // Act - Max would clamp to 30, but padding floor is 40
        float result = BoundAxis.BoundAxisValue(
            node,
            FlexDirection.Row,
            Direction.LTR,
            value: 100f,
            axisSize: 100f,
            widthSize: 100f);

        // Assert - Max (30) < Padding floor (40), so result is 40
        result.ShouldBe(40f);
    }

    #endregion
}
