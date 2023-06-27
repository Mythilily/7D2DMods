using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;


public class WHferal : IModApi
{
    public void InitMod(Mod _mod)
    {
        Log.Warning(" Loading Patch: " + GetType().ToString());
        var harmony = new Harmony("EWanderinghorde");
        harmony.PatchAll();
        Log.Out($"Cfg aggressive: {Configs._aggressive}");
        Log.Out($"Cfg chaotic: {Configs._chaotic}");
    }
    [HarmonyPatch(typeof(AIWanderingHordeSpawner))]
    [HarmonyPatch("UpdateSpawn")]
    class whferal
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {

            var codes = new List<CodeInstruction>(instructions);
            if (!Configs._aggressive && !Configs._chaotic)
            {
                for (int i = 0; i < codes.Count; i++)
                {
                    if (!(codes[i].opcode == OpCodes.Ldloc_2)) continue;
                    if (++i > codes.Count) break;
                    if (!(codes[i].opcode == OpCodes.Ldc_I4_2)) continue;
                    if (++i > codes.Count) break;
                    if (!(codes[i].opcode == OpCodes.Stfld)) continue;
                    codes[i].opcode = OpCodes.Nop; codes[i].operand = null;
                    codes[i - 1].opcode = OpCodes.Nop; codes[i].operand = null;
                    codes[i - 2].opcode = OpCodes.Nop; codes[i].operand = null;
                }
            }
            return codes;
        }
        static bool Prefix(AIWanderingHordeSpawner __instance, ref Vector3 ___endPos, ref Vector3 ___pitStopPos, out Vector3 __state)
        {
            __state = ___endPos;
            if (!Configs._aggressive && !Configs._chaotic)
                ___endPos = ___pitStopPos;
            return true;
        }
        static void Postfix(ref Vector3 ___endPos, Vector3 __state)
        {
            if (!Configs._aggressive && !Configs._chaotic)
                ___endPos = __state;
        }
    }

    [HarmonyPatch(typeof(AIWanderingHordeSpawner))]
    [HarmonyPatch("UpdateHorde")]
    class whferal2
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            var list = new List<CodeInstruction>(instr);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_R4 && Convert.ToDouble(list[i].operand) == 90f)
                {
                    Log.Out("Pitstop wander to 60");
                    list[i].operand = 60f;
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(AIDirectorHordeComponent))]
    [HarmonyPatch("FindTargets")]
    class whferal3
    {
        static void Postfix(AIDirectorHordeComponent __instance, ref Vector3 startPos, ref Vector3 endPos, Vector3 pitStop)
        {
            Log.Out($"Pitstop: {pitStop}");
            Log.Out($"Endpos: {endPos}");
            List<AIDirectorPlayerState> list = __instance.Director.GetComponent<AIDirectorPlayerManagementComponent>().trackedPlayers.list;
            int num = __instance.Random.RandomRange(0, list.Count);
            AIDirectorPlayerState aidirectorPlayerState = list[num];
            int num2 = 1;
            while (num2 < list.Count && aidirectorPlayerState.Dead)
            {
                num = (num + num2) % list.Count;
                aidirectorPlayerState = list[num];
                num2++;
            }
            if (!aidirectorPlayerState.Dead)
            {
                endPos = aidirectorPlayerState.Player.position;
                if (Configs._chaotic)
                {
                    startPos = aidirectorPlayerState.Player.position;
                }
            }
        }
    }
}
