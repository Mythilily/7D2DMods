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
using System.Threading;

class mythixsleeperscouts
{
    public class sleeperscouts : IHarmony
    {
        public void Start()
        {
            Log.Warning(" Loading Patch: " + this.GetType().ToString());
			var harmony = new Harmony("sleeperscouts");
			harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(EAIManager))]
    [HarmonyPatch("SleeperWokeUp")]
    public static class sleeperscoutsummon
    {
        static bool Prefix(EAIManager __instance, EntityAlive ___entity)
        {
            Thread CheckThread = new Thread((object EntityID) =>
            {
                Thread.Sleep(3000); // wait 3 seconds
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                int ListCount = GameManager.Instance.World.Entities.list.Count == null ? 0 : (GameManager.Instance.World.Entities.list.Count > 0 ? (GameManager.Instance.World.Entities.list.Count - 1) : 0);
                for (int i = ListCount; i >= 0; i--)
                {
                    Entity CurEntity = GameManager.Instance.World.Entities.list[i];
                    EntityAlive CurEntityAlive = null;
                    if (CurEntity is EntityAlive)
                    {
                        CurEntityAlive = (EntityAlive)CurEntity;
                        if (CurEntity.entityId == (int)EntityID)
                        {

                            if (CurEntityAlive.IsSleeper && !CurEntityAlive.IsDead())
                            {
                                Log.Warning("Spawning scout...");
                                GameManager.Instance.World.aiDirector.GetComponent<AIDirectorChunkEventComponent>().SpawnScouts(CurEntityAlive.position);
                            }
                            break;
                        }
                    }
                }
                watch.Stop();
                Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            });
            CheckThread.Start(___entity.entityId);
            return true;
        }
    }
}