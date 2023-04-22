using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class TriggerInteractableHighlighter : MonoBehaviour
    {
        #region Inspector

        [RuntimeField]
        public List<AbstractInteractable> available;

        [RuntimeField]
        public AbstractInteractable highlighting;

        #endregion

        protected virtual void Start()
        {
            highlighting = null;
            available = new List<AbstractInteractable>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out AbstractInteractable triggeringInteractable))
            {
                //Debug.Log("enter " + other.gameObject.name);

                if (available.Contains(triggeringInteractable))
                    return;

                available.Add(triggeringInteractable);
                UpdateHighlighting();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out AbstractInteractable triggeringInteractable))
            {
                if (!available.Contains(triggeringInteractable))
                    return;

                available.Remove(triggeringInteractable);
                UpdateHighlighting();
            }
        }

        private void UpdateHighlighting()
        {
            var closest = GetClosestInteractable();

            if (closest == highlighting)
                return;

            if (highlighting != null && highlighting.TryGetComponent(out AbstractAffordance affordance))
                affordance.StopHighlight();

            highlighting = closest;

            if (highlighting != null && highlighting.TryGetComponent(out affordance))
                affordance.StartHighlight();
        }

        private AbstractInteractable GetClosestInteractable()
        {
            Transform myTransform = transform;

            float minDistance = float.PositiveInfinity;
            AbstractInteractable closest = null;

            foreach (var interactable in available)
            {
                float distance = Vector3.Distance(interactable.transform.position, myTransform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }

            return closest;
        }
    }
}