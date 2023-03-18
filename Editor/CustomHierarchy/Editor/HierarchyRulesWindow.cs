using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.CustomHierarchy.Editor
{
    public class HierarchyRulesWindow : EditorWindow
    {
        [MenuItem("OutFoxeed/Hierarchy Rules", false, -200)]
        public static void ShowWindow()
        {
            var window = GetWindow<HierarchyRulesWindow>();
            window.titleContent = new GUIContent("Hierarchy Rules");
            window.Show();
        }

        private HierarchyRulesSO hierarchyRulesSo;
        private UnityEditor.Editor editor;
        
        private void OnEnable()
        {
            hierarchyRulesSo = CustomHierarchy.Rules;
            if (hierarchyRulesSo == null)
                return;
            editor = UnityEditor.Editor.CreateEditor(hierarchyRulesSo);
        }

        private void OnGUI()
        {
            if (editor == null)
                return;
            
            editor.OnInspectorGUI();
        }
    }
}