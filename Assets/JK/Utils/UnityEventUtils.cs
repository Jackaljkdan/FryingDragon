using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace JK.Utils
{
    public static class UnityEventUtils
    {
        public static UnityAction AddListenerOnce(this UnityEvent unityEvent, UnityAction call)
        {
            void wrapper()
            {
                if (unityEvent != null)
                    unityEvent.RemoveListener(wrapper);

                call();
            }

            unityEvent.AddListener(wrapper);

            return wrapper;
        }

        public static UnityAction<T> AddListenerOnce<T>(this UnityEvent<T> unityEvent, UnityAction<T> call)
        {
            void wrapper(T arg0)
            {
                if (unityEvent != null)
                    unityEvent.RemoveListener(wrapper);

                call(arg0);
            }

            unityEvent.AddListener(wrapper);

            return wrapper;
        }

        public static UnityAction<T0, T1> AddListenerOnce<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call)
        {
            void wrapper(T0 arg0, T1 arg1)
            {
                if (unityEvent != null)
                    unityEvent.RemoveListener(wrapper);

                call(arg0, arg1);
            }

            unityEvent.AddListener(wrapper);

            return wrapper;
        }

        public static void DrawGizmosToPersistentTargets(this UnityEvent self, Vector3 from, Color color)
        {
            Color prevColor = Gizmos.color;

            Gizmos.color = color;

            for (int i = 0; i < self.GetPersistentEventCount(); i++)
            {
                var target = self.GetPersistentTarget(i);

                Vector3 targetPosition;

                if (target == null)
                    continue;

                if (target is GameObject go)
                    targetPosition = go.transform.position;
                else if (target is Transform targetTransform)
                    targetPosition = targetTransform.position;
                else if (target is Component targetComponent)
                    targetPosition = targetComponent.transform.position;
                else
                    continue;

                if (from != targetPosition)
                    Gizmos.DrawLine(from, targetPosition);
            }

            Gizmos.color = prevColor;
        }

        public static void AddPersistentListener(this UnityEvent self, UnityAction call)
        {
#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(self, call);
#else
            self.AddListener(call);
#endif
        }

        public static void AddPersistentListener<T>(this UnityEvent<T> self, UnityAction<T> call)
        {
#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(self, call);
#else
            self.AddListener(call);
#endif
        }

        public static void AddPersistentListener<T0, T1>(this UnityEvent<T0, T1> self, UnityAction<T0, T1> call)
        {
#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(self, call);
#else
            self.AddListener(call);
#endif
        }

        public static void AddFloatPersistentListener(this UnityEventBase self, UnityAction<float> call, float argument)
        {
#if UNITY_EDITOR
            UnityEventTools.AddFloatPersistentListener(self, call, argument);
#endif
        }
    }
}