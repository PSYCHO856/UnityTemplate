using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LineSpacerAttribute : PropertyAttribute
    {
        public string title;

        public LineSpacerAttribute(string title)
        {
            this.title = title;
        }
    }
}