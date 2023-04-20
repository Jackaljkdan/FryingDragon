using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class AffordanceRaycaster : MonoBehaviour
    {
        #region Inspector

        public float maxDistance = 1.2f;

        public LayerMask mask = LayerMaskUtils.Everything;

        [RuntimeField]
        public IAffordance current;

        [DebugField]
        public Collider hitting;

        #endregion

        private void Update()
        {
            if (TryFindAffordanceByRaycast(transform, out IAffordance next))
            {
                if (current != next)
                {
                    current?.StopHighlight();
                    next.StartHighlight();
                }
            }
            else
            {
                current?.StopHighlight();
            }

            current = next;
        }

        private void OnDisable()
        {
            if (current != null)
            {
                current.StopHighlight();
                current = null;
            }
        }

        private bool TryFindAffordanceByRaycast(Transform myTransform, out IAffordance affordance)
        {
            if (Physics.Raycast(myTransform.position, myTransform.forward, out RaycastHit hit, maxDistance, mask))
            {
                hitting = hit.collider;
                return hit.collider.TryGetComponent(out affordance);
            }
            else
            {
                hitting = null;
                affordance = null;
                return false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Transform myTransform = transform;
            Debug.DrawRay(myTransform.position, myTransform.forward * maxDistance, Color.green);
        }
    }
}