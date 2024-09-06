using System;
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
    public bool IsCancelled { get; set; }

}

/// <summary>
/// Event triggered when a monster kills a player.
/// </summary>
public abstract class MonsterKillsPlayerEvent : IMonsterEvents
{
    
    /// <inheritdoc />
    public bool IsCancelled { get; set; } = false;

    /// <inheritdoc />
    public abstract IEnemy Enemy { get; }
}

/// <summary>
/// Event triggered when a monster is spawned. (SHOULD NOT BE CANCELLABLE FOR THE TIME BEING) 
/// </summary>
public abstract class MonsterSpawnEvent : IMonsterEvents
{
    /// <summary>
    /// Bool showing if the event is cancelled or not. 
    /// </summary>
    protected bool _isCancelled = false;
    public GameEventType EventType { get; }

    //TODO might need to make a "cancelable" interface to add as a caveat additional inteface to events, as not all 
    //TODO events should be cancellable. 
    /// <inheritdoc />
    public bool IsCancelled { get => _isCancelled; set => _isCancelled = value; }

    /// <inheritdoc />
    public abstract IEnemy Enemy { get; }
}

