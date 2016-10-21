using UnityEngine;

namespace Assets
{
    public static class Item
    {

        public enum Id
        {
            None=0,
            Health=1,
            Health2=2,
            Health3=3,
        }

        public static string GetSpriteResource(Id item)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (item)
            {
                case Id.Health:
                    return "Sprites/Health";
                case Id.Health2:
                    return "Sprites/Health2";
                case Id.Health3:
                    return "Sprites/Health3";
                default:
                    return null;
            }
        }

        public static string GetPrefabResource(Id item)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (item)
            {
                case Id.Health:
                    return "Prefabs/Health";
                case Id.Health2:
                    return "Prefabs/Health2";
                case Id.Health3:
                    return "Prefabs/Health3";
                default:
                    return null;
            }
        }
    }
}