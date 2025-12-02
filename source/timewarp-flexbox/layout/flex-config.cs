namespace TimeWarp.Flexbox;

/// <summary>
/// Global configuration settings that affect layout behavior.
/// </summary>
public class FlexConfig
{
  /// <summary>
  /// Gets or sets the point scale factor for pixel grid rounding.
  /// Used for high DPI displays. Default is 1.0f.
  /// </summary>
  public float PointScaleFactor { get; set; } = 1.0f;

  /// <summary>
  /// Gets or sets whether to use W3C web defaults instead of Yoga defaults.
  /// When true, uses W3C spec defaults (e.g., flex-shrink: 1).
  /// When false, uses Yoga defaults (e.g., flex-shrink: 0).
  /// Default is false for Yoga compatibility.
  /// </summary>
  public bool UseWebDefaults { get; set; }

  /// <summary>
  /// Gets or sets whether errata handling is enabled for strict W3C spec compliance.
  /// </summary>
  public bool UseErrata { get; set; }

  /// <summary>
  /// Gets the default configuration instance.
  /// </summary>
  public static FlexConfig Default { get; } = new();

  /// <summary>
  /// Creates a copy of this configuration.
  /// </summary>
  /// <returns>A new FlexConfig with the same settings.</returns>
  public FlexConfig Clone()
  {
    return new FlexConfig
    {
      PointScaleFactor = PointScaleFactor,
      UseWebDefaults = UseWebDefaults,
      UseErrata = UseErrata
    };
  }
}
