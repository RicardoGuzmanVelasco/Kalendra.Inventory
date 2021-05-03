using System.Collections.Generic;

namespace Kalendra.Inventory.Runtime.Domain
{
    public interface IInventory
    {
        IEnumerable<ItemPile> Items { get; }
        
        bool HasItem(IInventoryItem item, int minCount = 1);
        void AddItem(IInventoryItem item, int count = 1);
        void RemoveItem(IInventoryItem item, int count = 1);
    }
}