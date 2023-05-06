using OutfoxeedTools.Editor;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Editor.DataEditorWindow
{
    public class DataEditorConfigWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            var window = GetWindow<DataEditorConfigWindow>();
            window.titleContent = new GUIContent("Data Editor Config Window");
            window.Show();
        }


        private bool initalized;
        private void Awake() => Init();
        private void OnEnable() => Init();
        private void OnDisable() => initalized = false;

        private void OnDestroy()
        {
            SaveConfig();
            initalized = false;
        }

        public DataEditorConfig config;
        private SerializedObject so;
        private SerializedProperty spConfig;

        public Vector2 scrollPos;

        private void Init()
        {
            if (initalized)
                return;
            initalized = true;

            if (!DataEditorConfig.LoadConfigData(out config))
            {
                Debug.LogError("Data Editor Config Window: couldn't load config file");
                return;
            }

            so = new SerializedObject(this);
            spConfig = so.FindProperty("config");
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            
            GUILayout.Label("Data Editor Config Window", WindowUtility.TitleStyle);
            GUILayout.Space(5);

            if (spConfig == null)
            {
                GUILayout.Label("Config Serialized Property is null", WindowUtility.ErrorStyle);
                return;
            }

            so.Update();
            if (!spConfig.isExpanded)
                spConfig.isExpanded = true;
            EditorGUILayout.PropertyField(spConfig);
            so.ApplyModifiedProperties();

            GUILayout.Space(10);
            if (GUILayout.Button("Save"))
                SaveConfig();
            
            EditorGUILayout.EndScrollView();
        }

        
        private bool SaveConfig()
        {
            if (!DataEditorConfig.LoadConfigData(out DataEditorConfig dataEditorConfig))
            {
                Debug.LogError("Data Editor Config Window: couldn't save because couldn't load config file");
                return false;
            }

            if (dataEditorConfig.Equals(config))
                return false;
            
            FileReader.StoreDataOnJson(config, DataEditorConfig.ConfigFilePath);
            Debug.Log("Data Editor Config Window: successfully saved configs");
            return true;
        }
    }
}