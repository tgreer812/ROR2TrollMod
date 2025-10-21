namespace LightweightAPI
{
    public interface ISimpleItemService
    {
        bool IsEnabled { get; }
        void ToggleEnabled();
        void Enable();
        void Disable();
        void CycleItem();
        void CycleTier();
        string GetCurrentStatus();
        string GetAllItems();
        void SetItemByName(string itemName);
    }
}