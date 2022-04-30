using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;


class Mythix_WanderingHordesGSpatch : IModApi
{
    public void InitMod(Mod _modInstance)
    {
        Log.Out(" Loading Patch: " + GetType().ToString());
        var harmony = new Harmony("wanderingGS");
        harmony.PatchAll();
    }
    [HarmonyPatch(typeof(AIWanderingHordeSpawner), MethodType.Constructor)]
    [HarmonyPatch(new Type[]
    {
            typeof(World),
            typeof(AIWanderingHordeSpawner.HordeArrivedDelegate),
            typeof(List<AIDirectorPlayerState>),
            typeof(int),
            typeof(ulong),
            typeof(Vector3),
            typeof(Vector3),
            typeof(Vector3),
            typeof(int),
            typeof(bool)
    })]
    class WHPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = new List<CodeInstruction>(instructions);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_I4_S && (sbyte)list[i].operand == 0x32)
                {
                    Debug.Log("Patching...");
                    list[i].opcode = OpCodes.Ldc_I4;
                    list[i].operand = 0x1388;
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
}
