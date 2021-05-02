using System;
using System.Collections.Generic;
using System.Linq;

namespace Kalendra.Inventory.Tests.Editor.Domain
{
    public class GeneralistInventory
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
        public bool HasItem(IInventoryItem item) => itemPiles.Any(pile => pile.item == item);

        public void AddItem(IInventoryItem item) => AddItem(item, 1);
        public void AddItem(IInventoryItem item, int count)
        {
            switch(count)
            {
                case < 0:
                    throw new InvalidOperationException("Unable to add negative items");
                case 0:
                    return;
            }

            if(!HasItem(item))
                itemPiles.Add(new ItemPile(item, 0));

            var itemPile = itemPiles.First(pile => pile.item == item);
            itemPile.count += count;
        }

        public int GetItemCount(IInventoryItem item) => itemPiles.Where(i => i.item == item).Select(i => i.count).Sum();

        public void RemoveItem(IInventoryItem item, int count)
        {
            if(count < 0)
                throw new InvalidOperationException("Unable to remove negative items");
            
            var itemPile = itemPiles.First(pile => pile.item == item);
            itemPile.count -= count;

            CleanEmptyPiles();
        }
        #endregion

        #region Support methods
        void CleanEmptyPiles()
        {
            itemPiles = itemPiles.Where(pile => pile.count > 0).ToList();
        }
        #endregion
    }
}