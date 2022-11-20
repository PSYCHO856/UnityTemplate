using UnityEditor;

namespace Watermelon
{
    [PropertyValidator(typeof(PrefabAttribute))]
    public class PrefabPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            var requiredAttribute = PropertyUtility.GetAttribute<PrefabAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox(property.name + " can't be null!", MessageType.Error);
                }
                else
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(property.objectReferenceValue) == null &&
                        PrefabUtility.GetPrefabInstanceHandle(property.objectReferenceValue) == null)
                    {
                        var errorMessage = property.name + " must be a prefab!";
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