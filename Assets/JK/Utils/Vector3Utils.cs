using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class Vector3Utils
    {
        public static Vector3 Create(float value)
        {
            return new Vector3(value, value, value);
        }

        public static Vector3 Abs(this Vector3 self)
        {
            return new Vector3(
                Mathf.Abs(self.x),
                Mathf.Abs(self.y),
                Mathf.Abs(self.z)
            );
        }

        public static Vector3 WithX(this Vector3 self, float x)
        {
            self.x = x;
            return self;
        }

        public static Vector3 WithY(this Vector3 self, float y)
        {
            self.y = y;
            return self;
        }

        public static Vector3 WithZ(this Vector3 self, float z)
        {
            self.z = z;
            return self;
        }

        public static Vector3 WithSwappedYZ(this Vector3 self)
        {
            return new Vector3(
                self.x,
                self.z,
                self.y
            );
        }

        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        public static bool Approximately(Vector3 a, Vector3 b, float delta = 1e-5f)
        {
            return (a - b).magnitude <= delta;
        }
    }
}