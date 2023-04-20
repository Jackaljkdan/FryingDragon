using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [Serializable]
    public class DebouncedMethodCaller
    {
        public object obj;
        public MethodInfo callbackMethodInfo;
        public float lastInvokeTime;
        public float debounceSeconds;

        private object lastResult;

        public bool IsValid => callbackMethodInfo != null;

        public DebouncedMethodCaller(object obj, string methodName, float debounceSeconds)
        {
            this.obj = obj;
            this.debounceSeconds = debounceSeconds;

            if (obj != null && !string.IsNullOrEmpty(methodName))
                callbackMethodInfo = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        }

        public object InvokeIfAppropriate()
        {
            if (obj == null || callbackMethodInfo == null)
                return null;

            if (Time.time < lastInvokeTime)
                lastInvokeTime = -debounceSeconds * 2;

            if (Time.time < lastInvokeTime + debounceSeconds)
                return lastResult;

            lastInvokeTime = Time.time;
            lastResult = callbackMethodInfo?.Invoke(obj, null);
            return lastResult;
        }
    }

    public class DebouncedMethodCaller<T> : DebouncedMethodCaller
    {
        public DebouncedMethodCaller(object obj, string methodName, float debounceSeconds) : base(obj, methodName, debounceSeconds)
        {
            if (callbackMethodInfo != null)
            {
                if (callbackMethodInfo.ReturnType != typeof(T) && !callbackMethodInfo.ReturnType.IsSubclassOf(typeof(T)))
                {
                    JkDebug.LogError($"{obj.GetType().Name}.{methodName} does not return {typeof(T).Name}");
                    callbackMethodInfo = null;
                }
            }
        }

        public T InvokeIfAppropriateT()
        {
            if (IsValid)
            {
                object result = InvokeIfAppropriate();

                if (result != null)
                    return (T)result;
                else
                    return default;
            }
            else
                return default;
        }
    }

}