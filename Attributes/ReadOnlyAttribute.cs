using UnityEngine;

namespace OutFoxeedTools.Attributes
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public bool readOnly = true;

        public ReadOnlyAttribute() => readOnly = true;
        public ReadOnlyAttribute(bool readOnly) => this.readOnly = readOnly;
    }
}