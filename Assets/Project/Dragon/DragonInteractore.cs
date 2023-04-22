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

        public KeyCode primaryKey = KeyCode.E;
        public KeyCode secondaryKey = KeyCode.F;

        #endregion

        private void Update()
        {
            if (highlighting == null)
                return;

            if (Input.GetKeyDown(primaryKey))
                highlighting.Interact();

            if (Input.GetKeyDown(secondaryKey) && highlighting is MultipleInteractionForwarder multiple)
                multiple.SecondaryInteract();
        }
    }
}