using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [PropertyValidator(typeof(ComponentAttribute))]
    public class ComponentPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            var requiredAttribute = PropertyUtility.GetAttribute<ComponentAttribute>(property);

            if (!requiredAttribute.RequiredType.IsSubclassOf(typeof(Component)))
            {
                EditorGUILayout.HelpBox("Wrong component type!", MessageType.Warning);

                return;
            }

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox(property.name + " can't be null!", MessageType.Error);
                }
                else
                {
                    var referenceGameObject = (GameObject) property.objectReferenceValue;

                    if (referenceGameObject.GetComponent(requiredAttribute.RequiredType) == null)
                    {
                        var errorMessage = property.name + " must contains " + requiredAttribute.RequiredType.Name +
                                           " component!";
                        if (!string.IsNullOrEmpty(requiredAttribute.Message)) errorMessage = requiredAttribute.Message;

                        EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox(requiredAttribute.GetType().Name + " works only on reference types",
                    MessageType.Warning);
            }
        }
    }
}