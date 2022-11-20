using UnityEditor;

namespace Watermelon
{
    [PropertyValidator(typeof(RequiredAttribute))]
    public class RequiredPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            var requiredAttribute = PropertyUtility.GetAttribute<RequiredAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    var errorMessage = property.name + " is required";
                    if (!string.IsNullOrEmpty(requiredAttribute.Message)) errorMessage = requiredAttribute.Message;

                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
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