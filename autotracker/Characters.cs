using System.Collections.Generic;

namespace FF4PRAutotracker
{
    public class Characters
    {
        public static Dictionary<int,int> Jobs = new Dictionary<int,int>
        {
            [6] = 1, // "dkCecil"
            [3] = 2, // "kain",
            [2] = 3, // "rosa",
            [7] = 4, // "rydia",
            [10] = 5, // "cid",
            [8] = 6, // "tellah",
            [9] = 7, // "edward",
            [11] = 8, // "yang",
            [12] = 9, // "palom",
            [13] = 10, // "porom",
            [5] = 11, // "edge",
            [14] = 12, // "fusoya",
            [1] = 13, // "cecil",
            [15] = 14, //"golbez",
        };      

        public static Dictionary<int,string> Flags = new Dictionary<int,string>
        {
            [200] = "character-2",
            [201] = "character-3",
            [202] = "character-4",
            [203] = "character-5",
        };

        public static Dictionary<int,string> IdToSlot = new Dictionary<int,string>
        {
            [1] = "character-1",
            [3] = "character-2",
            [5] = "character-3",
            [6] = "character-4",
            [7] = "character-5",
        };

        public static Dictionary<string,int> counts = new();

        public static void SyncCounts()
        {
            Plugin.instance.Log.LogInfo("SyncCounts");
            counts = new();
            foreach (var item in Jobs)
            {
                int t = Last.Interpreter.DataStorage.instance.Get(Last.Interpreter.DataStorage.Category.kTreasureFlag1,item.Key);
                // Plugin.instance.Log.LogInfo($"  {item.Value}[{item.Key}] = {t}");

                //counts[item.Value] = (counts.ContainsKey(item.Value) ? counts[item.Value] : 0) + t;
                
            }

        }




    }
}