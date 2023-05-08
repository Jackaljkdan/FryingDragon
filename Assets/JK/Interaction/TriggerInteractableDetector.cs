using JK.Observables;
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
        public List<AbstractInteractable> available = new();

        [RuntimeField]
        public ObservableProperty<AbstractInteractable> highlighting = new();

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out AbstractInteractable triggeringInteractable))
            {
                //Debug.Log("enter " + other.gameObject.name);

                if (available.Contains(triggeringInteractable))
                    return;

                available.Add(triggeringInteractable);

                if (enabled)
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

                if (enabled)
                    UpdateHighlighting();
            }
        }

        private void OnEnable()
        {
            UpdateHighlighting();
        }

        private void OnDisable()
        {
            if (highlighting.Value != null && highlighting.Value.TryGetComponent(out AbstractAffordance affordance))
                affordance.StopHighlight();

            highlighting.Value = null;
        }

        private void UpdateHighlighting()
        {
            var closest = GetClosestInteractable();

            if (closest == highlighting.Value)
                return;

            if (highlighting.Value != null && highlighting.Value.TryGetComponent(out AbstractAffordance affordance))
                affordance.StopHighlight();

            highlighting.Value = closest;

            if (highlighting.Value != null && highlighting.Value.TryGetComponent(out affordance))
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