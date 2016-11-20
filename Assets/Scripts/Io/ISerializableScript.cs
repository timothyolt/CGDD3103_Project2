using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Io {
    public interface ISerializableScript {
        JToken ToJson(JsonSerializer serializer);
        void FromJson(JToken token);
    }
}