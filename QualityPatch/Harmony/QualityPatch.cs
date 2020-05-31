using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using DMT;
using HarmonyLib;

//[HarmonyPatch(typeof(ConsoleCmdGetTime))]
//[HarmonyPatch("Execute")]
//public class QualityPatch : IHarmony
//{
//    public void Start()
//    {
//        Debug.Log(" Loading Patch: " + GetType().ToString());
//        var harmony = HarmonyInstance.Create(GetType().ToString());
//        harmony.PatchAll(Assembly.GetExecutingAssembly());
//    }
//    static bool Prefix()
//    {
//        ulong worldtime = GameManager.Instance.World.GetWorldTime();
//        float dawntime = SkyManager.GetDawnTime();
//        float dusktime = SkyManager.GetDuskTime();
//        Debug.Log("#####LOGdummy#####");
//        Debug.Log("GetWorldTime " + worldtime);
//        Debug.Log("Hours " + GameUtils.WorldTimeToHours(worldtime));
//        Debug.Log("Days " + GameUtils.WorldTimeToDays(worldtime));
//        Debug.Log("Minutes " + GameUtils.WorldTimeToMinutes(worldtime));
//        Debug.Log("DawnTime " + dawntime);
//        Debug.Log("DuskTime " + dusktime);
//        Debug.Log("#####END#####");
//        return true;
//    }
//}

public class Mythix_QualityPatch
{
    public class QualityPatch : IHarmony
    {
        public void Start()
        {
            Debug.Log(" Loading Patch: " + GetType().ToString());
            Debug.LogWarning("Qualitypatch intended for Darkness Falls mod.\nAll 9 patches have to pass (except patch 4) otherwise it likely won't work properly");
            var harmony = new Harmony("qualitypatch");
            harmony.PatchAll();
        }
    }
    [HarmonyPatch(typeof(QualityInfo))]
    [HarmonyPatch("Cleanup")]
    public class ModQuality
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patch 1");
            var list = new List<CodeInstruction>(instructions);

            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_I4_7 && count < 2)
                {
                    if (count == 0)
                    {
                        Debug.Log("Patching...");
                    }
                    list[i].opcode = OpCodes.Ldc_I4_S;
                    list[i].operand = 9;
                    count++;
                }
                else if (count >= 2)
                {
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
    [HarmonyPatch(typeof(QualityInfo))]
    [HarmonyPatch("GetTierColor")]
    [HarmonyPatch(new Type[]
    {
            typeof(int)
    })]
    public class Tiercolor
    {
        static bool Prefix(ref int _tier)
        {
            if (_tier > 0)
            {
                _tier = (int)Math.Ceiling(_tier / 10m);
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(QualityInfo))]
    [HarmonyPatch("GetQualityColorHex")]
    [HarmonyPatch(new Type[]
    {
            typeof(int)
    })]
    public class Qualitycolorhex
    {
        static bool Prefix(ref int _quality)
        {
            if (_quality > 0)
            {
                _quality = (int)Math.Ceiling(_quality / 10m);
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(QualityInfo), MethodType.StaticConstructor)]
    public class ModQualityVars
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patch 2");
            var list = new List<CodeInstruction>(instructions);

            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_I4_7 && count < 2)
                {
                    if (count == 0)
                    {
                        Debug.Log("Patching...");
                    }
                    list[i].opcode = OpCodes.Ldc_I4_S;
                    list[i].operand = 9;
                    count++;
                }
                else if (count >= 2)
                {
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
    [HarmonyPatch(typeof(XUiC_RecipeStack))]
    [HarmonyPatch("SetRecipe")]
    [HarmonyPatch(new Type[]
    {
            typeof(Recipe),
            typeof(int),
            typeof(float),
            typeof(bool),
            typeof(int),
            typeof(int),
            typeof(float)
    })]

    public class OutputQuality
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patch 3");
            var list = new List<CodeInstruction>(instructions);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_R4 && (float)list[i].operand == 6)
                {
                    Debug.Log("Patching...");
                    list[i].operand = 80f;
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
    [HarmonyPatch(typeof(QualityInfo))]
    [HarmonyPatch("GetQualityLevelName")]
    public class Qualityname
    {
        static bool Prefix(ref string __result, ref int _quality, ref bool _useQualityColor)
        {
            Debug.Log("Patch 4");
            if (_quality != 0)
            {
                Debug.Log("Patching...");
                string text = string.Empty;
                _quality /= 10;
                switch (_quality)
                {
                    case 0:
                        text = Localization.Get("lblQualityDamaged");
                        break;
                    case 1:
                        text = Localization.Get("lblQualityPoor");
                        break;
                    case 2:
                        text = Localization.Get("lblQualityAverage");
                        break;
                    case 3:
                        text = Localization.Get("lblQualityGreat");
                        break;
                    case 4:
                        text = Localization.Get("lblQualityFlawless");
                        break;
                    case 5:
                        text = Localization.Get("lblQualityLegendary");
                        break;
                    case 6:
                        text = Localization.Get("lblQualityLegendaryPlus");
                        break;
                    case 7:
                        text = Localization.Get("lblQualityRelic");
                        break;
                    case 8:
                        text = Localization.Get("lblQualityDemonic");
                        break;
                }
                if (_useQualityColor)
                {
                    text = string.Format("[{0}]{1}[-]", QualityInfo.GetQualityColorHex(_quality), text);
                    __result = text;
                    Debug.Log("Patch done");
                }
            }
            else
            {
                __result = Localization.Get("lblQualityBroken");
            }
            return false;
        }
    }
    [HarmonyPatch(typeof(EntityNPC))]
    [HarmonyPatch("SetupStartingItems")]
    public class Startingitemsquality
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patch 5");
            var list = new List<CodeInstruction>(instructions);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_I4_6)
                {
                    Debug.Log("Patching...");
                    list[i].opcode = OpCodes.Ldc_I4_2;
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
    [HarmonyPatch(typeof(TraderInfo))]
    [HarmonyPatch("SpawnItem")]
    public class TraderSpawnItem
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patch 6");
            var list = new List<CodeInstruction>(instructions);

            for (int i = 0; i < list.Count; i++)
            {
                if (i == 133 && list[i].opcode == OpCodes.Ldc_I4_6)
                {
                    Debug.Log("Patching...");
                    list[i].opcode = OpCodes.Ldc_I4_S;
                    list[i].operand = 60;
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
    [HarmonyPatch(typeof(XUiM_Trader))]
    [HarmonyPatch("GetBuyPrice")]
    public class Traderbuyprice
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patch 7");
            var list = new List<CodeInstruction>(instructions);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_R4 && (float)list[i].operand == 5)
                {
                    Debug.Log("Patching...");
                    list[i].operand = 60f;
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
    [HarmonyPatch(typeof(XUiM_Trader))]
    [HarmonyPatch("GetSellPrice")]
    public class Tradersellprice
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patch 8");
            var list = new List<CodeInstruction>(instructions);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_R4 && (float)list[i].operand == 5)
                {
                    Debug.Log("Patching...");
                    list[i].operand = 60f;
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
    [HarmonyPatch(typeof(ItemValue), MethodType.Constructor)]
    [HarmonyPatch("ItemValue")]
    [HarmonyPatch(new Type[]
    {
            typeof(int),
            typeof(bool)
    })]
    public class Itemvaluequality
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patch 9");
            var list = new List<CodeInstruction>(instructions);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_I4_6)
                {
                    Debug.Log("Patching...");
                    list[i].opcode = OpCodes.Ldc_I4_S;
                    list[i].operand = 80;
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
}