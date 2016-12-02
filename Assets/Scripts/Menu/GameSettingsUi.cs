using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class GameSettingsUi : MonoBehaviour
    {
        public Dropdown ResolutionDropdown;
        public Toggle FullscreenToggle;
        public Toggle VSyncToggle;
        public Slider SfxVolumeSlider;
        public Slider MusicVolumeSlider;

        public Button CancelButton;
        public Button SaveButton;

        public InputField UsernameField;

        [UsedImplicitly]
        private void Start()
        {
            if (ResolutionDropdown != null)
            {
                GameSettings.Resolutions = Screen.resolutions;
                var i = 0;
                foreach (var res in GameSettings.Resolutions)
                {
                    ResolutionDropdown.options.Add(new Dropdown.OptionData($"{res.width} x {res.height} @ {res.refreshRate}"));
                    ++i;
                    if (Screen.currentResolution.Equals(res))
                        ResolutionDropdown.value = i;
                }
                ResolutionDropdown.onValueChanged.AddListener(index => GameSettings.CurrentResolution = GameSettings.Resolutions[index]);
            }
            if (FullscreenToggle != null)
            {
                FullscreenToggle.isOn = GameSettings.Fullscreen;
                FullscreenToggle.onValueChanged.AddListener(full => GameSettings.Fullscreen = full);
            }
            if (VSyncToggle != null)
            {
                VSyncToggle.isOn = GameSettings.Vsync;
                VSyncToggle.onValueChanged.AddListener(sync => GameSettings.Vsync = sync);
            }
            if (SfxVolumeSlider != null)
            {
                SfxVolumeSlider.value = GameSettings.SfxVolume;
                SfxVolumeSlider.onValueChanged.AddListener(value => GameSettings.SfxVolume = value);
            }
            if (MusicVolumeSlider != null)
            {
                MusicVolumeSlider.value = GameSettings.MusicVolume;
                MusicVolumeSlider.onValueChanged.AddListener(value => GameSettings.MusicVolume = value);
            }

            if (UsernameField != null)
            {
                UsernameField.text = GameSettings.Username;
                UsernameField.onEndEdit.AddListener(username => GameSettings.Username = username);
            }

            CancelButton?.onClick.AddListener(() =>
            {
                GameSettings.Load();
                var main = Instantiate(Resources.Load<GameObject>("Menus/Main"));
                main.transform.SetParent(transform.parent);
                main.transform.position = transform.position;
                main.transform.localScale = transform.localScale;
                Destroy(gameObject);
            });
            SaveButton?.onClick.AddListener(() =>
            {
                GameSettings.Save();
                var main = Instantiate(Resources.Load<GameObject>("Menus/Main"));
                main.transform.SetParent(transform.parent);
                main.transform.position = transform.position;
                main.transform.localScale = transform.localScale;
                Destroy(gameObject);
            });
        }
    }
}
