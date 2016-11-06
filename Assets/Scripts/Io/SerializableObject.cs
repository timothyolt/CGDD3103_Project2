using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Io
{
    public class SerializableObject : MonoBehaviour {
        public Component[] SerializablComponents;

        public JToken ToJson(string key) {
            var array = new JArray();
            foreach (var component in SerializablComponents) {
                if (component is ISerializableScript) {
                    var script = (ISerializableScript) component;
                    script.ToJson();
                }
                else if (component is Transform) {
                    var transform = (Transform) component;
                    
                }
            }
            return null;
        }
    }
}
