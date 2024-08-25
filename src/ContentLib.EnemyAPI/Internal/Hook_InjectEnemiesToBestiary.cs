using System.Linq;

namespace ContentLib.EnemyAPI.Internal;

internal static partial class EnemyDefinitionInjector
{
    private static void Hook_InjectEnemiesToBestiary(On.Terminal.orig_Start orig, Terminal self)
    {
        TerminalKeyword infoKeyword = self.terminalNodes.allKeywords.FirstOrDefault(keyword => keyword.word == "info");

        if (infoKeyword is null)
        {
            Plugin.s_log.LogError($"{nameof(TerminalKeyword)} 'info' wasn't found, enemies cannot be added to bestiary!");
            orig(self);
            return;
        }

        foreach (EnemyDefinition enemyDefinition in s_enemiesToRegister)
            AddEnemyToBestiary(enemyDefinition, self, infoKeyword);

        orig(self);
    }

    private static void AddEnemyToBestiary(EnemyDefinition enemyDefinition, Terminal terminal, TerminalKeyword infoKeyword)
    {
        // TODO: EnemyDefinition needs some TerminalNode stuff.
    }
}
