using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Assets.Scripts.Io {
    public class WritableOnlyResolver : DefaultContractResolver {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
            return
                base.CreateProperties(type, memberSerialization).Where(jsonProperty => jsonProperty.Writable).ToList();
        }
    }
}