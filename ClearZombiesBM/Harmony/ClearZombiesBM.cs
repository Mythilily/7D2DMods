using DMT;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

[HarmonyPatch(typeof(AIDirectorBloodMoonComponent))]
[HarmonyPatch("StartBloodMoon")]
public class ClearZombiesBM : IHarmony
{
    public void Start()
    {
        Debug.Log(" Loading Patch: " + GetType().ToString());
        var harmony = HarmonyInstance.Create(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
    static bool Prefix()
    {
        for (int i = 0; i < GameManager.Instance.World.Entities.list.Count; i++)
        {
            EntityAlive entity = GameManager.Instance.World.Entities.list[i] as EntityAlive;
            if (entity is EntityZombie || entity is EntityEnemyAnimal)
            {
                Debug.Log("Cleaning up entities");
                entity.IsDespawned = true;
                entity.MarkToUnload();
            }
        }
        return true;
    }
}



