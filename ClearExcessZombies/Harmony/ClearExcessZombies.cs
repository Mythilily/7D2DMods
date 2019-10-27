using DMT;
using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

public class Mythix_ClearExcessZombies
{
    public static EnumGameStats enemyCountEnum;
    class ClearExcessZombies : IHarmony
    {
        public void Start()
        {
            Debug.Log(" Loading Patch: " + GetType().ToString());
            var harmony = HarmonyInstance.Create(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
    [HarmonyPatch(typeof(World))]
    [HarmonyPatch("OnUpdateTick")]
    class DespawnZombies
    {
        static int maxzombies = GamePrefs.GetInt(EnumGamePrefs.MaxSpawnedZombies);
        static bool Prefix()
        {
            ulong worldtime = GameManager.Instance.World.GetWorldTime();
            if (worldtime % 500 == 0)
            {
                enemyCountEnum = EnumUtils.Parse<EnumGameStats>("EnemyCount");
                int enemycount = GameStats.GetInt(enemyCountEnum);
                int entities = 0, npc = 0;
                if (enemycount * 1.1 >= maxzombies)
                {
                    Debug.LogWarning("EnemyCount: " + enemycount);
                    Debug.LogWarning("Maxspawn: " + maxzombies);
                    Debug.LogWarning("Attempting to clear biome entities...");
                    for (int i = 0; i < GameManager.Instance.World.Entities.list.Count; i++)
                    {
                        EntityAlive entityalive = GameManager.Instance.World.Entities.list[i] as EntityAlive;
                        if (entityalive.GetSpawnerSource() == EnumSpawnerSource.Biome && (entityalive is EntityZombie || entityalive is EntityEnemyAnimal))
                        {
                            entityalive.MarkToUnload();
                            entities++;
                        }
                    }
                    if ((enemycount - entities) * 1.1 >= maxzombies)
                    {
                        Debug.LogWarning("Entity amount still too high.\n Attempting to clear all hostile entities but sleepers...");
                        for (int i = 0; i < GameManager.Instance.World.Entities.list.Count; i++)
                        {
                            EntityAlive entityalive = GameManager.Instance.World.Entities.list[i] as EntityAlive;
                            if ((entityalive is EntityZombie || entityalive is EntityEnemyAnimal) && !entityalive.IsSleeping)
                            {

                                entityalive.MarkToUnload();
                                entities++;
                            }
                        }
                    }
                    if ((enemycount - entities) * 1.1 >= maxzombies)
                    {
                        Debug.LogError("Severe entity amounts.\n Attempting to clear all entities...");
                        for (int i = 0; i < GameManager.Instance.World.Entities.list.Count; i++)
                        {
                            EntityAlive entityalive = GameManager.Instance.World.Entities.list[i] as EntityAlive;
                            if (!(entityalive is EntityNPC))
                            {
                                entityalive.MarkToUnload();
                                entities++;
                            }
                        }
                    }
                    if ((enemycount - entities) * 1.1 >= maxzombies)
                    {
                        Debug.LogError("Severe entity amounts.\n Attempting to clear all NPC entities...");
                        for (int i = 0; i < GameManager.Instance.World.Entities.list.Count; i++)
                        {
                            EntityAlive entityalive = GameManager.Instance.World.Entities.list[i] as EntityAlive;
                            if (entityalive is EntityNPC)
                            {
                                entityalive.MarkToUnload();
                                entities++;
                                npc++;
                            }
                        }
                    }
                    if (entities != 0)
                    {
                        Debug.Log("Unloaded " + entities + " entities");
                    }
                    if (npc != 0)
                    {
                        Debug.LogWarning("Unloaded " + npc + " NPCs");
                    }
                }
            }
            return true;
        }
    }
}