using System.Collections.Generic;

namespace Kalendra.Inventory.Runtime.Domain
{
    public interface ICategorizedInventory : IInventory
    {
        IEnumerable<IInventoryItemCategory> Categories { get; }
        
        bool HasCategory(IInventoryItemCategory category);
        IEnumerable<ItemPile> GetItems(IInventoryItemCategory category);
    }
}