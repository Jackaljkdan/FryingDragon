using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public abstract class AbstractZoneEnabler : MonoBehaviour
    {
        #region Inspector



        #endregion

        private Dictionary<Zone, int> refenceCount = new Dictionary<Zone, int>(8);

        public void EnterZoneFitterCollider(ZoneFitter fitter)
        {
            Zone zone = fitter.zone;

            zone.gameObject.SetActive(true);

            if (refenceCount.ContainsKey(zone))
                refenceCount[zone] += 1;
            else
                refenceCount[zone] = 1;
        }

        public void ExitZoneFitterCollider(ZoneFitter fitter)
        {
            Zone zone = fitter.zone;

            if (!refenceCount.TryGetValue(zone, out int count))
                count = 0;

            if (count > 1)
            {
                refenceCount[zone] -= 1;
            }
            else
            {
                refenceCount[zone] = 0;
                zone.gameObject.SetActive(false);
            }
        }
    }
}