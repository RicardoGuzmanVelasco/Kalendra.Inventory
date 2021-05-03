using System;

namespace Kalendra.Inventory.Runtime.Domain
{
    public interface IWeightedInventory : IInventory
    {
        int MaxWeight { get; }
        int CurrentWeight { get; }
        event Action OnOverweight;
        bool CanBearItem(IInventoryItem item, int count = 1);
    }
}