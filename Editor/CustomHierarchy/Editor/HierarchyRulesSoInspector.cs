using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.CustomHierarchy.Editor
{
    [CustomEditor(typeof(HierarchyRulesSO))]
    public class HierarchyRulesSoInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            HierarchyRulesSO hierarchyRulesSo = target as HierarchyRulesSO;
            if (hierarchyRulesSo == null)
                return;

            serializedObject.Update();

            // Title
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 30;
            GUILayout.Label("Hierarchy Rules", style);
            GUILayout.Space(20);

            // Fields
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(hierarchyRulesSo.startsWithHierarchyRules)));
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(hierarchyRulesSo.endsWithHierarchyRules)));
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(hierarchyRulesSo.containsHierarchyRules)));
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(hierarchyRulesSo.componentHierarchyRules)));
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(hierarchyRulesSo.tagHierarchyRules)));
            GUILayout.Space(10);
        
            serializedObject.ApplyModifiedProperties();
            
            // Update button
            if(GUILayout.Button("Update")) 
                CustomHierarchy.Initialize();
        }
    }
}