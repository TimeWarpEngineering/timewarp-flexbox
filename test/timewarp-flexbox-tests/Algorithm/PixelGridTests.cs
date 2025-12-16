/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for PixelGrid utilities
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

/// <summary>
/// Tests for PixelGrid utilities.
/// </summary>
public class PixelGridTests
{
    #region RoundValueToPixelGrid - Basic Rounding

    public void RoundValueToPixelGridShouldRoundToNearestPixel()
    {
        // Arrange - Scale factor of 1 (1:1 point to pixel)
        double value = 10.4;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - 10.4 rounds down to 10
        result.ShouldBe(10f);
    }

    public void RoundValueToPixelGridShouldRoundUpWhenFractionIsHalf()
    {
        // Arrange
        double value = 10.5;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - 10.5 rounds up to 11
        result.ShouldBe(11f);
    }

    public void RoundValueToPixelGridShouldRoundUpWhenFractionIsGreaterThanHalf()
    {
        // Arrange
        double value = 10.6;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - 10.6 rounds up to 11
        result.ShouldBe(11f);
    }

    #endregion

    #region RoundValueToPixelGrid - Various Scale Factors

    public void RoundValueToPixelGridShouldWorkWithScaleFactorOf2()
    {
        // Arrange - Scale factor of 2 (2x display)
        double value = 10.25;
        double pointScaleFactor = 2.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - 10.25 * 2 = 20.5 rounds to 21, then 21/2 = 10.5
        result.ShouldBe(10.5f);
    }

    public void RoundValueToPixelGridShouldWorkWithScaleFactorOf3()
    {
        // Arrange - Scale factor of 3 (3x display)
        double value = 10.0;
        double pointScaleFactor = 3.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - 10.0 * 3 = 30.0, already whole number
        result.ShouldBe(10f);
    }

    public void RoundValueToPixelGridShouldWorkWithFractionalScaleFactor()
    {
        // Arrange - Scale factor of 1.5
        double value = 10.0;
        double pointScaleFactor = 1.5;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - 10.0 * 1.5 = 15.0, already whole number
        result.ShouldBe(10f);
    }

    #endregion

    #region RoundValueToPixelGrid - Force Ceil

    public void RoundValueToPixelGridShouldForceCeilWhenRequested()
    {
        // Arrange
        double value = 10.1;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, forceCeil: true, forceFloor: false);

        // Assert - 10.1 rounds up to 11 when forceCeil is true
        result.ShouldBe(11f);
    }

    public void RoundValueToPixelGridShouldForceCeilWithScaleFactor()
    {
        // Arrange
        double value = 10.1;
        double pointScaleFactor = 2.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, forceCeil: true, forceFloor: false);

        // Assert - 10.1 * 2 = 20.2, ceil to 21, then 21/2 = 10.5
        result.ShouldBe(10.5f);
    }

    public void RoundValueToPixelGridShouldNotChangeCeilWhenAlreadyWhole()
    {
        // Arrange
        double value = 10.0;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, forceCeil: true, forceFloor: false);

        // Assert - 10.0 is already whole, stays 10
        result.ShouldBe(10f);
    }

    #endregion

    #region RoundValueToPixelGrid - Force Floor

    public void RoundValueToPixelGridShouldForceFloorWhenRequested()
    {
        // Arrange
        double value = 10.9;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, forceCeil: false, forceFloor: true);

        // Assert - 10.9 rounds down to 10 when forceFloor is true
        result.ShouldBe(10f);
    }

    public void RoundValueToPixelGridShouldForceFloorWithScaleFactor()
    {
        // Arrange
        double value = 10.9;
        double pointScaleFactor = 2.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, forceCeil: false, forceFloor: true);

        // Assert - 10.9 * 2 = 21.8, floor to 21, then 21/2 = 10.5
        result.ShouldBe(10.5f);
    }

    public void RoundValueToPixelGridShouldNotChangeFloorWhenAlreadyWhole()
    {
        // Arrange
        double value = 10.0;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, forceCeil: false, forceFloor: true);

        // Assert - 10.0 is already whole, stays 10
        result.ShouldBe(10f);
    }

    #endregion

    #region RoundValueToPixelGrid - Negative Values

    public void RoundValueToPixelGridShouldHandleNegativeValuesCorrectly()
    {
        // Arrange
        double value = -10.4;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - -10.4 should round to -10 (toward zero)
        result.ShouldBe(-10f);
    }

    public void RoundValueToPixelGridShouldHandleNegativeValueWithHalfFraction()
    {
        // Arrange
        double value = -10.5;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - -10.5 should round to -10 (following standard rounding)
        result.ShouldBe(-10f);
    }

    public void RoundValueToPixelGridShouldForceCeilOnNegativeValue()
    {
        // Arrange
        double value = -10.1;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, forceCeil: true, forceFloor: false);

        // Assert - -10.1 with forceCeil should go to -10
        result.ShouldBe(-10f);
    }

    public void RoundValueToPixelGridShouldForceFloorOnNegativeValue()
    {
        // Arrange
        double value = -10.9;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, forceCeil: false, forceFloor: true);

        // Assert - -10.9 with forceFloor should go to -11
        result.ShouldBe(-11f);
    }

    #endregion

    #region RoundValueToPixelGrid - NaN Handling

    public void RoundValueToPixelGridShouldReturnNaNForNaNValue()
    {
        // Arrange
        double value = double.NaN;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert
        float.IsNaN(result).ShouldBeTrue();
    }

    public void RoundValueToPixelGridShouldReturnNaNForNaNScaleFactor()
    {
        // Arrange
        double value = 10.0;
        double pointScaleFactor = double.NaN;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert
        float.IsNaN(result).ShouldBeTrue();
    }

    public void RoundValueToPixelGridShouldReturnNaNForBothNaN()
    {
        // Arrange
        double value = double.NaN;
        double pointScaleFactor = double.NaN;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert
        float.IsNaN(result).ShouldBeTrue();
    }

    #endregion

    #region RoundValueToPixelGrid - Zero Scale Factor

    public void RoundValueToPixelGridShouldHandleZeroScaleFactor()
    {
        // Arrange - Zero scale factor means no rounding
        double value = 10.5;
        double pointScaleFactor = 0.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - With zero scale factor, result may be NaN or special value
        // The actual behavior depends on implementation
        float.IsNaN(result).ShouldBeTrue();
    }

    #endregion

    #region RoundValueToPixelGrid - Edge Cases

    public void RoundValueToPixelGridShouldHandleZeroValue()
    {
        // Arrange
        double value = 0.0;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert
        result.ShouldBe(0f);
    }

    public void RoundValueToPixelGridShouldHandleVerySmallFraction()
    {
        // Arrange - Value that's almost a whole number
        double value = 10.0001;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - Should round to 10 due to inexact equals check
        result.ShouldBe(10f);
    }

    public void RoundValueToPixelGridShouldHandleValueVeryCloseToNext()
    {
        // Arrange - Value that's almost the next whole number
        double value = 10.9999;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert - Should round to 11 due to inexact equals check
        result.ShouldBe(11f);
    }

    public void RoundValueToPixelGridShouldHandleLargeValue()
    {
        // Arrange
        double value = 1000000.5;
        double pointScaleFactor = 1.0;

        // Act
        float result = PixelGrid.RoundValueToPixelGrid(value, pointScaleFactor, false, false);

        // Assert
        result.ShouldBe(1000001f);
    }

    #endregion
}
