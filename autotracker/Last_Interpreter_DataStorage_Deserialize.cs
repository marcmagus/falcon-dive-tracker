using HarmonyLib;
using System;
using System.Text;


namespace FF4PRAutotracker
{
    // Hook load from Save
    [HarmonyPatch(typeof(Last.Interpreter.DataStorage))]
    [HarmonyPatch("Deserialize")]
    [HarmonyPatch(new Type[] { typeof(string) })]
    public static class Last_Interpreter_DataStorage_Deserialize
    {
        static void Postfix(Last.Interpreter.DataStorage __instance, string __0)
        {
            try
            {
                Plugin.instance.Log.LogDebug($"Last.Interpreter.DataStorage::Deserialize entered.");
                UATServer.instance?.Sync();

                /*
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("--------------------");
                sb.AppendLine("void Last.Interpreter.DataStorage::Deserialize(string json)");
                sb.Append("- __instance: ").AppendLine(__instance.ToString());
                sb.Append("- Parameter 0 'json': ").AppendLine(__0?.ToString() ?? "null");

                Plugin.instance.Log.LogInfo(sb.ToString());
                */
            }
            catch (System.Exception ex)
            {
                Plugin.instance.Log.LogError($"Exception in patch of void Last.Interpreter.DataStorage::SetFlag(Last.Interpreter.DataStorage+Flags f, int index, int segment, int value):\n{ex}");
            }
        }
    }
}
