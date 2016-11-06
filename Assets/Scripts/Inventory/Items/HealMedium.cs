namespace Assets.Scripts.Inventory.Items {
    public class HealMedium : Heal {
        public override ItemId Id => ItemId.Health2;
        public override string Name => "Super Health";
        public override string SpriteString => $"{SpriteDir}/{Id}";
        public override string PrefabString => $"{PrefabDir}/{Id}";
        public override int HealAmount => 50;
    }
}