using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumArrayAttribute : PropertyAttribute
    {
        public Type selectedEnum;

        public EnumArrayAttribute(Type selectedEnum)
        {
            this.selectedEnum = selectedEnum;
        }
    }
}