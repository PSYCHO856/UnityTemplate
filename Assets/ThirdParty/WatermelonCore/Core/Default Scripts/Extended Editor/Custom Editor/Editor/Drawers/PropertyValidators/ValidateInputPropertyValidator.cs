using System.Reflection;
using UnityEditor;

namespace Watermelon
{
    [PropertyValidator(typeof(ValidateInputAttribute))]
    public class ValidateInputPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            var validateInputAttribute = PropertyUtility.GetAttribute<ValidateInputAttribute>(property);
            var target = PropertyUtility.GetTargetObject(property);

            var validationCallback = target.GetType().GetMethod(validateInputAttribute.CallbackName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (validationCallback != null &&
                validationCallback.ReturnType == typeof(ValidatorAttribute.ValidateResult) &&
                validationCallback.GetParameters().Length == 1)
            {
                var fieldInfo = target.GetType().GetField(property.name,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                var fieldType = fieldInfo.FieldType;
                var parameterType = validationCallback.GetParameters()[0].ParameterType;

                if (fieldType == parameterType)
                {
                    var result =
                        (ValidatorAttribute.ValidateResult) validationCallback.Invoke(target,
                            new[] {fieldInfo.GetValue(target)});
                    if (result != null && result.ValidateType != ValidatorAttribute.ValidateType.Success)
                    {
                        var messageType = MessageType.Error;

                        if (result.ValidateType == ValidatorAttribute.ValidateType.Warning)
                            messageType = MessageType.Warning;

                        EditorGUILayout.HelpBox(result.Message, messageType);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("The field type is not the same as the callback's parameter type",
                        MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.HelpBox(
                    validateInputAttribute.GetType().Name +
                    " needs a callback with boolean return type and a single parameter of the same type as the field",
                    MessageType.Warning);
            }
        }
    }
}