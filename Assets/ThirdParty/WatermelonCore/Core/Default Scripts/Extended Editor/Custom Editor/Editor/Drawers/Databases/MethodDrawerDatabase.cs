using System;
using System.Collections.Generic;

namespace Watermelon
{
    public static class MethodDrawerDatabase
    {
        private static readonly Dictionary<Type, MethodDrawer> drawersByAttributeType;

        static MethodDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, MethodDrawer>
            {
                [typeof(ButtonAttribute)] = new ButtonMethodDrawer()
            };
        }

        public static MethodDrawer GetDrawerForAttribute(Type attributeType)
        {
            return drawersByAttributeType.TryGetValue(attributeType, out var drawer) ? drawer : null;
        }
    }
}