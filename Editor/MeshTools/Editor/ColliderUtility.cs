using UnityEditor;
using UnityEngine;

namespace OutfoxeedTools.MeshTools.Editor
{
    public static class ColliderUtility
    {
        [MenuItem("OutFoxeed/Merge selected colliders", false, 2000)]
        public static void MergeSelectedColliders()
        {
            if (Selection.gameObjects.Length == 0) return;

            GameObject target = new GameObject("Colliders merge result");
            Vector3 center = Vector3.zero;
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                center += gameObject.transform.position;
            }
            center /= Selection.gameObjects.Length;
            target.transform.position = center;

            MergeColliders(Selection.gameObjects, target);
        }

        //public static void MergeCollidersOfParent(GameObject parent, GameObject target)
        //{
        //    GameObject[] childs = new GameObject[parent.transform.childCount];
        //    for (int i = 0; i < childs.Length; i++)
        //    {
        //        childs[i] = parent.transform.GetChild(i).gameObject;
        //    }

        //    Vector3 center = GetCenter(childs);

        //    foreach (GameObject child in childs)
        //    {
        //        // Copy Box colliders
        //        {
        //            if (child.TryGetComponent<BoxCollider>(out BoxCollider selectedBoxCollider))
        //            {
        //                BoxCollider newCollider = target.AddComponent<BoxCollider>();
        //                newCollider.center = child.transform.position - target.transform.position + selectedBoxCollider.center;
        //                newCollider.size = Vector3Multiplication(selectedBoxCollider.size, selectedBoxCollider.transform.localScale);
        //            }
        //        }

        //        // Copy sphere collider
        //        {
        //            if (child.TryGetComponent<SphereCollider>(out SphereCollider selectedSphereCollider))
        //            {
        //                SphereCollider newCollider = target.AddComponent<SphereCollider>();
        //                newCollider.center = child.transform.position - target.transform.position + selectedSphereCollider.center; 

        //                float multiplier = Mathf.Max(Mathf.Max(child.transform.localScale.x, child.transform.localScale.y), child.transform.localScale.z);
        //                newCollider.radius = multiplier * selectedSphereCollider.radius;
        //            }
        //        }

        //        // Copy capsule collider
        //        {
        //            if (child.TryGetComponent<CapsuleCollider>(out CapsuleCollider selectedCapsuleCollider))
        //            {
        //                CapsuleCollider newCollider = target.AddComponent<CapsuleCollider>();
        //                newCollider.center = child.transform.position - target.transform.position + selectedCapsuleCollider.center;

        //                int direction = selectedCapsuleCollider.direction;
        //                newCollider.direction = direction;
        //                newCollider.height = selectedCapsuleCollider.height * child.transform.lossyScale[direction];

        //                float biggestAxisScale = 0;
        //                for (int i = 0; i < 3; i++)
        //                {
        //                    if (i == direction) continue;
        //                    if (child.transform.lossyScale[i] > biggestAxisScale)
        //                        biggestAxisScale = child.transform.lossyScale[i];
        //                }
        //                newCollider.radius = biggestAxisScale * selectedCapsuleCollider.radius;
        //            }
        //        }
        //    }

        //}
        public static void MergeColliders(GameObject[] gameObjects, GameObject target)
        {
            // Get center
            Vector3 center = GetCenter(gameObjects);

            foreach (GameObject selectedGameObject in gameObjects)
            {
                // Copy Box colliders
                {
                    if (selectedGameObject.TryGetComponent<BoxCollider>(out BoxCollider selectedBoxCollider))
                    {
                        BoxCollider newCollider = target.AddComponent<BoxCollider>();
                        newCollider.center = selectedGameObject.transform.position - target.transform.position + Vector3Multiplication(selectedBoxCollider.center, target.transform.lossyScale);

                        newCollider.size = Vector3Multiplication(selectedGameObject.transform.lossyScale, selectedBoxCollider.size);
                    }
                }

                // Copy sphere collider
                {
                    if (selectedGameObject.TryGetComponent<SphereCollider>(out SphereCollider selectedSphereCollider))
                    {
                        SphereCollider newCollider = target.AddComponent<SphereCollider>();
                        newCollider.center = selectedGameObject.transform.position - target.transform.position + Vector3Multiplication(selectedSphereCollider.center, target.transform.lossyScale);

                        float multiplier = Mathf.Max(Mathf.Max(selectedGameObject.transform.lossyScale.x, selectedGameObject.transform.lossyScale.y), selectedGameObject.transform.lossyScale.z);
                        newCollider.radius = multiplier * selectedSphereCollider.radius;
                    }
                }

                // Copy capsule collider
                {
                    if (selectedGameObject.TryGetComponent<CapsuleCollider>(out CapsuleCollider selectedCapsuleCollider))
                    {
                        CapsuleCollider newCollider = target.AddComponent<CapsuleCollider>();
                        newCollider.center = selectedGameObject.transform.position - target.transform.position + Vector3Multiplication(selectedCapsuleCollider.center, target.transform.lossyScale);

                        int direction = selectedCapsuleCollider.direction;
                        newCollider.direction = direction;
                        newCollider.height = selectedCapsuleCollider.height * selectedGameObject.transform.lossyScale[direction];

                        float biggestAxisScale = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            if (i == direction) continue;
                            if (selectedGameObject.transform.lossyScale[i] > biggestAxisScale) 
                                biggestAxisScale = selectedGameObject.transform.lossyScale[i];
                        }
                        newCollider.radius = biggestAxisScale * selectedCapsuleCollider.radius;
                    }
                }
            }
        }

        static Vector3 Vector3Multiplication(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        static Vector3 GetCenter(GameObject[] gameObjects)
        {
            Vector3 center = Vector3.zero;
            foreach (GameObject gameObject in gameObjects)
            {
                center += gameObject.transform.position;
            }
            center /= gameObjects.Length;
            return center;
        }
    }
}