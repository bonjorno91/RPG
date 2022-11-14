using Code.Attributes;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    [CustomPropertyDrawer(typeof(InspectorLabel))]
    public class InspectorLabelPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            if (attribute is InspectorLabel inspectorLabel)
            {
                EditorGUI.PropertyField(position, property,new GUIContent(inspectorLabel.Label));
            }
        }
    }
}