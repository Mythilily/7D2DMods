using HarmonyLib;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using WorldGenerationEngineFinal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Biomeadjustment
{
    public static IList<Values> values;
    public class biomeadj : IModApi
    {
        public void InitMod(Mod _mod)
        {
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"Mods{System.IO.Path.DirectorySeparatorChar}Biomeadjustment{System.IO.Path.DirectorySeparatorChar}Config.json");
            var jsonText = File.ReadAllText(path);

            values = JsonConvert.DeserializeObject<IList<Values>>(jsonText);
            Log.Warning($"Forest: {values[0].forestWeight.ToString()}");
            Log.Warning($"Desert: {values[0].desertWeight.ToString()}");
            Log.Warning($"Snow: {values[0].snowWeight.ToString()}");
            Log.Warning($"Wasteland: {values[0].wastelandWeight.ToString()}");

            Log.Warning(" Loading Patch: " + this.GetType().ToString());
			var harmony = new Harmony("Biomeadj");
			harmony.PatchAll();
        }
    }
    public class Values
    {
        public int forestWeight { get; set; }
        public int desertWeight { get; set; }
        public int snowWeight { get; set; }
        public int wastelandWeight { get; set; }

    }

    [HarmonyPatch(typeof(BiomeRuleGrowBiomes))]
    [HarmonyPatch("Init")]
    public class biomeadj2
    {
        //static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        //{
        //    var list = new List<CodeInstruction>(instructions);
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        //Log.Out($"i={i} Opcode={list[i].opcode}");
        //        if (i == 76 && list[i].opcode == OpCodes.Ldc_I4_7)
        //        {
        //            list[i].opcode = OpCodes.Ldc_I4_2;
        //            Log.Out("Decreased forest weight to 2");
        //        }
        //    }
        //    return list.AsEnumerable();
        //}
        public static bool Prefix()
        {
            WorldBuilder.Instance.ForestBiomeWeight = values[0].forestWeight;
            WorldBuilder.Instance.DesertBiomeWeight = values[0].desertWeight;
            WorldBuilder.Instance.SnowBiomeWeight = values[0].snowWeight;
            WorldBuilder.Instance.WastelandBiomeWeight = values[0].wastelandWeight;
            return true;
        }
    }
    [HarmonyPatch(typeof(WorldBuilder))]
    [HarmonyPatch("generateBiomeTiles")]
    public class biomeadj3
    {
        public static bool Prefix(WorldBuilder __instance)
        {
            __instance.ForestBiomeWeight = values[0].forestWeight;
            __instance.DesertBiomeWeight = values[0].desertWeight;
            __instance.SnowBiomeWeight = values[0].snowWeight;
            __instance.WastelandBiomeWeight = values[0].wastelandWeight;
            Log.Out("Adjusted biome weights");
            return true;
        }
    }

}