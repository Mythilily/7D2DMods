using DMT;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;


public class Mythix_Disabledawncleanup
{
    public class Disabledawncleanup : IHarmony
    {
        public void Start()
        {
            Debug.Log(" Loading Patch: " + GetType().ToString());
            var harmony = HarmonyInstance.Create(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        [HarmonyPatch(typeof(EntityZombie))]
        [HarmonyPatch("OnAddedToWorld")]
        class disablecleanup
        {
            static void Postfix(EntityZombie __instance)
            {
                __instance.timeToDie = __instance.world.worldTime + 1800UL + (ulong)(22000f * __instance.rand.RandomFloat);
            }
            //static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            //{
            //    short removed = 0;
            //    var list = new List<CodeInstruction>(instructions);
            //    for (int i = 0; i < list.Count; i++)
            //    {
            //        if (i >= 17 && i <= 60 && list[17].opcode == OpCodes.Ldarg_0)
            //        {
            //            list.RemoveAt(i);
            //            removed++;
            //        }
            //    }
            //    Debug.Log("Removed "+removed);
            //    return list.AsEnumerable();
            //}
        }
    }
}
