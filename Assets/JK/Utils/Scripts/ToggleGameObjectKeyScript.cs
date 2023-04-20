using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class ToggleGameObjectKeyScript : MonoBehaviour
    {
        #region Inspector

        public GameObject target;

        public KeyCode key;

        #endregion

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(key))
                if (target != null)
                    target.SetActive(!target.activeSelf);
        }
    }
}