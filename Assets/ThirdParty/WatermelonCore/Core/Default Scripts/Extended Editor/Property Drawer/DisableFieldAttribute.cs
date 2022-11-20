using System;
using UnityEngine;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DisableFieldAttribute : PropertyAttribute
    {
    }
}