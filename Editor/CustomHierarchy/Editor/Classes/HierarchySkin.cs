using System;
using UnityEngine;

namespace OutfoxeedTools.CustomHierarchy
{
    [Serializable]
    public class HierarchySkin
    {
        public TextColor normal;
        public TextColor selected;

        public TextAnchor alignment;
        public bool upperCase;
        public FontStyle fontStyle;

        [System.Serializable]
        public class TextColor
        {
            public Color textColor;
            public Color bgColor;
            
            public TextColor(Color textColor, Color bgColor)
            {
                this.textColor = textColor;
                this.bgColor = bgColor;
            }
        }
    }
}