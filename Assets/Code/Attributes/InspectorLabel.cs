using UnityEngine;

namespace Code.Attributes
{
    public class InspectorLabel : PropertyAttribute
    {
        public string Label { get; }

        public InspectorLabel(string label)
        {
            Label = label;
        }
    }
}