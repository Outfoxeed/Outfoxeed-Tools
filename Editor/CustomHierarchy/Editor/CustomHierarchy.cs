using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.CustomHierarchy.Editor
{
    [InitializeOnLoad]
    public static class CustomHierarchy
    {
        static CustomHierarchy()
        {
            Initialize();
        }

        private static HierarchyRulesSO rules;

        public static HierarchyRulesSO Rules
        {
            get
            {
                if (rules == null)
                {
                    string[] guids = AssetDatabase.FindAssets("t:" + typeof(HierarchyRulesSO), null);
                    if (guids.Length > 0)
                        rules = AssetDatabase.LoadAssetAtPath<HierarchyRulesSO>(
                            AssetDatabase.GUIDToAssetPath(guids[0]));
                }

                return rules;
            }
        }

        public static void Initialize()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;

            if (Rules == null)
                return;

            // Define the types on initialisation
            foreach (ComponentHierarchyRule rule in rules.componentHierarchyRules)
            {
                rule.SetComponentType();
            }
        }

        private static void HierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
        {
            // Check if the object is valid
            UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceId);
            if (obj == null) return;
            GameObject gameObject = obj as GameObject;
            if (gameObject == null) return;

            if (rules == null)
                return;

            // Starts with rule
            foreach (var rule in rules.startsWithHierarchyRules)
            {
                if (!obj.name.StartsWith(rule.value))
                    continue;

                string name = obj.name.Remove(0, rule.value.Length);
                DrawWithSkin(obj, rule.skin, name, selectionRect);
                return;
            }

            // Ends with rule
            foreach (var rule in rules.endsWithHierarchyRules)
            {
                if (!obj.name.EndsWith(rule.value))
                    continue;
                
                string name = obj.name;
                name = name.Remove(name.Length - rule.value.Length, rule.value.Length);
                DrawWithSkin(obj, rule.skin, name, selectionRect);
                return;
            }

            // Contains rule
            foreach (var rule in rules.containsHierarchyRules)
            {
                if (!obj.name.Contains(rule.value)) 
                    continue;
                
                DrawWithSkin(obj, rule.skin, obj.name, selectionRect, true);
                return;
            }

            // Components rule
            foreach (var rule in rules.componentHierarchyRules)
            {
                System.Action draw = () => DrawWithSkin(obj, rule.skin, obj.name, selectionRect, true);
                
                // First try
                if (gameObject.GetComponent(rule.value) != null)
                {
                    draw?.Invoke();
                }
                // Second try
                else if (rule.ComponentType != null)
                {
                    if (gameObject.GetComponent(rule.ComponentType) != null)
                    {
                        draw?.Invoke();
                    }
                }
            }
            
            // Tag rule
            foreach (var rule in rules.tagHierarchyRules)
            {
                if (!gameObject.CompareTag(rule.value))
                    continue;
                DrawWithSkin(obj, rule.skin, obj.name, selectionRect, true);
            }
        }

        static void DrawWithSkin(UnityEngine.Object obj, HierarchySkin skin, string label, Rect rect,
            bool drawBackgroundOnlyOnLabel = false)
        {
            // Define colors and name
            var selected = Selection.Contains(obj);
            var bgColor = selected ? skin.selected.bgColor : skin.normal.bgColor;
            var textColor = selected ? skin.selected.textColor : skin.normal.textColor;
            var name = skin.upperCase ? label.ToUpper() : label;

            // Define style and content
            GUIStyle guiStyle = new GUIStyle()
            {
                normal = new GUIStyleState() {textColor = textColor},
                fontStyle = skin.fontStyle,
                alignment = skin.alignment
            };
            GUIContent guiContent = new GUIContent(name);

            // Set rect width to the witdh of the label if wanted
            if (drawBackgroundOnlyOnLabel)
            {
                rect.x += 18;
                rect.width = guiStyle.CalcSize(guiContent).x;
            }

            // Draw background color
                // If bgColor is transparent, first draw classic default color to erase default label
            if (bgColor.a <= 1f)
            {
                float grey = 56f / 255f;
                EditorGUI.DrawRect(rect,
                    selected ? new Color(44f / 255f, 93f / 255f, 135f / 255f) : new Color(grey, grey, grey));
            }
                // Draw bgColor rect
            EditorGUI.DrawRect(rect, bgColor);
            // Draw label
            EditorGUI.LabelField(rect, guiContent, guiStyle);
        }
    }
}