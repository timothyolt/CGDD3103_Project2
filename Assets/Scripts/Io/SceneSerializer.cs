using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Io
{
    public class SceneSerializer : MonoBehaviour
    {
        public static JsonSerializerSettings UnityTypeSerializerSettings => new JsonSerializerSettings
        {
            ContractResolver = new WritableOnlyResolver()
        };

        public static JToken ToJson(JsonSerializer serializer)
        {
            var jArray = new JArray();
            foreach (
                var rootSerializer in
                SceneManager.GetActiveScene()
                    .GetRootGameObjects()
                    .Select(gameObject => gameObject.GetComponent<GameObjectSerializer>())
                    .Where(gameObjectSerializer => gameObjectSerializer != null))
                jArray.Add(rootSerializer.ToJson(serializer));
            return new JObject {{"gameObjects", jArray}};
        }

        public static void FromJson(JToken gameSave)
        {
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var jsonGameObject in gameSave["gameObjects"] as JArray ?? new JArray())
            {
                GameObject first = null;
                foreach (var gameObject1 in rootGameObjects)
                {
                    if (gameObject1.name == jsonGameObject["name"].ToString())
                    {
                        first = gameObject1;
                        break;
                    }
                }
                GameObjectSerializer.FromJson(jsonGameObject, first);
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var timer = new Stopwatch();
                timer.Start();
                var serialized = ToJson(JsonSerializer.Create(UnityTypeSerializerSettings)).ToString();
                timer.Stop();
                Debug.Log($"Json serialization took {timer.ElapsedMilliseconds} ms");

                using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Create))
                using (var file = new StreamWriter(stream))
                    file.Write(serialized);
            }
            if (Input.GetKeyDown(KeyCode.Z)) {
                string serialized;
                using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Open))
                using (var file = new StreamReader(stream))
                    serialized = file.ReadToEnd();

                var timer = new Stopwatch();
                timer.Start();
                FromJson(JObject.Parse(serialized));
                timer.Stop();
                Debug.Log($"Json deserialization took {timer.ElapsedMilliseconds} ms");
            }
        }
    }
}
