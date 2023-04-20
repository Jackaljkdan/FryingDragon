using System;
using System.Collections.Generic;

namespace JK.Utils
{
    public static class ListUtils
    {
        /// <summary>
        /// Randomly shuffles a list in place
        /// https://stackoverflow.com/questions/273313/randomize-a-listt/1262619#1262619
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ShuffleInPlace<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // TODO: produce garbage, rusco?
        //public static IEnumerable<int> EnumerateUpTo(int n)
        //{
        //    for (int i = 0; i < n; i++)
        //        yield return i;
        //}

        public static T AtCatched<T>(this IList<T> list, int index, T defaultValue = default)
        {
            try
            {
                return list[index];
            }
            catch (NullReferenceException)
            {
                return defaultValue;
            }
            catch (ArgumentOutOfRangeException)
            {
                return defaultValue;
            }
        }
    }
}
