using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class CompositeAffordance : AbstractAffordance
    {
        #region Inspector

        public List<AbstractAffordance> targets;

        #endregion

        protected override void StartHighlightProtected(RaycastHit hit)
        {
            if (targets == null)
                return;

            foreach (var target in targets)
                target.StartHighlight(hit);
        }

        protected override void StopHighlightProtected()
        {
            if (targets == null)
                return;

            foreach (var target in targets)
                target.StopHighlight();
        }
    }
}