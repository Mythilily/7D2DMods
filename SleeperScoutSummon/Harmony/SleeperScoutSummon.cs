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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class mythixsleeperscouts
{
    static float seconds = 3f;
    public class sleeperscouts : IModApi
    {
        public void InitMod(Mod _mod)
        {
            var path = Path.ChangeExtension(Assembly.GetAssembly(typeof(mythixsleeperscouts)).Location, "config");

            Values value = JsonConvert.DeserializeObject<Values>(File.ReadAllText(path));
            seconds = value.SummonDelayInSeconds;
            Log.Warning($"Scout delay: {seconds}");

            Log.Warning(" Loading Patch: " + GetType().ToString());
			var harmony = new Harmony("sleeperscouts");
			harmony.PatchAll();
            
        }
    }
    internal class Values
    {
        public float SummonDelayInSeconds { get; set; }
    }

    [HarmonyPatch(typeof(EAIManager))]
    [HarmonyPatch("SleeperWokeUp")]
    public class sleeperscoutsummon
    {
        static WaitForSeconds entityWait = new WaitForSeconds(seconds);
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