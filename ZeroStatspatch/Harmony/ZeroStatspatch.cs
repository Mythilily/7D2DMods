using DMT;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;


class Mythix_ZeroStatspatch
{
    public class ZeroStats : IHarmony
    {
        public void Start()
        {
            Debug.Log(" Loading Patch: " + GetType().ToString());
            var harmony = HarmonyInstance.Create(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        [HarmonyPatch(typeof(Stat))]
        [HarmonyPatch("Tick")]
        class StatTickpatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var list = new List<CodeInstruction>(instructions);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].opcode == OpCodes.Ldc_R4 && (float)list[i].operand == 0.25)
                    {
                        Debug.Log("Patching...");
                        list[i].operand = 0f;
                        Debug.Log("Patch done");
                        break;
                    }
                }
                return list.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(Stat), MethodType.Getter)]
        [HarmonyPatch("Value")]
        class StatGetValue
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var list = new List<CodeInstruction>(instructions);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].opcode == OpCodes.Ldc_R4 && (float)list[i].operand == 0.25)
                    {
                        Debug.Log("Patching...");
                        list[i].operand = 0f;
                        Debug.Log("Patch done");
                        break;
                    }
                }
                return list.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(Stat), MethodType.Setter)]
        [HarmonyPatch("Value")]
        class StatSetValue
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var list = new List<CodeInstruction>(instructions);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].opcode == OpCodes.Ldc_R4 && (float)list[i].operand == 0.25)
                    {
                        Debug.Log("Patching...");
                        list[i].operand = 0f;
                        Debug.Log("Patch done");
                        break;
                    }
                }
                return list.AsEnumerable();
            }
        }
        [HarmonyPatch(typeof(Stat), MethodType.Setter)]
        [HarmonyPatch("MaxModifier")]
        class StatSetMaxModifier
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var list = new List<CodeInstruction>(instructions);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].opcode == OpCodes.Ldc_R4 && (float)list[i].operand == 0.75)
                    {
                        Debug.Log("Patching...");
                        list[i].operand = 1f;
                        Debug.Log("Patch done");
                        break;
                    }
                }
                return list.AsEnumerable();
            }
        }
    }
}
