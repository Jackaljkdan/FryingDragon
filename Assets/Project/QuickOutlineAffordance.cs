using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project
{
    [DisallowMultipleComponent]
    public class QuickOutlineAffordance : AbstractAffordance
    {
        #region Inspector

        public Outline outline;

        private void Reset()
        {
            outline = GetComponentInChildren<Outline>();

            if (outline == null)
                outline = gameObject.AddComponent<Outline>();
        }

        #endregion

        protected override void StartHighlightProtected(RaycastHit hit)
        {
            outline.enabled = true;
        }

        protected override void StopHighlightProtected()
        {
            outline.enabled = false;
        }
    }
}