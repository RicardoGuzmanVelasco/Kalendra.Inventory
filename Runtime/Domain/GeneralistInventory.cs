using System;
using System.Collections.Generic;
using System.Linq;

namespace Kalendra.Inventory.Runtime.Domain
{
    public class GeneralistInventory : IInventory, ICategorizedInventory, IWeightedInventory
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
        
        public GeneralistInventory(int maxWeight)
        {
            IncreaseMaxWeight(maxWeight);
        }
        #endregion

        #region IInventory implementation
        public IEnumerable<ItemPile> Items => itemPiles;
        
        public bool HasItem(IInventoryItem item, int minCount = 1) => GetItemCount(item) >= minCount;

        public void AddItem(IInventoryItem item, int count = 1)
        {
            if(count < 0)
                throw new InvalidOperationException("Unable to add negative items");
            if(count == 0)
                return;

            AddItemToPiles(item, count);
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
        
        #region ICategorizedInventory implementation
        public IEnumerable<IInventoryItemCategory> Categories =>
            itemPiles.Select(pile => pile.item.Category).Distinct();
        
        public bool HasCategory(IInventoryItemCategory category) =>
            itemPiles.Any(pile => pile.item.Category == category);
        
        public IEnumerable<ItemPile> GetItems(IInventoryItemCategory category) =>
            itemPiles.Where(pile => pile.item.Category == category);
        #endregion
        
        #region IWeightedInventory implementation
        public int MaxWeight { get; private set; }
        public int CurrentWeight => itemPiles.Sum(p => p.item.Weight * p.count);

        public void IncreaseMaxWeight(int deltaWeight)
        {
            AssertPositiveMaxWeight(deltaWeight);
            MaxWeight += deltaWeight;
        }
        
        public event Action OnOverweight;
        
        public bool CanBearItem(IInventoryItem item, int count = 1) => MaxWeight > 0 && MaxWeight - CurrentWeight >= item.Weight * count;
        #endregion

        #region Support methods
        IEnumerable<ItemPile> GetPiles(IInventoryItem item) => itemPiles.Where(pile => pile.item == item);
        
        int GetItemCount(IInventoryItem item) => GetPiles(item).Sum(p => p.count);
        
        void CleanEmptyPiles() => itemPiles = itemPiles.Where(pile => pile.count > 0).ToList();
        
        void AddItemToPiles(IInventoryItem item, int count)
        {
            CheckIfNotifyOverweight(item, count);

            if(!HasItem(item))
                itemPiles.Add(new ItemPile(item, 0));

            var itemPile = GetPiles(item).First();
            itemPile.count += count;
        }

        void CheckIfNotifyOverweight(IInventoryItem item, int count)
        {
            //Already was overweight.
            if(CurrentWeight > MaxWeight)
                return;
            
            if(!CanBearItem(item, count))
                OnOverweight?.Invoke();
        }

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
        
        void AssertPositiveMaxWeight(int weight)
        {
            if(weight + CurrentWeight <= 0)
                throw new ArgumentOutOfRangeException(nameof(weight), "Max weight must be positive");
        }
        #endregion
    }
}