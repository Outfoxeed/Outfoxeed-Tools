using UnityEditor;
using UnityEngine;

namespace OutFoxeedTools.TransformInspector.Editor
{
    [CustomEditor(typeof(Transform))]
    public class TransformInspector : UnityEditor.Editor
    {
        private Transform _transform;

        private Color darkButtonColor = new Color(0.2f, 0.2f, 0.2f);

        void OnEnable()
        {
            _transform = target as Transform;
        }

        public override void OnInspectorGUI()
        {
            Vector3 localPosition = _transform.localPosition;
            Vector3 eulerAngles = _transform.eulerAngles;
            Vector3 localScale = _transform.localScale;

            localPosition = Vector3AndButtons("Position", localPosition, 0.8f);
            eulerAngles = Vector3AndButtons("Rotation", eulerAngles, 0.8f);
            localScale = Vector3AndButtons("Scale", localScale, 0.8f);

            #region Last line
            using (new GUILayout.HorizontalScope())
            {
                GUILayoutOption[] smallButtonsOptions = {GUILayout.Width(Screen.width * 0.15f)};
                GUI.backgroundColor = darkButtonColor;
                GUI.contentColor = Color.green;

                // Copy Button
                if (GUILayout.Button("Copy", smallButtonsOptions))
                {
                    CopyPasteManager.StoreTransformData(_transform);
                }

                GUI.contentColor = Color.red;

                // Paste button
                if (GUILayout.Button("Paste", smallButtonsOptions))
                {
                    CopyPasteManager.TransformData data = CopyPasteManager.GetTransformData();
                    localPosition = data.position.ToVector3();
                    eulerAngles = data.eulerAngles.ToVector3();
                    localScale = data.localScale.ToVector3();

                    Undo.RecordObject(_transform, $"Pasted world position on {_transform.name} Transform");
                    _transform.localPosition = localPosition;
                    _transform.eulerAngles = eulerAngles;
                    _transform.localScale = localScale;
                }

                GUI.backgroundColor = Color.grey;
                GUI.contentColor = Color.white;

                // Separator
                GUILayout.Label("", GUILayout.Width(Screen.width * 0.40f));

                GUILayoutOption[] options = {GUILayout.Height(20), GUILayout.Width(Screen.width * 0.2f)};

                // Reset button
                if (GUILayout.Button("Reset", options))
                {
                    localPosition = Vector3.zero;
                    eulerAngles = Vector3.zero;
                    localScale = Vector3.one;

                    Undo.RecordObject(_transform, $"Reset Transform of {_transform.name} GameObject");
                    _transform.localPosition = localPosition;
                    _transform.eulerAngles = eulerAngles;
                    _transform.localScale = localScale;
                }
            }

            #endregion

            #region Apply transform modification

            if (localPosition != _transform.localPosition)
            {
                Undo.RecordObject(_transform, $"Moved {_transform.name} GameObject");
                _transform.localPosition = localPosition;
            }

            if (eulerAngles != _transform.eulerAngles)
            {
                Undo.RecordObject(_transform, $"Rotated {_transform.name} GameObject");
                _transform.eulerAngles = eulerAngles;
            }

            if (localScale != _transform.localScale)
            {
                Undo.RecordObject(_transform, $"Scaled {_transform.name} GameObject");
                _transform.localScale = localScale;
            }

            #endregion
        }

        Vector3 Vector3AndButtons(string label, Vector3 value, float vector3FieldWidthPercentage)
        {
            float buttonWidth = Screen.width * ((1 - vector3FieldWidthPercentage) * 0.3f);
            using (new GUILayout.HorizontalScope())
            {
                value = CustomVector3Field(label, value, vector3FieldWidthPercentage * Screen.width);

                GUI.contentColor = Color.green;
                GUI.backgroundColor = darkButtonColor;
                if (GUILayout.Button("C", GUILayout.Width(buttonWidth)))
                {
                    CopyPasteManager.StoreTransformDataPart(label, value);
                }

                GUI.contentColor = Color.red;
                if (GUILayout.Button("P", GUILayout.Width(buttonWidth)))
                {
                    value = CopyPasteManager.GetTransformDataPart(label);
                }

                GUI.backgroundColor = Color.white;
                GUI.contentColor = Color.white;
            }

            return value;
        }

        Vector3 CustomVector3Field(string label, Vector3 value, float width)
        {
            Vector3 result;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(label, GUILayout.Width(width * 0.2f));

                GUILayoutOption[] options = {GUILayout.Width(width * 0.8f)};
                result = EditorGUILayout.Vector3Field("", value, options);
            }

            return result;
        }
    }
}