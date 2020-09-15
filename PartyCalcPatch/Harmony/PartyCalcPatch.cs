using DMT;
using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;

class Khaine_PartyCalcPatch : IHarmony
{
    public void Start()
    {
        Debug.Log(" Loading Patch: " + this.GetType().ToString());
        var harmony = new Harmony("Khaine.PartyCalcPatch.Patch");
        harmony.PatchAll();
    }
    [HarmonyPatch(typeof(GameStageDefinition))]
    [HarmonyPatch("CalcPartyLevel")]
    public class MethodCalcPartyLevel
    {
        static void Postfix(ref List<int> playerGameStages, ref int __result)
        {
            int partycount = playerGameStages.Count;
            double GS = __result;
            for (int i = 0; i < playerGameStages.Count; i++)
            {
                Log.Warning($"Gamestage #{i}: {playerGameStages[i]}");
            }
            playerGameStages.Sort();

            if (partycount > 1 && GS > playerGameStages[partycount - 1] * partycount * 0.66)
                GS = playerGameStages[partycount - 1] * partycount * 0.66;
            Log.Warning($"GS={GS}");
            __result = Convert.ToInt32(GS);
        }
    }
}