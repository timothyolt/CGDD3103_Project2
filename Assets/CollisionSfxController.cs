using UnityEngine;

public class CollisionSfxController : MonoBehaviour
{
    public TextAsset SoundEffectFile;
    public float MinimumSustainTime;
    public float SustainMultiplier;
    private SfxrSynth _synth;
    private float _sustainTime;

    // Use this for initialization
	void Start () {

        _synth = new SfxrSynth();
	    _synth.parameters.SetSettingsString(SoundEffectFile.text);
	    _sustainTime = _synth.parameters.sustainTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        _synth.SetParentTransform(gameObject.transform);
        _synth.parameters.sustainTime = MinimumSustainTime + _sustainTime*collision.relativeVelocity.sqrMagnitude * SustainMultiplier;
        _synth.Play();
    }
}
