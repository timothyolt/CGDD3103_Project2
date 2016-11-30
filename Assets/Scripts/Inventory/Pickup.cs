using System;
using Assets.Scripts.Inventory.Items;
using Assets.Scripts.Io;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Inventory {
    public class Pickup : MonoBehaviour, ISerializableScript {
        public Item.ItemId Item;

        public JToken ToJson(JsonSerializer serializer) => new JObject(new JProperty("item", (int) Item));

        public void FromJson(JToken token)
        {
            var item = token.First as JProperty;
            if (item == null)
                Debug.LogError("Serialized item was not a property");
            else if (item.Name != "item")
                Debug.LogError("Item property missing");
            else
                Item = (Item.ItemId) Enum.Parse(typeof(Item.ItemId), item.Value.ToString());
        }
    }
}