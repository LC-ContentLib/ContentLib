namespace ContentLib.Core.Model.Entity.Util;

/// <summary>
/// Interface representing the general functionality of something that can be killed. Typically added to an IGameEntity.
/// </summary>
public interface IKillable
{
    /// <summary>
    /// Kills the subject.
    /// </summary>
    void Kill();
}