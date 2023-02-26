using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public static class CoreExtensions
    {
        /// <summary>
        /// Add element to dictionary or add some new values if it exists
        /// </summary>
        public static int AddOrAdjustValue<T>(this Dictionary<T, int> dictionary, T key, int value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] += value;
            else
                dictionary.Add(key, value);

            return dictionary[key];
        }

        /// <summary>
        /// Add element to dictionary or set some new values if it exists
        /// </summary>
        public static int AddOrSetValue<T>(this Dictionary<T, int> dictionary, T key, int value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);

            return dictionary[key];
        }
    }
}
