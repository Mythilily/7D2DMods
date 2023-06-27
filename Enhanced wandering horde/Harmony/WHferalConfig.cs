using System.Collections.Generic;

public static class WHferalConfig
{
    public static bool CheckFeatureStatus(string strFeature)
    {
        BlockValue blockValue = Block.GetBlockValue("WHferalConfigBlock");
        if (blockValue.type == 0)
            return false;

        bool result = false;
        if(blockValue.Block.Properties.Contains(strFeature))
            result = blockValue.Block.Properties.GetBool(strFeature);

        return result;
    }

    
    public static bool CheckFeatureStatus(string strClass, string strFeature)
    {
        BlockValue blockValue = Block.GetBlockValue("WHferalConfigBlock");
        if (blockValue.type == 0)
            return false;

        bool result = false;
        if(blockValue.Block.Properties.Classes.ContainsKey(strClass))
        {
            DynamicProperties dynamicProperties3 = blockValue.Block.Properties.Classes[strClass];
            foreach(System.Collections.Generic.KeyValuePair<string, object> keyValuePair in dynamicProperties3.Values.Dict.Dict)
                if(string.Equals(keyValuePair.Key, strFeature, System.StringComparison.CurrentCultureIgnoreCase))
                    result = StringParsers.ParseBool(dynamicProperties3.Values[keyValuePair.Key]);
        }

        return result;
    }

    public static string GetPropertyValue(string strClass, string strFeature)
    {
        BlockValue blockValue = Block.GetBlockValue("WHferalConfigBlock");
        if(blockValue.type == 0)
            return string.Empty;


        string result = string.Empty;
        if(blockValue.Block.Properties.Classes.ContainsKey(strClass))
        {
            DynamicProperties dynamicProperties3 = blockValue.Block.Properties.Classes[strClass];
            foreach(KeyValuePair<string, object> keyValuePair in dynamicProperties3.Values.Dict.Dict)
            {
                if(keyValuePair.Key == strFeature)
                    return keyValuePair.Value.ToString();
            }
        }

        return result;
    }
}

