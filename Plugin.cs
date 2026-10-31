using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace RipIt.SellXP
{
    [BepInPlugin("com.juanberta.riplit.sellxp", "Rip It! Sell XP", "1.0.0")]
    public class Plugin : BasePlugin
    {
        public const string HarmonyId = "com.juanberta.riplit.sellxp";

        internal static ManualLogSource PluginLog;

        public override void Load()
        {
            PluginLog = Log;
            Harmony.CreateAndPatchAll(typeof(Plugin).Assembly, HarmonyId);
            PluginLog.LogInfo("Rip It! Sell XP v1.0.0 loaded.");
        }
    }

    /// <summary>
    /// Gives experience points whenever a card is successfully sold, using the game's own
    /// rarity-based XP calculation (the same one used when placing a new card in its slot).
    ///
    /// TrySellCard_Internal is the single choke point every sell path goes through
    /// (manual sell slot drop, auto-sell duplicates, Shift+Click sell from ShiftBind),
    /// so one patch covers all of them without double-counting.
    /// </summary>
    [HarmonyPatch(typeof(CurrencyManager), "TrySellCard_Internal")]
    internal static class TrySellCardPatch
    {
        private static void Postfix(CurrencyManager __instance, Cards.CardInfo card, bool __result)
        {
            if (!__result)
            {
                return;
            }

            if (card == null)
            {
                return;
            }

            try
            {
                ExperienceManager.AddExperience(card);
                Plugin.PluginLog.LogInfo(
                    $"Sell XP granted: page={card.BinderPage} rarity={card.Rarity} edition={card.Edition}");
            }
            catch (Exception ex)
            {
                Plugin.PluginLog.LogError($"Failed to grant sell XP: {ex}");
            }
        }
    }
}
