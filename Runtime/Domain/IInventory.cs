using System.Collections.Generic;

namespace Kalendra.Inventory.Tests.Editor.Domain
{
    public interface IInventory
    {
        IReadOnlyCollection<ItemPile> Items { get; }
        
        bool HasItem(IInventoryItem item, int minCount = 1);
        void AddItem(IInventoryItem item, int count = 1);
        void RemoveItem(IInventoryItem item, int count = 1);
    }
}