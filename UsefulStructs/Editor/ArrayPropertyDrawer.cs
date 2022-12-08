using UnityEditor;
using UnityEngine;

namespace OutFoxeedTools.UsefulStructs.Editor
{
    [CustomPropertyDrawer(typeof(Array<>))]
    public class ArrayPropertyDrawer : PropertyDrawer
    {
        private const string subArrayName = "items";
        
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative(subArrayName), new GUIContent(label));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var subArrayProperty = property.FindPropertyRelative(subArrayName);
            return base.GetPropertyHeight(property, label) * GetPropertyLineHeight(subArrayProperty);
        }
        public static float GetPropertyLineHeight(SerializedProperty property) 
            => 2 + (property.isExpanded ? (property.arraySize + 1) * 1.05f : 0);
    }
}