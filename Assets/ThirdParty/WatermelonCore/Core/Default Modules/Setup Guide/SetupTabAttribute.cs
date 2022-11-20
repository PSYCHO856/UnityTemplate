using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SetupTabAttribute : Attribute
    {
        public int priority = int.MaxValue;
        public bool showScrollView = true;
        public string tabName;

        public string texture;

        public SetupTabAttribute(string tabName)
        {
            this.tabName = tabName;
        }
    }
}