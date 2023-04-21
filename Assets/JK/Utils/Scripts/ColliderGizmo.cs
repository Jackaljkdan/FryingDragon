using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class ColliderGizmo : MonoBehaviour
    {
        #region Inspector

        public new Collider collider;

        public Color gizmoColor = Color.green;

        private void Reset()
        {
            collider = GetComponentInParent<Collider>();
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (collider != null)
                collider.DrawGizmo(gizmoColor);
        }
    }
}