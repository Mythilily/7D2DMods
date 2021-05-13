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
        static WaitForSeconds entityWait = new WaitForSeconds(3f);
        static bool Prefix(EAIManager __instance, EntityAlive ___entity)
        {

            IEnumerator DelayThenRun(EntityAlive entity)
            {
                yield return entityWait; //saves allocation/gc

                if (entity != null && entity.IsSleeper && entity.IsAlive())
                {
                    //Log.Warning("Spawning scout...");
                    GameManager.Instance.World.aiDirector.GetComponent<AIDirectorChunkEventComponent>() //could cache this
                        .SpawnScouts(entity.position);
                }
            }
            GameManager.Instance.StartCoroutine(DelayThenRun(___entity));
            return true;
        }
    }
}