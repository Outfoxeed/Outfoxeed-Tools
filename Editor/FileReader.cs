using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;

namespace OutfoxeedTools.Editor
{
    public static class FileReader
    {
        public static string GetFilePath(string fileName)
        {
            string guid = AssetDatabase.FindAssets(fileName)[0];
            if (string.IsNullOrEmpty(guid)) return $"{fileName} path not found";
            return AssetDatabase.GUIDToAssetPath(guid);
        }

        #region Text file manipulation
        public static string GetTextFromFile(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            string result = reader.ReadToEnd();
            reader.Close();
            return result;
        }
        public static string[] GetLines(string filePath)
        {
            List<string> lines = new List<string>();

            StreamReader reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }
            reader.Close();

            return lines.ToArray();
        }

        public static void SetFileText(string newText, string filePath)
        {
            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(newText);
            writer.Close();
        }

        public static void AddLine(string newLine, string filePath)
        {
            string[] currentLines = GetLines(filePath);

            StreamWriter writer = new StreamWriter(filePath);

            foreach (string pref in currentLines)
            {
                writer.WriteLine(pref);
            }
            writer.Write(newLine);

            writer.Close();
        }
        public static void RemoveFromTextFile(string textToRemove, string filePath)
        {
            string text = GetTextFromFile(filePath);
            SetFileText(text.Replace(textToRemove, ""), filePath);
        }
        #endregion

        #region Json Manipulation
        public static T GetDataFromJson<T>(string jsonPath) where T : struct
        {
            using (StreamReader reader = new StreamReader(jsonPath))
            {
                string json = reader.ReadToEnd();
                T result = JsonConvert.DeserializeObject<T>(json);
                reader.Close();

                return result;
            }
        }
        public static void StoreDataOnJson<T>(T data, string jsonPath) where T : struct
        {
            using (StreamWriter writer = new StreamWriter(jsonPath))
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                writer.Write(json);
                writer.Close();
            }
        }
        #endregion
    }
}