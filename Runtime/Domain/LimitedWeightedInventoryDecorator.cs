using System;
using System.Collections.Generic;

namespace Kalendra.Inventory.Runtime.Domain
{
    /// <summary>
    /// Doesn't add any item whose weight surpasses available weight in decorated inventory.
    /// </summary>
    public class LimitedWeightedInventoryDecorator : IWeightedInventory
    {
        readonly IWeightedInventory decorated;

        #region Constructors
        public LimitedWeightedInventoryDecorator(IWeightedInventory decorated) => this.decorated = decorated;
        #endregion
        
        void AddItemIfDecoratedCanBear(IInventoryItem item, int count)
        {
            if(CanBearItem(item))
                decorated.AddItem(item, count);
        }
        
        #region IInventory implementation (delegated)
        public IEnumerable<ItemPile> Items => decorated.Items;

        public bool HasItem(IInventoryItem item, int minCount = 1)
        {
            return decorated.HasItem(item, minCount);
        }

        public void AddItem(IInventoryItem item, int count = 1)
        {
            AddItemIfDecoratedCanBear(item, count);
        }

        public void RemoveItem(IInventoryItem item, int count = 1)
        {
            decorated.RemoveItem(item, count);
        }
        #endregion

        #region IWeightedInventory implementation (delegated)
        public int MaxWeight => decorated.MaxWeight;
        public int CurrentWeight => decorated.CurrentWeight;

        public event Action OnOverweight
        {
            add => decorated.OnOverweight += value;
            remove => decorated.OnOverweight -= value;
        }

        public bool CanBearItem(IInventoryItem item, int count = 1) => decorated.CanBearItem(item, count);
        #endregion
    }
}