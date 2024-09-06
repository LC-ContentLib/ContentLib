using ContentLib.EnemyAPI.Model.Enemy;

namespace ContentLib.Core.Model.Event;

public abstract class BrackenSeenByPlayerEvent : IMonsterEvents
{
    /// <summary>
    /// Boolean check to see if the event is cancelled. 
    /// </summary>
    protected bool _isCancelled;
    
    /// <inheritdoc />
    public bool IsCancelled { get => _isCancelled; set => _isCancelled = value; }

    /// <inheritdoc />
    public abstract IEnemy Enemy { get; }
}