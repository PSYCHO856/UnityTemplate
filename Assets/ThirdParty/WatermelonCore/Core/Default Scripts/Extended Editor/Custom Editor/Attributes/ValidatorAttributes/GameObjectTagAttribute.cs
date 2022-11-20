using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GameObjectTagAttribute : ValidatorAttribute
    {
        public GameObjectTagAttribute(string tag, string message = null)
        {
            Tag = tag;
            Message = message;
        }

        public string Message { get; }

        public string Tag { get; }

        public override string DefaultMessage => "GameObject's tag must be " + Tag + "!";

        public override ValidateResult Validate(object value, object target)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(Tag)) return new ValidateResult(ValidateType.Warning, "Tag can't be empty!");

            if (value != null && !value.Equals(null))
            {
                var targetType = value.GetType();

                if (targetType == typeof(GameObject))
                {
                    var valueObject = (GameObject) value;

                    if (!valueObject.CompareTag(Tag))
                    {
                        if (!string.IsNullOrEmpty(Message))
                            return new ValidateResult(ValidateType.Error, Message);
                        return new ValidateResult(ValidateType.Error, DefaultMessage);
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