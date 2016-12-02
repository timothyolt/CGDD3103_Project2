using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    [RequireComponent(typeof(AudioSource))]
    public class MainMenuGlobal : MonoBehaviour
    {
        [UsedImplicitly]
        private void Start()
        {
            GetComponent<AudioSource>().volume = GameSettings.MusicVolume;
            GameSettings.MusicVolumeUpdate += volume => GetComponent<AudioSource>().volume = volume;
        }
    }
}
