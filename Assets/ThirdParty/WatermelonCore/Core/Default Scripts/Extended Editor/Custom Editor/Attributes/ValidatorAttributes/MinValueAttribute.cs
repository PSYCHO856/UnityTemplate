using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinValueAttribute : ValidatorAttribute
    {
        public MinValueAttribute(float minValue)
        {
            MinValue = minValue;
        }

        public MinValueAttribute(int minValue)
        {
            MinValue = minValue;
        }

        public override string DefaultMessage => "Value must be greater than " + MinValue + "!";

        public float MinValue { get; }
    }
}