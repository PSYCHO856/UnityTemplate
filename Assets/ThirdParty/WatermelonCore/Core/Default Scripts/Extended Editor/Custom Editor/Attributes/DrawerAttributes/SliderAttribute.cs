using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SliderAttribute : DrawerAttribute
    {
        public SliderAttribute(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public SliderAttribute(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public float MinValue { get; }

        public float MaxValue { get; }
    }
}