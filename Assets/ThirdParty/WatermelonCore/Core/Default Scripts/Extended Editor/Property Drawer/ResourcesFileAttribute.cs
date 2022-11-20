using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ResourcesFileAttribute : PropertyAttribute
    {
        public ResourcesFileAttribute(string folderPath, Type type)
        {
            path = folderPath;
            this.type = type;
        }

        public string path { get; }

        public Type type { get; }
    }
}