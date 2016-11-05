namespace Assets.Scripts.Inventory {
    public static class Item {
        public enum Id {
            //None=0,
            Health = 1,
            Health2 = 2,
            Health3 = 3,
            Shot1 = 4,
            Shot2 = 5,
            Shot3 = 6
        }

        private const string PrefabDir = "Prefabs";

        public static string GetSpriteResource(Id item) {
            switch (item) {
                case Id.Health:
                    return "Sprites/Health";
                case Id.Health2:
                    return "Sprites/Health2";
                case Id.Health3:
                    return "Sprites/Health3";
                case Id.Shot1:
                    return "Sprites/Shot1";
                case Id.Shot2:
                    return "Sprites/Shot2";
                case Id.Shot3:
                    return "Sprites/Shot3";
                default:
                    return null;
            }
        }

        public static string GetPrefabResource(Id item) {
            switch (item) {
                case Id.Health:
                    return $"{PrefabDir}/{Id.Health}";
                case Id.Health2:
                    return $"{PrefabDir}/{Id.Health2}";
                case Id.Health3:
                    return $"{PrefabDir}/{Id.Health3}";
                case Id.Shot1:
                    return $"{PrefabDir}/{Id.Shot1}";
                case Id.Shot2:
                    return $"{PrefabDir}/{Id.Shot2}";
                case Id.Shot3:
                    return $"{PrefabDir}/{Id.Shot3}";
                default:
                    return null;
            }
        }

        public static string GetName(Id item) {
            switch (item) {
                case Id.Health:
                    return "Health";
                case Id.Health2:
                    return "Super Health";
                case Id.Health3:
                    return "Total Health";
                case Id.Shot1:
                    return "Light Shot";
                case Id.Shot2:
                    return "Heavy Shot";
                case Id.Shot3:
                    return "Spike Shot";
                default:
                    return null;
            }
        }
    }
}