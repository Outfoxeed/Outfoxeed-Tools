using OutfoxeedTools.Editor;
using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.MeshTools.Editor
{
    public class MeshToolsWindow : EditorWindow
    {
        [MenuItem("OutFoxeed/Mesh Tools Window", false, -95)]
        public static void ShowWindow()
        {
            GetWindow<MeshToolsWindow>("Mesh Tools");
        }

        // Mesh merge vars
        private GameObject meshMergeTarget;
            // Selected
        private string selectionMergeResultName;
            // From parent
        private Transform meshesParent;
        private const string meshMergeTargetTitle = "Target of mesh merge";
        private bool disableChildsAfterMerge;
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

        void OnGUI()
        {
            GUILayoutOption scrollWidthOption = GUILayout.Width(Screen.width - 10);
            
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            // Meshes combine
            #region Meshes combine
            GUILayout.Label("Merge meshes", WindowUtility.TitleStyle);
            GUILayout.Space(15);

                #region Merge selected meshes
            GUILayout.Label("Selected meshes", WindowUtility.SubTitleStyle);
            using (new GUILayout.VerticalScope())
            {
                // Target of mesh merge field
                using (new GUILayout.HorizontalScope(scrollWidthOption))
                {
                    meshMergeTarget = WindowUtility.ObjectFieldWithEraseButton<GameObject>("Target of mesh merge", meshMergeTarget);
                }

                // Merge result name if (target merge is null)
                if (meshMergeTarget == null)
                {
                    using (new GUILayout.HorizontalScope(scrollWidthOption))
                    {
                        selectionMergeResultName = WindowUtility.TextFieldWithDefaultButton("Merge result name", 
                            selectionMergeResultName, "Merge result");
                    }
                }

                // Merge meshes button
                if (GUILayout.Button("Merge selected meshes", scrollWidthOption) && Selection.gameObjects.Length > 0)
                {
                    if (meshMergeTarget == null)
                    {
                        GameObject meshResultGameObject = MeshUtility.CombineSelectedMeshes();
                        meshResultGameObject.name = selectionMergeResultName;
                    }
                    else MeshUtility.CombineSelectedMeshesOnTarget(meshMergeTarget);
                }
                // Merge meshes and colliders button
                if (GUILayout.Button("Merge selected Meshes/Colliders", scrollWidthOption) && Selection.gameObjects.Length > 0)
                {
                    if (meshMergeTarget == null)
                    {
                        GameObject mergeResultGameObject = MeshUtility.CombineSelectedMeshes();
                        ColliderUtility.MergeColliders(Selection.gameObjects, mergeResultGameObject);
                        mergeResultGameObject.name = selectionMergeResultName;
                    }
                    else
                    {
                        MeshUtility.CombineSelectedMeshesOnTarget(meshMergeTarget);
                        ColliderUtility.MergeColliders(Selection.gameObjects, meshMergeTarget);
                    }
                }
            }
                #endregion

            GUILayout.Space(30);

                #region Merge childs of selection
            GUILayout.Label("Childs of a parent", WindowUtility.SubTitleStyle);
            using (new GUILayout.VerticalScope())
            {
                // Parent of meshes selection
                using (new GUILayout.HorizontalScope(scrollWidthOption))
                {
                    meshesParent = WindowUtility.ObjectFieldWithEraseButton("Parent of meshes", meshesParent);
                }
                
                // Disable parent's childs toggle
                using (new GUILayout.HorizontalScope(scrollWidthOption))
                {
                    disableChildsAfterMerge =
                        WindowUtility.LittleToggle("Disable parent's childs", disableChildsAfterMerge);
                }

                GUILayout.Space(5);

                // Merge meshes button
                if (GUILayout.Button("Merge meshes", scrollWidthOption))
                {
                    GameObject parent = null;
                    if (meshesParent != null) parent = meshesParent.gameObject;
                    if (parent == null)
                    {
                        if (Selection.activeGameObject == null) Debug.LogError(NoSelectionAndNoParentMessage);
                        else parent = Selection.activeGameObject;
                    }

                    GameObject target = meshMergeTarget;
                    if (target == null)
                    {
                        if (Selection.activeGameObject == null) Debug.LogError(NoSelectionAndNoMeshTargetMessage);
                        else target = Selection.activeGameObject;
                    }

                    if (parent != null && target != null)
                        MeshUtility.CombineMeshesOfParent(parent, target, disableChildsAfterMerge);
                }
                // Merge meshes and colliders button
                //if (GUILayout.Button("Merge Meshes/Colliders", scrollWidthOption))
                //{
                //    GameObject parent = null;
                //    if (meshesParent != null) parent = meshesParent.gameObject;
                //    if (parent == null)
                //    {
                //        if (Selection.activeGameObject == null) Debug.LogError(NoSelectionAndNoParentMessage);
                //        else parent = Selection.activeGameObject;
                //    }

                //    GameObject target = meshMergeTarget;
                //    if (target == null)
                //    {
                //        if (Selection.activeGameObject == null) Debug.LogError(NoSelectionAndNoMeshTargetMessage);
                //        else target = Selection.activeGameObject;
                //    }

                //    if (parent != null && target != null)
                //    {
                //        OutFoxeed.MeshUtility.CombineMeshesOfParent(parent, target, disableChildsAfterMerge);
                //        OutFoxeed.ColliderUtility.MergeCollidersOfParent(parent, target);
                //    }
                //}

                // Help button
                if (GUILayout.Button("HELP", scrollWidthOption)) Debug.Log(meshMergeHelpMessage);

                GUILayout.Space(5);

                // Target of the merge field
                using (new GUILayout.HorizontalScope(scrollWidthOption))
                {
                    meshMergeTarget = WindowUtility.ObjectFieldWithEraseButton<GameObject>("Target of mesh merge", meshMergeTarget);
                }
            }
                #endregion

            #endregion

            GUILayout.Space(40);

            // Mesh save
            #region Mesh save
            GUILayout.Label("Save Mesh", WindowUtility.TitleStyle);
            using (new GUILayout.VerticalScope())
            {
                // Mesh / Mesh Filter fields
                using (new GUILayout.HorizontalScope(scrollWidthOption))
                {
                    meshFilterToSave = WindowUtility.ObjectField<MeshFilter>("Mesh filter", meshFilterToSave);
                    meshToSave = WindowUtility.ObjectField<Mesh>("Mesh", meshToSave);
                }

                // Name field
                using (new GUILayout.HorizontalScope(scrollWidthOption))
                {
                    meshToSaveName = WindowUtility.TextFieldWithDefaultButton("Name",
                        meshToSaveName, "mesh");
                }

                // Other parameters
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("", GUILayout.Width(Screen.width * 0.2f));
                    using (new GUILayout.HorizontalScope(GUILayout.Width(Screen.width * 0.3f)))
                    {
                        GUILayout.Label("Optimize");
                        optimizeMeshOnSave = EditorGUILayout.Toggle(optimizeMeshOnSave);
                    }

                    using (new GUILayout.HorizontalScope(GUILayout.Width(Screen.width * 0.3f)))
                    {
                        GUILayout.Label("New instance");
                        createInstanceOnSave = EditorGUILayout.Toggle(createInstanceOnSave);
                    }
                }

                // Save Mesh button
                if (GUILayout.Button("Save Mesh", scrollWidthOption))
                {
                    if (meshToSave != null)
                        MeshUtility.SaveMesh(meshToSave, meshToSaveName, createInstanceOnSave,
                            optimizeMeshOnSave);
                    else if (meshFilterToSave != null)
                        MeshUtility.SaveMesh(meshFilterToSave.sharedMesh, meshToSaveName,
                            createInstanceOnSave, optimizeMeshOnSave);
                    else if (Selection.activeGameObject != null)
                    {
                        MeshFilter filter = Selection.activeGameObject.GetComponent<MeshFilter>();
                        if (filter != null)
                            MeshUtility.SaveMesh(filter.sharedMesh, meshToSaveName, createInstanceOnSave,
                                optimizeMeshOnSave);
                    }
                }
            }
            #endregion

            GUILayout.Space(10);

            EditorGUILayout.EndScrollView();
        }
    }
}