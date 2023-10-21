using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Editor.DataEditorWindow
{
    [InitializeOnLoad]
    public static class DataEditorWindowManager
    {
        static DataEditorWindowManager()
        {
            Debug.Log("CONSTRUCTOR");
        }
    }
}