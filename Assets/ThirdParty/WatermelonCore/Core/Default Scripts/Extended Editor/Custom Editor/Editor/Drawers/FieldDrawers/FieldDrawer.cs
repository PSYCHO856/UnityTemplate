using System.Reflection;
using UnityEngine;

namespace Watermelon
{
    public abstract class FieldDrawer
    {
        public abstract void DrawField(Object target, FieldInfo field);
    }
}