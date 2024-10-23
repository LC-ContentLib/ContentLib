namespace ContentLib.Core.Model.Event.Listener;

/// <summary>
/// Interface that is used to identify classes that can be registered via the GameEventManager. The intended use of
/// this interface is to implement in a class that will have multiple methods that take in IGameEvent parameters, to
/// then be utilised in the registration process, as mentioned above. 
/// </summary>
public interface IListener
{
    
}