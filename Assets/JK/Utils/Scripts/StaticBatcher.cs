using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class StaticBatcher : MonoBehaviour
    {
        #region Inspector



        #endregion

        private void Start()
        {
            StaticBatchingUtility.Combine(gameObject);
        }
    }
}