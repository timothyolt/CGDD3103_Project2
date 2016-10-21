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

        public static string GetResourceString(Id item)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (item)
            {
                case Id.Health:
                    return "Sprites/health";
                default:
                    return null;
            }
        }
    }
}