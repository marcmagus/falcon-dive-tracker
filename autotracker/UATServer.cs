using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Text.Json;


namespace FF4PRAutotracker
{
    public class UATServer : WebSocketBehavior
    {
        public static Dictionary<int,string> scenarios = new Dictionary<int,string>
        {
            [6] = "boss-mist-dragon",
            [7] = "boss-rydia",
            [8] = "boss-baron-soldiers",
            [17] = "boss-waterhag",
            [13] = "boss-octomamm",
            [15] = "boss-edward",
            [16] = "boss-antlion",
            [19] = "boss-mombomb",
            [21] = "boss-fabul-gauntlet",
            [26] = "boss-scarmiglione",
            [27] = "boss-scarmiglione-z",
            [28] = "boss-dkcecil",
            /* [31] = "boss-baron-guards", */ // No actual flag
            [31] = "boss-yang",
            [32] = "boss-baigan",
            [33] = "boss-cagnazzo",
            [75] = "boss-odin",
            [98] = "boss-dark-elf",
            [40] = "boss-magus-sisters",
            [43] = "boss-barbariccia",
            [72] = "boss-rat-tail",
            [391] = "boss-nothing-machine",
            /* [46] = "boss-calcobrena", */ // No actual flag
            [46] = "boss-golbez",
            [47] = "boss-lugae",
            [48] = "boss-dark-imps",
            [86] = "boss-asura",
            [87] = "boss-leviathan",
            [60] = "boss-demon-wall",
            [78] = "boss-sylvan-cave",
            [52] = "boss-king-queen-eblan",
            [53] = "boss-rubicante",
            [91] = "boss-bahamut",
            [67] = "boss-elements",
            [68] = "boss-cpu",
            [82] = "boss-white-dragon",
            [83] = "boss-dark-bahamut",
            [80] = "boss-plague",
            [84] = "boss-lunasaur",
            [81] = "boss-ogopogo",
            [200] = "character-2",
            [201] = "character-3",
            [202] = "character-4",
            [203] = "character-5",
            [204] = "bomb-ring",
            [205] = "kaipo-pass",
            [206] = "legend-sword",
            [207] = "sand-ruby",
            [208] = "baron-key",
            [209] = "twin-harp",
            [210] = "earth-crystal",
            [211] = "magma-rock",
            [212] = "lugae-key",
            [213] = "hovercraft",
            [214] = "lucas-necklace",
            [215] = "dark-crystal",
            [216] = "rat-tail",
            [222] = "pink-tail",
            [223] = "trash-can",
            [224] = "frying-pan",
            [225] = "adamantite",
            [301] = "crystal-shard-1",
            [302] = "crystal-shard-2",
            [303] = "crystal-shard-3",
            [304] = "crystal-shard-4",
            [321] = "nothing-1",
            [322] = "nothing-2",
            [323] = "nothing-3",
            [324] = "nothing-4",
            [325] = "nothing-5",
            [326] = "nothing-6",
            [327] = "nothing-7",
            [328] = "nothing-8",
            [329] = "nothing-9",
};

        // Need a static reference to the instance so Harmony hooks can find it
        public static UATServer instance {get; set;}

        protected class Command
        {
            public string cmd {get; set; }
            public string slot {get; set; }
        }

        /// <summary>
        /// UAT <c>Var</c> - sent for each variable after a Sync, or when any variable changed.
        /// </summary>
        protected class Var 
        {
            public string cmd {get; set; } = "Var";
            public string name {get; set;}
            public Object value {get;set;} 
        }

        /// <summary>
        /// Send an array containing all variables to the client, used in response to a Sync command.
        /// </summary>
        protected void Sync()
        {
            List<Var> vars = new List<Var>();
            foreach (var item in scenarios)
            {
                vars.Add(new Var{
                    name=item.Value,
                    value=Last.Interpreter.DataStorage.instance.Get(Last.Interpreter.DataStorage.Category.kScenarioFlag1,item.Key)
                    });
            }

            SendAsync(JsonSerializer.Serialize(vars),null);
            /* Plugin.instance.Log.LogDebug($"{JsonSerializer.Serialize(vars)}"); */
        }

        /// <summary>
        /// Send a single variable to the client.
        /// </summary>
        /// <param name="index">Index of ScenarioFlag1 referring to the flag</param>
        /// <param name="value">New value</param>
        public void SendVar(int index, int value)
        {
            if (UATServer.scenarios.ContainsKey(index))
            {
                SendAsync(JsonSerializer.Serialize(new[] {
                    new Var {
                        name = scenarios[index],
                        value = value,
                    }}), null);
            }
            else
            {
                Plugin.instance.Log.LogError($"Attempted to send unknown variable {index}");
            }
        }

       protected override void OnMessage (MessageEventArgs e)
        { 
            var commands = JsonSerializer.Deserialize<Command[]>(e.Data);
            foreach (Command command in commands)
            {
                if (command.cmd == "Sync")
                {
                    Sync();
                } 
                else 
                {
                    // ErrorReply
                    Plugin.instance.Log.LogError($"UAT Bad Command: {command.cmd}");
                    SendAsync(JsonSerializer.Serialize(new [] {
                        new 
                        {
                            cmd = "ErrorReply",
                            name = command.cmd,
                            reason = "unknown cmd"
                        }
                    }), null);
                }
            }
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            instance=this;
            Plugin.instance.Log.LogInfo($"UAT Connection received.");
            SendAsync(JsonSerializer.Serialize(new [] {
                new
                {
                    cmd = "Info",
                    protocol = 0,
                    name = PluginInfo.PLUGIN_NAME,
                    version = PluginInfo.PLUGIN_VERSION
                }
            }), null);
        }

    }
}
