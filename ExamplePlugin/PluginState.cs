using System;
using System.Collections.Generic;
using System.Text;
using RoR2;

namespace ExamplePlugin
{
    public static class PluginState
    {
        public static string LastPurchaser = "";

        public static List<NetworkUser> AllPlayers = new List<NetworkUser>();

        public static string Summary
        {
            get 
            {
                string ret = string.Empty;
                ret += $"LastPurchaser: {LastPurchaser}\n";
                ret += "=====Players=====\n";
                foreach (var player in AllPlayers) 
                {
                    ret += $"{player.userName}\n";
                }
                ret += "==========\n";
                return ret; 
            }
        }
    }
}
