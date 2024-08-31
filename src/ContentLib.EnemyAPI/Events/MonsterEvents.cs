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
    public abstract IEnemy Enemy { get; }
    public GameEventType EventType => GameEventType.MonsterPlayerCollisionEvent;

    public bool IsCancelled { get; set; }

}

public enum MonsterType
{
    Bracken,Coilhead
}