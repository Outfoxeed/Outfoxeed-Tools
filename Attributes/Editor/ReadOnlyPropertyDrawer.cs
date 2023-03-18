using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            ReadOnlyAttribute readOnlyAttribute = attribute as ReadOnlyAttribute;
            if (!(readOnlyAttribute is {readOnly: true}))
                return;

            var previousGUI = GUI.enabled;

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = previousGUI;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}