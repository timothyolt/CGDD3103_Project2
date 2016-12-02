using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    [RequireComponent(typeof(AudioSource))]
    public class MainMenuGlobal : MonoBehaviour
    {
        private AudioSource _audioSource;

        [UsedImplicitly]
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = GameSettings.MusicVolume;
            GameSettings.MusicVolumeUpdate += UpdateVolume;
        }

        private void UpdateVolume(float volume)
        {
            if (_audioSource != null)
                _audioSource.volume = volume;
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            GameSettings.SfxVolumeUpdate -= UpdateVolume;
        }
    }
}
