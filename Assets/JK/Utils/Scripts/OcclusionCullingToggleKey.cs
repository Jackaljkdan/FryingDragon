using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class OcclusionCullingToggleKey : MonoBehaviour
    {
        #region Inspector

        public KeyCode key = KeyCode.Alpha0;

        #endregion

        public Dictionary<Camera, bool> occlusionValuesOnStart;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(key))
            {
                if (occlusionValuesOnStart == null)
                    occlusionValuesOnStart = new Dictionary<Camera, bool>();

                foreach (Camera camera in transform.root.GetComponentsInChildren<Camera>())
                {
                    if (!occlusionValuesOnStart.TryGetValue(camera, out bool startedOn))
                    {
                        startedOn = camera.useOcclusionCulling;
                        occlusionValuesOnStart[camera] = startedOn;
                    }

                    if (startedOn)
                        camera.useOcclusionCulling = !camera.useOcclusionCulling;
                }
            }
        }
    }
}