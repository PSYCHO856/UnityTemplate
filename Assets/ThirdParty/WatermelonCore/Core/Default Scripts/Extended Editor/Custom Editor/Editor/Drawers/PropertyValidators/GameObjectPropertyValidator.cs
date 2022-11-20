using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [PropertyValidator(typeof(GameObjectTagAttribute))]
    public class GameObjectPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            var requiredAttribute = PropertyUtility.GetAttribute<GameObjectTagAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox(property.name + " can't be null!", MessageType.Error);
                }
                else
                {
                    var go = (GameObject) property.objectReferenceValue;
                    if (!go.CompareTag(requiredAttribute.Tag))
                    {
                        var errorMessage = requiredAttribute.DefaultMessage;
                        if (!string.IsNullOrEmpty(requiredAttribute.Message)) errorMessage = requiredAttribute.Message;

                        EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    }
                }
            }
            else
            {
                var warning = requiredAttribute.GetType().Name + " works only on reference types";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
            }
        }
    }
}