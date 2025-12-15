/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for Cache utilities
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

/// <summary>
/// Tests for Cache utilities.
/// </summary>
public class CacheTests
{
    #region Same Sizing Mode and Size - Can Use Cache

    public void SameWidthAndHeightSpecsShouldUseCachedMeasurement()
    {
        // Arrange - Same sizing modes and available sizes
        SizingMode widthMode = SizingMode.StretchFit;
        float availableWidth = 100f;
        SizingMode heightMode = SizingMode.StretchFit;
        float availableHeight = 200f;

        SizingMode lastWidthMode = SizingMode.StretchFit;
        float lastAvailableWidth = 100f;
        SizingMode lastHeightMode = SizingMode.StretchFit;
        float lastAvailableHeight = 200f;

        float lastComputedWidth = 100f;
        float lastComputedHeight = 200f;

        // Act
        bool canUse = Cache.CanUseCachedMeasurement(
            widthMode, availableWidth,
            heightMode, availableHeight,
            lastWidthMode, lastAvailableWidth,
            lastHeightMode, lastAvailableHeight,
            lastComputedWidth, lastComputedHeight,
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert
        canUse.ShouldBeTrue();
    }

    #endregion

    #region Negative Computed Dimensions - Cannot Use Cache

    public void NegativeComputedWidthShouldNotUseCachedMeasurement()
    {
        // Arrange - Negative computed width
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.StretchFit, 100f,
            SizingMode.StretchFit, 200f,
            SizingMode.StretchFit, 100f,
            SizingMode.StretchFit, 200f,
            lastComputedWidth: -1f,
            lastComputedHeight: 200f,
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert
        canUse.ShouldBeFalse();
    }

    public void NegativeComputedHeightShouldNotUseCachedMeasurement()
    {
        // Arrange - Negative computed height
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.StretchFit, 100f,
            SizingMode.StretchFit, 200f,
            SizingMode.StretchFit, 100f,
            SizingMode.StretchFit, 200f,
            lastComputedWidth: 100f,
            lastComputedHeight: -1f,
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert
        canUse.ShouldBeFalse();
    }

    #endregion

    #region StretchFit (Exact) Matches Old Computed Size

    public void StretchFitMatchingOldComputedSizeShouldUseCachedMeasurement()
    {
        // Arrange - StretchFit mode with size matching old computed
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.StretchFit, 100f,      // Current: StretchFit, available = 100
            SizingMode.StretchFit, 200f,      // Current height
            SizingMode.MaxContent, float.NaN, // Last: MaxContent, undefined
            SizingMode.MaxContent, float.NaN, // Last height mode
            lastComputedWidth: 100f,          // Last computed width = available
            lastComputedHeight: 200f,
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert - Can use because StretchFit 100 matches lastComputedWidth 100
        canUse.ShouldBeTrue();
    }

    #endregion

    #region MaxContent to FitContent - Old Still Fits

    public void FitContentWhenOldWasMaxContentAndStillFitsShouldUseCachedMeasurement()
    {
        // Arrange - FitContent mode when old was MaxContent and computed size still fits
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.FitContent, 150f,      // Current: FitContent, available = 150
            SizingMode.FitContent, 250f,      // Current height
            SizingMode.MaxContent, float.NaN, // Last: MaxContent
            SizingMode.MaxContent, float.NaN, // Last height mode
            lastComputedWidth: 100f,          // Last computed width = 100 (fits in 150)
            lastComputedHeight: 200f,
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert - Can use because 100 <= 150
        canUse.ShouldBeTrue();
    }

    public void FitContentWhenOldWasMaxContentButDoesNotFitShouldNotUseCachedMeasurement()
    {
        // Arrange - FitContent mode when old was MaxContent but computed size is too big
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.FitContent, 50f,       // Current: FitContent, available = 50
            SizingMode.FitContent, 250f,      // Current height
            SizingMode.MaxContent, float.NaN, // Last: MaxContent
            SizingMode.FitContent, 250f,      // Last height mode matches
            lastComputedWidth: 100f,          // Last computed width = 100 (doesn't fit in 50)
            lastComputedHeight: 200f,
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert - Cannot use because 100 > 50
        canUse.ShouldBeFalse();
    }

    #endregion

    #region FitContent Stricter Constraint

    public void FitContentStricterButStillValidShouldUseCachedMeasurement()
    {
        // Arrange - FitContent with stricter constraint but computed still valid
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.FitContent, 150f,      // Current: FitContent, available = 150 (stricter)
            SizingMode.FitContent, 250f,      // Current height
            SizingMode.FitContent, 200f,      // Last: FitContent, available = 200
            SizingMode.FitContent, 300f,      // Last height
            lastComputedWidth: 100f,          // Last computed = 100 (still <= 150)
            lastComputedHeight: 200f,
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert - Can use because lastComputedWidth (100) <= new size (150)
        canUse.ShouldBeTrue();
    }

    public void FitContentStricterAndComputedTooLargeShouldNotUseCachedMeasurement()
    {
        // Arrange - FitContent with stricter constraint and computed is too large
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.FitContent, 80f,       // Current: FitContent, available = 80 (stricter)
            SizingMode.FitContent, 250f,      // Current height
            SizingMode.FitContent, 200f,      // Last: FitContent, available = 200
            SizingMode.FitContent, 300f,      // Last height
            lastComputedWidth: 100f,          // Last computed = 100 (> 80, not valid)
            lastComputedHeight: 200f,
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert - Cannot use because lastComputedWidth (100) > new size (80)
        canUse.ShouldBeFalse();
    }

    #endregion

    #region Different Sizing Modes - Cannot Use Cache

    public void DifferentWidthModeShouldNotUseCachedMeasurement()
    {
        // Arrange - Different width modes
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.MaxContent, 100f,      // Current: MaxContent
            SizingMode.StretchFit, 200f,      // Current height
            SizingMode.StretchFit, 100f,      // Last: StretchFit
            SizingMode.StretchFit, 200f,      // Last height
            lastComputedWidth: 100f,
            lastComputedHeight: 200f,
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert - Different modes and no special case applies
        canUse.ShouldBeFalse();
    }

    #endregion

    #region Margins Affect Comparison

    public void MarginsShouldAffectSizeComparison()
    {
        // Arrange - With margins, effective size is reduced
        // StretchFit mode with available 100 - margin 10 = 90, should match computed 90
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.StretchFit, 100f,      // Available = 100
            SizingMode.StretchFit, 200f,
            SizingMode.MaxContent, float.NaN,
            SizingMode.MaxContent, float.NaN,
            lastComputedWidth: 90f,           // Computed = 90
            lastComputedHeight: 190f,
            marginRow: 10f, marginColumn: 10f,
            config: null);

        // Assert - 100 - 10 margin = 90, matches computed 90
        canUse.ShouldBeTrue();
    }

