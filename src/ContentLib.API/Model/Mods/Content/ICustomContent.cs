namespace ContentLib.API.Model.Mods;

/// <summary>
/// Interface representing the generalisation of content that can be utilised by an IMod. This can include things such
/// as enemies, items, levels etc. 
/// </summary>
public interface ICustomContent
{
    /// <summary>
    /// The owner of this content (the ModDefinition).
    /// </summary>
    IMod? Mod { get; }
    
    /// <summary>
    /// The properties of the content
    /// </summary>
    ICustomContentProperties Properties { get; set; }
    
    /// <summary>
    /// Check to see if the specified Custom Content Properties are valid to be added to this Content. 
    /// </summary>
    /// <param name="properties">The properties to check.</param>
    /// <returns>True if the properties are valid for this Content, False Otherwise.</returns>
    bool ArePropertiesValid(ICustomContentProperties properties);
    
}