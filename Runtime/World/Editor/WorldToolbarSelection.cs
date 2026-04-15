using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace OutfoxeedTools.Editor
{
    [InitializeOnLoad]
    public class WorldToolbarSelection
    {
        static WorldToolbarSelection()
        {
            ToolbarExtender.RightToolbarGUI.Add(DrawWorldSelection);
        }

        static void DrawWorldSelection()
        {
            GUILayout.FlexibleSpace();
            
            WorldConfig selectedWorldConfig = EditorGUILayout.ObjectField(WorldLoader.LoadedWorldConfig, typeof(WorldConfig), false) as WorldConfig;
            if (selectedWorldConfig != WorldLoader.LoadedWorldConfig)
            {
                WorldLoader.LoadWorld(selectedWorldConfig);
            }
            
            GUILayout.FlexibleSpace();
        }
    }
}