    #endregion

    #region With Config (Pixel Grid Rounding)

    public void ConfigWithPointScaleFactorShouldUseRoundedComparison()
    {
        // Arrange - Config with point scale factor should round values
        Config config = new() { PointScaleFactor = 2.0f };

        // Values that would be different but round to the same pixel
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.StretchFit, 100.3f,    // Rounds to 100
            SizingMode.StretchFit, 200f,
            SizingMode.StretchFit, 100.4f,    // Rounds to 100
            SizingMode.StretchFit, 200f,
            lastComputedWidth: 100f,
            lastComputedHeight: 200f,
            marginRow: 0f, marginColumn: 0f,
            config: config);

        // Assert - Should be able to use cache due to rounding
        canUse.ShouldBeTrue();
    }

    public void ConfigWithZeroPointScaleFactorShouldNotUseRounding()
    {
        // Arrange - Config with zero point scale factor should not round
        Config config = new() { PointScaleFactor = 0f };

        // Exact same values without rounding
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.StretchFit, 100f,
            SizingMode.StretchFit, 200f,
            SizingMode.StretchFit, 100f,
            SizingMode.StretchFit, 200f,
            lastComputedWidth: 100f,
            lastComputedHeight: 200f,
            marginRow: 0f, marginColumn: 0f,
            config: config);

        // Assert
        canUse.ShouldBeTrue();
    }

    #endregion

    #region Undefined Values

    public void UndefinedComputedDimensionsShouldBeAllowed()
    {
        // Arrange - Undefined computed dimensions are allowed
        bool canUse = Cache.CanUseCachedMeasurement(
            SizingMode.MaxContent, float.NaN,
            SizingMode.MaxContent, float.NaN,
            SizingMode.MaxContent, float.NaN,
            SizingMode.MaxContent, float.NaN,
            lastComputedWidth: float.NaN,     // Undefined
            lastComputedHeight: float.NaN,    // Undefined
            marginRow: 0f, marginColumn: 0f,
            config: null);

        // Assert - Same mode with undefined values should work
        canUse.ShouldBeTrue();
    }

    #endregion
}
