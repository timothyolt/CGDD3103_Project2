using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameSettings
    {
        public static Resolution[] Resolutions;

        private static Resolution _currentResolution;

        public static Resolution CurrentResolution
        {
            get { return _currentResolution; }
            set
            {
                _currentResolution = value;
                UpdateResolution(value, _fullscreen);
            }
        }

        public static bool Fullscreen
        {
            get { return _fullscreen; }
            set
            {
                _fullscreen = value; 
                UpdateResolution(_currentResolution, value);
            }
        }

        public static void UpdateResolution(Resolution res, bool full) => Screen.SetResolution(res.width, res.height, full, res.refreshRate);

        public static bool Vsync
        {
            get { return _vsync; }
            set
            {
                _vsync = value;
                QualitySettings.vSyncCount = value ? 1 : 0;
            }
        }

        public static float SfxVolume = 1;
        public static float MusicVolume = 1;
        private static bool _fullscreen;
        private static bool _vsync;

        public static void Save()
        {
            using (var stream = new FileStream($"{Application.persistentDataPath}/options", FileMode.Create))
            using (var file = new StreamWriter(stream))
                file.Write(new JObject
                {
                    {"resolution", JToken.FromObject(CurrentResolution)},
                    {"fullscreen", Fullscreen },
                    {"vsync", Vsync},
                    {"sfxVolume", SfxVolume },
                    {"musicVolume", MusicVolume }
                });
        }

        public static void Load()
        {
            string fileString = null;
            try
            {
                using (var stream = new FileStream($"{Application.persistentDataPath}/options", FileMode.Open))
                using (var file = new StreamReader(stream))
                    fileString = file.ReadToEnd();
            }
            catch (IOException)
            {
                Debug.Log("failed to load settings");
            }
            if (string.IsNullOrEmpty(fileString))
                return;
            var serialized = JObject.Parse(fileString);
            Resolutions = Screen.resolutions;
            CurrentResolution = JsonConvert.DeserializeObject<Resolution>(serialized["resolution"].ToString());
            Fullscreen = bool.Parse(serialized["fullscreen"].ToString());
            Vsync = bool.Parse(serialized["vsync"].ToString());
            SfxVolume = float.Parse(serialized["sfxVolume"].ToString());
            MusicVolume = float.Parse(serialized["musicVolume"].ToString());
        }
    }
}
