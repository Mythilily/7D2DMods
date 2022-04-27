using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using UnityEngine;

class mythixscoutsmod : IModApi
{
    public void InitMod(Mod _modInstance)
    {
        Log.Out(" Loading Patch: " + GetType().ToString());
        var harmony = new Harmony("scoutsmodpatch");
        harmony.PatchAll();
    }

    [HarmonyPatch(typeof(AIDirectorChunkEventComponent))]
    [HarmonyPatch("checkHordeLevel")]
    public static class scoutsmodprob
    {
        private static float myrandomfloat(GameRandom __instance)
        {
            return 0f;
        }

        private static bool myplaytesting()
        {
            return false;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            Log.Out("@1");
            MethodInfo methodInfo = AccessTools.Method(typeof(GameUtils), "IsPlaytesting", null, null);
            MethodInfo methodInfo2 = AccessTools.Method(typeof(mythixscoutsmod.scoutsmodprob), "myplaytesting", null, null);
            MethodInfo methodInfo3 = AccessTools.PropertyGetter(typeof(GameRandom), "RandomFloat");
            MethodInfo methodInfo4 = AccessTools.Method(typeof(mythixscoutsmod.scoutsmodprob), "myrandomfloat", null, null);
            instr = Transpilers.MethodReplacer(instr, methodInfo, methodInfo2);
            instr = Transpilers.MethodReplacer(instr, methodInfo3, methodInfo4);
            return instr;
        }
    }

    [HarmonyPatch(typeof(AIScoutHordeSpawner))]
    [HarmonyPatch("spawnHordeNear")]
    public static class scoutsrandomnumber
    {
        public static int randomNumber()
        {
            int SHZombiesMin = int.Parse(SHConfig.GetPropertyValue("SHConfigLoad", "SHZombiesMin"));
            int SHZombiesMax = int.Parse(SHConfig.GetPropertyValue("SHConfigLoad", "SHZombiesMax"));
            int random = UnityEngine.Random.Range(SHZombiesMin, SHZombiesMax);
            Log.Out($"Min: {SHZombiesMin} Max: {SHZombiesMax} Random: {random}");
            return random;
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            Debug.Log("Scoutsmod patch 2");
            List<CodeInstruction> list = new List<CodeInstruction>(instr);
            for (int i = 0; i < list.Count; i++)
            {
                bool flag = list[i].opcode == OpCodes.Ldc_I4_5;
                if (flag)
                {
                    Debug.Log("Patching...");
                    list[i].opcode = OpCodes.Call;
                    list[i].operand = AccessTools.Method(typeof(mythixscoutsmod.scoutsrandomnumber), "randomNumber", null, null);
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable<CodeInstruction>();
        }
    }
}