﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Io {
    public interface ISerializableScript {
        JToken ToJson();
        void FromJson(JToken token);
    }
}