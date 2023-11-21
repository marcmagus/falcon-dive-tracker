using HarmonyLib;
using System;
using System.Text;


namespace FF4PRAutotracker
{
    // Hook Set (category, index, value) looking for ScenarioFlag1 Sets
    [HarmonyPatch(typeof(Last.Interpreter.DataStorage))]
    [HarmonyPatch("Set")]
    [HarmonyPatch(new Type[] { typeof(Last.Interpreter.DataStorage.Category), typeof(int), typeof(int) })]
    public static class Last_Interpreter_DataStorage_Set
    {
        public static void Postfix(Last.Interpreter.DataStorage __instance, Last.Interpreter.DataStorage.Category __0, int __1, int __2)
        {
            Last.Interpreter.DataStorage.Category category = __0;
            int index = __1;
            int value = __2;
            try
            {
                if (category == Last.Interpreter.DataStorage.Category.kScenarioFlag1)
                {
                    Plugin.instance.Log.LogInfo($"Set Scenario {index} => {value}");
                    if ((Scenarios.Flags.ContainsKey(index)) && (UATServer.instance != null))
                    {
                        UATServer.instance.SendScenario(index,value);
                    }
                }
                else if (Last.Interpreter.DataStorage.Category.kTreasureFlag1 <= category && 
                         category <= Last.Interpreter.DataStorage.Category.kTreasureFlag4)
                {
                    index += (category - Last.Interpreter.DataStorage.Category.kTreasureFlag1) * 256; // Byte-limited flag indices
                    if ((Treasures.Flags.ContainsKey(index)) && (UATServer.instance != null))
                    {
                        // Plugin.instance.Log.LogDebug($"Opened [{index}] {Treasures.Flags[index]}");
                        UATServer.instance.SendTreasure(index);
                    }
                }

                /*
                StringBuilder sb = new StringBuilder();
                sb.Append("Set(");
                sb.Append(" 'category': ").Append(category.ToString());
                sb.Append(" 'index': ").Append(index.ToString());
                sb.Append(" 'value': ").Append(value.ToString());
                sb.Append(" )");
                Plugin.instance.Log.LogDebug(sb.ToString());
                */
            }
            catch (System.Exception ex)
            {
                Plugin.instance.Log.LogError($"Exception in patch of void Last.Interpreter.DataStorage::SetFlag(Last.Interpreter.DataStorage+Flags f, int index, int segment, int value):\n{ex}");
            }
        }
    }
}
