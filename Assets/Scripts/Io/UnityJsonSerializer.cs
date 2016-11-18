using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

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

        public static void JObjectToScene(JObject gameSave) {
            var rootGameObjects = SceneManager.GetActiveScene()
                .GetRootGameObjects();
            foreach (var jsonGameObject in (gameSave["gameObjects"] as JArray) ?? new JArray()) {
                JTokenToGameObject(jsonGameObject, rootGameObjects.FirstOrDefault(gameObject => gameObject.name == jsonGameObject["name"].ToString()));
            }
        }

        private static void JTokenToGameObject(JToken jsonGameObject, GameObject targetGameObject) {
            if (targetGameObject == null) 
                targetGameObject = Instantiate(Resources.Load<GameObject>(jsonGameObject["prefab"].ToString()));
            foreach (var property in jsonGameObject.Children<JProperty>()) {
                switch (property.Name) {
                    case "name":
                        targetGameObject.name = property.Value.ToString();
                        break;
                    case "components":
                        JTokenToComponents(property.Value, targetGameObject);
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

        private static void JTokenToComponents(JToken jsonComponents, GameObject targetGameObject) {
            foreach (var jComponent in jsonComponents.Children<JProperty>())
            {
                if (jComponent.Name == typeof(Transform).Name) TransformDeserialize(jComponent.Value, targetGameObject.GetComponent<Transform>());
                else if (jComponent.Name[0] == '@') {
                    ISerializableScript serializableScript = null;
                    try {
                        var type = Type.GetType(jComponent.Name.Substring(1));
                        if (type == null)
                        {
                            Debug.LogError($"Type '{jComponent.Name}' not available");
                            continue;
                        }
                        serializableScript = Activator.CreateInstance(type) as ISerializableScript;
                    }
                    catch (Exception e) {
                        Debug.LogError($"{jComponent.Name} reflection error");
                        Debug.LogException(e);
                    }
                    if (serializableScript == null)
                    {
                        Debug.LogError($"Script '{jComponent.Name}' does not implement ISerializableScript");
                        continue;
                    }
                    try {
                        serializableScript.FromJson(jComponent.Value);
                    }
                    catch (Exception e) {
                        Debug.LogError($"Script '{jComponent.Name}' threw '{e.GetType()}' during deserialization");
                        Debug.LogException(e);
                    }
                }
            }
        }

        public Component[] SerializableComponents;
        public string Prefab;

        public JObject ToJObject(JsonSerializer serializer) {
            var components = new JObject();
            foreach (var component in SerializableComponents) {
                var key = component.GetType().Name;
                if (component is ISerializableScript) components['@' + component.GetType().FullName] = (component as ISerializableScript).ToJson();
                else if (component is Transform) components[key] = TransformSerialize(component as Transform, serializer);
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
                {"prefab", Prefab},
                {"components", components},
                {"children", children}
            };
        }

        public static JObject TransformSerialize(Transform transform, JsonSerializer serializer) => new JObject {
            {"position", JObject.FromObject(transform.localPosition, serializer)},
            {"rotation", JObject.FromObject(transform.localRotation.eulerAngles, serializer)},
            {"scale", JObject.FromObject(transform.localScale, serializer)}
        };

        public static void TransformDeserialize(JToken jTransform, Transform targetTransform) {
            if (jTransform == null || targetTransform == null) return;
            targetTransform.localPosition = JsonConvert.DeserializeObject<Vector3>(jTransform["position"].ToString());
            targetTransform.localRotation = new Quaternion {eulerAngles = JsonConvert.DeserializeObject<Vector3>(jTransform["rotation"].ToString())};
            targetTransform.localScale = JsonConvert.DeserializeObject<Vector3>(jTransform["scale"].ToString());
        }

        [UsedImplicitly]
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var timer = new Stopwatch();
                timer.Start();
                using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Create))
                using (var file = new StreamWriter(stream)) {
                    file.Write(SceneToJObject().ToString());
                }
                timer.Stop();
                Debug.Log($"Json serialization took {timer.ElapsedMilliseconds} ms");
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                var timer = new Stopwatch();
                timer.Start();
                using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Open))
                using (var file = new StreamReader(stream))
                {
                    JObjectToScene(JObject.Parse(file.ReadToEnd()));
                }
                timer.Stop();
                Debug.Log($"Json deserialization took {timer.ElapsedMilliseconds} ms");
            }
        }
    }
}