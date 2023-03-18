using System.Collections.Generic;
using OutfoxeedTools.Editor;

namespace OutfoxeedTools.SceneTools.Editor
{
    public static class TypesFinder
    {
        public static System.Type[] GetTypesToFind()
        {
            List<System.Type> tempTypes = new List<System.Type>();

            foreach (string line in GetEachPrefs())
            {
                if (line == "") continue;

                System.Type type = TypeUtilities.StringToType(line);

                if (type != null) tempTypes.Add(type);
            }
            return tempTypes.ToArray();
        }
        static string GetTypesToFindFilePath()
        {
            return FileReader.GetFilePath("typesToFind");
        }

        static string[] GetEachPrefs()
        {
            return FileReader.GetLines(GetTypesToFindFilePath());
        }

        public static void AddToPrefs(string newLine)
        {
            FileReader.AddLine(newLine, GetTypesToFindFilePath());
        }
        public static void RemoveFromPrefs(System.Type typeToRemove)
        {
            FileReader.RemoveFromTextFile(typeToRemove.ToString().Remove(0, "UnityEngine.".Length),GetTypesToFindFilePath());
        }
    }
}
