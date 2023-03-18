using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

// ReSharper disable once CheckNamespace
namespace OutfoxeedTools.Editor
{
    [InitializeOnLoad]
    public class ToolbarButtons
    {
        [InitializeOnLoad]
        public class SceneSwitchLeftButton
        {
            static SceneSwitchLeftButton()
            {
                ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
                // ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);
            }

            static void OnLeftToolbarGUI()
            {
                GUILayout.FlexibleSpace();
                foreach (EditorBuildSettingsScene editorBuildSettingsScene in EditorBuildSettings.scenes)
                {
                    if (GUILayout.Button(editorBuildSettingsScene.path.Split("/")[^1].Split(".")[^2]))
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            EditorSceneManager.OpenScene(editorBuildSettingsScene.path);
                    }
                }
            }
            
            // private static void OnRightToolbarGUI()
            // {
            //     
            // }
        }
    }
}