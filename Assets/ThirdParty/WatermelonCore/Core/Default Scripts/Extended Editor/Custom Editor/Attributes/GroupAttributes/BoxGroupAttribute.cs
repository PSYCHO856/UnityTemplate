using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BoxGroupAttribute : GroupAttribute
    {
        public BoxGroupAttribute(string name = "")
            : base(name)
        {
        }
    }
}