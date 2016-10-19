using UnityEngine;

namespace Assets
{
    public struct Item
    {
        public int Id { get; private set; }
        public string UiSprite { get; private set; }
        public override bool Equals(object obj)
        {
            if (obj is Item)
                return ((Item) obj).Id == Id;
            return base.Equals(obj);
        }

        public bool Equals(Item other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(Item item1, Item item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Item item1, Item item2)
        {
            return !item1.Equals(item2);
        }

        public static Item FromId(int id)
        {
            switch (id)
            {
                case 1:
                    return Health;
                default:
                    return None;
            }
        }

        //public static Item None { get { return (Item) (_none ?? (_none = new Item() { Id = 0, UiSprite = null }));} }
        //private static Item? _none = null;
        //public static Item Health { get { return (Item)(_health ?? (_health = new Item() { Id = 1, UiSprite = Resources.Load<Sprite>("Sprites/health") })); } }
        //private static Item? _health = null;
        public static Item None = new Item() {Id = 0, UiSprite = null};
        public static Item Health = new Item() {Id = 1, UiSprite = "Sprites/health"};
    }
}