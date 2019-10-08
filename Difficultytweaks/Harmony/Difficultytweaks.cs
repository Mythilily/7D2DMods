using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using DMT;

public class Mythix_difficultytweaks
{
    public class Difficultytweaks : IHarmony
    {
        public void Start()
        {
            Debug.Log(" Loading Patch: " + GetType().ToString());
            var harmony = HarmonyInstance.Create(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        [HarmonyPatch(typeof(ItemActionAttack))]
        [HarmonyPatch("difficultyModifier")]
        class Nodamagereduction
        {
            static bool Prefix(ref int __result, int _strength, Entity _attacker, Entity _target)
            {
                if (_attacker == null || _target == null)
                {
                    __result = _strength;
                    return false;
                }

                if (_attacker.IsClientControlled() && _target.IsClientControlled())
                {
                    __result = _strength;
                    return false;
                }

                int @int = GameStats.GetInt(EnumGameStats.GameDifficulty);
                if (_attacker.IsClientControlled() && !_target.IsClientControlled() && EntityClass.list[_target.entityClass].bIsEnemyEntity)
                {
                    if (@int > 2)
                    {
                        __result = _strength;
                        return false;
                    }
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(EntityZombie))]
        [HarmonyPatch("ProcessDamageResponseLocal")]
        class NoZombieRage
        {
            static bool Prefix(EntityZombie __instance, DamageResponse _dmResponse)
            {
                __instance.ProcessDamageResponseLocal(_dmResponse);
                return false;
            }
        }
    }
}
