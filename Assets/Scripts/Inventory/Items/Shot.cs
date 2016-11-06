using Newtonsoft.Json;

namespace Assets.Scripts.Inventory.Items {
    public abstract class Shot : Item {
        [JsonIgnore]
        public abstract float ShotForce { get; }
    }
}