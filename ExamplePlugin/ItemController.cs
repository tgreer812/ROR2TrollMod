using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RoR2;

namespace ExamplePlugin
{
    public static class ItemController
    {
        private static int selectedIndex = 0;
        private static List<ItemData> availableItems = new List<ItemData>();
        private static ItemTier? currentTierFilter = null;
        
        public static ItemData CurrentlySelectedItem
        {
            get 
            { 
                if (availableItems.Count == 0) return null;
                return availableItems[selectedIndex]; 
            }
        }

        public static string currentlySelectedItemName
        {
            get 
            { 
                var item = CurrentlySelectedItem;
                return item != null ? $"ItemIndex.{item.Name}" : "ItemIndex.Scrap";
            }
        }

        public static PickupIndex CurrentPickupIndex
        {
            get
            {
                var item = CurrentlySelectedItem;
                return item?.PickupIndex ?? PickupIndex.none;
            }
        }

        private static bool enabled = false;

        public static bool IsEnabled { get { return enabled; } }

        public static void ToggleEnabled() => enabled = !enabled;

        public static void Enable() => enabled = true;

        public static void Disable() => enabled = false;

        public static void Initialize()
        {
            RefreshAvailableItems();
            Log.Info($"ItemController initialized with {availableItems.Count} items");
        }

        public static void RefreshAvailableItems()
        {
            if (!ItemCatalogManager.IsInitialized)
            {
                Log.Warning("ItemCatalogManager not initialized, using empty item list");
                availableItems.Clear();
                return;
            }

            if (currentTierFilter.HasValue)
            {
                availableItems = ItemCatalogManager.GetItemsByTier(currentTierFilter.Value).ToList();
                Log.Info($"Filtered to {availableItems.Count} {ItemCatalogManager.GetTierName(currentTierFilter.Value)} items");
            }
            else
            {
                availableItems = ItemCatalogManager.AvailableItems.ToList();
                Log.Info($"Using all {availableItems.Count} available items");
            }

            // Reset index if it's out of bounds
            if (selectedIndex >= availableItems.Count)
            {
                selectedIndex = 0;
            }
        }

        public static void CycleItem()
        {
            if (availableItems.Count == 0)
            {
                Log.Warning("No items available to cycle through");
                return;
            }
            selectedIndex = (selectedIndex + 1) % availableItems.Count;
        }

        public static void CycleTier()
        {
            var tiers = new ItemTier?[] 
            { 
                null, // All items
                ItemTier.Tier1, 
                ItemTier.Tier2, 
                ItemTier.Tier3, 
                ItemTier.Boss, 
                ItemTier.Lunar 
            };

            var currentIndex = Array.IndexOf(tiers, currentTierFilter);
            var nextIndex = (currentIndex + 1) % tiers.Length;
            currentTierFilter = tiers[nextIndex];

            RefreshAvailableItems();

            var tierName = currentTierFilter.HasValue 
                ? ItemCatalogManager.GetTierName(currentTierFilter.Value)
                : "All Tiers";
            Log.Info($"Switched to tier: {tierName} ({availableItems.Count} items)");
        }

        public static void SetItemByName(string itemName)
        {
            var item = ItemCatalogManager.FindItemByName(itemName);
            if (item != null && availableItems.Contains(item))
            {
                selectedIndex = availableItems.IndexOf(item);
                Log.Info($"Selected item: {item}");
            }
            else
            {
                Log.Warning($"Item '{itemName}' not found or not available in current filter");
            }
        }

        public static string GetAllItems()
        {
            if (availableItems.Count == 0)
                return "{\"items\":[],\"count\":0,\"message\":\"No items available\"}";

            var itemsJson = new StringBuilder();
            itemsJson.Append("{\"items\":[");
            
            for (int i = 0; i < availableItems.Count; i++)
            {
                var item = availableItems[i];
                if (i > 0) itemsJson.Append(",");
                
                itemsJson.Append("{");
                itemsJson.Append($"\"index\":{i},");
                itemsJson.Append($"\"name\":\"{EscapeJsonString(item.Name)}\",");
                itemsJson.Append($"\"displayName\":\"{EscapeJsonString(item.DisplayName)}\",");
                itemsJson.Append($"\"description\":\"{EscapeJsonString(item.Description)}\",");
                itemsJson.Append($"\"tier\":\"{item.Tier}\",");
                itemsJson.Append($"\"tierName\":\"{EscapeJsonString(item.TierName)}\",");
                itemsJson.Append($"\"isSelected\":{(i == selectedIndex).ToString().ToLower()}");
                itemsJson.Append("}");
            }
            
            itemsJson.Append("],");
            itemsJson.Append($"\"count\":{availableItems.Count},");
            itemsJson.Append($"\"selectedIndex\":{selectedIndex},");
            itemsJson.Append($"\"currentTier\":\"{(currentTierFilter?.ToString() ?? "All")}\",");
            itemsJson.Append($"\"enabled\":{enabled.ToString().ToLower()}");
            itemsJson.Append("}");
            
            return itemsJson.ToString();
        }

        private static string EscapeJsonString(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return str.Replace("\\", "\\\\")
                     .Replace("\"", "\\\"")
                     .Replace("\n", "\\n")
                     .Replace("\r", "\\r")
                     .Replace("\t", "\\t");
        }

        public static List<ItemData> SearchItems(string searchTerm)
        {
            return ItemCatalogManager.SearchItems(searchTerm)
                .Where(item => availableItems.Contains(item))
                .ToList();
        }

        public static string GetCurrentStatus()
        {
            if (availableItems.Count == 0)
                return "No items available";

            var item = CurrentlySelectedItem;
            var tierName = currentTierFilter.HasValue 
                ? ItemCatalogManager.GetTierName(currentTierFilter.Value)
                : "All Tiers";

            return $"[{selectedIndex + 1}/{availableItems.Count}] {item?.DisplayName} ({tierName}) - Enabled: {enabled}";
        }
    }
}
