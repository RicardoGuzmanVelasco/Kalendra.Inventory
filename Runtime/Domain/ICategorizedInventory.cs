using System.Collections.Generic;

namespace Kalendra.Inventory.Runtime.Domain
{
    public interface ICategorizedInventory : IInventory
    {
        bool HasCategory(IInventoryItemCategory category);
        IEnumerable<ItemPile> GetItems(IInventoryItemCategory category);
        IEnumerable<IInventoryItemCategory> Categories { get; }
    }
}