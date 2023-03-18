using UnityEngine;

namespace OutfoxeedTools.Attributes
{
    public class LabelAttribute : PropertyAttribute
    {
        public string label;
        public LabelAttribute(string label) => this.label = label;
    }
}