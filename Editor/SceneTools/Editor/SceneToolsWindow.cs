using System;
using OutfoxeedTools.Editor;
using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.SceneTools.Editor
{
    public class SceneToolsWindow : EditorWindow
    {
        [MenuItem("OutFoxeed/Scene Tools Window", false, -100)]
        public static void ShowWindow()
        {
            GetWindow<SceneToolsWindow>("Scene Tools");
        }

        private Color baseColor;

        // Time scale
        private float timeScale;

        // Types finder vars
        System.Type[] typesToFind;
        void ResetTypesToFind() => typesToFind = TypesFinder.GetTypesToFind();
        private bool editingTypes;
        private string typeToAdd;

        // Tp vars
        private float surfaceOffset;
        private bool smartTp;

        // Mesh merge vars
        private Transform meshesParent;
        private const string meshMergeTargetTitle = "Target of mesh merge";
        private bool disableChildsAfterMerge;
        private GameObject meshMergeTarget;
        private const string meshesParentTitle = "Parent of meshes";

        // Mesh save vars
        private bool optimizeMeshOnSave;
        private bool createInstanceOnSave;
        private string meshToSaveName;
        private MeshFilter meshFilterToSave;
        private Mesh meshToSave;

        #region Messages
        private readonly string meshMergeHelpMessage =
            $"<color=white>'{meshesParentTitle}'</color><color=yellow> is the parent of all the meshes you want to combine </color>\n "
            + "<color=yellow>If null, your selection in the scene will be taken </color>\n "
            + $"<color=white>'{meshMergeTargetTitle}'</color><color=yellow> is the object that will get the result of the merge </color>\n"
            + "<color=yellow>If null, your selection in the scene will be taken</color>";
        private readonly string NoSelectionAndNoParentMessage =
            $"Please select something in the scene as a target of the merge or fill the '{meshesParentTitle}' field";
        private readonly string NoSelectionAndNoMeshTargetMessage =
            $"Please select something in the scene as a target of the merge or fill the '{meshMergeTargetTitle}' field";
        #endregion

        private Vector2 scrollPos;

        void OnEnable()
        {
            timeScale = 1f;
            ResetTypesToFind();

            baseColor = GUI.contentColor;
        }


        private void OnGUI()
        {
            GUILayoutOption scrollWidthOption = GUILayout.Width(Screen.width - 10);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            //if(GUILayout.Button("Idk")) CustomFolders.IDK();
            GUILayout.Space(10);

            #region Time scale gestion
            GUILayout.Label("Time scale gestion", WindowUtility.TitleStyle, scrollWidthOption);
            using (new GUILayout.HorizontalScope(scrollWidthOption))
            {
                if (GUILayout.Button("-"))
                {
                    timeScale--;
                    SetTimeScale();
                }
                if (GUILayout.Button("+"))
                {
                    timeScale++;
                    SetTimeScale();
                }
            }
            float tempValue = EditorGUILayout.Slider(timeScale, 0f, 1f);
            if (tempValue > 0 && tempValue < 1)
            {
                timeScale = tempValue;
                SetTimeScale();
            }
            using (new GUILayout.HorizontalScope(scrollWidthOption))
            {
                tempValue = WindowUtility.FloatField("Time scale", timeScale, 1f);
                if (tempValue != timeScale)
                {
                    timeScale = tempValue;
                    SetTimeScale();
                }
            }
            #endregion

            GUILayout.Space(20);

            #region Types finding
            GUILayout.Label("Find object of type", WindowUtility.TitleStyle);

            // For each Type in preferences --> Find buttons
            foreach (System.Type type in typesToFind)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    // Delete button
                    if (editingTypes)
                    {
                        GUI.contentColor = Color.red;
                        if (GUILayout.Button("X", GUILayout.Width(30)))
                        {
                            TypesFinder.RemoveFromPrefs(type);
                            Debug.Log($"Scene Tools: <color=red>Type <color=white>'{type.ToString()}'</color> successfully removed</color>");
                            ResetTypesToFind();
                        }
                        GUI.contentColor = baseColor;
                    }

                    // Find button
                    if (GUILayout.Button("Find " + type.ToString().Split('.')[1]))
                    {
                        TrySelectObjectOfType(type);
                    }

                    // Find all buttons
                    if (GUILayout.Button("All", GUILayout.Width(75)))
                    {
                        TrySelectObjectsOfType(type);
                    }
                }
            }

            // Add new types field and buttons
            if (editingTypes)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Help"))
                    {
                        Debug.Log("Scene Tools: <color=yellow>OutFoxeed tool: Enter type name in field and push 'Add'</color>");
                    }
                    typeToAdd = EditorGUILayout.TextField(typeToAdd);
                    if (GUILayout.Button("Add"))
                    {
                        if (TypeUtilities.StringToType(typeToAdd) == null) Debug.LogError("Scene Tools: Not a valid type");
                        else
                        {
                            TypesFinder.AddToPrefs(typeToAdd);
                            Debug.Log($"Scene Tools: <color=green>Type <color=white>'{typeToAdd.ToString()}'</color> successfully added</color>");
                            ResetTypesToFind();
                        }
                    }
                }
            }

            // Toggle edit types mode
            using (new EditorGUILayout.HorizontalScope())
            {
                if (editingTypes) GUI.contentColor = Color.red;
                if (GUILayout.Button(editingTypes ? "Stop editing types list" : "Edit types list"))
                {
                    editingTypes = !editingTypes;
                    typeToAdd = "";
                }
                GUI.contentColor = baseColor;
            }

            #endregion

            GUILayout.Space(20);

            #region Tp Selection next camera
            GUILayout.Label("Tp Selection", WindowUtility.TitleStyle);

            // Tp to cam
            if (GUILayout.Button("Tp to camera"))
            {
                if (Selection.activeGameObject != null)
                {
                    Undo.RecordObject(Selection.activeGameObject.transform, "Teleported selection to camera");
                    Selection.activeGameObject.transform.position = SceneView.GetAllSceneCameras()[0].transform.position;
                }
            }

            // Tp on surface below cam
            using (new EditorGUILayout.HorizontalScope())
            {
                // Y offset
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Y Offset", GUILayout.Width(50));
                    surfaceOffset = EditorGUILayout.FloatField(surfaceOffset, GUILayout.Width(50));
                }

                // Tp button
                GUILayoutOption[] layoutOptions = { GUILayout.MinWidth(position.width * 0.4f), GUILayout.MaxWidth(position.width * 0.8f) };
                if (GUILayout.Button("Tp Object on surface", layoutOptions))
                {
                    if (Selection.activeGameObject != null)
                    {
                        Transform transform = Selection.activeGameObject.transform;

                        Vector3 wantedPos = SceneView.GetAllSceneCameras()[0].transform.position;
                        RaycastHit hit;
                        if (Physics.Raycast(wantedPos, Vector3.down, out hit, 100f, ~0, QueryTriggerInteraction.Ignore))
                        {
                            wantedPos = hit.point + Vector3.up * surfaceOffset;
                            if (smartTp) wantedPos += Vector3.up * transform.localScale.y * 0.5f;
                        }
                        else
                        {
                            Debug.LogWarning("Scene Tools: Surface not found underneath the camera position");
                        }

                        Undo.RecordObject(transform, "Teleported player to scene camera");
                        transform.position = wantedPos;
                        transform.eulerAngles = Vector3.zero;
                    }
                }

                // Smart tp toggle
                using (new EditorGUILayout.HorizontalScope())
                { 
                    GUILayout.Label(new GUIContent("Smart TP", "Will try to put the object right on the floor with a y offset of 0"),GUILayout.Width(55));
                    smartTp = EditorGUILayout.Toggle(smartTp);
                }
            }
            #endregion

            GUILayout.Space(10);

            EditorGUILayout.EndScrollView();
        }

        void SetTimeScale()
        {
            if (!Application.isPlaying) return;
            Time.timeScale = timeScale;
        }

        #region Try Select functions
        void TrySelectObjectOfType(Type type)
        {
            UnityEngine.Object selection = FindObjectOfType(type);
            if (selection != null) Selection.activeObject = selection;
        }
        void TrySelectObjectsOfType(Type type)
        {
            UnityEngine.Object[] objs = FindObjectsOfType(type);
            if (objs.Length > 0) Selection.objects = objs;
        }
        #endregion
    }

}
