using JK.Interaction;
using JK.Utils;
using Project.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    public class DragonInteractore : TriggerInteractableHighlighter
    {
        #region Inspector

        #endregion

        private AxisAsButtonClass primary = new AxisAsButtonClass("Interact");
        private AxisAsButtonClass secondary = new AxisAsButtonClass("InteractSecondary");

        private void Update()
        {
            if (highlighting == null)
                return;

            if (primary.GetAxisDown())
                highlighting.Interact();

            if (secondary.GetAxisDown() && highlighting is MultipleInteractionForwarder multiple)
                multiple.SecondaryInteract();
        }
    }
}