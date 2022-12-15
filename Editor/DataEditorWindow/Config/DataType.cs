namespace Editor.DataEditorWindow
{
    public partial struct DataEditorConfig
    {
        [System.Serializable]
        public struct DataType
        {
            public string typeName;
            public string creationPath;

            public bool Equals(DataType other)
                => typeName == other.typeName && creationPath == other.creationPath;
        }
    }
}