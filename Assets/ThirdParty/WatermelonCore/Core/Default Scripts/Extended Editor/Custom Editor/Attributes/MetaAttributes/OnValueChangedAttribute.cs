using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class OnValueChangedAttribute : MetaAttribute
    {
        public OnValueChangedAttribute(string callbackName)
        {
            CallbackName = callbackName;
        }

        public string CallbackName { get; }
    }
}