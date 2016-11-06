namespace Assets.Scripts.Inventory.Items {
    public class HealSmall : Heal {
        public override ItemId Id => ItemId.Health;
        public override string Name => "Health";
        public override string SpriteString => $"{SpriteDir}/{Id}";
        public override string PrefabString => $"{PrefabDir}/{Id}";
        public override int HealAmount => 25;
    }
}