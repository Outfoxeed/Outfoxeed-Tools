using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace OutfoxeedTools.Editor.GDWindow
{
    [InitializeOnLoad]
    public class GDWindowToolbarButton
    {
        static GDWindowToolbarButton()
        {
            ToolbarExtender.RightToolbarGUI.Add(DrawGDWindowButton);
        }

        static void DrawGDWindowButton()
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("GD Window", new GUIStyle(GUI.skin.button){normal = {}}))
            {
                GDWindow.ShowWindow();
            }
            GUI.backgroundColor = backgroundColor;
            GUILayout.FlexibleSpace();
        }
    }
}