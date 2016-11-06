namespace Assets.Scripts.Inventory.Items {
    public class ShotSpike : Shot {
        public override ItemId Id => ItemId.Shot3;
        public override string Name => "Spike Shot";
        public override string SpriteString => $"{SpriteDir}/{Id}";
        public override string PrefabString => $"{PrefabDir}/{Id}";
        public override float ShotForce => 1500f;
    }
}