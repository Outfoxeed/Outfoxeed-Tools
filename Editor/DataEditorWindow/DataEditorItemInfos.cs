using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.DataEditorWindow
{
    public class DataEditorItemInfos<T> where T : ScriptableObject
    {
        // All objects of type T
        public T[] AllObjects { get; protected set; }
        
        // Selected object
        private T selectedObject;
        public T SelectedObject
        {
            get => selectedObject;
            set
            {
                selectedObject = value;
                SelectedObjectName = selectedObject.name;
                SelectedSerializedObject = new SerializedObject(selectedObject);
                SelectedObjectEditor = UnityEditor.Editor.CreateEditor(selectedObject);
            }
        }
        public SerializedObject SelectedSerializedObject { get; protected set; }
        public string SelectedObjectName { get; protected set; }
        public UnityEditor.Editor SelectedObjectEditor { get; protected set; }

        public DataEditorItemInfos(System.Type type, T selectedObject = null)
        {
            // Init allObjects array
            List<T> objects = new();
            string[] guids = AssetDatabase.FindAssets("t:"+type, null);
            foreach (string guid in guids)
            {
                T item = (T) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(T));
                if (item != null)
                {
                    objects.Add(item);
                }
            }
            AllObjects = objects.ToArray();
            
            // Select first object
            if(AllObjects == null)
                return;
            if (AllObjects.Length == 0)
                return;
            SelectedObject = selectedObject ? selectedObject : AllObjects?[0];
        }
    }
}