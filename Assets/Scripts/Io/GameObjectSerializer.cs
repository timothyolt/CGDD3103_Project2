using System.Linq;
using Assets.Scripts.Io.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Io {
    public class GameObjectSerializer : MonoBehaviour
    {
        public static void FromJson(JToken jsonGameObject, GameObject targetGameObject)
        {
            if (targetGameObject == null)
                targetGameObject = Instantiate(Resources.Load<GameObject>(jsonGameObject["prefab"].ToString()));
            foreach (var property in jsonGameObject.Children<JProperty>())
            {
                switch (property.Name)
                {
                    case "children":
                        foreach (var childJsonGameObject in (property.Value as JArray) ?? new JArray())
                            FromJson(childJsonGameObject, targetGameObject.transform?.FindChild(childJsonGameObject["name"].ToString())?.gameObject);
                        break;
                    case "name":
                        targetGameObject.name = property.Value.ToString();
                        break;
                    case "components":
                        ComponentSerializer.CollectionFromJson(property.Value, targetGameObject);
                        break;
                    case "prefab":
                        //Ignore
                        break;
                    default:
                        Debug.LogWarning($"game object property '{property.Name}' does not have a supported deserializer");
                        break;
                }
            }
        }

        public Component[] SerializableComponents;
        public string Prefab;

        public JObject ToJson(JsonSerializer serializer) {
            var components = ComponentSerializer.CollectionToJson(SerializableComponents, serializer);
            var children = new JArray();
            foreach (
                var childSerializer in
                transform.Cast<Transform>()
                    .Select(child => child.GetComponent<GameObjectSerializer>())
                    .Where(childSerializer => childSerializer != null)) {
                children.Add(childSerializer.ToJson(serializer));
            }
            return new JObject {
                {"name", gameObject.name},
                {"prefab", Prefab},
                {"components", components},
                {"children", children}
            };
        }
    }
}