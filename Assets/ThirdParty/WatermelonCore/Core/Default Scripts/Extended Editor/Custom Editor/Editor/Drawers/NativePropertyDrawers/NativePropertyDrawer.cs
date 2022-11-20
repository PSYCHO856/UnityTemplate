using System.Reflection;
using UnityEngine;

namespace Watermelon
{
    public abstract class NativePropertyDrawer
    {
        public abstract void DrawNativeProperty(Object target, PropertyInfo property);
    }
}