using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System.Reflection;
using BepInEx.Logging;
using WebSocketSharp.Server;
using Il2CppSystem.Runtime.InteropServices;
using Serial.FF0.Map;


namespace FF4PRAutotracker
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        public static Plugin instance { get; set ; }

        private WebSocketServer wss;

        public override void Load()
        {
            instance = this;
            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Log.LogInfo($"Calling Harmony patcher...");
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var type in AccessTools.GetTypesFromAssembly(assembly))
            {
                PatchClassProcessor processor = harmony.CreateClassProcessor(type);
                if (processor.Patch()?.Count > 0)
                    Log.LogInfo($"[Harmony] {type.Name} successfully applied.");
            }
            Log.LogInfo($"...finished patching.");

            // Starting up WebSocket Server
            wss = new WebSocketServer("ws://localhost:65399")
            {
                ReuseAddress = true
            };
            wss.AddWebSocketService<UATServer>("/");
            wss.Start();

        }
    }



}
