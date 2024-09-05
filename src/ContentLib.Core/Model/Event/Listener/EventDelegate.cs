using System;

namespace ContentLib.Core.Model.Event.Attributes;

/// <summary>
/// Attribute that will be used in conjunction with the IListener to allow for automatic subscription of Listener class
/// event delegates. The idea is that this would be put above each method that is intended to be subscribed to in the
/// GameEventManager, so when the listener is passed through the GameEventManager subscription method, it will subscribe
/// all methods that have this attribute. 
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class EventDelegate : Attribute
{
    
}