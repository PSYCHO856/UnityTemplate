
using UnityEngine;
namespace MobileKit
{
    public static class VectorExtensions
    {
        /// <summary>
        ///  Adds to each component specified value
        /// </summary>
        /// <param name="floatValue">value to add</param>
        /// <returns></returns>
        public static Vector3 AddFloat(this Vector3 vector, float floatValue)
        {
            vector.x += floatValue;
            vector.y += floatValue;
            vector.z += floatValue;

            return vector;
        }

        /// <summary>
        /// Adds to x component specified value
        /// </summary>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static Vector3 AddToX(this Vector3 vector, float value)
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
        public static Vector3 AddToY(this Vector3 vector, float value)
        {
            vector.y += value;

            return vector;
        }

        /// <summary>
        /// Adds to z component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to add</param>
        /// <returns></returns>
        public static Vector3 AddToZ(this Vector3 vector, float value)
        {
            vector.z += value;

            return vector;
        }

        /// <summary>
        /// Multiplies x component to specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to multiply</param>
        /// <returns></returns>
        public static Vector3 MultX(this Vector3 vector, float value)
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
        public static Vector3 MultY(this Vector3 vector, float value)
        {
            vector.y *= value;

            return vector;
        }

        /// <summary>
        /// Multiplies z component to specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to multiply</param>
        /// <returns></returns>
        public static Vector3 MultZ(this Vector3 vector, float value)
        {
            vector.z *= value;

            return vector;
        }

        /// <summary>
        /// Sets to x component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public static Vector3 SetX(this Vector3 vector, float value)
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
        public static Vector3 SetY(this Vector3 vector, float value)
        {
            vector.y = value;

            return vector;
        }

        /// <summary>
        /// Sets to z component specified value
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public static Vector3 SetZ(this Vector3 vector, float value)
        {
            vector.z = value;

            return vector;
        }

        /// <summary>
        /// Sets x,y,z specified value
        /// </summary>
        /// <param name="valueOfXYZ">value to set</param>
        /// <returns></returns>
        public static Vector3 SetAll(this Vector3 vector, float valueOfXYZ)
        {
            vector.x = valueOfXYZ;
            vector.y = valueOfXYZ;
            vector.z = valueOfXYZ;

            return vector;
        }

        /// <summary>
        /// Convert float value to Vector3
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this float value)
        {
            return new Vector3(value, value, value);
        }

        /// <summary>
        /// Convert int value to Vector3
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this int value)
        {
            return new Vector3(value, value, value);
        }
        
        /// <summary>
        /// Convert to World position
        /// </summary>
        public static Vector3 ToWorldPosition(this Vector3 vector, float z = 0)
        {
            vector.z = z;
            return Camera.main.ScreenToWorldPoint(vector);
        }
    }
}