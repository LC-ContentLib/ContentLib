using System.Collections.Generic;

namespace ContentLib.API.Model.Mods;
/// <summary>
/// Interface representing the general functionality of a Lethal Company Mod. 
/// </summary>
public interface IMod
{
    /// <summary>
    /// Gets the unique identifier (GUID) for the mod.
    /// </summary>
    string ModGUID { get; }

    /// <summary>
    /// Gets the name of the mod.
    /// </summary>
    string ModName { get; }

    /// <summary>
    /// Gets the content definitions associated with the mod.
    /// </summary>
    List<ICustomContent> Content { get; }

    /// <summary>
    /// Registers a content definition for the mod.
    /// </summary>
    /// <param name="customContent">The content definition to register.</param>
    /// <returns>True if the content was successfully registered, otherwise false.</returns>
    bool RegisterContent(ICustomContent customContent);

    /// <summary>
    /// Initializes or merges the mod content, typically called on enable.
    /// </summary>
    void OnEnable();
}