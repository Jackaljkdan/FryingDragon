using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public class AffordanceForwarder : AbstractAffordance
    {
        #region Inspector

        public AbstractAffordance target;

        #endregion

        protected override void StartHighlightProtected(RaycastHit hit)
        {
            if (target != null)
                target.StartHighlight(hit);
        }

        protected override void StopHighlightProtected()
        {
            if (target != null)
                target.StopHighlight();
        }
    }
}