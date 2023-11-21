using HarmonyLib;
using System;
using System.Linq;
using System.Text;


namespace FF4PRAutotracker
{
    // Hook SetCharacterStatus(CharacterStatus master, bool isInit) looking for new characters
    [HarmonyPatch(typeof(Last.Data.User.OwnedCharacterData))]
    [HarmonyPatch("SetCharacterStatus")]
    [HarmonyPatch(new Type[] { typeof(Last.Data.Master.CharacterStatus), typeof(bool) })]
    public static class Last_Data_User_OwnedCharacterData
    {
        public static void Postfix(Last.Data.User.OwnedCharacterData __instance, Last.Data.Master.CharacterStatus __0, bool __1)
/*{
    try {
       StringBuilder sb = new StringBuilder();
       sb.AppendLine("--------------------");
       sb.AppendLine("void Last.Data.User.OwnedCharacterData::SetCharacterStatus(Last.Data.Master.CharacterStatus master, bool isInit)");
       sb.Append("- __instance: ").AppendLine(__instance.ToString());
       sb.Append("- Parameter 0 'master': ").AppendLine(__0?.ToString() ?? "null");
       sb.Append("- Parameter 1 'isInit': ").AppendLine(__1.ToString());
       UnityExplorer.ExplorerCore.Log(sb.ToString());
    }
    catch (System.Exception ex) {
        UnityExplorer.ExplorerCore.LogWarning($"Exception in patch of void Last.Data.User.OwnedCharacterData::SetCharacterStatus(Last.Data.Master.CharacterStatus master, bool isInit):\n{ex}");
    }
}*/

        {
            Last.Data.Master.CharacterStatus master = __0;
            bool isInit = __1;
            try
            {
                if (isInit)
                {
                    if (Characters.IdToSlot.ContainsKey(master.Id) && (UATServer.instance != null))
                    {
                        UATServer.instance.SendCharacter(master.Id, master.JobId);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Plugin.instance.Log.LogError($"Exception in patch of void Last.Interpreter.DataStorage::SetFlag(Last.Interpreter.DataStorage+Flags f, int index, int segment, int value):\n{ex}");
            }
        }
    }
}
