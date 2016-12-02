using System;
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

        public static event Action<Resolution> ResolutionUpdate;
        public static Resolution CurrentResolution
        {
            get { return _currentResolution; }
            set
            {
                _currentResolution = value;
                UpdateResolution(value, _fullscreen);
                ResolutionUpdate?.Invoke(value);
            }
        }

        public static event Action<bool> FullscreenUpdate;
        public static bool Fullscreen
        {
            get { return _fullscreen; }
            set
            {
                _fullscreen = value; 
                UpdateResolution(_currentResolution, value);
                FullscreenUpdate?.Invoke(value);
            }
        }

        public static void UpdateResolution(Resolution res, bool full) => Screen.SetResolution(res.width, res.height, full, res.refreshRate);

        public static event Action<bool> VsyncUpdate;
        public static bool Vsync
        {
            get { return _vsync; }
            set
            {
                _vsync = value;
                QualitySettings.vSyncCount = value ? 1 : 0;
                VsyncUpdate?.Invoke(value);
            }
        }

        public static event Action<float> SfxVolumeUpdate;
        public static float SfxVolume
        {
            get { return _sfxVolume; }
            set
            {
                _sfxVolume = value; 
                SfxVolumeUpdate?.Invoke(value);
            }
        }

        public static event Action<float> MusicVolumeUpdate;
        public static float MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = value;
                MusicVolumeUpdate?.Invoke(value);
            }
        }

        public static event Action<string> UsernameUpdate;
        public static string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                UsernameUpdate?.Invoke(value);
            }
        }

        private static float _musicVolume = 1;
        private static bool _fullscreen;
        private static bool _vsync;
        private static float _sfxVolume = 1;
        private static string _username = "Username";

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
                    {"musicVolume", MusicVolume },
                    {"username", Username }
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
            CurrentResolution = JsonConvert.DeserializeObject<Resolution>(serialized["resolution"]?.ToString());
            Fullscreen = bool.Parse(serialized["fullscreen"]?.ToString() ?? "false");
            Vsync = bool.Parse(serialized["vsync"]?.ToString() ?? "false");
            SfxVolume = float.Parse(serialized["sfxVolume"]?.ToString() ?? "1");
            MusicVolume = float.Parse(serialized["musicVolume"]?.ToString() ?? "1");
            Username = serialized["username"]?.ToString() ?? "username";
        }
    }
}
