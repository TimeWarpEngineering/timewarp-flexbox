/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/numeric/Comparison.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides floating-point comparison utilities used throughout Yoga.
/// </summary>
/// <remarks>
/// Design Decision: In C++, these are standalone functions with concepts.
/// In C#, we use a static class with overloaded methods for float/double.
/// NaN is used to represent "undefined" values in Yoga's layout system.
///
/// Question: Should we use a tolerance-based struct instead of hardcoded epsilon?
/// Decision: Following C++ which uses hardcoded 0.0001f for simplicity and performance.
/// </remarks>
public static class Comparison
{
    /// <summary>
    /// The tolerance used for inexact floating-point comparisons.
    /// Matches the C++ hardcoded epsilon of 0.0001f.
    /// </summary>
    public const float Tolerance = 0.0001f;

    /// <summary>
    /// Checks if a float value is undefined (NaN).
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is NaN, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUndefined(float value) => float.IsNaN(value);

    /// <summary>
    /// Checks if a double value is undefined (NaN).
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is NaN, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUndefined(double value) => double.IsNaN(value);

    /// <summary>
    /// Checks if a float value is defined (not NaN).
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is not NaN, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDefined(float value) => !float.IsNaN(value);

    /// <summary>
    /// Checks if a double value is defined (not NaN).
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is not NaN, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDefined(double value) => !double.IsNaN(value);

    /// <summary>
    /// Checks if a float value is infinite (positive or negative).
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is infinite, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInfinity(float value) => float.IsInfinity(value);

    /// <summary>
    /// Checks if a double value is infinite (positive or negative).
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is infinite, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInfinity(double value) => double.IsInfinity(value);

    /// <summary>
    /// Returns the maximum of two float values, treating undefined values specially.
    /// If one value is undefined (NaN), returns the other value.
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>The maximum defined value, or NaN if both are undefined.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MaxOrDefined(float a, float b)
    {
        if (IsDefined(a) && IsDefined(b))
        {
            return MathF.Max(a, b);
        }

        return IsUndefined(a) ? b : a;
    }

    /// <summary>
    /// Returns the maximum of two double values, treating undefined values specially.
    /// If one value is undefined (NaN), returns the other value.
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>The maximum defined value, or NaN if both are undefined.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MaxOrDefined(double a, double b)
    {
        if (IsDefined(a) && IsDefined(b))
        {
            return Math.Max(a, b);
        }

        return IsUndefined(a) ? b : a;
    }

    /// <summary>
    /// Returns the minimum of two float values, treating undefined values specially.
    /// If one value is undefined (NaN), returns the other value.
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>The minimum defined value, or NaN if both are undefined.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MinOrDefined(float a, float b)
    {
        if (IsDefined(a) && IsDefined(b))
        {
            return MathF.Min(a, b);
        }

        return IsUndefined(a) ? b : a;
    }

    /// <summary>
    /// Returns the minimum of two double values, treating undefined values specially.
    /// If one value is undefined (NaN), returns the other value.
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>The minimum defined value, or NaN if both are undefined.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MinOrDefined(double a, double b)
    {
        if (IsDefined(a) && IsDefined(b))
        {
            return Math.Min(a, b);
        }

        return IsUndefined(a) ? b : a;
    }

    /// <summary>
    /// Compares two float values for approximate equality using a tolerance of 0.0001f.
    /// Returns true if both values are undefined (NaN).
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>True if the values are approximately equal or both undefined.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InexactEquals(float a, float b)
    {
        if (IsDefined(a) && IsDefined(b))
        {
            return MathF.Abs(a - b) < Tolerance;
        }

        return IsUndefined(a) && IsUndefined(b);
    }

    /// <summary>
    /// Compares two double values for approximate equality using a tolerance of 0.0001.
    /// Returns true if both values are undefined (NaN).
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>True if the values are approximately equal or both undefined.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InexactEquals(double a, double b)
    {
        if (IsDefined(a) && IsDefined(b))
        {
            return Math.Abs(a - b) < Tolerance;
        }

        return IsUndefined(a) && IsUndefined(b);
    }

    /// <summary>
    /// Compares two spans of float values for approximate element-wise equality.
    /// </summary>
    /// <param name="span1">First span.</param>
    /// <param name="span2">Second span.</param>
    /// <returns>True if all corresponding elements are approximately equal.</returns>
    public static bool InexactEquals(ReadOnlySpan<float> span1, ReadOnlySpan<float> span2)
    {
        if (span1.Length != span2.Length)
        {
            return false;
        }

        for (int i = 0; i < span1.Length; i++)
        {
            if (!InexactEquals(span1[i], span2[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Compares two float arrays for approximate element-wise equality.
    /// </summary>
    /// <param name="array1">First array.</param>
    /// <param name="array2">Second array.</param>
    /// <returns>True if all corresponding elements are approximately equal.</returns>
    public static bool InexactEquals(float[] array1, float[] array2)
    {
        return InexactEquals(array1.AsSpan(), array2.AsSpan());
    }
}
