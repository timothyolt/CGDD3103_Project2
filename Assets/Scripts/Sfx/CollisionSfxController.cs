using Assets.Scripts.Inventory;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Sfx {
    [RequireComponent(typeof(ProjectilePickup))]
    public class CollisionSfxController : MonoBehaviour {
        private SfxrParams _start;
        private SfxrParams _stop;

        private SfxrSynth _synth;
        public AudioSource AudioSource;
        public float ImpactSquaredMax;
        public float ImpactSquaredMin;
        public TextAsset SfxStartParams;
        public TextAsset SfxStopParams;


        public void UpdateSfx() {
            (_start = new SfxrParams()).SetSettingsString(SfxStartParams?.text ?? "");
            (_stop = new SfxrParams()).SetSettingsString(SfxStopParams?.text ?? "");
        }

        [UsedImplicitly]
        private void Start() {
            _synth = new SfxrSynth();
            if (AudioSource != null)
            {
                AudioSource.volume = GameSettings.SfxVolume;
                _synth.SetAudioSource(AudioSource);
            }
            UpdateSfx();
        }

        [UsedImplicitly]
        private void OnCollisionEnter(Collision collision) {
            if (_synth == null || GetComponent<ProjectilePickup>().VolatileTime <= 0) return;
            _synth.Stop();
            var lerp =
                Mathf.Clamp(
                    (collision.relativeVelocity.sqrMagnitude - ImpactSquaredMin)/(ImpactSquaredMax - ImpactSquaredMin),
                    0f, 1f);
            _synth.parameters = SfxrParams.Lerp(_start, _stop, lerp);
            _synth.Play();
        }
    }
}