using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using DMT;

public class Mythix_doubledamage
{
    public class Difficultytweaks : IHarmony
    {
        public void Start()
        {
            Log.Warning(" Loading Patch: " + GetType().ToString());
            var harmony = new Harmony("Doubledamage");
            harmony.PatchAll();
        }
        [HarmonyPatch(typeof(ItemActionAttack))]
        [HarmonyPatch("difficultyModifier")]
        class Dbldamage
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
                else
                {
                    _strength = Mathf.RoundToInt((float)_strength * 2f);
                    __result = _strength;
                    return false;
                }
                return true;
            }
        }
    }
}
