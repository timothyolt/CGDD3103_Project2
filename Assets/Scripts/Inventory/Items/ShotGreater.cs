namespace Assets.Scripts.Inventory.Items {
    public class ShotGreater : Shot {
        public override ItemId Id => ItemId.Shot2;
        public override string Name => "Greater Shot";
        public override string SpriteString => $"{SpriteDir}/{Id}";
        public override string PrefabString => $"{PrefabDir}/{Id}";
        public override float ShotForce => 1000f;
    }
}