using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class PerformanceActivator : MonoBehaviour
    {
        #region Inspector



        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out AbstractPerformanceActivationTarget target))
                target.SetActiveForPerformance(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out AbstractPerformanceActivationTarget target))
                target.SetActiveForPerformance(false);
        }
    }
}