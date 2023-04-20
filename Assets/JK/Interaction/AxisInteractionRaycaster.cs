using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class AxisInteractionRaycaster : MonoBehaviour
    {
        #region Inspector

        public string axis = "Fire1";

        public float maxDistance = 1.2f;

        public LayerMask mask = LayerMaskUtils.Everything;

        [RuntimeHeader]

        public bool hasInput;

        #endregion

        private void Update()
        {
            if (UnityEngine.Input.GetAxisRaw(axis) > 0)
            {
                if (hasInput)
                    return;

                hasInput = true;

                Transform myTransform = transform;

                if (Physics.Raycast(myTransform.position, myTransform.forward, out RaycastHit hit, maxDistance, mask))
                    if (hit.collider.TryGetComponent(out IInteractable interactable))
                        interactable.Interact(hit);
            }
            else
            {
                hasInput = false;
            }
        }
    }
}