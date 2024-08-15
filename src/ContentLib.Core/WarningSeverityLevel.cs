namespace ContentLib.Core;

/// <summary>
/// An Enum for configuring how severe a warning is when registering a <see cref="ContentDefinition"/>.
/// </summary>
public enum WarningSeverityLevel
{
    /// <summary>
    /// Throw on warnings during registration.
    /// </summary>
    WarningsAsExceptions = 0,

    /// <summary>
    /// Log warnings during registration.
    /// </summary>
    WarningsAsWarnings = 1,

    /// <summary>
    /// Warnings will not get logged.
    /// </summary>
    IgnoreWarnings = 2,
}
