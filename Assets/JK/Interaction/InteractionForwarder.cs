using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public class InteractionForwarder : MonoBehaviour, IInteractable
    {
        #region Inspector

        public AbstractInteractable target;

        #endregion

        public void Interact(RaycastHit hit)
        {
            if (target != null)
                target.Interact(hit);
        }

        public void Interact()
        {
            if (target != null)
                target.Interact();
        }
    }
}