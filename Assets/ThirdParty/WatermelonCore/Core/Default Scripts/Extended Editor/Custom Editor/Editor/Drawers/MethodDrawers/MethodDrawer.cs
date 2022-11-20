using System.Reflection;
using UnityEngine;

namespace Watermelon
{
    public abstract class MethodDrawer
    {
        public abstract void DrawMethod(Object target, MethodInfo methodInfo);
    }
}