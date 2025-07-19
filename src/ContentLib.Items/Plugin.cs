#if !UNITY_EDITOR
using System;
using BepInEx;
using BepInEx.Logging;
using MonoDetour;

namespace ContentLib.Items;

/// <summary>
/// BepInEx plugin of ContentLib.Items.
/// Depend on this with <c>[BepInDependency(ItemsPlugin.Id)]</c>.
/// </summary>
[BepInAutoPlugin]
public partial class ItemsPlugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; } = BepInEx.Logging.Logger.CreateLogSource(Name);
    internal static ItemsPlugin Instance =>
        _instance
        ?? throw new NullReferenceException(
            "ContentLib.Core hasn't been initialized yet! "
                + "Please depend on it with [BepInDependency(ItemsPlugin.Id)]"
        );

    private static ItemsPlugin? _instance = null;

    private void Awake()
    {
        MonoDetourManager.InvokeHookInitializers(typeof(ItemsPlugin).Assembly);

        Log.LogInfo($"Plugin {Name} is loaded!");
    }
}
#endif
