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

public class Mythix_Combine
{
    private static Action myAction = () => { };
    public class CombineQuality : IHarmony
    {
        public void Start()
        {
            Debug.Log(" Loading Patch: " + GetType().ToString());
            var harmony = HarmonyInstance.Create(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
    [HarmonyPatch(typeof(XUiC_CombineGrid))]
    [HarmonyPatch("Merge_SlotChangedEvent")]
    public class Combinevars
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var list = new List<CodeInstruction>(instructions);
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 147 && list[i].opcode == OpCodes.Ldc_I4_1)
                {
                    Debug.Log("Patching Combine Quality...");
                    list[i].opcode = OpCodes.Ldc_I4_S;
                    list[i].operand = (short)40;
                }
                if (i >= 214 && i <= 277 && list[i].opcode == OpCodes.Ldc_I4_6)
                {
                    list[i].opcode = OpCodes.Ldc_I4_S;
                    list[i].operand = (short)40;
                    count++;
                    if (count >= 3)
                    {
                        Debug.Log("Patch done");
                        break;
                    }
                }
            }
            return list.AsEnumerable();
        }
    }
}