namespace Kalendra.Inventory.Runtime.Domain
{
    public class ItemPile
    {
        public readonly IInventoryItem item;
        public int count;

        public ItemPile(IInventoryItem item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }
}