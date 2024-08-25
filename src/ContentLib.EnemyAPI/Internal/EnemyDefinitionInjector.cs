using System.Collections.Generic;

namespace ContentLib.EnemyAPI.Internal;

internal static partial class EnemyDefinitionInjector
{
    internal static bool s_lateForRegister = false;
    private static List<EnemyDefinition> s_registeredEnemies = null!;

    internal static void Register(EnemyDefinition enemyDefinition)
    {
        if (s_registeredEnemies is null)
        {
            s_registeredEnemies = [];
            On.QuickMenuManager.Start += Hook_InjectToQuickMenuManager;
        }

        s_registeredEnemies.Add(enemyDefinition);
    }
}
