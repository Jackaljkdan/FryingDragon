using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class RandomizationToggleKey : MonoBehaviour
    {
        #region Inspector

        public KeyCode key = KeyCode.Y;

        #endregion

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(key))
            {
                foreach (var randomizer in transform.root.GetComponentsInChildren<AbstractRandomizer>())
                    randomizer.Randomize();
            }
        }
    }
}