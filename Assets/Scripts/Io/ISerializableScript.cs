using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Io {
    public interface ISerializableScript {
        JToken ToJson();
    }
}