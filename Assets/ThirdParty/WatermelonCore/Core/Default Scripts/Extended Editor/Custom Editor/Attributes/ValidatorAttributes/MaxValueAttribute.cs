using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MaxValueAttribute : ValidatorAttribute
    {
        public MaxValueAttribute(float maxValue)
        {
            MaxValue = maxValue;
        }

        public MaxValueAttribute(int maxValue)
        {
            MaxValue = maxValue;
        }

        public override string DefaultMessage => "Value must be less than " + MaxValue + "!";

        public float MaxValue { get; }
    }
}