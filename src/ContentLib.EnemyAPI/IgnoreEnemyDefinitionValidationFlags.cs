using System;

namespace ContentLib.EnemyAPI;

/// <summary>
/// Flags to ignore validation checks for <see cref="EnemyDefinition"/>.
/// </summary>
[Flags]
public enum IgnoreEnemyDefinitionValidationFlags
{
    /// <summary>
    /// Ignore check for a scan node.
    /// </summary>
    NoScanNode = 1 << 0,

    /// <summary>
    /// Ignore check for <see cref="EnemyAICollisionDetect"/> script.
    /// </summary>
    NoCollisionDetect = 1 << 1,

    /// <summary>
    /// Ignore check for no rigid body on <see cref="EnemyAICollisionDetect"/> script.<br/>
    /// A rigid body enables the enemy to collide with certain things, like opening doors.
    /// </summary>
    NoRigidBodyOnCollisionDetect = 1 << 2,
}
