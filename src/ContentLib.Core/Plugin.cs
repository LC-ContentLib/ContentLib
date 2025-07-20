#if !UNITY_EDITOR_ASSEMBLY
using System;
using BepInEx;
using BepInEx.Logging;
using MonoDetour;

namespace ContentLib.Core;

/// <summary>
/// BepInEx plugin of ContentLib.Core.
/// Depend on this with <c>[BepInDependency(CorePlugin.Id)]</c>.
/// </summary>
[BepInAutoPlugin]
public partial class CorePlugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; } = BepInEx.Logging.Logger.CreateLogSource(Name);
    internal static CorePlugin Instance =>
        _instance
        ?? throw new NullReferenceException(
            "ContentLib.Core hasn't been initialized yet! "
                + "Please depend on it with [BepInDependency(CorePlugin.Id)]"
        );

    private static CorePlugin? _instance = null;

    private void Awake()
    {
        _instance = this;
        MonoDetourManager.InvokeHookInitializers(typeof(CorePlugin).Assembly);
        BundleLoader.LoadAllBundles(Paths.PluginPath, ".autoload.contentbundle");

        Log.LogInfo($"Plugin {Name} is loaded!");
    }

    private void Start()
    {
        BundleLoader.CloseBundleLoadingWindow();
    }
}
#endif
