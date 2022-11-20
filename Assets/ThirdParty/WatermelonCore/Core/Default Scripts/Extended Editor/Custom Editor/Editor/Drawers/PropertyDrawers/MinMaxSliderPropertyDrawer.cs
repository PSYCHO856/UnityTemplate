using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [PropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            var minMaxSliderAttribute = PropertyUtility.GetAttribute<MinMaxSliderAttribute>(property);

            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                var controlRect = EditorGUILayout.GetControlRect();
                var labelWidth = EditorGUIUtility.labelWidth;
                var floatFieldWidth = EditorGUIUtility.fieldWidth;
                var sliderWidth = controlRect.width - labelWidth - 2f * floatFieldWidth;
                var sliderPadding = 5f;

                var labelRect = new Rect(
                    controlRect.x,
                    controlRect.y,
                    labelWidth,
                    controlRect.height);

                var sliderRect = new Rect(
                    controlRect.x + labelWidth + floatFieldWidth + sliderPadding,
                    controlRect.y,
                    sliderWidth - 2f * sliderPadding,
                    controlRect.height);

                var minFloatFieldRect = new Rect(
                    controlRect.x + labelWidth,
                    controlRect.y,
                    floatFieldWidth,
                    controlRect.height);

                var maxFloatFieldRect = new Rect(
                    controlRect.x + labelWidth + floatFieldWidth + sliderWidth,
                    controlRect.y,
                    floatFieldWidth,
                    controlRect.height);

                // Draw the label
                EditorGUI.LabelField(labelRect, property.displayName);

                // Draw the slider
                EditorGUI.BeginChangeCheck();

                var sliderValue = property.vector2Value;
                EditorGUI.MinMaxSlider(sliderRect, ref sliderValue.x, ref sliderValue.y, minMaxSliderAttribute.MinValue,
                    minMaxSliderAttribute.MaxValue);

                sliderValue.x = EditorGUI.FloatField(minFloatFieldRect, sliderValue.x);
                sliderValue.x = Mathf.Clamp(sliderValue.x, minMaxSliderAttribute.MinValue,
                    Mathf.Min(minMaxSliderAttribute.MaxValue, sliderValue.y));

                sliderValue.y = EditorGUI.FloatField(maxFloatFieldRect, sliderValue.y);
                sliderValue.y = Mathf.Clamp(sliderValue.y, Mathf.Max(minMaxSliderAttribute.MinValue, sliderValue.x),
                    minMaxSliderAttribute.MaxValue);

                if (EditorGUI.EndChangeCheck()) property.vector2Value = sliderValue;
            }
            else
            {
                var warning = minMaxSliderAttribute.GetType().Name + " can be used only on Vector2 fields";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, PropertyUtility.GetTargetObject(property));

                EditorDrawUtility.DrawPropertyField(property);
            }
        }
    }
}