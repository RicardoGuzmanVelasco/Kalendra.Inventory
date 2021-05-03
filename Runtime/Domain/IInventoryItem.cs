namespace Kalendra.Inventory.Runtime.Domain
{
    public interface IInventoryItem
    {
        IInventoryItemCategory Category { get; }
        int Weight { get; }
    }

    public interface IInventoryItemCategory
    {
        
    }
}