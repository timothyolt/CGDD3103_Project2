using UnityEngine;
using System.Collections;
using Assets.Scripts.Io;
using JetBrains.Annotations;

public class JsonSerializer : MonoBehaviour {

	[UsedImplicitly]
    private void Start () {
	    foreach (var serializable in FindObjectsOfType<SerializableObject>()) {
	        
	    }
    }

    [UsedImplicitly]
    private void Update () {
	}
}
