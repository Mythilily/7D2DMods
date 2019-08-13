using DMT;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

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
            var harmony = HarmonyInstance.Create(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
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
                    Debug.Log("Patching...");
                    list[i].opcode = OpCodes.Ldc_I4_S;
                    list[i].operand = (short)9;
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
    [HarmonyPatch(typeof(QualityInfo), MethodType.StaticConstructor)]
    public class ModQualityVars
    {

        static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patch 2");
            var list = new List<CodeInstruction>(instructions);

            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Ldc_I4_7 && count < 2)
                {
                    Debug.Log("Patching...");
                    list[i].opcode = OpCodes.Ldc_I4_S;
                    list[i].operand = (short)9;
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
                if (list[i].opcode == OpCodes.Ldc_R4 && (float)list[i].operand == 6f)
                {
                    Debug.Log("Patching...");
                    list[i].operand = 8f;
                    Debug.Log("Patch done");
                    break;
                }
            }
            return list.AsEnumerable();
        }
    }
    [HarmonyPatch(typeof(QualityInfo))]
    [HarmonyPatch("GetQualityLevelName")]
    [HarmonyPatch(new Type[]
        {
            typeof(int),
            typeof(bool)
        })]
    public class ModQualityLoc
    {
        static bool prefix(ref string __result, int _quality, bool _useQualityColor = false)
        {
            Debug.Log("Patch 4");
            if (_quality >= 6)
            {
                Debug.Log("Patching...");
                string text = string.Empty;
                switch (_quality)
                {
                    case 6:
                        text = Localization.Get("lblQualityLegendaryPlus", string.Empty);
                        break;
                    case 7:
                        text = Localization.Get("lblQualityRelic", string.Empty);
                        break;
                    case 8:
                        text = Localization.Get("lblQualityDemonic", string.Empty);
                        break;
                }
                if (_useQualityColor)
                {
                    text = string.Format("[{0}]{1}[-]", QualityInfo.GetQualityColorHex(_quality), text);
                    __result = text;
                    Debug.Log("Patch done");
                    return false;
                }
            }
            return true;
        }
    }
}