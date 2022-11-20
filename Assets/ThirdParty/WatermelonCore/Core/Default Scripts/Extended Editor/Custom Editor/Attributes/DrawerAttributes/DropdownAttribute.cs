using System;
using System.Collections;
using System.Collections.Generic;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DropdownAttribute : DrawerAttribute
    {
        public DropdownAttribute(string valuesFieldName)
        {
            ValuesFieldName = valuesFieldName;
        }

        public string ValuesFieldName { get; }
    }

    public interface IDropdownList : IEnumerable<KeyValuePair<string, object>>
    {
    }

    public class DropdownList<T> : IDropdownList
    {
        private readonly List<KeyValuePair<string, object>> values;

        public DropdownList()
        {
            values = new List<KeyValuePair<string, object>>();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string displayName, T value)
        {
            values.Add(new KeyValuePair<string, object>(displayName, value));
        }

        public static explicit operator DropdownList<object>(DropdownList<T> target)
        {
            var result = new DropdownList<object>();
            foreach (var kvp in target) result.Add(kvp.Key, kvp.Value);

            return result;
        }
    }
}