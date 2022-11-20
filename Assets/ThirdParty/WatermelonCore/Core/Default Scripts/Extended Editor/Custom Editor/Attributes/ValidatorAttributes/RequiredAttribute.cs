using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : ValidatorAttribute
    {
        public RequiredAttribute(string message = null)
        {
            Message = message;
        }

        public string Message { get; }

        public override string DefaultMessage => "Value can't be null!";

        public override ValidateResult Validate(object value, object target)
        {
            if (value != null && !value.Equals(null))
            {
                var targetType = value.GetType();

                if (targetType == typeof(string))
                    if (string.IsNullOrEmpty((string) value))
                    {
                        if (!string.IsNullOrEmpty(Message))
                            return new ValidateResult(ValidateType.Error, Message);
                        return new ValidateResult(ValidateType.Error, "Value can't be empty!");
                    }

                return new ValidateResult(ValidateType.Success, DefaultMessage);
            }

            if (!string.IsNullOrEmpty(Message)) return new ValidateResult(ValidateType.Error, Message);

            return new ValidateResult(ValidateType.Error, "Value can't be null!");
        }
    }
}