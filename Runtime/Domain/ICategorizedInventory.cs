using System.Collections.Generic;

namespace Kalendra.Inventory.Tests.Editor.Domain
{
    public interface ICategorizedInventory : IInventory
    {
        bool HasCategory(IInventoryItemCategory category);
        IEnumerable<ItemPile> GetItems(IInventoryItemCategory category);
    }
}