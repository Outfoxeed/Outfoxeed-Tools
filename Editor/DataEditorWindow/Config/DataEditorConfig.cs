using System;
using OutfoxeedTools.Editor;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Editor.DataEditorWindow
{
    [System.Serializable]
    public partial struct DataEditorConfig
    {
        public DataType[] dataTypes;
        
        public bool Equals(DataEditorConfig other)
        {
            if (dataTypes.Length != other.dataTypes.Length)
                return false;
            for (int i = 0; i < dataTypes.Length; i++)
            {
                if (!dataTypes[i].Equals(other.dataTypes[i]))
                    return false;
            }
            return true;
        }

        #region Static methods
        const string CONFIG_FILE_NAME = "data_editor_window_config";
        private static string configFilePath = "";
        public static string ConfigFilePath
        {
            get
            {
                if (configFilePath == "")
                {
                    string guid = AssetDatabase.FindAssets(CONFIG_FILE_NAME)[0];
                    if (string.IsNullOrEmpty(guid))
                    {
                        Debug.LogError($"Data Editor Config: {CONFIG_FILE_NAME} path not found");
                        return configFilePath;
                    }
                    configFilePath = AssetDatabase.GUIDToAssetPath(guid);
                }
                return configFilePath;
            }
        }
        public static bool LoadConfigData(out DataEditorConfig dataEditorConfig)
        {
            if (string.IsNullOrEmpty(ConfigFilePath))
            {
                Debug.LogError("Data Editor Config: couldn't load config data because the file path is null");
                dataEditorConfig = new DataEditorConfig(){dataTypes = Array.Empty<DataType>()};
                return false;
            }
            dataEditorConfig = FileReader.GetDataFromJson<DataEditorConfig>(ConfigFilePath);
            return true;
        }
        #endregion
    }
}