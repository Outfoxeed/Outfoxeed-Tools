using OutFoxeedTools.Editor;
using UnityEngine;

namespace OutFoxeedTools.TransformInspector.Editor
{
    public static class CopyPasteManager
    {
        // Enum and conversion
        #region Transform part enum
        public enum TransformPart
        {
            Position,
            EulerAngles,
            LocalScale
        };

        static TransformPart GetTransformPart(string value) => value switch
        {
            "Position" => TransformPart.Position,
            "Rotation" => TransformPart.EulerAngles,
            "Scale" => TransformPart.LocalScale,
            _ => TransformPart.Position
        };
        #endregion

        static string GetTransformDataPath()
        {
            return FileReader.GetFilePath("TransformData");
        }


        public static TransformData GetTransformData() 
            => FileReader.GetDataFromJson<TransformData>(GetTransformDataPath());
        public static void StoreTransformData(TransformData data)
            => FileReader.StoreDataOnJson(data, GetTransformDataPath());

        // Get part of TransformData
        #region Get part of stored TransformData
        public static Vector3 GetTransformDataPart(string part) => GetTransformDataPart(GetTransformPart(part));
        public static Vector3 GetTransformDataPart(TransformPart part)
        {
            TransformData data = GetTransformData();
            return part switch
            {
                TransformPart.Position => data.position.ToVector3(),
                TransformPart.EulerAngles => data.eulerAngles.ToVector3(),
                TransformPart.LocalScale => data.localScale.ToVector3(),
                _ => Vector3.zero
            };
        }
        #endregion

        // Store/Copy values 
        #region Store/Copy values methods
        // Store TransformData
        public static void StoreTransformData(Transform transform)
        {
            TransformData data = new TransformData(transform);
            StoreTransformData(data); 
        }

        // Change part of the stored TransformData
        public static void StoreTransformDataPart(string part, Vector3 value)
        {
            StoreTransformDataPart(GetTransformPart(part), value);
        }
        public static void StoreTransformDataPart(TransformPart part, Vector3 value)
        {
            TransformData transformData = GetTransformData();
            switch (part)
            {
                case TransformPart.Position:
                    transformData.position = new Vector3Data(value);
                    break;
                case TransformPart.EulerAngles:
                    transformData.eulerAngles = new Vector3Data(value);
                    break;
                case TransformPart.LocalScale:
                    transformData.localScale = new Vector3Data(value);
                    break;
            }

            StoreTransformData(transformData);
        }
        #endregion

        // Data structs
        #region Data structs
        public struct TransformData
        {
            public Vector3Data position;
            public Vector3Data eulerAngles;
            public Vector3Data localScale;

            public TransformData(Transform transform)
            {
                position = new Vector3Data(transform.position);
                eulerAngles = new Vector3Data(transform.eulerAngles);
                localScale = new Vector3Data(transform.localScale);
            }

            public override string ToString() =>
                $"Position = {position}, EulerAngles = {eulerAngles}, LocalScale = {localScale}";
        }

        public struct Vector3Data
        {
            public float x;
            public float y;
            public float z;

            public Vector3Data(Vector3 vector3)
            {
                x = vector3.x;
                y = vector3.y;
                z = vector3.z;
            }

            public Vector3 ToVector3() => new Vector3(x, y, z);
            public override string ToString() => $"{{x: {x}, y: {y}, z: {z}}}";
        }
        #endregion
    }
}