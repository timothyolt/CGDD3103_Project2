using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Io {
    public class UnityJsonSerializer : MonoBehaviour {
        public static JsonSerializerSettings UnityTypeSerializerSettings => new JsonSerializerSettings {
            ContractResolver = new WritableOnlyResolver()
        };

        public Component[] SerializableComponents;

        public static JObject AllToJObject() {
            var jArray = new JArray();
            foreach (var serializable in FindObjectsOfType<UnityJsonSerializer>()) {
                var jObject = serializable.ToJObject();
                //Extract parent instance id from transform component
                var parentInstanceId =
                    jObject.SelectToken($"components.{typeof(Transform).Name}.parentInstanceId")?.ToString();
                JToken parentToken = null;
                //Find already serialized parent
                if (parentInstanceId != null)
                    parentToken =
                        jArray.FirstOrDefault(
                            parentCandidate => //need recursion
                                    string.Equals(parentInstanceId, parentCandidate.SelectToken("instanceId").ToString()));
                //Parent not serialized yet, add to array and move on
                if (parentToken == null)
                    jArray.Add(jObject);
                //Parent found, add to children
                else {
                    var childArrayToken = parentToken.SelectToken("children");
                    if (childArrayToken == null || childArrayToken.Type != JTokenType.Array)
                        parentToken["children"] = new JArray();
                    parentToken.Children<JArray>().First().Add(jObject);
                }
            }
            return new JObject {{"gameObjects", jArray}};
        }

        public JObject ToJObject() {
            var components = new JObject();
            foreach (var component in SerializableComponents) {
                var key = component.GetType().Name;
                var serializer = JsonSerializer.Create(UnityTypeSerializerSettings);
                if (component is ISerializableScript) components[key] = (component as ISerializableScript).ToJson();
                else if (component is Transform) components[key] = ToJObject(component as Transform, serializer);
            }
            return new JObject {
                {"name", gameObject.name},
                {"instanceId", gameObject.GetInstanceID()},
                {"components", components}
            };
        }

        public static JObject ToJObject(Transform transform, JsonSerializer serializer) => new JObject {
            {"position", JObject.FromObject(transform.position, serializer)},
            {"rotation", JObject.FromObject(transform.rotation, serializer)},
            {"scale", JObject.FromObject(transform.position, serializer)},
            {"parentInstanceId", transform.parent.gameObject.GetInstanceID() }
        };

        [UsedImplicitly]
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q))
                AllToJObject();
        }
    }
}