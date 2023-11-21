using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Text.Json;


namespace FF4PRAutotracker
{
    public class UATServer : WebSocketBehavior
    {


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
        public void Sync()
        {
            List<Var> vars = new List<Var>();
            foreach (var item in Scenarios.Flags)
            {
                vars.Add(new Var{
                    name=item.Value,
                    value=Last.Interpreter.DataStorage.instance.Get(Last.Interpreter.DataStorage.Category.kScenarioFlag1,item.Key)
                    });
            }
            Treasures.SyncCounts();
            foreach (var item in Treasures.counts)
            {
                vars.Add(new Var{
                    name=item.Key,
                    value=item.Value
                });
            }
            foreach (var character in Last.Management.UserDataManager.Instance().GetOwnedCharactersClone(false))
            {
                vars.Add(new Var{
                    name=Characters.IdToSlot[character.characterStatusId],
                    value=new List<Object> {
                        true,
                        Characters.Jobs[character.JobId]
                        },
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
        public void SendScenario(int index, int value)
        {
            if (Scenarios.Flags.ContainsKey(index))
            {
                SendAsync(JsonSerializer.Serialize(new[] {
                    new Var {
                        name = Scenarios.Flags[index],
                        value = value,
                    }}), null);
                Plugin.instance.Log.LogInfo($"Scenario: [{index}] -> {value}");
            }
            else
            {
                Plugin.instance.Log.LogError($"Attempted to send unknown variable {index}");
            }
        }

        public void SendCharacter(int id, int jobId)
        {
            Plugin.instance.Log.LogInfo($"Sending Character {id}...");
            if (Characters.IdToSlot.ContainsKey(id) && Characters.Jobs.ContainsKey(jobId))
            {
                SendAsync(JsonSerializer.Serialize(new[] {
                new Var {
                    name = Characters.IdToSlot[id],
                    value = new List<Object> {
                        true,
                        Characters.Jobs[jobId],
                    },
                }}), null);
                Plugin.instance.Log.LogInfo($"Character: [{id}] -> {Characters.IdToSlot[id]} = {jobId}");
            }
            else
            {
                Plugin.instance.Log.LogError($"Attempted to send unknown character {id} job {jobId}");
            }
        }

        public void SendTreasure(int index)
        {
            if (Treasures.Flags.ContainsKey(index))
            {
                Treasures.SyncCounts();
                SendAsync(JsonSerializer.Serialize(new[] {
                    new Var {
                        name = Treasures.Flags[index],
                        value = Treasures.counts[Treasures.Flags[index]],
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
