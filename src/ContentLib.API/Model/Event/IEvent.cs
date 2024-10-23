namespace ContentLib.API.Model.Event;

/// <summary>
/// Interface representing the general functionality of an In-Game Event. Used for the purposes of mods to be able to
/// subscribe to the Event and implement logic whenever said Event is triggered. 
/// </summary>
public interface IGameEvent
{
    
    /// <summary>
    /// Checks to see if the Event has been cancelled. 
    /// </summary>
    bool IsCancelled { get; set; }
}

