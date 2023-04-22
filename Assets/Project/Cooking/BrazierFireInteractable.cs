using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Cooking
{
    [DisallowMultipleComponent]
    public class BrazierFireInteractable : AbstractInteractable
    {
        #region Inspector

        public Brazier brazier;

        public Cooking cooking;

        private void Reset()
        {
            brazier = GetComponent<Brazier>();
            cooking = GetComponent<Cooking>();
        }

        #endregion

        protected override void InteractProtected(RaycastHit hit)
        {
            if (brazier.bowl == null)
                return;

            if (cooking.IsCooking)
                return;

            cooking.StartCooking();
        }
    }
}