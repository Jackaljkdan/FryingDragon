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
            if (other.gameObject.TryGetComponent<AbstractInteractable>(out AbstractInteractable abstractInteractable))
                interactable = abstractInteractable;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<AbstractInteractable>(out _))
                interactable = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && interactable)
                interactable.Interact();
        }
    }
}