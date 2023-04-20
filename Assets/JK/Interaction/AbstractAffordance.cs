using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public abstract class AbstractAffordance : MonoBehaviour, IAffordance
    {
        #region Inspector

        [field:
            IconBox(methodName: nameof(EditorColliderWarning)),
            RuntimeField,
            SerializeField,
        ]
        public bool IsHighlighting { get; private set; }

        protected virtual EditorMessageArgs EditorColliderWarning()
        {
            if (GetComponent<Collider>() != null)
                return EditorMessageArgs.DontShow;

            foreach (var forwarder in GetComponentsInChildren<AffordanceForwarder>())
            {
                if (forwarder.target == this)
                    return EditorMessageArgs.DontShow;
            }

            return new EditorMessageArgs(EditorMessageType.Warning, "This affordance will not work without a collider");
        }

        #endregion

        protected virtual void Start()
        {
            // allow disabling in inspector
        }

        public void StartHighlight(RaycastHit hit)
        {
            if (!enabled)
                return;

            if (!IsHighlighting)
            {
                //Debug.Log($"starting {name} highlight");
                StartHighlightProtected(hit);
                IsHighlighting = true;
            }
        }

        public void StartHighlight()
        {
            StartHighlight(default);
        }

        public void StopHighlight()
        {
            if (!enabled)
                return;

            if (IsHighlighting)
            {
                StopHighlightProtected();
                IsHighlighting = false;
            }
        }

        protected abstract void StartHighlightProtected(RaycastHit hit);

        protected abstract void StopHighlightProtected();
    }
}