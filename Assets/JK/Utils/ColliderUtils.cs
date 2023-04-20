using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class ColliderUtils
    {
        public static void DrawGizmo(this Collider self, Color color)
        {
            if (self is BoxCollider boxCollider)
            {
                GizmosUtils.WithRestoredColor(() =>
                {
                    var savedMatrix = Gizmos.matrix;

                    Gizmos.color = color;
                    Gizmos.matrix = boxCollider.transform.localToWorldMatrix;
                    Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);

                    Gizmos.matrix = savedMatrix;
                });
            }
            else if (self is SphereCollider sphereCollider)
            {
                GizmosUtils.WithRestoredColor(() =>
                {
                    var savedMatrix = Gizmos.matrix;

                    Gizmos.color = color;
                    Gizmos.matrix = sphereCollider.transform.localToWorldMatrix;
                    Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);

                    Gizmos.matrix = savedMatrix;
                });
            }
            else
            {
                Debug.LogError($"Drawing gizmo for {self.GetType().Name} is not implemented".Contextualized(self, includeHierarchy: true));
            }
        }
    }
}