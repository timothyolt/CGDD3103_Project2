using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.UniqueIdentifier {
    [CustomPropertyDrawer(typeof(Guid))]
    public class GuidDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            if (string.IsNullOrEmpty(prop.stringValue))
                prop.stringValue = Guid.NewGuid().ToString();
            
            var offsetPosition = position;
            //offsetPosition.height = 16;
            EditorGUI.LabelField(offsetPosition, label, new GUIContent(prop.stringValue));
        }
    }
}