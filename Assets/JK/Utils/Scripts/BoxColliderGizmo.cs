using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class BoxColliderGizmo : MonoBehaviour
    {
        #region Inspector

        public BoxCollider boxCollider;

        public Color gizmoColor = Color.green;

        private void Reset()
        {
            boxCollider = GetComponentInParent<BoxCollider>();
        }

        #endregion

        private void OnDrawGizmos()
        {
            Color savedColor = Gizmos.color;
            var savedMatrix = Gizmos.matrix;

            Gizmos.color = gizmoColor;
            Gizmos.matrix = boxCollider.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);

            Gizmos.color = savedColor;
            Gizmos.matrix = savedMatrix;
        }
    }
}