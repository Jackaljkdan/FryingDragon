using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class InteractAction : AbstractAction
    {
        #region Inspector

        public float maxDistance = 1.2f;

        #endregion

        public override bool TryAction(ActionArgs args)
        {
            if (!args.hasHit)
                return false;

            if (args.hit.distance > maxDistance)
                return false;

            if (!args.hit.collider.TryGetComponent(out IInteractable interactable))
                return false;

            interactable.Interact(args.hit);
            return true;
        }
    }
}