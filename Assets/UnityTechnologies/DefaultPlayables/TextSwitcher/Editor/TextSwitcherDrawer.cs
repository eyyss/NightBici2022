using UnityEditor;
using UnityEngine;
using TMPro;

[CustomPropertyDrawer(typeof(TextSwitcherBehaviour))]
public class TextSwitcherDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int fieldCount = 4; // color, fontSize, text, alignment
        return fieldCount * EditorGUIUtility.singleLineHeight + (fieldCount - 1) * EditorGUIUtility.standardVerticalSpacing;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty colorProp = property.FindPropertyRelative("color");
        SerializedProperty fontSizeProp = property.FindPropertyRelative("fontSize");
        SerializedProperty textProp = property.FindPropertyRelative("text");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        Rect fieldRect = new Rect(position.x, position.y, position.width, lineHeight);

        EditorGUI.PropertyField(fieldRect, colorProp);

        fieldRect.y += lineHeight + spacing;
        EditorGUI.PropertyField(fieldRect, fontSizeProp);

        fieldRect.y += lineHeight + spacing;
        EditorGUI.PropertyField(fieldRect, textProp);

    }


}
