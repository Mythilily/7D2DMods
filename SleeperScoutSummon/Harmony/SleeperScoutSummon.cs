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
    public static IList<Values> values;
    public class sleeperscouts : IModApi
    {
        public void InitMod(Mod _mod)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"Mods{Path.DirectorySeparatorChar}SleeperScoutSummon{Path.DirectorySeparatorChar}Config.json");
            var jsonText = File.ReadAllText(path);

            values = JsonConvert.DeserializeObject<IList<Values>>(jsonText);
            Log.Warning($"Scout delay: {values[0].SummonDelayInSeconds.ToString()}");

            Log.Warning(" Loading Patch: " + GetType().ToString());
			var harmony = new Harmony("sleeperscouts");
			harmony.PatchAll();
            
        }
    }
    public class Values
    {
        public float SummonDelayInSeconds { get; set; }
    }

    [HarmonyPatch(typeof(EAIManager))]
    [HarmonyPatch("SleeperWokeUp")]
    public class sleeperscoutsummon
    {
        static WaitForSeconds entityWait = new WaitForSeconds(values[0].SummonDelayInSeconds);
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