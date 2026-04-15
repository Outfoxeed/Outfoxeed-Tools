using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace OutfoxeedTools.Editor
{
    public class WorldLoaderEditorWindow : EditorWindow
    {
        private static WorldConfig _worldConfig;

        private Button _loadWorldButton; 
        
        [MenuItem("OutfoxeedTools/WorldLoader")]
        private static void ShowWindow()
        {
            var window = GetWindow<WorldLoaderEditorWindow>();
            window.titleContent = new GUIContent("World Loader");
            window.Show();
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnEditorPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnEditorPlayModeStateChanged;
        }

        private void CreateGUI()
        {
            ObjectField objectField = new ObjectField("World");
            objectField.objectType = typeof(WorldConfig);
            objectField.allowSceneObjects = false;
            objectField.SetValueWithoutNotify(_worldConfig);
            objectField.RegisterValueChangedCallback(OnWorldSelectedChanged);
            rootVisualElement.Add(objectField);

            Button loadWorldButton = new Button(OnLoadWorldButtonClicked);
            loadWorldButton.text = "Load World";
            loadWorldButton.SetEnabled(Application.isPlaying);
            rootVisualElement.Add(loadWorldButton);
            
        }

        private void OnLoadWorldButtonClicked()
        {
            WorldLoader.LoadWorld(_worldConfig);
        }

        private void OnWorldSelectedChanged(ChangeEvent<Object> evt)
        {
            _worldConfig = (WorldConfig)evt.newValue;
        }
        
        private void OnEditorPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            _loadWorldButton.SetEnabled(playModeStateChange == PlayModeStateChange.EnteredPlayMode);
        }
    }
}
