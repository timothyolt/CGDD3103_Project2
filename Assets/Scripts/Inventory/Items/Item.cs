using System;
using Newtonsoft.Json;

namespace Assets.Scripts.Inventory.Items {
    [Serializable]
    public abstract class Item {
        public enum ItemId {
            //None=0,
            Health = 1,
            Health2 = 2,
            Health3 = 3,
            Shot1 = 4,
            Shot2 = 5,
            Shot3 = 6
        }

        protected const string SpriteDir = "Sprites";

        protected const string PrefabDir = "Prefabs";
        public abstract ItemId Id { get; }

        [JsonIgnore]
        public abstract string Name { get; }

        public int Count { get; set; }

        [JsonIgnore]
        public abstract string SpriteString { get; }

        [JsonIgnore]
        public abstract string PrefabString { get; }

        public static Item FromId(ItemId id, int count = 1) {
            switch (id) {
                case ItemId.Health:
                    return new HealSmall {Count = count};
                case ItemId.Health2:
                    return new HealMedium {Count = count};
                case ItemId.Health3:
                    return new HealLarge {Count = count};
                case ItemId.Shot1:
                    return new ShotLesser {Count = count};
                case ItemId.Shot2:
                    return new ShotGreater {Count = count};
                case ItemId.Shot3:
                    return new ShotSpike {Count = count};
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), id, null);
            }
        }

        public virtual Item DeepCopy() {
            return FromId(Id, Count);
        }
    }
}