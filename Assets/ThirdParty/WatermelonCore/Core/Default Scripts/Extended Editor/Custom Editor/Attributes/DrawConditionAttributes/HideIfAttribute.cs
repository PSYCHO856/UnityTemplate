using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HideIfAttribute : DrawConditionAttribute
    {
        public HideIfAttribute(string conditionName)
        {
            ConditionName = conditionName;
        }

        public string ConditionName { get; }
    }
}