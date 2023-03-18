namespace OutfoxeedTools.Editor
{
    public static class TypeUtilities
    {
        public static System.Type StringToType(string text)
        {
            System.Type type = System.Type.GetType("UnityEngine." + text + ", " + "UnityEngine");
            type ??= System.Type.GetType(text);
            return type;
        }
    }
}