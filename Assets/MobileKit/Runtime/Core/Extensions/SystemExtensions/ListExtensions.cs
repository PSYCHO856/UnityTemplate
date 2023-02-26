using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MobileKit
{
    public static class ListExtensions
    {
        /// <summary>
        /// Get array with unique ids of another array
        /// </summary>
        public static int[] GetUniqueRandomObjectIDs<T>(T[] array, int count)
        {
#if UNITY_EDITOR
            if (count >= array.Length)
            {
                Debug.LogError("Array is to small!");
            }

            if (count == 1)
                Debug.LogWarning("Don't use GetUniqueRandomObjectIDs if count is 1!");
#endif

            List<int> objectIDs = new List<int>();

            for (int i = 0; i < count; i++)
            {
                int randomValue = -1;

                do
                {
                    randomValue = Random.Range(0, array.Length);
                }
                while (objectIDs.FindIndex(x => x == randomValue) != -1);

                objectIDs.Add(randomValue);
            }

            return objectIDs.ToArray();
        }

        /// <summary>
        /// Get unique random objects
        /// </summary>
        public static T[] GetUniqueRandomObjects<T>(T[] array, int count)
        {
#if UNITY_EDITOR
            if (count >= array.Length)
            {
                Debug.LogError("Array is to small!");
            }

            if (count == 1)
                Debug.LogWarning("Don't use GetUniqueRandomObjects if count is 1!");
#endif

            List<int> objectIDs = new List<int>();
            List<T> resultList = new List<T>();

            for (int i = 0; i < count; i++)
            {
                int randomValue = -1;

                do
                {
                    randomValue = Random.Range(0, array.Length);
                }
                while (objectIDs.FindIndex(x => x == randomValue) != -1);

                objectIDs.Add(randomValue);

                resultList.Add(array[randomValue]);
            }

            return resultList.ToArray();
        }

        /// <summary>
        /// Get array with unique ids of another list
        /// </summary>
        public static int[] GetUniqueRandomObjectIDs<T>(List<int> array, int count)
        {
#if UNITY_EDITOR
            if (count >= array.Count)
            {
                Debug.LogError("Array is to small!");
            }

            if (count == 1)
                Debug.LogWarning("Don't use GetUniqueRandomObjectIDs if count is 1!");
#endif

            List<int> objectIDs = new List<int>();

            for (int i = 0; i < count; i++)
            {
                int randomValue = -1;

                do
                {
                    randomValue = Random.Range(0, array.Count);
                }
                while (objectIDs.FindIndex(x => x == randomValue) != -1);

                objectIDs.Add(randomValue);
            }

            return objectIDs.ToArray();
        }

        /// <summary>
        /// Get unique random objects
        /// </summary>
        public static T[] GetUniqueRandomObjects<T>(List<T> array, int count)
        {
#if UNITY_EDITOR
            if (count >= array.Count)
            {
                Debug.LogError("Array is to small!");
            }

            if (count == 1)
                Debug.LogWarning("Don't use GetUniqueRandomObjects if count is 1!");
#endif

            List<int> objectIDs = new List<int>();
            List<T> resultList = new List<T>();

            for (int i = 0; i < count; i++)
            {
                int randomValue = -1;

                do
                {
                    randomValue = Random.Range(0, array.Count);
                }
                while (objectIDs.FindIndex(x => x == randomValue) != -1);

                objectIDs.Add(randomValue);

                resultList.Add(array[randomValue]);
            }

            return resultList.ToArray();
        }

        /// <summary>
        /// 随机取一个符合谓词要求的元素
        /// </summary>
        public static T GetRandomItem<T>(this IEnumerable<T> array, Func<T, bool> predict) where T : class
        {
            T selected = null;
            int valid = 0;
            foreach (var item in array)
            {
                if (!predict(item))
                    continue;
                if (Random.Range(0, ++valid) == 0 || selected == null )
                    selected = item;
            }

            return selected;
        }

        /// <summary>
        /// 随机取一个列表中符合谓词要求的元素的序号
        /// </summary>
        public static int FindRandomIndex<T>(this List<T> array, Func<T, bool> predict) where T : class
        {
            int index = -1;
            int valid = 0;
            for (int i = 0; i < array.Count; i++)
            {
                if (!predict(array[i]))
                {
                    continue;
                }

                if (Random.Range(0, ++valid) == 0)
                {
                    index = i;
                }
            }
            return index;
        }
        
        /// <summary>
        /// Check if index is inside array range
        /// </summary>
        public static bool IsInRange<T>(this T[] array, int value)
        {
            return (value >= 0 && value < array.Length);
        }

        /// <summary>
        /// Check if index is inside list range
        /// </summary>
        public static bool IsInRange<T>(this List<T> list, int value)
        {
            return (value >= 0 && value < list.Count);
        }

        /// <summary>
        /// Check if array is null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return (array == null || array.Length == 0);
        }

        /// <summary>
        /// Check if list is null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return (list == null || list.Count == 0);
        }

        /// <summary>
        /// Display array to debug console
        /// </summary>
        public static void Display<T>(this T[] array, Func<T, string> function)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Debug.Log(function(array[i]));
            }
        }

        /// <summary>
        /// Display list to debug console
        /// </summary>
        public static void Display<T>(this List<T> array, Func<T, string> function)
        {
            for (int i = 0; i < array.Count; i++)
            {
                Debug.Log(function(array[i]));
            }
        }

        /// <summary>
        /// Get random item from array
        /// </summary>
        public static T GetRandomItem<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        /// <summary>
        /// Get random item from list
        /// </summary>
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Get item from array by index or random value if index higher than array size
        /// </summary>
        public static T GetByIndexOrRandom<T>(this T[] array, int index)
        {
            if (array.IsInRange(index))
            {
                return array[index];
            }

            return array.GetRandomItem();
        }

        /// <summary>
        /// Get item from list by index or random value if index higher than list size
        /// </summary>
        public static T GetByIndexOrRandom<T>(this List<T> list, int index)
        {
            if (list.IsInRange(index))
            {
                return list[index];
            }

            return list.GetRandomItem();
        }

        /// <summary>
        /// Randomize array elements
        /// </summary>
        public static void Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            int k;
            T temp;

            while (n > 1)
            {
                k = Random.Range(0, n--);
                temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        /// <summary>
        /// Randomize list elements
        /// </summary>
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            int k;
            T temp;

            while (n > 1)
            {
                k = Random.Range(0, n--);
                temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }

        /// <summary>
        /// Check if arrays are equal
        /// </summary>
        public static bool ArraysEqual<T>(this T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check if lists are equal
        /// </summary>
        public static bool ArraysEqual<T>(this List<T> a1, List<T> a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Count != a2.Count)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Count; i++)
            {
                if (!comparer.Equals(a1[i], a2[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Crop array to length
        /// </summary>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Check if index is last
        /// </summary>
        public static bool IsLast<T>(this T[] array, int index)
        {
            return array.Length - 1 == index;
        }

        /// <summary>
        /// Check if index is last
        /// </summary>
        public static bool IsLast<T>(this List<T> list, int index)
        {
            return list.Count - 1 == index;
        }
        
        /// <summary>
        ///  按权重随机, 返回随到的序号
        /// </summary>
        public static int WeightedRandom<T>(this List<T> list, List<int> weights)
        {
            int totalWeight = 0;
            foreach (var weight in weights)
            {
                totalWeight += weight;
            }
            
            int targetWeight = Random.Range(0, totalWeight);
            for (int i = 0; i < list.Count; i++)
            {
                int weight = weights[i];
                if (weight > 0 && targetWeight <= weight)
                {
                    return i;
                }
                targetWeight -= weight;
            }
            return 0;
        }
        
        /// <summary>
        ///  按权重随机, 返回随到的序号
        /// </summary>
        public static int WeightedRandom<T>(this T[] list, int[] weights)
        {
            if (list.Length > weights.Length)
            {
                Debug.LogWarning("List Count should less or equal weights count.");
                return 0;
            }
            int totalWeight = 0;
            foreach (var weight in weights)
            {
                totalWeight += weight;
            }
            
            int targetWeight = Random.Range(0, totalWeight);
            for (int i = 0; i < list.Length; i++)
            {
                int weight = weights[i];
                if (weight > 0 && targetWeight <= weight)
                {
                    return i;
                }
                targetWeight -= weight;
            }
            return 0;
        }

    }
}
