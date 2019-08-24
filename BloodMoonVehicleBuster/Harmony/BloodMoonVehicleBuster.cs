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
        bool isBloodMoon = SkyManager.BloodMoon();
        if (__instance is VPEngine && isBloodMoon)
        {
            try
            {
                foreach (EntityPlayerLocal entityplayer in GameManager.Instance.World.GetLocalPlayers())
                {
                    if (entityplayer.AttachedToEntity && ((entityplayer.AttachedToEntity.GetType()) != typeof(EntityBicycle)))
                    {
                        GameManager.ShowTooltip(entityplayer, Localization.Get("BMEngineWarning"));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Caught exception: " + ex);
            }
            return true;
        }
        return __result;
    }
}