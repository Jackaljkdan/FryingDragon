using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class LayerMaskUtils
    {
        public static readonly LayerMask Everything = ~0;

        public static int GetLayerBitmask(string layerName)
        {
            return GetLayerBitmask(LayerMask.NameToLayer(layerName));
        }

        public static int GetLayerBitmask(int layerIndex)
        {
            return 1 << layerIndex;
        }
    }
}