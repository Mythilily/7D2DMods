//Harmony Patch: Disables vehicles with an engine during blood moon.
//Author: Mythix(dino)
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using DMT;

[HarmonyPatch(typeof(WorldGenerationEngine.GenerationManager))]
[HarmonyPatch("GenerateTowns")]

public class TownRWGfix : IHarmony
{
    public void Start()
    {
        Debug.Log(" Loading Patch: " + GetType().ToString());
        var harmony = HarmonyInstance.Create(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        Debug.Log("Initializing town size patch");
        var list = new List<CodeInstruction>(instructions);

        //int oldValuemin = 500;
        //int newValuemin = 250;
        int count = 0;
        for (int i = 0; i < list.Count; i++)
        {
            //if (list[i].opcode == OpCodes.Ldc_I4 && (list[i].operand).ToString() == oldValuemin.ToString())
            //{
            //    Debug.Log("Patching min value");
            //    list[i].operand = newValuemin;
            //    Debug.Log("Patch done");
            //}
            if (list[i].opcode == OpCodes.Ldc_I4_8 && count < 2)
            {
                Debug.Log("Patching towns");
                list[i].opcode = OpCodes.Ldc_I4_S;
                list[i].operand = 11;
                count++;
                Debug.Log("Counter " + count);
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
