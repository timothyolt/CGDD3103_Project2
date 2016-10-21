using UnityEngine;

namespace Assets
{
    public static class Item
    {

        public enum Id
        {
            None=0,
            Health=1,
        }

        public static string GetSpriteResource(Id item)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (item)
            {
                case Id.Health:
                    return "Sprites/Health";
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
                default:
                    return null;
            }
        }
    }
}