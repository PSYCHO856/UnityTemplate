using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MobileKit
{
    public static class StringExtensions
    {
        /// <summary>
        /// Add space before capital letters
        /// </summary>
        public static string AddSpaces(this string value)
        {
            return Regex.Replace(value, "([a-z]) ?([A-Z])", "$1 $2");
        }

        /// <summary>
        /// Get value inside [] brackets
        /// </summary>
        public static string FindStringInsideBrackets(this string value)
        {
            Match match = Regex.Match(value, @"\[([^)]*)\]");

            return match.Result("$1");
        }

        /// <summary>
        /// Try to convert string to enum
        /// </summary>
        public static T ToEnum<T>(this string value, bool ignoreCase, T defaultValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            T result;

            try
            {
                result = (T)Enum.Parse(typeof(T), value, true);
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                result = defaultValue;
            }

            return result;
        }
    }
}