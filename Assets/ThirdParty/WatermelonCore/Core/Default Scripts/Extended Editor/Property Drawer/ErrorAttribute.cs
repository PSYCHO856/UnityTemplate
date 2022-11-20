using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ErrorAttribute : HelpBoxAttribute
    {
        public ErrorAttribute(string title) : base(title)
        {
        }
    }
}