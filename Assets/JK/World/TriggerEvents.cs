using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.World
{
    [DisallowMultipleComponent]
    public class TriggerEvents : MonoBehaviour
    {
        #region Inspector

        public bool interactsWithOtherTriggers = false;

        public Color colliderGizmoColor = Color.green;

        public Color onEnterGizmoColor = Color.green;

        public UnityEvent onEnter = new UnityEvent();

        public Color onExitGizmoColor = Color.yellow;

        public UnityEvent onExit = new UnityEvent();

        #endregion

        private void Start()
        {
            // allow behaviour to be enabled/disabled in inspector
        }

        public virtual bool ShouldInteract(Collider other)
        {
            if (!enabled)
                return false;

            if (other.isTrigger && !interactsWithOtherTriggers)
                return false;

            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (ShouldInteract(other))
                onEnter.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (ShouldInteract(other))
                onExit.Invoke();
        }

#if UNITY_EDITOR
        private bool? gizmoHasCollider;
        private Collider gizmoCollider;

        protected virtual void OnDrawGizmos()
        {
            if (enabled)
            {
                onEnter.DrawGizmosToPersistentTargets(transform.position, onEnterGizmoColor);
                onExit.DrawGizmosToPersistentTargets(transform.position, onExitGizmoColor);

                if (gizmoHasCollider == null)
                    gizmoHasCollider = TryGetComponent(out gizmoCollider);

                if (gizmoCollider != null)
                    gizmoCollider.DrawGizmo(colliderGizmoColor);
            }
        }
#endif

        public void DestroyGameObject()
        {
            Destroy(gameObject);
        }

        public void DestroyThis()
        {
            Destroy(this);
        }
    }
}