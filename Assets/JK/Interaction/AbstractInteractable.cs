using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public abstract class AbstractInteractable : MonoBehaviour, IInteractable
    {
        #region Inspector

        [SerializeField, IconBox(methodName: nameof(EditorColliderWarning), hideField: true)]
        private int _;

        protected EditorMessageArgs EditorColliderWarning()
        {
            if (GetComponent<Collider>() != null)
                return EditorMessageArgs.DontShow;

            foreach (var forwarder in GetComponentsInChildren<InteractionForwarder>())
            {
                if (forwarder.target == this)
                    return EditorMessageArgs.DontShow;
            }

            return new EditorMessageArgs(EditorMessageType.Warning, "This interaction will not work without a collider");
        }

        #endregion

        protected virtual void Start()
        {
            // allow disabling in inspector
        }

        public void Interact(RaycastHit hit)
        {
            if (enabled)
                InteractProtected(hit);
        }

        public void Interact()
        {
            Interact(default);
        }

        protected abstract void InteractProtected(RaycastHit hit);
    }
}