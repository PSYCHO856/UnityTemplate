using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : DrawConditionAttribute
    {
        public ShowIfAttribute(string conditionName)
        {
            ConditionName = conditionName;
        }

        public string ConditionName { get; }
    }
}