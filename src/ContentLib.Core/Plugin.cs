using BepInEx;
using BepInEx.Logging;

namespace ContentLib;

/// <summary>
/// BepInEx plugin of ContentLib.Core.
/// Depend on this with <c>[BepInDependency(CorePlugin.Id)]</c>.
/// </summary>
[BepInAutoPlugin]
public partial class CorePlugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; } = BepInEx.Logging.Logger.CreateLogSource(Name);

    private void Awake()
    {
        Log.LogInfo($"Plugin {Name} is loaded!");
    }
}
