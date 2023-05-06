using System;
using System.Collections.Generic;
using System.Linq;
using OutfoxeedTools.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Editor.DataEditorWindow
{
    [ExecuteInEditMode]
    public class DataEditorWindow : EditorWindow, IHasCustomMenu
    {
        #region Open Window
        [MenuItem("OutFoxeed/Data Editor Window", priority = -300)]
        public static DataEditorWindow ShowWindow()
        {
            var window = GetWindow<DataEditorWindow>();
            window.titleContent = new GUIContent("Data Editor Window");
            window.Show();
            return window;
        }

        public static void OpenWindowWithObject(ScriptableObject so)
        {
            DataEditorWindow newInstance = ShowWindow();
            newInstance.selectedType = so.GetType();
            newInstance.itemInfos = new DataEditorItemInfos<ScriptableObject>(newInstance.selectedType);
            newInstance.itemInfos.SelectedObject = so;
        }


        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            UnityEngine.Object target = EditorUtility.InstanceIDToObject(instanceId);
            if (target is ScriptableObject so && scriptableObjectsTypes.Contains(target.GetType()))
            {
                OpenWindowWithObject(so);
                return true;
            }

            return false;
        }

        #region Menu Item
        [MenuItem("Assets/Open with Data Editor Window", true)]
        public static bool OpenValidation() => Selection.activeObject is ScriptableObject;

        [MenuItem("Assets/Open with Data Editor Window", priority = 1500)]
        public static void OpenAsset() => OpenWindowWithObject(Selection.activeObject as ScriptableObject);
        #endregion
        #endregion

        private bool initialized;
        protected Dictionary<System.Type, string> typesAndCreationPath;
        protected static System.Type[] scriptableObjectsTypes;
        private System.Type selectedType;

        private bool guiVarsInitialized;
        private GUIStyle selectedButtonStyle;

        private DataEditorItemInfos<ScriptableObject> itemInfos;
        private string newObjectName = "New Data Name";

        // Scroll vars
        private Vector2 allItemsScroll;
        private Vector2 itemDataScroll;

        // Style
        private GUIStyle style;

        private void Awake() => Init();
        protected virtual void OnEnable() => Init();
        private void OnDisable() => initialized = false;
        private void OnDestroy() => initialized = false;

        protected void Init()
        {
            if (initialized)
                return;
            initialized = true;

            // Load JSON Data
            if (!DataEditorConfig.LoadConfigData(out DataEditorConfig config))
                return;
            // // //

            // Stock data and cast into types
            typesAndCreationPath = new();
            if (config.dataTypes != null)
            {
                foreach (DataEditorConfig.DataType dataType in config.dataTypes)
                {
                    if (dataType.typeName.Contains("Example"))
                        continue;
                    
                    System.Type type = TypeUtilities.StringToType(dataType.typeName);
                    if (type == null)
                    {
                        Debug.LogWarning($"Data Editor Window Config: Type named '{dataType.typeName}' couldn't be parsed into a type.");
                        continue;
                    }

                    if (typesAndCreationPath.ContainsKey(type))
                        continue;

                    typesAndCreationPath.Add(type, dataType.creationPath);
                }
            }
            // // //

            // Set types
            scriptableObjectsTypes = typesAndCreationPath.Keys.ToArray();
        }


        private void OnGUI()
        {
            InitializeGuiVars();

            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope(GUILayout.Height(10)))
                {
                    float modeButtonsWidth = Screen.width - 80;
                    using (new GUILayout.HorizontalScope(style, GUILayout.Width(modeButtonsWidth), GUILayout.Height(10)))
                    {
                        DrawModeButtons(modeButtonsWidth);
                    }
                    using (new GUILayout.HorizontalScope(style, GUILayout.Height(10)))
                    {
                        DrawOptionButton();
                    }
                }

                using (new GUILayout.HorizontalScope())
                {
                    using (new GUILayout.VerticalScope(style, GUILayout.Width(Screen.width * 0.2f)))
                    {
                        DrawAllItems();
                    }

                    using (new GUILayout.VerticalScope(style))
                    {
                        DrawItemInfos();
                    }
                }
            }
        }


        #region Helpers
        private void DrawModeButtons(float width)
        {
            GUILayout.Label("Data Types: ");

            float buttonWidth = (width - 100) / (float) scriptableObjectsTypes.Length;
            foreach (Type scriptableObjectsType in scriptableObjectsTypes)
            {
                using (new GUILayout.VerticalScope())
                {
                    GUIStyle style = scriptableObjectsType == selectedType ? selectedButtonStyle : GUI.skin.button;
                    if (GUILayout.Button(scriptableObjectsType.ToString().Split(".")[^1], style,
                            GUILayout.Width(buttonWidth)))
                    {
                        if (scriptableObjectsType == selectedType)
                            return;
                        selectedType = scriptableObjectsType;
                        itemInfos = new DataEditorItemInfos<ScriptableObject>(scriptableObjectsType);
                    }
                }
            }
        }

        private void DrawOptionButton()
        {
            if (GUILayout.Button("Config"))
                DataEditorConfigWindow.ShowWindow();
        }

        private void DrawAllItems()
        {
            if (itemInfos == null)
            {
                if(GUILayout.Button("Refresh"))
                    Reset();
                return;
            }
            allItemsScroll = EditorGUILayout.BeginScrollView(allItemsScroll);

            foreach (ScriptableObject scriptableObject in itemInfos.AllObjects)
            {
                GUIStyle style = scriptableObject == itemInfos.SelectedObject
                    ? selectedButtonStyle
                    : GUI.skin.button;
                if (GUILayout.Button(scriptableObject.name, style))
                {
                    itemInfos.SelectedObject = scriptableObject;
                }
            }

            GUILayout.Space(20);

            newObjectName = EditorGUILayout.TextField(newObjectName);
            if (GUILayout.Button("Create New"))
            {
                ScriptableObject newScriptableObject = CreateInstance(selectedType);
                var path = $"{typesAndCreationPath[selectedType]}{newObjectName}.asset";
                Debug.Log(path);
                AssetDatabase.CreateAsset(newScriptableObject, path);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newScriptableObject;
                itemInfos = new DataEditorItemInfos<ScriptableObject>(selectedType, newScriptableObject);
            }

            if (GUILayout.Button("Delete selected file"))
            {
                if (EditorUtility.DisplayDialog("Are you sure ?",
                        $"You are going to delete the '{selectedType}' asset named '{itemInfos.SelectedObjectName}'\nAre you sure ?",
                        "Delete", "Cancel"))
                {
                    string path = AssetDatabase.GetAssetPath(itemInfos.SelectedObject);
                    if (string.IsNullOrEmpty(path))
                    {
                        Debug.LogError($"Couldn't find the asset {itemInfos.SelectedObjectName} in the project");
                        return;
                    }

                    AssetDatabase.DeleteAsset(path);

                    if (string.IsNullOrEmpty(path))
                    {
                        Debug.LogError("File path null or empty");
                        return;
                    }

                    AssetDatabase.DeleteAsset(path);
                    Reset();
                }
            }

            GUILayout.Space(5);
            if (GUILayout.Button("Refresh"))
            {
                Reset();
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawItemInfos()
        {
            if (itemInfos == null || itemInfos.SelectedObject == null)
                return;
            itemDataScroll = EditorGUILayout.BeginScrollView(itemDataScroll);

            GUILayout.Label(itemInfos.SelectedObjectName, WindowUtility.TitleStyle);
            if (GUILayout.Button("Select"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = itemInfos.SelectedObject;
            }

            GUILayout.Space(5);

            itemInfos.SelectedObjectEditor.OnInspectorGUI();
            EditorGUILayout.EndScrollView();
        }


        private void InitializeGuiVars()
        {
            if (guiVarsInitialized)
                return;
            guiVarsInitialized = true;

            selectedButtonStyle = new GUIStyle(GUI.skin.button)
            {
                normal =
                {
                    textColor = Color.red
                },
                hover =
                {
                    textColor = Color.red
                }
            };

            style = EditorStyles.helpBox;
        }
        #endregion

        private void Reset()
        {
            initialized = false;
            OnEnable();
            if (selectedType != null)
                itemInfos = new DataEditorItemInfos<ScriptableObject>(selectedType, itemInfos.SelectedObject);
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Refresh"), false, Reset);
        }
    }
}