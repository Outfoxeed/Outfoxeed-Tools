using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.MeshTools.Editor
{
    public static class MeshUtility
    {
        #region Mesh combines
        [MenuItem("OutFoxeed/MeshUtility/Combine meshes of Selection's childs", false, 0)]
        public static void CombineMeshesOfSelectionChilds()
        {
            if (Selection.activeGameObject == null) return;
            CombineMeshesOfParent(Selection.activeGameObject, Selection.activeGameObject, true);
        }

        [MenuItem("OutFoxeed/MeshUtility/Combine selected meshes", false, 0)]
        public static GameObject CombineSelectedMeshes()
        {
            if (Selection.gameObjects.Length == 0) return null;

            GameObject target = new GameObject("Selections Merge Result");
            CombineSelectedMeshesOnTarget(target);

            return target;
        }
        public static void CombineSelectedMeshesOnTarget(GameObject target)
        {
            if (Selection.gameObjects.Length == 0) return;

            MeshRF[] meshRfs = GetGameObjectsMeshRfs(Selection.gameObjects);
            CombinesMeshes(meshRfs, target, false);
        }

        public static void CombinesMeshes(MeshRF[] toCombine, GameObject target, bool disableUsedMeshes)
        {
            // Move the group of objects to 0,0,0
            Vector3 meshesCenter = Vector3.zero;
            foreach (MeshRF meshRf in toCombine)
            {
                meshesCenter += meshRf.filter.transform.position;
            }
            meshesCenter /= toCombine.Length;
            foreach (MeshRF meshRf in toCombine)
            {
                meshRf.filter.transform.position -= meshesCenter;
            }

            // For collecting all materials
            Material[] allMaterials = new Material[0];

            // Create mesh
            Mesh finalMesh = CreateMeshFromMeshes(toCombine, out allMaterials);

            // Push back the meshes where they were
            foreach (MeshRF meshRf in toCombine)
            {
                meshRf.filter.transform.position += meshesCenter;
            }

            // Objects to record
            List<UnityEngine.Object> objectsToRecord = new List<UnityEngine.Object>();
            // Add filters gameobjects to objects to record (GameObjects disabled after)
            if (disableUsedMeshes)
                foreach (MeshRF meshRf in toCombine) objectsToRecord.Add(meshRf.filter.gameObject);
            // Add parent components, Transform and GameObject
            objectsToRecord.Add(target);
            objectsToRecord.Add(target.transform);

            Undo.RecordObjects(objectsToRecord.ToArray(), $"Combine {toCombine.Length} meshes to {target.name} GameObject");

            // Apply mesh and materials
            MeshRF targetMeshRf = ForceGetGameObjectMeshRf(target);
            targetMeshRf.filter.sharedMesh = finalMesh;
            targetMeshRf.renderer.sharedMaterials = allMaterials;
            target.transform.position = meshesCenter;

            // Disable all filter/renderer childs
            if (disableUsedMeshes)
                foreach (MeshRF meshRf in toCombine) meshRf.filter.gameObject.SetActive(false);
        }
        public static void CombineMeshesOfParent(GameObject parent, GameObject target, bool disableParentChilds)
        {
            Vector3 parentStartPos = parent.transform.position;
            Quaternion parentStartRot = parent.transform.rotation;
            Vector3 parentStartScale = parent.transform.localScale;
            Undo.RecordObject(parent.transform, "Reset transform for meshes merge");
            parent.transform.position = Vector3.zero;
            parent.transform.rotation = Quaternion.identity;
            parent.transform.localScale = Vector3.one;

            // Get Mesh renderer and filter of each child
            MeshRF[] childsMeshRfs = GetChildsMeshRfs(parent);
            if (childsMeshRfs.Length == 0) return;

            Material[] allChildsMaterials = new Material[0];
            Mesh finalMesh = CreateMeshFromMeshes(childsMeshRfs, out allChildsMaterials);

            // Objects to record
            List<UnityEngine.Object> objectsToRecord = new List<UnityEngine.Object>();
            // Add filters gameobjects to objects to record (GameObjects disabled after)
            if (disableParentChilds)
                foreach (MeshRF meshRf in childsMeshRfs) objectsToRecord.Add(meshRf.filter.gameObject);
            // Add parent components, Transform and GameObject
            objectsToRecord.Add(parent);
            objectsToRecord.Add(parent.transform);
            objectsToRecord.Add(target);

            Undo.RecordObjects(objectsToRecord.ToArray(), $"Combine meshes of {parent.name}'s childs to {target.name} GameObject");

            // Apply mesh and materials
            MeshRF targetMeshRf = ForceGetGameObjectMeshRf(target);
            targetMeshRf.filter.sharedMesh = finalMesh;
            targetMeshRf.renderer.sharedMaterials = allChildsMaterials;

            // Disable all fiter/renderers childs
            if (disableParentChilds)
                foreach (MeshRF meshRf in childsMeshRfs) meshRf.filter.gameObject.SetActive(false);

            // Relocate parent
            parent.transform.localScale = parentStartScale;
            parent.transform.rotation = parentStartRot;
            parent.transform.position = parentStartPos;

            // Enable parent
            parent.SetActive(true);
        }
        #endregion

        #region Mesh saving
        [MenuItem("OutFoxeed/MeshUtility/Save selected mesh", false, 100)]
        public static void SaveSelectionMesh()
        {
            if (Selection.activeGameObject == null) return;
            MeshFilter filter = Selection.activeGameObject.GetComponent<MeshFilter>();
            if (filter == null) return;

            SaveMesh(filter.sharedMesh, "mesh", false, false);
        }
        public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
        {
            if (mesh == null) return;

            string path = EditorUtility.SaveFilePanel("Save Asset", "Assets/", name, "asset");
            if (string.IsNullOrEmpty(path)) return;

            path = FileUtil.GetProjectRelativePath(path);

            Mesh meshToSave = (makeNewInstance) ? UnityEngine.Object.Instantiate(mesh) as Mesh : mesh;

            if (optimizeMesh) UnityEditor.MeshUtility.Optimize(meshToSave);

            AssetDatabase.CreateAsset(meshToSave, path);
            AssetDatabase.SaveAssets();
        }
        #endregion

        // Get Mesh renderers and mesh filters
        static MeshRF[] GetChildsMeshRfs(GameObject parent)
        {
            List<MeshRF> meshRfs = new List<MeshRF>();

            MeshFilter parentMeshFilter = parent.GetComponent<MeshFilter>();

            List<MeshFilter> meshFilters = new List<MeshFilter>();
            parent.GetComponentsInChildren<MeshFilter>(true, meshFilters);
            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (meshFilter == parentMeshFilter) continue;

                MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRfs.Add(new MeshRF(meshRenderer, meshFilter));
                }
            }

            return meshRfs.ToArray();
        }
        public static MeshRF[] GetGameObjectsMeshRfs(GameObject[] gameObjects)
        {
            List<MeshRF> results = new List<MeshRF>();
            foreach (GameObject gameObject in gameObjects)
            {
                MeshRF? newMeshRf = GetGameObjectMeshRf(gameObject);
                if (newMeshRf != null) results.Add((MeshRF)newMeshRf);
            }
            return results.ToArray();
        }
        static MeshRF? GetGameObjectMeshRf(GameObject gameObject)
        {
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            if (filter == null || renderer == null) return null;

            MeshRF? result = new MeshRF(renderer, filter);
            return result;
        }
        static MeshRF ForceGetGameObjectMeshRf(GameObject gameObject)
        {
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            if (filter == null)
            {
                Undo.RecordObject(gameObject, "Add Mesh Filter");
                filter = gameObject.AddComponent<MeshFilter>();
            }

            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                Undo.RecordObject(gameObject, "Add Mesh Renderer");
                renderer = gameObject.AddComponent<MeshRenderer>();
            }

            return new MeshRF(renderer, filter);
        }

        // Create mesh from meshes
        #region Create Mesh from meshes
        static Mesh CreateMeshFromMeshes(MeshRF[] meshRfs, out Material[] materials)
        {
            materials = GetMaterialsFromMeshRFs(meshRfs);
            CombineInstance[] combinesInstances = GetCombineInstanceForEachMaterial(meshRfs, materials);

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combinesInstances, false);

            return mesh;
        }
        static Material[] GetMaterialsFromMeshRFs(MeshRF[] meshRfs)
        {
            List<Material> allMaterials = new List<Material>();
            foreach (MeshRF meshRf in meshRfs)
            {
                foreach (Material sharedMat in meshRf.renderer.sharedMaterials)
                {
                    if (!allMaterials.Contains(sharedMat)) allMaterials.Add(sharedMat);
                }
            }
            return allMaterials.ToArray();
        }
        static CombineInstance[] GetCombineInstanceForEachMaterial(MeshRF[] meshRfs, Material[] materials)
        {
            // Get a sub mesh for each material
            List<Mesh> materialSubMeshes = new List<Mesh>();
            foreach (Material mat in materials)
            {
                List<CombineInstance> combineInstances = new List<CombineInstance>();

                foreach (MeshRF meshRf in meshRfs)
                {
                    Material[] rendererMaterials = meshRf.renderer.sharedMaterials;
                    for (int i = 0; i < rendererMaterials.Length; i++)
                    {
                        if (rendererMaterials[i] != mat) continue;

                        CombineInstance combineInstance = new CombineInstance();
                        combineInstance.mesh = meshRf.filter.sharedMesh;
                        combineInstance.subMeshIndex = i;
                        combineInstance.transform = meshRf.filter.transform.localToWorldMatrix;

                        combineInstances.Add(combineInstance);
                    }
                }
                //Debug.Log(combineInstances.Count);

                // Combine all CombineInstances related of this material
                Mesh mesh = new Mesh();
                mesh.CombineMeshes(combineInstances.ToArray(), true);
                materialSubMeshes.Add(mesh);
            }
            //Debug.Log(materialSubMeshes.Count);

            // From meshes of each material, create CombineInstances
            List<CombineInstance> materialsCombineInstances = new List<CombineInstance>();
            foreach (Mesh subMesh in materialSubMeshes)
            {
                CombineInstance combineInstance = new CombineInstance();
                combineInstance.mesh = subMesh;
                //combineInstance.subMeshIndex = 0;
                combineInstance.transform = Matrix4x4.identity;
                materialsCombineInstances.Add(combineInstance);
            }
            //Debug.Log(finalCombineInstances.Count);

            return materialsCombineInstances.ToArray();
        }
        #endregion

        // Debug
        static GameObject CreateMesh(Mesh mesh, string name = "CreatedMesh")
        {
            GameObject gameObject = new GameObject(name);
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();

            filter.sharedMesh = mesh;
            renderer.sharedMaterial = new Material(Shader.Find("Standard"));
            renderer.sharedMaterial.color = Color.magenta;

            return gameObject;
        }

        // Struct containing Mesh Renderer and Mesh Filter
        public struct MeshRF
        {
            public MeshRenderer renderer;
            public MeshFilter filter;

            public MeshRF(MeshRenderer meshRenderer, MeshFilter meshFilter)
            {
                renderer = meshRenderer;
                filter = meshFilter;
            }
        }
    }
}
