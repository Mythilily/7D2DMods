//Harmony Patch: Reduces Range on RandomPOIGoto.
//Author: Mythix(dino)
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using DMT;

[HarmonyPatch(typeof(ObjectiveRandomPOIGoto))]
[HarmonyPatch("GetPosition")]

public class QuestSmallerRange : IHarmony
{
    public void Start()
    {
        Debug.Log(" Loading Patch: " + GetType().ToString());
        var harmony = HarmonyInstance.Create(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var list = new List<CodeInstruction>(instructions);

        int count = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].opcode == OpCodes.Ldc_I4 && (int)(list[i].operand) == 1000 && count < 2)
            {
                Debug.Log("Patching RndPOIGoto");
                list[i].operand = 500;
                count++;
                if (count == 2)
                {
                    Debug.Log("Patch done");
                    break;
                }
            }
        }
        return list.AsEnumerable();
    }
}