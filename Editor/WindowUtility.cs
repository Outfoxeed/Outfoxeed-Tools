using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.Editor
{
    public static class WindowUtility
    {
        public static GUIStyle GetDarkButtonStyle()
        {
            GUIStyle style = GUI.skin.button;
            return style;
        }

        private static GUIStyle titleStyle;
        public static GUIStyle TitleStyle
        {
            get
            {
                if (titleStyle == null)
                {
                    titleStyle = new GUIStyle(GUI.skin.label);
                    titleStyle.normal.textColor = Color.white;
                    titleStyle.fontSize = 20;
                }
                return titleStyle;
            }
        }

        private static GUIStyle subTitleStyle;
        public static GUIStyle SubTitleStyle
        {
            get
            {
                if (subTitleStyle == null)
                {
                    subTitleStyle = new GUIStyle(GUI.skin.label);
                    subTitleStyle.normal.textColor = Color.white;
                    subTitleStyle.fontSize = 15;
                }
                return subTitleStyle;
            }
        }

        private static GUIStyle errorStyle;
        public static GUIStyle ErrorStyle
        {
            get
            {
                if (errorStyle == null)
                {
                    errorStyle = new GUIStyle(GUI.skin.label);
                    errorStyle.normal.textColor = Color.red;
                    errorStyle.fontSize += 2;
                }
                return errorStyle;
            }   
        }

        static GUILayoutOption NinetyNineWidthPercent() => GUILayout.Width(Screen.width * 0.99f);

        public static T ObjectFieldWithEraseButton<T>(string label, T obj) where T : UnityEngine.Object
        {
            GUILayout.Label(label);
            obj = (T)EditorGUILayout.ObjectField(obj, typeof(T), true);
            if (GUILayout.Button(" X ")) obj = null;

            return obj;
        }
        public static T ObjectField<T>(string label, T obj) where T : UnityEngine.Object
        {
            GUILayout.Label(label);
            obj = (T)EditorGUILayout.ObjectField(obj, typeof(T), true);

            return obj;
        }

        public static float FloatField(string label, float value)
        {
            GUILayout.Label(label);
            value = EditorGUILayout.FloatField(value);
            return value;
        }
        public static float FloatField(string label, float value, float defaultValue)
        {
            GUILayout.Label(label);
            value = EditorGUILayout.FloatField(value);
            if (GUILayout.Button(" X ")) value = defaultValue;
            return value;
        }

        public static string TextFieldWithDefaultButton(string label, string value, string defaultValue)
        {
            GUILayout.Label(label);
            value = EditorGUILayout.TextField(value);
            if (GUILayout.Button(" X ")) value = defaultValue;

            return value;
        }

        public static bool LittleToggle(string label, bool value)
        {
            GUILayout.Label(label, GUILayout.Width(150));
            value = EditorGUILayout.Toggle(value);

            return value;
        }
    }
}
