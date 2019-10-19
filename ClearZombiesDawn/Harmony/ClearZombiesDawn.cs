using DMT;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

[HarmonyPatch(typeof(World))]
[HarmonyPatch("IsDaytime")]
public class ClearZombiesDawn : IHarmony
{
    public void Start()
    {
        Debug.Log(" Loading Patch: " + GetType().ToString());
        var harmony = HarmonyInstance.Create(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
    static bool Prefix()
    {
        ulong worldtime = GameManager.Instance.World.GetWorldTime();
        float dawntime = SkyManager.GetDawnTime();
        if (GameUtils.WorldTimeToHours(worldtime) == dawntime && GameUtils.WorldTimeToMinutes(worldtime) < 1)
        {
            for (int i = 0; i < GameManager.Instance.World.Entities.list.Count; i++)
            {
                EntityAlive entityalive = GameManager.Instance.World.Entities.list[i] as EntityAlive;
                Entity entity = GameManager.Instance.World.Entities.list[i] as Entity;
                if (entity.GetSpawnerSource() == EnumSpawnerSource.Biome && entityalive.GetMaxHealth() == entityalive.Health && (entityalive is EntityZombie || entityalive is EntityEnemyAnimal))
                {
                    
                    entityalive.IsDespawned = true;
                    entityalive.MarkToUnload();
                }
            }
            Debug.Log("Cleaned up entities");
        }
        return true;
    }
}