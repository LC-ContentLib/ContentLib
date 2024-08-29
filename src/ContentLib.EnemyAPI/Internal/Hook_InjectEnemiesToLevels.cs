namespace ContentLib.EnemyAPI.Internal;

internal static partial class EnemyDefinitionInjector
{
    private static void Hook_InjectEnemiesToLevels(On.RoundManager.orig_Start orig, RoundManager self)
    {
        orig(self);

        foreach (EnemyDefinition enemyDefinition in s_enemiesToRegister)
        {
            foreach (SelectableLevel level in StartOfRound.Instance.levels)
            {
                AddEnemyToLevel(enemyDefinition, level);
            }
        }
    }

    private static void AddEnemyToLevel(EnemyDefinition enemyDefinition, SelectableLevel level)
    {
        // TODO: Get weight for level.

        SpawnableEnemyWithRarity enemyWithRarity = new()
        {
            enemyType = enemyDefinition.EnemyType,
            rarity = 0, // TODO: Set weight for level.
        };

        // TODO: Add enemy to the relevant enemy lists of the level.
    }
}
