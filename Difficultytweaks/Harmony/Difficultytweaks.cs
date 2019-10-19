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
        [HarmonyPatch("GetMoveSpeedAggro")]
        class NoZombieRage
        {
            //static bool Prefix(EntityZombie __instance, ref float __result)
            //{
            //    bool nullifyrage = false;
            //    EntityClass entityClass = EntityClass.list[__instance.entityClass];
            //    bool.TryParse(entityClass.Properties.Values["Ragemode"], out nullifyrage);
            //    if (nullifyrage || __instance.Buffs.HasBuff("buffRageMode"))
            //        return true;
            //    FieldInfo fieldInfo = typeof(EntityAlive).GetField("IsFeral", BindingFlags.NonPublic);
            //    FieldInfo fieldInfo2 = typeof(EntityZombie).GetField("moveSpeeds", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            //    Debug.LogWarning("Part 1 done");
            //    bool IsFeral2 = (bool)fieldInfo.GetValue(__instance);
            //    float[] moveSpeeds2 = (float[])fieldInfo2.GetValue(__instance);
            //    Debug.LogWarning("Part 2 done");
            //    EnumGamePrefs eProperty = EnumGamePrefs.ZombieMove;
            //    if (__instance.IsBloodMoon)
            //    {
            //        eProperty = EnumGamePrefs.ZombieBMMove;
            //    }
            //    else if (IsFeral2)
            //    {
            //        eProperty = EnumGamePrefs.ZombieFeralMove;
            //    }
            //    else if (__instance.world.IsDark())
            //    {
            //        eProperty = EnumGamePrefs.ZombieMoveNight;
            //    }
            //    int @int = GamePrefs.GetInt(eProperty);
            //    float num = moveSpeeds2[@int];
            //    float speed = EffectManager.GetValue(PassiveEffects.RunSpeed, null, num, __instance, null, default(FastTags), true, true, true, true, 1, true);
            //    Debug.LogWarning("Part 3 done");
            //    Debug.LogWarning("Output: "+speed);
            //    __result = speed;
            //    return false;
            //}
            static bool Prefix(EntityZombie __instance, ref float __result, ref bool ___IsFeral, ref float[] ___moveSpeeds)
            {
                bool nullifyrage = false;
                EntityClass entityClass = EntityClass.list[__instance.entityClass];
                bool.TryParse(entityClass.Properties.Values["Ragemode"], out nullifyrage);
                if (nullifyrage || __instance.Buffs.HasBuff("buffRageMode"))
                    return true;
                EnumGamePrefs eProperty = EnumGamePrefs.ZombieMove;
                if (__instance.IsBloodMoon)
                {
                    eProperty = EnumGamePrefs.ZombieBMMove;
                }
                else if (___IsFeral)
                {
                    eProperty = EnumGamePrefs.ZombieFeralMove;
                }
                else if (__instance.world.IsDark())
                {
                    eProperty = EnumGamePrefs.ZombieMoveNight;
                }
                int @int = GamePrefs.GetInt(eProperty);
                float num = ___moveSpeeds[@int];
                if (num < 1f)
                {
                    num = __instance.moveSpeedAggro * (1f - num) + __instance.moveSpeedAggroMax * num;
                }
                else
                {
                    num = __instance.moveSpeedAggroMax * num;
                }
                float speed = EffectManager.GetValue(PassiveEffects.RunSpeed, null, num, __instance, null, default(FastTags), true, true, true, true, 1, true);
                __result = speed;
                return false;
            }
            //static float Postfix(EntityZombie __instance, float __result, bool ___IsFeral, float[] ___moveSpeeds)
            //{
            //    bool nullifyrage = false;
            //    EntityClass entityClass = EntityClass.list[__instance.entityClass];
            //    bool.TryParse(entityClass.Properties.Values["Ragemode"], out nullifyrage);
            //    if (nullifyrage || __instance.Buffs.HasBuff("buffRageMode"))
            //        return __result;
            //    EnumGamePrefs eProperty = EnumGamePrefs.ZombieMove;
            //    if (__instance.IsBloodMoon)
            //    {
            //        eProperty = EnumGamePrefs.ZombieBMMove;
            //    }
            //    else if (___IsFeral)
            //    {
            //        eProperty = EnumGamePrefs.ZombieFeralMove;
            //    }
            //    else if (__instance.world.IsDark())
            //    {
            //        eProperty = EnumGamePrefs.ZombieMoveNight;
            //    }
            //    int @int = GamePrefs.GetInt(eProperty);
            //    float num = ___moveSpeeds[@int];
            //    if (num < 1f)
            //    {
            //        num = __instance.moveSpeedAggro * (1f - num) + __instance.moveSpeedAggroMax * num;
            //    }
            //    else
            //    {
            //        num = __instance.moveSpeedAggroMax * num;
            //    }
            //    float speed = EffectManager.GetValue(PassiveEffects.RunSpeed, null, num, __instance, null, default(FastTags), true, true, true, true, 1, true);
            //    __result = speed;
            //    return __result;
            //}
        }
    }
}
