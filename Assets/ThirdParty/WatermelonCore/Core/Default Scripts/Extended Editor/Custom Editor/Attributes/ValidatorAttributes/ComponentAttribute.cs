using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ComponentAttribute : ValidatorAttribute
    {
        public ComponentAttribute(Type type, string message = null)
        {
            RequiredType = type;
            Message = message;
        }

        public string Message { get; }

        public override string DefaultMessage => "GameObject must contains " + RequiredType.Name + " component!";

        public Type RequiredType { get; }

        public override ValidateResult Validate(object value, object target)
        {
#if UNITY_EDITOR
            if (!RequiredType.IsSubclassOf(typeof(Component)))
                return new ValidateResult(ValidateType.Warning, "Wrong component type!");

            if (value != null && !value.Equals(null))
            {
                var targetType = value.GetType();

                if (targetType == typeof(GameObject))
                {
                    var valueObject = (GameObject) value;

                    if (valueObject.GetComponent(RequiredType) == null)
                    {
                        if (!string.IsNullOrEmpty(Message))
                            return new ValidateResult(ValidateType.Error, Message);
                        return new ValidateResult(ValidateType.Error,
                            "GameObject must contains " + RequiredType.Name + " component!");
                    }
                }
                else
                {
                    return new ValidateResult(ValidateType.Error, "Wrong field type!");
                }

                return new ValidateResult(ValidateType.Success, DefaultMessage);
            }
#endif

            return new ValidateResult(ValidateType.Error, "Value can't be null!");
        }
    }
}