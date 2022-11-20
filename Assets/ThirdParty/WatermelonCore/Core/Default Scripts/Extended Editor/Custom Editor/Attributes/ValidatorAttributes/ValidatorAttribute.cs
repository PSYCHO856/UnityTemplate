using System;

namespace Watermelon
{
    public abstract class ValidatorAttribute : ExtendedEditorAttribute
    {
        [Flags]
        public enum ValidateType
        {
            Success = 1 << 0,
            Warning = 1 << 1,
            Error = 1 << 2
        }

        public abstract string DefaultMessage { get; }

        public virtual ValidateResult Validate(object value, object target)
        {
            return new(ValidateType.Success, DefaultMessage);
        }

        public class ValidateResult
        {
            public ValidateResult(ValidateType validateType, string message)
            {
                ValidateType = validateType;
                Message = message;
            }

            public ValidateType ValidateType { get; }

            public string Message { get; }
        }
    }
}