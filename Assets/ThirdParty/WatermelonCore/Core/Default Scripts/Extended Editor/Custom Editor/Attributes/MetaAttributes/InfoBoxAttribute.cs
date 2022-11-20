using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class InfoBoxAttribute : MetaAttribute
    {
        public InfoBoxAttribute(string text, InfoBoxType type = InfoBoxType.Normal, string visibleIf = null)
        {
            Text = text;
            Type = type;
            VisibleIf = visibleIf;
        }

        public InfoBoxAttribute(string text, string visibleIf)
            : this(text, InfoBoxType.Normal, visibleIf)
        {
        }

        public string Text { get; }

        public InfoBoxType Type { get; }

        public string VisibleIf { get; }
    }

    public enum InfoBoxType
    {
        Normal,
        Warning,
        Error
    }
}