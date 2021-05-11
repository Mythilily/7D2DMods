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
using System.Collections;

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
    public class sleeperscoutsummon
    {
        static bool Prefix(EAIManager __instance, EntityAlive ___entity)
        {
            IEnumerator DelayThenRun(int entityId)
            {
                yield return new WaitForSeconds(3f);
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                if (GameManager.Instance.World.Entities.dict.TryGetValue(entityId, out var entity)
                    && entity != null
                    && entity is EntityAlive entityAlive
                    && entityAlive.IsSleeper
                    && entityAlive.IsAlive())
                {
                    Log.Warning("Spawning scout...");
                    GameManager.Instance.World.aiDirector.GetComponent<AIDirectorChunkEventComponent>().SpawnScouts(entityAlive.position);
                }

                watch.Stop();
                Log.Out($"Execution Time: {watch.ElapsedMilliseconds} ms");
            }
            GameManager.Instance.StartCoroutine(DelayThenRun(___entity.entityId));
            return true;
        }
    }
}