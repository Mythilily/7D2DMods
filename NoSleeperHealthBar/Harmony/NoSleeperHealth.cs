using DMT;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

[HarmonyPatch(typeof(XUiC_TargetBar))]
[HarmonyPatch("Update")]
public class NoSleeperHealth : IHarmony
{
    public void Start()
    {
        Debug.Log(" Loading Patch: " + GetType().ToString());
        var harmony = HarmonyInstance.Create(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
    static bool Prefix(float _dt, XUiC_TargetBar __instance)
    {
        foreach (Entity entity in GameManager.Instance.World.Entities.list)
        {
            EntityAlive entityAlive = null;
            WorldRayHitInfo hitInfo = __instance.xui.playerUI.entityPlayer.HitInfo;
            if (hitInfo.bHitValid && hitInfo.transform && hitInfo.tag.StartsWith("E_"))
            {
                Transform hitRootTransform;
                if ((hitRootTransform = GameUtils.GetHitRootTransform(hitInfo.tag, hitInfo.transform)) != null)
                {
                    entityAlive = hitRootTransform.GetComponent<EntityAlive>();
                    if (entityAlive.IsSleeping)
                    {
                        //__instance.ViewComponent.IsVisible = false;
                        return false;
                    }
                    else
                    {
                        //__instance.ViewComponent.IsVisible = true;
                        return true;
                    }
                }
            }
        }
        return true;
    }
}



