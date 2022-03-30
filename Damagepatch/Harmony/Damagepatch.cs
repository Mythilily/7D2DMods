using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

public class Mythix_damagepatch
{
    public class Difficultytweaks : IModApi
    {
        public void InitMod(Mod _mod)
        {
            Log.Warning(" Loading Patch: " + GetType().ToString());
            var harmony = new Harmony("Damagepatch");
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
                if (!_attacker.IsClientControlled() && !_target.IsClientControlled())
                {
                    __result = _strength;
                    return false;
                }

                int @int = GameStats.GetInt(EnumGameStats.GameDifficulty);
                if (_attacker.IsClientControlled())
                {
                    if (@int > 2)
                    {
                        __result = _strength;
                        return false;
                    }
                }
                else
                {
                    //Log.Warning($"Original: {_strength}");
                    switch (@int)
                    {
                        case 0:
                            _strength = Mathf.RoundToInt((float)_strength * 0.5f);
                            break;
                        case 1:
                            _strength = Mathf.RoundToInt((float)_strength * 0.75f);
                            break;
                        case 3:
                            _strength = Mathf.RoundToInt((float)_strength * 1.5f);
                            break;
                        case 4:
                            _strength = Mathf.RoundToInt((float)_strength * 2f);
                            break;
                        case 5:
                            _strength = Mathf.RoundToInt((float)_strength * 2.5f);
                            break;
                    }
                    //Log.Warning($"Result: {_strength}");
                    __result = _strength;
                    return false;
                }
                return true;
            }
        }
    }
}
