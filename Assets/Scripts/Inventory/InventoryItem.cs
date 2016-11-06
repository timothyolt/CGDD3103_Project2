using System;

namespace Assets.Scripts.Inventory {
    [Serializable]
    public class InventoryItem {
        public InventoryItem(Item.Id id, int count) {
            Id = id;
            Count = count;
        }

        public InventoryItem(InventoryItem copy) {
            Id = copy.Id;
            Count = copy.Count;
        }

        public Item.Id Id { get; set; }

        public int Count { get; set; }
    }
}