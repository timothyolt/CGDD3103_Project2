using Newtonsoft.Json;

namespace Assets.Scripts.Inventory.Items {
    public abstract class Heal : Item {
        [JsonIgnore]
        public abstract int HealAmount { get; }
    }
}