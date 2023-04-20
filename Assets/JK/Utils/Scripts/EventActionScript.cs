using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class EventActionScript : MonoBehaviour
    {
        #region Inspector

        public UnityEvent onAction = new UnityEvent();

        public Color gizmoLineColor = Color.green;

        #endregion

        public void PerformAction()
        {
            onAction.Invoke();
        }

        private void OnDrawGizmos()
        {
            onAction.DrawGizmosToPersistentTargets(transform.position, gizmoLineColor);
        }
    }
}