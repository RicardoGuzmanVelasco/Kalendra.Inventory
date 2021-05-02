using System;
using System.Collections.Generic;
using System.Linq;

namespace Kalendra.Inventory.Tests.Editor.Domain
{
    public class GeneralistInventory : IInventory
    {
        List<ItemPile> itemPiles = new List<ItemPile>();

        #region Constructors
        public GeneralistInventory() { }

        public GeneralistInventory(IEnumerable<ItemPile> piles)
        {
            foreach(var pile in piles)
                itemPiles.Add(pile);
        }
        public GeneralistInventory(IEnumerable<IInventoryItem> items)
        {
            foreach(var item in items)
                AddItem(item);
        }
        #endregion

        public IReadOnlyCollection<ItemPile> Items => itemPiles;

        #region IInventory implementation
        public bool HasItem(IInventoryItem item, int minCount = 1) => GetItemCount(item) >= minCount;

        public void AddItem(IInventoryItem item, int count = 1)
        {
            if(count < 0)
                throw new InvalidOperationException("Unable to add negative items");
            if(count == 0)
                return;

            if(!HasItem(item))
                itemPiles.Add(new ItemPile(item, 0));

            var itemPile = GetPiles(item).First();
            itemPile.count += count;
        }

        public void RemoveItem(IInventoryItem item, int count = 1)
        {
            if(count < 0)
                throw new InvalidOperationException("Unable to remove negative items");

            if(GetItemCount(item) < count)
                throw new InvalidOperationException($"Trying to remove {count} items but just found {GetItemCount(item)}");
            
            RemoveItemFromPilesUntilCount(item, count);
        }
        #endregion

        #region Support methods
        IEnumerable<ItemPile> GetPiles(IInventoryItem item) => itemPiles.Where(pile => pile.item == item);
        
        int GetItemCount(IInventoryItem item) => GetPiles(item).Sum(p => p.count);
        
        void CleanEmptyPiles() => itemPiles = itemPiles.Where(pile => pile.count > 0).ToList();
        
        void RemoveItemFromPilesUntilCount(IInventoryItem item, int count)
        {
            if(count <= 0)
                return;
            
            var pileToRemoveFrom = GetPiles(item).First();
            var countToRemove = Math.Min(count, pileToRemoveFrom.count);

            pileToRemoveFrom.count -= countToRemove;
            CleanEmptyPiles();
            
            RemoveItemFromPilesUntilCount(item, count - countToRemove);
        }
        #endregion
    }
}