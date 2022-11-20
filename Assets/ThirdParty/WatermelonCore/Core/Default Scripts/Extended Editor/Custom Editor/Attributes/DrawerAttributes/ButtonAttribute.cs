using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : DrawerAttribute
    {
        public ButtonAttribute(string text = null)
        {
            Text = text;
        }

        public string Text { get; }
    }
}