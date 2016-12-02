using System;
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
            if (gameSave == null)
                return;
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SaveFile();
                SceneManager.LoadScene(0);
            }
        }

        public static void SaveFile(bool logSaveMetrics = false)
        {
            JObject current = null;
            string outputString = null;
            try
            {
                using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Open))
                using (var file = new StreamReader(stream))
                    current = JObject.Parse(file.ReadToEnd()) ?? new JObject();
            }
            catch (Exception)
            {
                Debug.Log("Error reading existing savefile. Creating new.");
            }

            var timer = new Stopwatch();
            timer.Start();
            try
            {
                if (current == null)
                    current = new JObject();
                if (current[GameSettings.Username] == null)
                    current[GameSettings.Username] = new JObject();
                current[GameSettings.Username][SceneManager.GetActiveScene().name] =
                    ToJson(JsonSerializer.Create(UnityTypeSerializerSettings), true);
                outputString = current.ToString();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            timer.Stop();
            if (logSaveMetrics)
            {
                Saves.Add(timer.ElapsedMilliseconds);
                Debug.Log($"Json serialization took {timer.ElapsedMilliseconds} ms. Average {Saves.Average()} ms.");
            }

            using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Create))
            using (var file = new StreamWriter(stream))
                file.Write(outputString);
        }

        public static void LoadFile(bool logLoadMetrics = false)
        {
            string inputString;
            using (var stream = new FileStream($"{Application.persistentDataPath}/gamesave", FileMode.Open))
            using (var file = new StreamReader(stream))
                inputString = file.ReadToEnd();

            var timer = new Stopwatch();
            timer.Start();
            try
            {
                FromJson(JObject.Parse(inputString)?[GameSettings.Username]?[SceneManager.GetActiveScene().name], true);
            }
            catch (JsonReaderException e)
            {
                Debug.LogWarning($"JsonReader Error: {e.Message}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            timer.Stop();
            if (!logLoadMetrics) return;
            Loads.Add(timer.ElapsedMilliseconds);
            Debug.Log($"Json deserialization took {timer.ElapsedMilliseconds} ms. Average {Loads.Average()} ms.");
        }

        [UsedImplicitly]
        private void Start()
        {
            try
            {
                LoadFile();
            } catch (IOException) { }
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            SceneManager.LoadScene(0);
        }
    }
}
