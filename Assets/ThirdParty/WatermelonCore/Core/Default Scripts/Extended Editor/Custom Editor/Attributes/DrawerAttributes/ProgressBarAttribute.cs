using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ProgressBarAttribute : DrawerAttribute
    {
        public ProgressBarColor Color;
        public float MaxValue;
        public string Name;

        public ProgressBarAttribute(string name = "", float maxValue = 100,
            ProgressBarColor color = ProgressBarColor.Blue)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;
        }
    }

    public enum ProgressBarColor
    {
        Red,
        Pink,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet,
        White
    }
}