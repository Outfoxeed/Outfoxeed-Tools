using OutfoxeedTools.Editor;

namespace OutfoxeedTools.CustomHierarchy
{
    [System.Serializable]
    public class HierarchyRule
    {
        public string value;
        public HierarchySkin skin;
    }
    
    [System.Serializable]
    public class ComponentHierarchyRule : HierarchyRule
    {
        public System.Type ComponentType { get; private set; }

        public void SetComponentType()
        {
            ComponentType = TypeUtilities.StringToType(value);
        }
    }
}



