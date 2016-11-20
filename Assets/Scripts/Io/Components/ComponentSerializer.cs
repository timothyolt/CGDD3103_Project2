using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.Io.Components
{
    public abstract class ComponentSerializer
    {
        // ReSharper disable once StaticMemberInGenericType
        public static readonly Dictionary<string, ComponentSerializer> RegisteredComponents = new Dictionary<string, ComponentSerializer> {
            {typeof(Transform).Name, new TransformSerializer()}
        };

        public static JToken CollectionToJson(IEnumerable<Component> components, JsonSerializer serializer)
        {
            var output = new JObject();
            foreach (var component in components) {
                if (component is ISerializableScript)
                    output[$"@{component.GetType().FullName}"] = (component as ISerializableScript).ToJson();
                else output[component.GetType().Name] = RegisteredComponents[component.GetType().Name].ToJson(component, serializer);
            }
            return output;
        }

        public static void CollectionFromJson(JToken jsonComponents, GameObject targetGameObject)
        {
            foreach (var jComponent in jsonComponents.Children<JProperty>())
            {
                if (jComponent.Name[0] == '@')
                {
                    ISerializableScript serializableScript = null;
                    try
                    {
                        var type = Type.GetType(jComponent.Name.Substring(1));
                        if (type == null)
                        {
                            Debug.LogError($"Type '{jComponent.Name}' not available");
                            continue;
                        }
                        serializableScript = Activator.CreateInstance(type) as ISerializableScript;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"{jComponent.Name} reflection error");
                        Debug.LogException(e);
                    }
                    if (serializableScript == null)
                    {
                        Debug.LogError($"Script '{jComponent.Name}' does not implement ISerializableScript");
                        continue;
                    }
                    try
                    {
                        serializableScript.FromJson(jComponent.Value);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Script '{jComponent.Name}' threw '{e.GetType()}' during deserialization");
                        Debug.LogException(e);
                    }
                }
                else RegisteredComponents[jComponent.Name]?.FromJson(jComponent.Value, targetGameObject);
            }
        }

        public abstract JToken ToJson(Component input, JsonSerializer serializer);

        public abstract void FromJson(JToken input, GameObject gameObject);

    }
}
