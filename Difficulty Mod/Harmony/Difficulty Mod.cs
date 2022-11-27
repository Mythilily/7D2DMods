using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;


public class DifficultyMod : IModApi
{
    public void InitMod(Mod _mod)
    {
        Log.Warning(" Loading Patch: " + GetType().ToString());
        var harmony = new Harmony("diffmod");
        harmony.PatchAll();
    }
    [HarmonyPatch(typeof(ItemActionAttack))]
    [HarmonyPatch("difficultyModifier")]
    class first
    {
        static bool Prefix(ref int __result, int _strength, Entity _attacker, Entity _target)
        {
            float player0 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "playerDamage_0"));
            float player1 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "playerDamage_1"));
            float player2 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "playerDamage_2"));
            float player3 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "playerDamage_3"));
            float player4 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "playerDamage_4"));
            float player5 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "playerDamage_5"));

            float entityenemy0 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "enemyDamage_0"));
            float entityenemy1 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "enemyDamage_1"));
            float entityenemy2 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "enemyDamage_2"));
            float entityenemy3 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "enemyDamage_3"));
            float entityenemy4 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "enemyDamage_4"));
            float entityenemy5 = float.Parse(DMConfig.GetPropertyValue("DMConfigLoad", "enemyDamage_5"));

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
                //Log.Warning($"Original player: {_strength}");
                switch (@int)
                {
                    case 0:
                        _strength = Mathf.RoundToInt((float)_strength * player0);
                        break;
                    case 1:
                        _strength = Mathf.RoundToInt((float)_strength * player1);
                        break;
                    case 2:
                        _strength = Mathf.RoundToInt((float)_strength * player2);
                        break;
                    case 3:
                        _strength = Mathf.RoundToInt((float)_strength * player3);
                        break;
                    case 4:
                        _strength = Mathf.RoundToInt((float)_strength * player4);
                        break;
                    case 5:
                        _strength = Mathf.RoundToInt((float)_strength * player5);
                        break;
                }
                //Log.Warning($"Result player: {_strength}");
                __result = _strength;
                return false;
            }
            else
            {
                //Log.Warning($"Original: {_strength}");
                switch (@int)
                {
                    case 0:
                        _strength = Mathf.RoundToInt((float)_strength * entityenemy0);
                        break;
                    case 1:
                        _strength = Mathf.RoundToInt((float)_strength * entityenemy1);
                        break;
                    case 2:
                        _strength = Mathf.RoundToInt((float)_strength * entityenemy2);
                        break;
                    case 3:
                        _strength = Mathf.RoundToInt((float)_strength * entityenemy3);
                        break;
                    case 4:
                        _strength = Mathf.RoundToInt((float)_strength * entityenemy4);
                        break;
                    case 5:
                        _strength = Mathf.RoundToInt((float)_strength * entityenemy5);
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
