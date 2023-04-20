using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    public class LightCubeLOD : MonoBehaviour
    {
        #region Inspector

        //public new Light light;

        //public float sizeMultiplier = 1;

        //private void Reset()
        //{
        //    OnValidate();
        //}

        //[ContextMenu("Validate"), AutomatedTask]
        //private void OnValidate()
        //{
        //    if (light == null)
        //        light = GetComponentInParent<Light>();

        //    EnvelopeLight();
        //    gameObject.SetActive(false);

        //    UndoUtils.SetDirty(this);
        //    UndoUtils.SetDirty(gameObject);
        //}

        #endregion

        //public void EnvelopeLight()
        //{
        //    if (light == null)
        //        return;

        //    float size = light.range * 2 * sizeMultiplier;

        //    transform.localScale = new Vector3(
        //        size,
        //        size,
        //        size
        //    );
        //}
    }
}