//Harmony Patch: No damage reduction at higher difficulties
//Author: Mythix(dino)
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using DMT;

[HarmonyPatch(typeof(ItemActionAttack))]
[HarmonyPatch("difficultyModifier")]

public class Nodifficultypenalty : IHarmony
{
    public void Start()
    {
        Debug.Log(" Loading Patch: " + GetType().ToString());
        var harmony = HarmonyInstance.Create(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
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
    //static int Postfix(int __result, int _strength, Entity _attacker, Entity _target)
    //{
    //    Debug.Log("Patching...");
    //    int @int = GameStats.GetInt(EnumGameStats.GameDifficulty);
    //    if (_attacker.IsClientControlled() && !_target.IsClientControlled() && EntityClass.list[_target.entityClass].bIsEnemyEntity)
    //    {
    //        Debug.Log("Diff " + @int + " |result " + __result + " |strength " + _strength);
    //        Debug.Log("Patching player damage");
    //        if (@int > 2 && __result < _strength)
    //        {
    //            Debug.Log("True");
    //            return _strength;
    //            Debug.Log("Done");
    //        }
    //    }
    //    return __result;
    //}
