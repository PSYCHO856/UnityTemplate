using System;
using Random = UnityEngine.Random;

namespace MobileKit
{
    public static class BaseTypeExtensions
    {
        #region RandomSign 
        /// <summary>
        /// Set random sign, only int, float ,double
        /// </summary>
        public static int SetRandomSign<T>(this int value) where T : struct, IConvertible, IComparable<T>
        {
            return value * Random.value < 0.5f ? 1 : -1;
        }
        //
        // /// <summary>
        // /// Set random sign
        // /// </summary>
        // public static float SetRandomSign(this float value)
        // {
        //     return value * Random.value < 0.5f ? 1 : -1;
        // }
        //
        // /// <summary>
        // /// Set random sign
        // /// </summary>
        // public static double SetRandomSign(this double value)
        // {
        //     return value * Random.value < 0.5f ? 1 : -1;
        // }
        #endregion
    }
}