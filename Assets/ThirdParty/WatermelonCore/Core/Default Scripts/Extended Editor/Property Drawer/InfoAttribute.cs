using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InfoAttribute : HelpBoxAttribute
    {
        public InfoAttribute(string title) : base(title)
        {
        }
    }
}