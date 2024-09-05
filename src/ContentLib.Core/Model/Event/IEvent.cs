namespace ContentLib.Core.Model.Event;

/// <summary>
/// Interface representing the general functionality of an In-Game Event. Used for the purposes of mods to be able to
/// subscribe to the Event and implement logic whenever said Event is triggered. 
/// </summary>
public interface IGameEvent
{
    /// <summary>
    /// The type of Event.
    /// </summary>
    GameEventType EventType { get; }
    
    /// <summary>
    /// Checks to see if the Event has been cancelled. 
    /// </summary>
    bool IsCancelled { get; set; }
}

public enum GameEventType
{
    MonsterPlayerCollisionEvent,
    MonsterKillsPlayerEvent,
    ShipLanded, 
    ShipTakeoff, 
    ItemPickup,
    PlayerDamaged, 
    PlayerDeath,
    PlayerTeleported,
    TVSwitch,
    
}