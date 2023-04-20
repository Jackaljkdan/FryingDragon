using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class AlternativesToggleKey : MonoBehaviour
    {
        #region Inspector

        public KeyCode key = KeyCode.U;

        #endregion

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(key))
            {
                foreach (var alternatives in transform.root.GetComponentsInChildren<RandomAddressableAlternatives>())
                    alternatives.Choose();
            }
        }
    }
}