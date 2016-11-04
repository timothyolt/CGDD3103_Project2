using JetBrains.Annotations;
using UnityEngine;

namespace Assets
{
    public class CollisionSfxController : MonoBehaviour
    {
        public AudioSource AudioSource;
        public TextAsset SfxStartParams;
        public TextAsset SfxStopParams;
        public float ImpactSquaredMin;
        public float ImpactSquaredMax;

        private SfxrSynth _synth;
        private SfxrParams _start;
        private SfxrParams _stop;


        public void UpdateSfx()
        {
            (_start = new SfxrParams()).SetSettingsString(SfxStartParams?.text ?? "");
            (_stop = new SfxrParams()).SetSettingsString(SfxStopParams?.text ?? "");
        }

        [UsedImplicitly]
        private void Start () {
            _synth = new SfxrSynth();
            if (AudioSource != null)
                _synth.SetAudioSource(AudioSource);
            UpdateSfx();
        }

        [UsedImplicitly]
        private void OnCollisionEnter(Collision collision)
        {
            if (_synth == null) return;
            _synth.Stop();
            var lerp = Mathf.Clamp((collision.relativeVelocity.sqrMagnitude - ImpactSquaredMin)/(ImpactSquaredMax - ImpactSquaredMin), 0f, 1f);
            _synth.parameters = SfxrParams.Lerp(_start, _stop, lerp);
            _synth.Play();
        }
    }
}
