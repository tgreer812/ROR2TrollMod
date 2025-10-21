using LightweightAPI;

namespace ExamplePlugin
{
    public class RoR2ItemService : LightweightAPI.ISimpleItemService
    {
        public bool IsEnabled => ItemController.IsEnabled;

        public void ToggleEnabled() => ItemController.ToggleEnabled();

        public void Enable() => ItemController.Enable();

        public void Disable() => ItemController.Disable();

        public void CycleItem() => ItemController.CycleItem();

        public void CycleTier() => ItemController.CycleTier();

        public string GetCurrentStatus() => ItemController.GetCurrentStatus();

        public string GetAllItems() => ItemController.GetAllItems();

        public void SetItemByName(string itemName) => ItemController.SetItemByName(itemName);
    }
}
