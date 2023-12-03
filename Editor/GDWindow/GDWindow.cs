using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.Editor.GDWindow
{
    public class GDWindow : EditorWindow
    {
        // Data
        private Dictionary<string, List<ScriptableObject>> _gdDatas;
        private const string _labelName = "GD";
        
        // GUI
        private Vector2 _scrollPos;
        // //  Selected button style
        private readonly Color _selectedButtonColor = Color.green;
        //

        [MenuItem("OutFoxeed/GD Window", priority = -500)]
        public static void ShowWindow()
        {
            var window = GetWindow<GDWindow>();
            window.titleContent = new GUIContent("GD Window");
            window.Show();
        }

        private void OnEnable()
        {
            LoadAllAssets<ScriptableObject>();
        }

        private void OnGUI()
        {
            // Title 
            GUILayout.Label("GD WINDOW", new GUIStyle(GUI.skin.label){fontSize = 30});

            // Refresh button
            float refreshButtonWidth = 60f;
            float refreshButtonHeight = 20f;
            Rect refreshButtonRect = new Rect(Screen.width - refreshButtonWidth, 0, 
                refreshButtonWidth, refreshButtonHeight);
            if (GUI.Button(refreshButtonRect, "Refresh"))
            {
                LoadAllAssets<ScriptableObject>();
            }
            
            GUILayout.Space(10);
            
            // Content
            using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPos))
            {
                _scrollPos = scrollView.scrollPosition;

                foreach (KeyValuePair<string,List<ScriptableObject>> keyValuePair in _gdDatas)
                {
                    GUILayout.Label(keyValuePair.Key + ":");

                    foreach (ScriptableObject scriptableObject in keyValuePair.Value)
                    {
                        GUIStyle style = scriptableObject == Selection.activeObject
                            ? new GUIStyle(GUI.skin.button){normal = {textColor = _selectedButtonColor}}
                            : GUI.skin.button;
                        
                        if (GUILayout.Button(scriptableObject.name, style))
                        {
                            Selection.activeObject = scriptableObject;
                        }
                    }
                }
            }
        }
        
        private void LoadAllAssets<T>() where T : ScriptableObject
        {
            if (_gdDatas is not null)
            {
                _gdDatas.Clear();
            }
            else
            {
                _gdDatas = new();
            }
            
            string[] guids = AssetDatabase.FindAssets("t:"+typeof(T), null);
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T item = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
                if (item == null)
                {
                    continue;
                }
                
                if (!AssetDatabase.GetLabels(item).Contains(_labelName))
                {
                    continue;
                }

                string key = path.Split('/')[^2];
                if (_gdDatas.TryGetValue(key, out var directoryDatas))
                {
                    directoryDatas.Add(item);
                }
                else
                {
                    _gdDatas[key] = new List<ScriptableObject>() { item };
                }
            }
        }
    }
}