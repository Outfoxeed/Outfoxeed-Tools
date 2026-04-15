using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelAttributePropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LabelAttribute labelAttribute = attribute as LabelAttribute;
            if (attribute == null)
                return;
            
            EditorGUI.PropertyField(position, property, new GUIContent(labelAttribute.label));
        }
    }
}