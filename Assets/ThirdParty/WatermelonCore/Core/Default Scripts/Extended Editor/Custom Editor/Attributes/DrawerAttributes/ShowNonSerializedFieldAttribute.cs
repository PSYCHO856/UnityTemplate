using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowNonSerializedFieldAttribute : DrawerAttribute
    {
    }
}