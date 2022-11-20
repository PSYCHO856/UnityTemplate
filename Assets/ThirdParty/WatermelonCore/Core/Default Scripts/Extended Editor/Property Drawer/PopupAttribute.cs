using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PopupAttribute : PropertyAttribute
    {
        public PopupAttribute(params object[] arrayParams)
        {
            this.arrayParams = arrayParams;
        }

        public object[] arrayParams { get; }
    }
}