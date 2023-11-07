using HarmonyLib;
using System;
using System.Text;


namespace FF4PRAutotracker
{
    // Hook New Game
    [HarmonyPatch(typeof(Last.Interpreter.DataStorage))]
    [HarmonyPatch("Initialize")]
    [HarmonyPatch(new Type[] { })]
    public static class Last_Interpreter_DataStorage_Initialize
    {
        static void Postfix(Last.Interpreter.DataStorage __instance)
        {
            try
            {
                Plugin.instance.Log.LogDebug($"Last.Interpreter.DataStorage::Initialize entered.");
                UATServer.instance?.Sync();

                /*
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("--------------------");
                sb.AppendLine("void Last.Interpreter.DataStorage::Initialize()");
                sb.Append("- __instance: ").AppendLine(__instance.ToString());


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
