using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public class AffordanceForwarder : MonoBehaviour, IAffordance
    {
        #region Inspector

        public AbstractAffordance target;

        #endregion

        public void StartHighlight(RaycastHit hit)
        {
            if (target != null)
                target.StartHighlight(hit);
        }

        public void StartHighlight()
        {
            if (target != null)
                target.StartHighlight();
        }

        public void StopHighlight()
        {
            if (target != null)
                target.StopHighlight();
        }
    }
}