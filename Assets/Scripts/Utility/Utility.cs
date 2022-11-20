using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static void Shuffle<T>(List<T> list)
    {
        var cache = new List<T>();
        while (list.Count > 0)
        {
            var currentIndex = Random.Range(0, list.Count);
            cache.Add(list[currentIndex]);
            list.RemoveAt(currentIndex);
        }

        foreach (var t in cache) list.Add(t);
    }
}