using JK.Interaction;
using Project.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    public class DragonInteractore : MonoBehaviour
    {
        #region Inspector



        #endregion

        private AbstractInteractable interactable;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out AbstractInteractable abstractInteractable))
            {
                Debug.Log("enter " + other.gameObject.name);

                UnhighlightCurrent();

                interactable = abstractInteractable;

                if (interactable.TryGetComponent(out AbstractAffordance affordance))
                    affordance.StartHighlight();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out AbstractInteractable triggeringInteractable))
                if (triggeringInteractable == interactable)
                    UnhighlightCurrent();
        }

        private void UnhighlightCurrent()
        {
            if (interactable == null)
                return;

            if (interactable.TryGetComponent(out AbstractAffordance affordance))
                affordance.StopHighlight();

            interactable = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && interactable != null)
                interactable.Interact();
        }
    }
}