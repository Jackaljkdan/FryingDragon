using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class TransformUtils
    {
        public static void RotateXWithBounds(this Transform self, float amount, float lowerBound = 360 - 89, float upperBound = 89)
        {
            Vector3 euler = self.localEulerAngles;
            euler.x -= amount;

            if (euler.x > 180)
                euler.x = Mathf.Max(lowerBound, euler.x);
            else
                euler.x = Mathf.Min(upperBound, euler.x);

            self.localEulerAngles = euler;

            //Debug.Log("euler " + transform.eulerAngles);
        }

        public static void DestroyChildren(this Transform self, bool avoidDestroyImmediate = false)
        {
            if (PlatformUtils.IsEditor && !Application.isPlaying)
            {
                if (!avoidDestroyImmediate)
                {
                    while (self.childCount > 0)
                        GameObject.DestroyImmediate(self.GetChild(self.childCount - 1).gameObject);
                }
                else
                {
                    for (int i = 0; i < self.childCount; i++)
                    {
                        Transform child = self.GetChild(i);
                        EditorApplicationUtils.DelayCall(() => GameObject.DestroyImmediate(child.gameObject));
                    }
                }
            }
            else
            {
                foreach (Transform child in self)
                    GameObject.Destroy(child.gameObject);
            }
        }

        public static void DestroyAndDetachChildren(this Transform self, bool avoidDestroyImmediate = false)
        {
            self.DestroyChildren(avoidDestroyImmediate);
            self.DetachChildren();
        }
    }
}