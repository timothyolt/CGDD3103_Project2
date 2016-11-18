using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
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
*/

namespace Assets.Scripts.Io {
    public class UnityJsonSerializer : MonoBehaviour {
        public static JsonSerializerSettings UnityTypeSerializerSettings => new JsonSerializerSettings {
            ContractResolver = new WritableOnlyResolver()
        };

        public static JObject SceneToJObject() {
            var jArray = new JArray();
            var jsonSerializer = JsonSerializer.Create(UnityTypeSerializerSettings);
            foreach (
                var rootSerializer in
                SceneManager.GetActiveScene()
                    .GetRootGameObjects()
                    .Select(root => root.GetComponent<UnityJsonSerializer>())
                    .Where(serializer => serializer != null)) {
                jArray.Add(rootSerializer.ToJObject(jsonSerializer));
            }
            return new JObject {{"gameObjects", jArray}};
        }

        public Component[] SerializableComponents;

        public JObject ToJObject(JsonSerializer serializer) {
            var components = new JObject();
            foreach (var component in SerializableComponents) {
                var key = component.GetType().Name;
                if (component is ISerializableScript) components[key] = (component as ISerializableScript).ToJson();
                else if (component is Transform) components[key] = ToJObject(component as Transform, serializer);
            }
            var children = new JArray();
            foreach (
                var childSerializer in
                transform.Cast<Transform>()
                    .Select(child => child.GetComponent<UnityJsonSerializer>())
                    .Where(childSerializer => childSerializer != null)) {
                children.Add(childSerializer.ToJObject(serializer));
            }
            return new JObject {
                {"name", gameObject.name},
                {"instanceId", gameObject.GetInstanceID()},
                {"components", components},
                {"children", children}
            };
        }

        public static JObject ToJObject(Transform transform, JsonSerializer serializer) => new JObject {
            {"position", JObject.FromObject(transform.position, serializer)},
            {"rotation", JObject.FromObject(transform.rotation.eulerAngles, serializer)},
            {"scale", JObject.FromObject(transform.position, serializer)}//,
            //{"parentInstanceId", transform.parent.gameObject.GetInstanceID() }
        };

        [UsedImplicitly]
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Create))
                using (var file = new StreamWriter(stream)) {
                    file.Write(SceneToJObject().ToString());
                }
            }
        }
    }
}