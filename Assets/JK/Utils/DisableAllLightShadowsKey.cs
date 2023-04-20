using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class DisableAllLightShadowsKey : MonoBehaviour
    {
        #region Inspector

        public KeyCode key = KeyCode.O;

        #endregion

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(key))
            {
                foreach (Light light in transform.root.GetComponentsInChildren<Light>())
                    light.shadows = LightShadows.None;
            }
        }
    }
}