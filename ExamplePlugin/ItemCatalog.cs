using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamplePlugin
{
    public class ItemData
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public ItemTier Tier { get; set; }
        public string TierName { get; set; }
        public ItemIndex Index { get; set; }
        public PickupIndex PickupIndex { get; set; }
        public bool IsHidden { get; set; }
        public bool IsAvailable { get; set; }
        public string[] Tags { get; set; }
        
        public override string ToString()
        {
            return $"{DisplayName} ({TierName}) - {Name}";
        }
    }

    public static class ItemCatalogManager
    {
        private static List<ItemData> _allItems = new List<ItemData>();
        private static bool _isInitialized = false;

        public static IReadOnlyList<ItemData> AllItems => _allItems.AsReadOnly();
        
        public static IReadOnlyList<ItemData> AvailableItems => 
            _allItems.Where(item => item.IsAvailable && !item.IsHidden).ToList().AsReadOnly();

        public static IReadOnlyList<ItemData> GetItemsByTier(ItemTier tier) =>
            AvailableItems.Where(item => item.Tier == tier).ToList().AsReadOnly();

        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// Initialize the item catalog. Call this after the game's ItemCatalog is ready.
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized)
            {
                Log.Warning("ItemCatalogManager already initialized!");
                return;
            }

            try
            {
                _allItems.Clear();
                
                // Wait for RoR2's ItemCatalog to be ready
                if (ItemCatalog.itemCount == 0)
                {
                    Log.Warning("RoR2 ItemCatalog not ready yet, deferring initialization");
                    return;
                }

                Log.Info($"Initializing ItemCatalogManager with {ItemCatalog.itemCount} items from RoR2");

                // Iterate through all items in the game's catalog
                for (int i = 0; i < ItemCatalog.itemCount; i++)
                {
                    var itemIndex = (ItemIndex)i;
                    var itemDef = ItemCatalog.GetItemDef(itemIndex);
                    
                    if (itemDef == null) continue;

                    var itemData = new ItemData
                    {
                        Name = itemDef.name,
                        DisplayName = Language.GetString(itemDef.nameToken),
                        Description = Language.GetString(itemDef.descriptionToken),
                        Tier = itemDef.tier,
                        TierName = GetTierName(itemDef.tier),
                        Index = itemIndex,
                        PickupIndex = PickupCatalog.FindPickupIndex(itemIndex),
                        IsHidden = itemDef.hidden,
                        IsAvailable = itemDef.canRemove, // Items that can be removed are generally "real" items
                        Tags = itemDef.tags?.Select(tag => tag.ToString()).ToArray() ?? new string[0]
                    };

                    _allItems.Add(itemData);
                }

                _isInitialized = true;
                Log.Info($"ItemCatalogManager initialized with {_allItems.Count} total items ({AvailableItems.Count} available)");
                
                // Log summary by tier
                foreach (ItemTier tier in Enum.GetValues(typeof(ItemTier)))
                {
                    var tierItems = GetItemsByTier(tier);
                    if (tierItems.Count > 0)
                    {
                        Log.Info($"  {GetTierName(tier)}: {tierItems.Count} items");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to initialize ItemCatalogManager: {ex}");
            }
        }

        /// <summary>
        /// Get a human-readable name for an item tier
        /// </summary>
        public static string GetTierName(ItemTier tier)
        {
            return tier switch
            {
                ItemTier.Tier1 => "Common (White)",
                ItemTier.Tier2 => "Uncommon (Green)", 
                ItemTier.Tier3 => "Legendary (Red)",
                ItemTier.Lunar => "Lunar (Blue)",
                ItemTier.Boss => "Boss (Yellow)",
                ItemTier.VoidTier1 => "Void (White)",
                ItemTier.VoidTier2 => "Void (Green)",
                ItemTier.VoidTier3 => "Void (Red)",
                ItemTier.VoidBoss => "Void Boss",
                _ => tier.ToString()
            };
        }

        /// <summary>
        /// Find an item by its name (case insensitive)
        /// </summary>
        public static ItemData FindItemByName(string name)
        {
            return AvailableItems.FirstOrDefault(item => 
                string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(item.DisplayName, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get items matching a search term (searches name, display name, and description)
        /// </summary>
        public static List<ItemData> SearchItems(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return new List<ItemData>();

            var term = searchTerm.ToLower();
            return AvailableItems.Where(item =>
                item.Name.ToLower().Contains(term) ||
                item.DisplayName.ToLower().Contains(term) ||
                item.Description.ToLower().Contains(term)
            ).ToList();
        }

        /// <summary>
        /// Force refresh the catalog (useful if items are added/removed dynamically)
        /// </summary>
        public static void Refresh()
        {
            _isInitialized = false;
            Initialize();
        }

        /// <summary>
        /// Get items as a simple list of names for backward compatibility
        /// </summary>
        public static List<string> GetItemNames(ItemTier? tier = null)
        {
            var items = tier.HasValue ? GetItemsByTier(tier.Value) : AvailableItems;
            return items.Select(item => $"ItemIndex.{item.Name}").ToList();
        }
    }
}
