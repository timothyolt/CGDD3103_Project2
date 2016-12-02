using System.Collections.Generic;
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

        public static JToken ToJson(JsonSerializer serializer, bool ensureParents = false)
        {
            var jArray = new JArray();
            if (ensureParents)
                foreach (var gameObjectSerializer in FindObjectsOfType<GameObjectSerializer>())
                    gameObjectSerializer.EnsureParentSerializer();
            foreach (
                var rootSerializer in
                SceneManager.GetActiveScene()
                    .GetRootGameObjects()
                    .Select(gameObject => gameObject.GetComponent<GameObjectSerializer>())
                    .Where(gameObjectSerializer => gameObjectSerializer != null))
                jArray.Add(rootSerializer.ToJson(serializer));
            return new JObject {{"gameObjects", jArray}};
        }

        public static void FromJson(JToken gameSave, bool ensureParents = false)
        {
            if (ensureParents)
                foreach (var gameObjectSerializer in FindObjectsOfType<GameObjectSerializer>())
                    gameObjectSerializer.EnsureParentSerializer();
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var jsonGameObject in gameSave["gameObjects"] as JArray ?? new JArray())
                GameObjectSerializer.FromJson(jsonGameObject, rootGameObjects.FirstOrDefault(gameObject => gameObject.name == jsonGameObject["name"].ToString()));
        }

        private bool _logSaveMetrics;
        private static readonly List<long> Saves = new List<long>();
        private bool _logLoadMetrics;
        private static readonly List<long> Loads = new List<long>();
        [UsedImplicitly]
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SaveFile(_logSaveMetrics);
                _logSaveMetrics = true;
            }
            if (Input.GetKeyDown(KeyCode.Z)) {
                LoadFile(_logLoadMetrics);
                _logLoadMetrics = true;
            }
        }

        public static void SaveFile(bool logSaveMetrics = false)
        {
            var timer = new Stopwatch();
            timer.Start();
            var serialized = ToJson(JsonSerializer.Create(UnityTypeSerializerSettings), true).ToString();
            timer.Stop();
            if (logSaveMetrics)
            {
                Saves.Add(timer.ElapsedMilliseconds);
                Debug.Log($"Json serialization took {timer.ElapsedMilliseconds} ms. Average {Saves.Average()} ms.");
            }

            using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Create))
            using (var file = new StreamWriter(stream))
                file.Write(serialized);
        }

        public static void LoadFile(bool logLoadMetrics = false)
        {
            string serialized;
            using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Open))
            using (var file = new StreamReader(stream))
                serialized = file.ReadToEnd();

            var timer = new Stopwatch();
            timer.Start();
            FromJson(JObject.Parse(serialized), true);
            timer.Stop();
            if (!logLoadMetrics) return;
            Loads.Add(timer.ElapsedMilliseconds);
            Debug.Log($"Json deserialization took {timer.ElapsedMilliseconds} ms. Average {Loads.Average()} ms.");
        }
    }
}
