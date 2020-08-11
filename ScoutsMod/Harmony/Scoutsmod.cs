using DMT;
using HarmonyLib;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

class mythixscoutsmod
{
    static int[] values = new int[2] { 3, 7 };
    public class scoutsmod : IHarmony
    {
        const Int32 buffersize = 256;
        public void Start()
        {
            Log.Out("Pre-initiliazing configuration values");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Mods\\ScoutsMod\\Config\\Scouthorde.txt");
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, buffersize))
            {
                string line;
                int result;
                int cnt = 0;
                while ((line = streamReader.ReadLine()) != null || cnt < 2)
                {
                    if (int.TryParse(line, out result))
                    {
                        values[cnt] = result;
                        cnt++;
                    }

                }
            }
            Log.Warning(" Loading Patch: " + this.GetType().ToString());
			var harmony = new Harmony("scoutsmodpatch");
			harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(AIDirectorChunkEventComponent))]
    [HarmonyPatch("checkHordeLevel")]

    public static class scoutsmodprob
    {
        static float myrandomfloat(GameRandom __instance)
        {
            return 0;
        }
        static bool myplaytesting()
        {
            return false;
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            Log.Warning("@1");
            var method = AccessTools.Method(typeof(GameUtils), "IsPlaytesting");
            var fmethod = AccessTools.Method(typeof(scoutsmodprob), "myplaytesting");
            var method2 = AccessTools.PropertyGetter(typeof(GameRandom), "RandomFloat");
            var fmethod2 = AccessTools.Method(typeof(scoutsmodprob), "myrandomfloat");
            instr = Transpilers.MethodReplacer(instr, method, fmethod);
            instr = Transpilers.MethodReplacer(instr, method2, fmethod2);
            return instr;
        }
    }
    [HarmonyPatch(typeof(AIScoutHordeSpawner))]
    [HarmonyPatch("spawnHordeNear")]
    public static class scoutsrandomnumber
    {

        public static int randomNumber()
        {
            return UnityEngine.Random.Range(values[0], values[1]);
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            Debug.Log("Scoutsmod patch 2");
            var list = new List<CodeInstruction>(instr);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_I4_5)
                {
                    Debug.Log("Patching...");
                    list[i].opcode = OpCodes.Call;
                    list[i].operand = AccessTools.Method(typeof(scoutsrandomnumber), "randomNumber");
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }

}