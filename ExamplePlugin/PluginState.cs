using System;
using System.Collections.Generic;
using System.Text;
using RoR2;

namespace ExamplePlugin
{
    public static class PluginState
    {
        public static string LastPurchaseInteractorUsername = "";

        public static List<NetworkUser> AllPlayers = new List<NetworkUser>();
        
        public static List<string> SelectedPlayers = new List<string>();

        public static string Summary
        {
            get 
            {
                string ret = string.Empty;
                ret += $"LastPurchaser: {LastPurchaseInteractorUsername}\n";
                ret += "=====All Players=====\n";
                foreach (var player in AllPlayers) 
                {
                    ret += $"{player.userName}\n";
                }
                ret += "=====Selected Players=====\n";
                foreach (var playerName in SelectedPlayers) 
                {
                    ret += $"{playerName}\n";
                }
                ret += "==========\n";
                return ret; 
            }
        }
    }
}
