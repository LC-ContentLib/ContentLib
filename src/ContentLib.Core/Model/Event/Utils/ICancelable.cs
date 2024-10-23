namespace ContentLib.Core.Model.Event.Utils;

/// <summary>
/// Interface representing the general functionality of something that is cancelable. <br></br>
/// <i>Developer note: Caution should be taken when using this interface for an IGameEvent,as the patch logic is very
/// easy to get wrong and cause game breaking bugs. Any PR that includes a use of this interface needs to be reviewed
/// with a high level of scrutiny.</i> 
/// </summary>
public interface ICancelable
{
    /// <summary>
    /// A settable check stating if a cancellation is to occur. 
    /// </summary>
    bool IsCancelled { get; set; }
}