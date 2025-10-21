using BepInEx;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using On;
using System.Collections.Generic;
using R2API.Utils;
using LightweightAPI;

namespace ExamplePlugin
{
    // This is an example plugin that can be put in
    // BepInEx/plugins/ExamplePlugin/ExamplePlugin.dll to test out.
    // It's a small plugin that adds a relatively simple item to the game,
    // and gives you that item whenever you press F2.

    // This attribute specifies that we have a dependency on a given BepInEx Plugin,
    // We need the R2API ItemAPI dependency because we are using for adding our item to the game.
    // You don't need this if you're not using R2API in your plugin,
    // it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    //[BepInDependency(ItemAPI.PluginGUID)]

    // This one is because we use a .language file for language tokens
    // More info in https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Assets/Localization/
    //[BepInDependency(LanguageAPI.PluginGUID)]

    // This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    // This is the main declaration of our plugin class.
    // BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    // BaseUnityPlugin itself inherits from MonoBehaviour,
    // so you can use this as a reference for what you can declare and use in your plugin class
    // More information in the Unity Docs: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync,
                      VersionStrictness.DifferentModVersionsAreOk)]
    public class ExamplePlugin : BaseUnityPlugin
    {
        // The Plugin GUID should be a unique ID for this plugin,
        // which is human readable (as it is used in places like the config).
        // If we see this PluginGUID as it is on thunderstore,
        // we will deprecate this mod.
        // Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "AuthorName";
        public const string PluginName = "ExamplePlugin";
        public const string PluginVersion = "1.0.0";

        // The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            // Init our logging class so that we can properly log for debugging
            Log.Init(Logger);
            Run.onRunStartGlobal += OnRunStart;
            Run.onRunDestroyGlobal += OnRunEnd;

        }

        // OnEnable is called when the plugin becomes enabled and active
        // This is where we register our hooks
        private void OnEnable()
        {
            // Register our hook - this will be called every time a HealthComponent takes damage
            // EXAMPLE HOOK registration
            //On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.ChestBehavior.BaseItemDrop += ChestBehavior_BaseItemDropHook;
            On.RoR2.PurchaseInteraction.OnInteractionBegin += PurchaseInteraction_OnInteractionBeginHook;
        }

        // OnDisable is called when the plugin becomes disabled or inactive
        // This is where we unregister our hooks to prevent memory leaks
        private void OnDisable()
        {
            // Unregister our hook
            // EXAMPLE HOOK unregister
            //On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            On.RoR2.ChestBehavior.BaseItemDrop -= ChestBehavior_BaseItemDropHook;
            On.RoR2.PurchaseInteraction.OnInteractionBegin -= PurchaseInteraction_OnInteractionBeginHook;
        }

        private void OnRunStart(Run run)
        {
            Logger.LogInfo($"Run started: {run.name}");

            // Initialize our item catalog system
            ItemCatalogManager.Initialize();
            ItemController.Initialize();

            // Start the web API
            StartWebApi();

            // Enumerate all players currently in the run
            foreach (var user in NetworkUser.readOnlyInstancesList)
            {
                Logger.LogInfo($"Player: {user.userName} ({user.Network_id.steamId})");
            }

            // Optionally store them for later
            PluginState.AllPlayers = new List<NetworkUser>(NetworkUser.readOnlyInstancesList);
        }

        private void OnRunEnd(Run run)
        {
            Logger.LogInfo("Run ended, clearing player list");
            PluginState.AllPlayers?.Clear();
            StopWebApi();
        }

        private SimpleHttpServer _httpServer;

        private static void ChestBehavior_BaseItemDropHook(On.RoR2.ChestBehavior.orig_BaseItemDrop orig, ChestBehavior self)
        {
            
            if (ItemController.IsEnabled && ItemController.CurrentPickupIndex != PickupIndex.none)
            {
                self.dropPickup = ItemController.CurrentPickupIndex;
                Log.Info($"Overriding pickup to: {ItemController.CurrentlySelectedItem?.DisplayName} ({self.dropPickup})");
            }
            orig(self);
        }

        private static void PurchaseInteraction_OnInteractionBeginHook(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
        {
            // Save the name of the last person that opened a chest
            var body = activator ? activator.GetComponent<CharacterBody>() : null;
            var user = body?.master?.playerCharacterMasterController?.networkUser;
            var openerName = user?.userName ?? body?.GetUserName() ?? "Unknown";
            PluginState.LastPurchaser = openerName;
            orig(self, activator);
        }

        // The Update() method is run on every frame of the game.
        private void Update()
        {
            // This if statement checks if the player has currently pressed F2.
            if (Input.GetKeyDown(KeyCode.F2))
            {
                // Cycle through items
                ItemController.CycleItem();
                Log.Info($"Item: {ItemController.GetCurrentStatus()}");
            }

            if (Input.GetKeyDown(KeyCode.F3)) 
            {
                ItemController.ToggleEnabled();
                Log.Info($"Item Controller: {ItemController.GetCurrentStatus()}");
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                // Cycle through tiers
                ItemController.CycleTier();
                Log.Info($"Tier switched: {ItemController.GetCurrentStatus()}");
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                Log.Info(PluginState.Summary);
            }
        }

        private async void StartWebApi()
        {
            try
            {
                _httpServer = new SimpleHttpServer((level, message) => 
                {
                    if (level == "ERROR")
                        Log.Error(message);
                    else if (level == "WARNING")
                        Log.Warning(message);
                    else
                        Log.Info(message);
                });

                var itemService = new RoR2ItemService();
                _httpServer.SetItemService(itemService);
                
                await _httpServer.StartAsync("http://localhost:8080/");
                Log.Info("HTTP server started successfully at http://localhost:8080");
                Log.Info("Available endpoints:");
                Log.Info("  GET  /api/status - Get current status");
                Log.Info("  GET  /api/items - Get all available items");
                Log.Info("  POST /api/cycle - Cycle to next item");
                Log.Info("  POST /api/cycle-tier - Cycle to next tier");
                Log.Info("  POST /api/toggle - Toggle enabled/disabled");
                Log.Info("  POST /api/enable - Enable item picker");
                Log.Info("  POST /api/disable - Disable item picker");
                Log.Info("  POST /api/set-item - Set item by name (JSON: {\"itemName\":\"ItemName\"})");
            }
            catch (System.Exception ex)
            {
                Log.Error($"Failed to start HTTP server: {ex.Message}");
            }
        }

        private async void StopWebApi()
        {
            if (_httpServer != null)
            {
                try
                {
                    await _httpServer.StopAsync();
                    _httpServer.Dispose();
                    _httpServer = null;
                    Log.Info("HTTP server stopped successfully");
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Failed to stop HTTP server: {ex.Message}");
                }
            }
        }
    }
}
