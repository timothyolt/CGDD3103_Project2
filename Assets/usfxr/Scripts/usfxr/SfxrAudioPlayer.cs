#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class SfxrAudioPlayer : MonoBehaviour {

	/**
	 * usfxr
	 *
	 * Copyright 2013 Zeh Fernando
     * Copyright 2016 Tim Oltjenbruns
	 *
	 * Licensed under the Apache License, Version 2.0 (the "License");
	 * you may not use this file except in compliance with the License.
	 * You may obtain a copy of the License at
	 *
	 * 	http://www.apache.org/licenses/LICENSE-2.0
	 *
	 * Unless required by applicable law or agreed to in writing, software
	 * distributed under the License is distributed on an "AS IS" BASIS,
	 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	 * See the License for the specific language governing permissions and
	 * limitations under the License.
	 *
	 */

	/**
	 * SfxrAudioPlayer
	 * This is the (internal) behavior script responsible for streaming audio to the engine
	 *
	 * @author Zeh Fernando
	 */


	// Properties
    public AudioSource Source { get; set; }
	private bool		isDestroyed = false;		// If true, this instance has been destroyed and shouldn't do anything yes
	private bool		needsToDestroy = false;		// If true, it has been scheduled for destruction (from outside the main thread)
	private bool		runningInEditMode = false;	// If true, it is running from the editor and NOT playing
    private bool destroyScriptOnly; // If true, the parent object was not created by Usfxr, and we should only destroy the script

	// Instances
	private SfxrSynth	sfxrSynth;					// SfxrSynth instance that will generate the audio samples used by this
    private bool _traced;


    // ================================================================================================================
	// INTERNAL INTERFACE ---------------------------------------------------------------------------------------------

	void Start()
	{
        if (Source == null)
            Source = GetComponent<AudioSource>();
	    if (Source == null)
	    {
            // Creates an empty audio source so this GameObject can receive audio events
            Source = gameObject.AddComponent<AudioSource>();
            Source.volume = 1f;
            Source.pitch = 1f;
            Source.priority = 128;
        }
	    else
	        destroyScriptOnly = true;
        Source.clip = new AudioClip();
	    Source.enabled = true;
        Source.Play();
    }

	void Update()
	{
        // Destroys self in case it has been queued for deletion
        if (sfxrSynth == null || !(Source?.isPlaying ?? false))
        {
            // Rogue object (leftover)
            // When switching between play and edit mode while the sound is playing, the object is restarted
            // So, queues for destruction
            needsToDestroy = true;
		}

		if (needsToDestroy) {
			needsToDestroy = false;
			Destroy();
		}
	}
    
	void OnAudioFilterRead(float[] __data, int __channels) {
        // Requests the generation of the needed audio data from SfxrSynth
        if (!isDestroyed && !needsToDestroy && sfxrSynth != null) {
			bool hasMoreSamples = sfxrSynth.GenerateAudioFilterData(__data, __channels);

			// If no more samples are needed, there's no more need for this GameObject so schedule a destruction (cannot do this in this thread)
			if (!hasMoreSamples) {
				needsToDestroy = true;
				if (runningInEditMode) {
					// When running in edit mode, Update() is not called on every frame
					// We can't call Destroy() directly either, since Destroy() must be ran from the main thread
					// So we just attach our Update() to the editor's update event
					#if UNITY_EDITOR
					EditorApplication.update += Update;
					#endif
				}
			}
		}
  	}


	// ================================================================================================================
	// PUBLIC INTERFACE -----------------------------------------------------------------------------------------------

	public void SetSfxrSynth(SfxrSynth __sfxrSynth) {
		// Sets the SfxrSynth instance that will generate the audio samples used by this
		sfxrSynth = __sfxrSynth;
	}

	public void SetRunningInEditMode(bool __runningInEditMode) {
		// Sets the SfxrSynth instance that will generate the audio samples used by this
		runningInEditMode = __runningInEditMode;
	}

	public void Destroy() {
        Source?.Stop();
		// Stops audio immediately and destroys self
		if (!isDestroyed) {
			isDestroyed = true;
			sfxrSynth = null;
			if (runningInEditMode || !Application.isPlaying) {
				// Since we're running in the editor, we need to remove the update event, AND destroy immediately
				#if UNITY_EDITOR
				EditorApplication.update -= Update;
				#endif
                if (destroyScriptOnly)
                    DestroyImmediate(this);
                else
                    DestroyImmediate(gameObject);
			} else {
                if (destroyScriptOnly)
				    Destroy(this);
                else
                    Destroy(gameObject);
            }
		}
	}
}
