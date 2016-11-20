using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Io.Components
{
    public class TransformSerializer : ComponentSerializer {

        public override JToken ToJson(Component input, JsonSerializer serializer) {
            var transform = input as Transform;
            if (transform == null) return null;
            return new JObject {
                    {"position", JObject.FromObject(transform.localPosition, serializer)},
                    {"rotation", JObject.FromObject(transform.localRotation.eulerAngles, serializer)},
                    {"scale", JObject.FromObject(transform.localScale, serializer)}
                };
        }

        public override void FromJson(JToken input, GameObject gameObject)
        {
            var transform = gameObject.transform;
            if (transform == null || input == null) return;
            transform.localPosition = JsonConvert.DeserializeObject<Vector3>(input["position"].ToString());
            transform.localRotation = new Quaternion { eulerAngles = JsonConvert.DeserializeObject<Vector3>(input["rotation"].ToString()) };
            transform.localScale = JsonConvert.DeserializeObject<Vector3>(input["scale"].ToString());
        }
    }
}
