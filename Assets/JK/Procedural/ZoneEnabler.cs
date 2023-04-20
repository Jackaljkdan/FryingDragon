using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class ZoneEnabler : AbstractZoneEnabler
    {
        #region Inspector

        private void Reset()
        {
            var collider = GetComponent<Collider>();
            collider.isTrigger = true;
            UndoUtils.SetDirty(collider);
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponentInParent(out ZoneFitter fitter))
                EnterZoneFitterCollider(fitter);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponentInParent(out ZoneFitter fitter))
                ExitZoneFitterCollider(fitter);
        }
    }
}