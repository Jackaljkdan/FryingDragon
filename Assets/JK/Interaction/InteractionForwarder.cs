using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public class InteractionForwarder : AbstractInteractable
    {
        #region Inspector

        public AbstractInteractable target;

        #endregion

        protected override void InteractProtected(RaycastHit hit)
        {
            if (target != null)
                target.Interact(hit);
        }
    }
}