// using UnityEditor;
// using UnityEngine;
//
// namespace OutFoxeed.UsefulStructs.Editor
// {
//     [CustomPropertyDrawer(typeof(Array2D<>))]
//     public class Array2DPropertyDrawer : PropertyDrawer
//     {
//         private const string array2DName = "arrays";
//         
//         public override void OnGUI(Rect position, SerializedProperty property,
//             GUIContent label)
//         {
//             property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(position, property.isExpanded, label);
//             if (property.isExpanded)
//             {
//                 var array2DProp = property.FindPropertyRelative(array2DName);
//                 for (int i = 0; i < array2DProp.arraySize; i++)
//                 {
//                     EditorGUI.PropertyField(position, array2DProp.GetArrayElementAtIndex(i));
//                 }
//             }
//             EditorGUI.EndFoldoutHeaderGroup();
//         }
//
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             var array2DProp = property.FindPropertyRelative(array2DName);
//             
//             var lineCount = 1f;
//             if (property.isExpanded)
//                 lineCount += 10;
//             // if (array2DProp.isExpanded)
//             // {
//             //     Debug.Log("expanded");
//             //     lineCount++;
//             //     for (int i = 0; i < array2DProp.arraySize; i++)
//             //     {
//             //         var arrayProp = array2DProp.GetArrayElementAtIndex(i);
//             //         lineCount += ArrayPropertyDrawer.GetPropertyLineHeight(arrayProp);
//             //     }
//             // }
//             
//             return base.GetPropertyHeight(property, label) * lineCount;
//         }
//     }
// }