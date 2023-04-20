using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class RandomUtils
    {
        public static float Exponential(float k)
        {
            float uniformRand = UnityEngine.Random.Range(0.0f, 1.0f);
            return -Mathf.Log(uniformRand) * 1 / k;
        }

        /// <summary>
        /// Interpret the parameter as the average time between two events
        /// and the return value as a random time to wait with that average
        /// </summary>
        public static float ExponentialInversed(float k)
        {
            float uniformRand = UnityEngine.Random.Range(0.0f, 1.0f);
            return -Mathf.Log(uniformRand) * k;
        }

        public static float TimeUntilNextEvent(float averageTime)
        {
            return ExponentialInversed(averageTime);
        }

        public static float TimeUntilNextEvent(float averageTime, float maxDelta)
        {
            float randomDelta = UnityEngine.Random.Range(-maxDelta, maxDelta);
            return averageTime + randomDelta;
        }

        public static float ZeroOne()
        {
            return UnityEngine.Random.Range(0f, 1f);
        }

        public static bool Should(float probability)
        {
            return UnityEngine.Random.Range(0f, 1f) <= probability;
        }

        public static T Choose<T>(T first, T second, float firstProbability = 0.5f)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= firstProbability)
                return first;
            else
                return second;
        }

        public static T Choose<T>(List<T> list, out int randomIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static T Choose<T>(List<T> list)
        {
            return Choose(list, out _);
        }

        public static T ChooseExcept<T>(List<T> list, int exceptIndex, out int randomIndex)
        {
            if (list.Count == 1)
            {
                randomIndex = 0;
                return list[0];
            }

            do
            {
                randomIndex = UnityEngine.Random.Range(0, list.Count);
            }
            while (randomIndex == exceptIndex);

            return list[randomIndex];
        }

        public static T ChooseExcept<T>(List<T> list, int exceptIndex)
        {
            return ChooseExcept(list, exceptIndex, out _);
        }

        public static T ChooseExcept<T>(List<T> list, T exceptElement, out int randomIndex)
        {
            return ChooseExcept(list, list.IndexOf(exceptElement), out randomIndex);
        }

        public static T ChooseExcept<T>(List<T> list, T exceptElement)
        {
            return ChooseExcept(list, exceptElement, out _);
        }
    }
}