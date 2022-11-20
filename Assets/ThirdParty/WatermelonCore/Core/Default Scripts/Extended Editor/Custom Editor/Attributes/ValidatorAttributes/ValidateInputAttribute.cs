using System;
using System.Reflection;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ValidateInputAttribute : ValidatorAttribute
    {
        public ValidateInputAttribute(string callbackName)
        {
            CallbackName = callbackName;
        }

        public override string DefaultMessage => "Custom validate function";

        public string CallbackName { get; }

        public override ValidateResult Validate(object value, object target)
        {
            var validationCallback = target.GetType().GetMethod(CallbackName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (validationCallback != null && validationCallback.ReturnType == typeof(ValidateResult) &&
                validationCallback.GetParameters().Length == 1)
            {
                var message = (ValidateResult) validationCallback.Invoke(target, new[] {value});
                if (message != null) return message;
            }
            else
            {
                return new ValidateResult(ValidateType.Warning,
                    GetType().Name +
                    " needs a callback with boolean return type and a single parameter of the same type as the field");
            }

            return new ValidateResult(ValidateType.Success, DefaultMessage);
        }
    }
}