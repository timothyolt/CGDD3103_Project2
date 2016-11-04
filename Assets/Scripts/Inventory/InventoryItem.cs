namespace Assets.Scripts.Inventory
{
    public class InventoryItem
    {
        public InventoryItem(Item.Id item, int count)
        {
            Item = item;
            Count = count;
        }

        public InventoryItem(InventoryItem copy)
        {
            Item = copy.Item;
            Count = copy.Count;
        }

        public Item.Id Item { get; set; }

        public int Count { get; set; }
    }
}