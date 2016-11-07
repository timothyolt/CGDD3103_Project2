using Assets.Scripts.Io;
using JetBrains.Annotations;
using UnityEngine;

public class JsonSerializer : MonoBehaviour {
    [UsedImplicitly]
    private void Start() {
        foreach (var serializable in FindObjectsOfType<SerializableObject>()) {}
    }

    [UsedImplicitly]
    private void Update() {}
}