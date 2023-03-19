using System;
using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.Editor.EditorHelpers
{
    public static partial class EditorHelpers
    {
        [InitializeOnLoad]
        public static class GenericContextualMenu
        {
            static GenericContextualMenu()
            {
                EditorApplication.contextualPropertyMenu += OnEditorApplicationContextualPropertyMenu;
            }

            private static void OnEditorApplicationContextualPropertyMenu(GenericMenu menu, SerializedProperty property)
            {
                TryAddContextualCreationItems(menu, property);
            }

            private static void TryAddContextualCreationItems(GenericMenu menu, SerializedProperty property)
            {
                if (property.propertyType != SerializedPropertyType.ManagedReference)
                {
                    return;
                }

                Type fieldType = GetFieldType(property);
                if (!fieldType.IsAbstract)
                {
                    return;
                }

                AddCreationMenuItems(menu, property.Copy(), fieldType);
            }

            private static void AddCreationMenuItems<T>(GenericMenu menu, SerializedProperty property) =>
                AddCreationMenuItems(menu, property, typeof(T));

            private static void AddCreationMenuItems(GenericMenu menu, SerializedProperty property,
                System.Type baseType)
            {
                if (property.propertyType != SerializedPropertyType.ManagedReference)
                    return;

                foreach (Type inheritingType in GetInheritingTypes(baseType))
                {
                    GUIContent guiContent = new GUIContent($"{baseType.Name}/Set as {inheritingType.Name}");
                    bool on = property.managedReferenceValue != null && property.managedReferenceValue.GetType() == inheritingType;
                    menu.AddItem(guiContent, on, () =>
                    {
                        try
                        {
                            property.managedReferenceValue = Activator.CreateInstance(inheritingType);
                            property.serializedObject.ApplyModifiedProperties();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(
                                $"Creating new instance of type '{inheritingType}' threw an exception {DateTime.Now:hh:mm:ss tt zz}\n{e}");
                        }
                    });
                }
            }
        }
    }
}