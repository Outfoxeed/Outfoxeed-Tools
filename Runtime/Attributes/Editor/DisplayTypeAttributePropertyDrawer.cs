using OutfoxeedTools.Editor.EditorHelpers;
using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(DisplayTypeAttribute))]
    public class DisplayTypeAttributePropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                label.text += $" ({property.managedReferenceValue.GetType().LastName()})";
            }
            else
            {
                label.text += $" ({property.type.Replace("`1", "")})";
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}