using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PrefabAttribute : ValidatorAttribute
    {
        public PrefabAttribute(string message = null)
        {
            Message = message;
        }

        public string Message { get; }

        public override string DefaultMessage => "GameObject must be a prefab!";

        public override ValidateResult Validate(object value, object target)
        {
#if UNITY_EDITOR
            if (value != null && !value.Equals(null))
            {
                var targetType = value.GetType();

                if (targetType.IsSubclassOf(typeof(Object)))
                {
                    var valueObject = (Object) value;

                    if (PrefabUtility.GetCorrespondingObjectFromSource(valueObject) == null &&
                        PrefabUtility.GetPrefabInstanceHandle(valueObject) == null)
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