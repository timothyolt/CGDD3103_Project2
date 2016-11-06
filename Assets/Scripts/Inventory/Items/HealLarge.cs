namespace Assets.Scripts.Inventory.Items {
    public class HealLarge : Heal {
        public override ItemId Id => ItemId.Health3;
        public override string Name => "Total Health";
        public override string SpriteString => $"{SpriteDir}/{Id}";
        public override string PrefabString => $"{PrefabDir}/{Id}";
        public override int HealAmount => 100;
    }
}