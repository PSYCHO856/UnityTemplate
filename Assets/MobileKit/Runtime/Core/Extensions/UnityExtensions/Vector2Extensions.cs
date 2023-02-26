using UnityEngine;

namespace MobileKit
{
    public static class Vector2Extensions
    {
        /// <summary>
        ///  Adds to each component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="floatValue">value to add</param>
        /// <returns></returns>
        public static Vector2 AddFloat(this Vector2 vector, float floatValue)
        {
            vector.x += floatValue;
            vector.y += floatValue;

            return vector;
        }

        /// <summary>
        /// Adds to x component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static Vector2 AddToX(this Vector2 vector, float value)
        {
            vector.x += value;

            return vector;
        }

        /// <summary>
        /// Adds to y component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static Vector2 AddToY(this Vector2 vector, float value)
        {
            vector.y += value;

            return vector;
        }

        /// <summary>
        /// Multiplies x component to specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to multiply</param>
        /// <returns></returns>
        public static Vector2 MultX(this Vector2 vector, float value)
        {
            vector.x *= value;

            return vector;
        }

        /// <summary>
        /// Multiplies y component to specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to multiply</param>
        /// <returns></returns>
        public static Vector2 MultY(this Vector2 vector, float value)
        {
            vector.y *= value;

            return vector;
        }

        /// <summary>
        /// Sets to x component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public static Vector2 SetX(this Vector2 vector, float value)
        {
            vector.x = value;

            return vector;
        }

        /// <summary>
        /// Sets to y component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public static Vector2 SetY(this Vector2 vector, float value)
        {
            vector.y = value;

            return vector;
        }

        /// <summary>
        /// Convert float value to Vector2
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns></returns>
        public static Vector2 ToVector2(this float value)
        {
            return new Vector2(value, value);
        }

        /// <summary>
        /// Convert int value to Vector2
        /// </summary>
        /// <param name="value">value to convert</param>
        public static Vector2 ToVector2(this int value)
        {
            return new Vector2(value, value);
        }

        /// <summary>
        /// Convert Vector2 to Vector3
        /// </summary>
        public static Vector3 ToVector3(this Vector2 vector, float z = 0)
        {
            return new Vector3(vector.x, vector.y, z);
        }
    }
}