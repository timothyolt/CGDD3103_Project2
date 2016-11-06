namespace Assets.Scripts.Inventory.Items {
    public class ShotLesser : Shot {
        public override ItemId Id => ItemId.Shot1;
        public override string Name => "Lesser Shot";
        public override string SpriteString => $"{SpriteDir}/{Id}";
        public override string PrefabString => $"{PrefabDir}/{Id}";
        public override float ShotForce => 500f;
    }
}