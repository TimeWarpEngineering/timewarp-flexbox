namespace TimeWarp.Flexbox;

/// <summary>
/// Represents a size with width and height dimensions.
/// Used as the return type for measurement callbacks.
/// </summary>
public readonly struct Size : IEquatable<Size>
{
  /// <summary>
  /// Gets the width dimension.
  /// </summary>
  public float Width { get; }

  /// <summary>
  /// Gets the height dimension.
  /// </summary>
  public float Height { get; }

  /// <summary>
  /// Creates a new Size with the specified dimensions.
  /// </summary>
  /// <param name="width">The width dimension.</param>
  /// <param name="height">The height dimension.</param>
  public Size(float width, float height)
  {
    Width = width;
    Height = height;
  }

  /// <summary>
  /// A size with zero width and height.
  /// </summary>
  public static readonly Size Zero = new(0, 0);

  public bool Equals(Size other) => Width.Equals(other.Width) && Height.Equals(other.Height);

  public override bool Equals(object? obj) => obj is Size other && Equals(other);

  public override int GetHashCode() => HashCode.Combine(Width, Height);

  public static bool operator ==(Size left, Size right) => left.Equals(right);

  public static bool operator !=(Size left, Size right) => !left.Equals(right);

  public override string ToString() => $"({Width} x {Height})";
}
