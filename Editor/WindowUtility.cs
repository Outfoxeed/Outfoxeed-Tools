using UnityEditor;
using UnityEngine;

namespace OutFoxeedTools.Editor
{
    public static class WindowUtility
    {
        public static GUIStyle GetDarkButtonStyle()
        {
            GUIStyle style = GUI.skin.button;
            return style;
        }

        private static GUIStyle _titleStyle;
        public static GUIStyle TitleStyle
        {
            get
            {
                if (_titleStyle == null)
                {
                    _titleStyle = new GUIStyle(GUI.skin.label);
                    _titleStyle.normal.textColor = Color.white;
                    _titleStyle.fontSize = 20;
                }
                return _titleStyle;
            }
        }

        private static GUIStyle _subTitleStyle;
        public static GUIStyle SubTitleStyle
        {
            get
            {
                if (_subTitleStyle == null)
                {
                    _subTitleStyle = new GUIStyle(GUI.skin.label);
                    _subTitleStyle.normal.textColor = Color.white;
                    _subTitleStyle.fontSize = 15;
                }
                return _subTitleStyle;
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
