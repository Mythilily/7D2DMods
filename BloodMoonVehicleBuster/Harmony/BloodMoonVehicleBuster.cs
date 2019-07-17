//Harmony Patch: Disables vehicles with an engine during blood moon.
//Author: Mythix(dino)
using Harmony;
using System;
using System.Reflection;
using UnityEngine;
using DMT;

[HarmonyPatch(typeof(VehiclePart))]
[HarmonyPatch("IsBroken")]

public class NovehicleBM : IHarmony
{
    public void Start()
    {
        Debug.Log(" Loading Patch: " + GetType().ToString());
        var harmony = HarmonyInstance.Create(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
    static bool Postfix(bool __result, VehiclePart __instance)
    {
        if (__instance is VPEngine)
        {
            bool isBloodMoon = SkyManager.BloodMoon();
            if (isBloodMoon)
                return true;
        }
        return __result;
    }
}
