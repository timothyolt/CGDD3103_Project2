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
            FullscreenToggle?.onValueChanged.AddListener(full => GameSettings.Fullscreen = full);
            VSyncToggle?.onValueChanged.AddListener(sync => GameSettings.Vsync = sync);
            SfxVolumeSlider?.onValueChanged.AddListener(value => GameSettings.SfxVolume = value);
            MusicVolumeSlider?.onValueChanged.AddListener(value => GameSettings.MusicVolume = value);

            CancelButton?.onClick.AddListener(() =>
            {
                var main = Instantiate(Resources.Load<GameObject>("Menus/Main"));
                main.transform.SetParent(transform.parent);
                main.transform.position = transform.position;
                main.transform.localScale = transform.localScale;
                Destroy(gameObject);
            });
            SaveButton?.onClick.AddListener(GameSettings.Save);
        }
    }
}
