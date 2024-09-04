using ContentLib.EnemyAPI.Model.Enemy;

namespace ContentLib.Core.Model.Event;
/// <summary>
/// Interface that represents the general functionality of an event that was triggered by an action relating to an
/// in-game monster. 
/// </summary>
public interface IMonsterEvents : IGameEvent
{
    /// <summary>
    /// Gets the AI of the monster triggering the event. 
    /// </summary>
    /// <returns>The event triggering monster's AI</returns>
    IEnemy Enemy { get; }
}

public abstract class MonsterCollideWithPlayerEvent : IMonsterEvents
{
    /// <inheritdoc />
    public abstract IEnemy Enemy { get; }

    /// <inheritdoc />
    public GameEventType EventType => GameEventType.MonsterPlayerCollisionEvent;

    /// <inheritdoc />
    public bool IsCancelled { get; set; }

}

public abstract class MonsterKillsPlayerEvent : IMonsterEvents
{
    /// <inheritdoc />
    public GameEventType EventType => GameEventType.MonsterKillsPlayerEvent;

    /// <inheritdoc />
    public bool IsCancelled { get; set; } = false;

    /// <inheritdoc />
    public abstract IEnemy Enemy { get; }
}

public enum MonsterType
{
    Bracken,Coilhead
}