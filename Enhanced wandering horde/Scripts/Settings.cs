using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public class Configs
{
    public static bool _aggressive;
    public static bool _chaotic;
    static Configs()
    {
        var path = Path.ChangeExtension(Assembly.GetAssembly(typeof(Configs)).Location, "config");
        Log.Warning($"Config path={path}");
        if (File.Exists(path))
        {
            values config = JsonConvert.DeserializeObject<values>(File.ReadAllText(path));
            _aggressive = config.aggressive;
            _chaotic = config.chaotic;
        }
    }
    internal class values
    {
        public bool aggressive;
        public bool chaotic;
    }
}