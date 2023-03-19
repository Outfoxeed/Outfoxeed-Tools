using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.Editor.EditorHelpers
{
    public static partial class EditorHelpers
    {
        public static IEnumerable<System.Type> GetInheritingTypes(this System.Type type)
        {
            return type.Assembly.GetTypes().Where(t => !t.IsAbstract && t != type && type.IsAssignableFrom(t));
        }

        public static System.Type GetFieldType(this SerializedProperty serializedProperty)
        {
            if (serializedProperty.propertyType != SerializedPropertyType.ManagedReference)
            {
                Debug.LogError("Cannot get managedReferenceFieldType of serializedPropery. Method ignored");
                return null;
            }
            
            string managedReferenceFieldTypeName = serializedProperty.managedReferenceFieldTypename;
            if (string.IsNullOrEmpty(managedReferenceFieldTypeName))
            {
                Debug.LogError("managedReferenceFieldTypeName is null");
                return null;
            }

            string[] values = managedReferenceFieldTypeName.Split();
            Type result = null;
            switch (values.Length)
            {
                case 1:
                    result = Type.GetType(values[0]);
                    break;
                case 2:
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(values[0]);
                    result = assembly.GetType(values[1]);
                    break;
                default:
                    Debug.LogError("Too much values in managedReferenceFIeldTypeName");
                    return null;
            }

            if (result is null)
            {
                Debug.LogError("Found field type of serializedProperty is null");
                return null;
            }

            return result;
        }

        public static string LastName(this System.Type type)
        {
            return type.ToString().Split(new[] {'.', '+'})[^1];
        }
    }
}