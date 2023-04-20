using JK.Attention;
using JK.Lighting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class LightUtils
    {
        private static List<AbstractFlicker> flickersBuffer;

        private static void SetFlickersEnabled(this Component self, bool enabled)
        {
            if (flickersBuffer == null)
                flickersBuffer = new List<AbstractFlicker>(4);

            self.GetComponentsInChildren(flickersBuffer);

            foreach (AbstractFlicker flicker in flickersBuffer)
                flicker.enabled = enabled;
        }

        public static void SetFlickersEnabled(this Light self, bool enabled)
        {
            SetFlickersEnabled((Component)self, enabled);
        }

        public static void SetFlickersEnabled(this LightLOD self, bool enabled)
        {
            SetFlickersEnabled((Component)self, enabled);
        }
    }
}