using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class MultipleInteractionForwarder : AbstractInteractable
    {
        #region Inspector

        public AbstractInteractable primaryTarget;
        public AbstractInteractable secondaryTarget;
        public AbstractInteractable tertiaryTarget;
        public AbstractInteractable quaternaryTarget;

        #endregion

        protected override void InteractProtected(RaycastHit hit)
        {
            if (primaryTarget != null)
                primaryTarget.Interact(hit);
        }

        public void PrimaryInteract()
        {
            if (primaryTarget != null)
                primaryTarget.Interact();
        }

        public void SecondaryInteract()
        {
            if (secondaryTarget != null)
                secondaryTarget.Interact();
        }

        public void TertiaryInteract()
        {
            if (tertiaryTarget != null)
                tertiaryTarget.Interact();
        }

        public void QuaternaryInteract()
        {
            if (quaternaryTarget != null)
                quaternaryTarget.Interact();
        }
    }
}