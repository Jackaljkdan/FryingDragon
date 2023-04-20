using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class MaterialUtils
    {
        private static int emissionColorId;

        public static Color GetEmissionColor(this Material self)
        {
            if (emissionColorId == 0)
                emissionColorId = Shader.PropertyToID("_EmissionColor");

            return self.GetColor(emissionColorId);
        }

        public static void SetEmissionColor(this Material self, Color color)
        {
            if (emissionColorId == 0)
                emissionColorId = Shader.PropertyToID("_EmissionColor");

            self.SetColor(emissionColorId, color);
        }
    }
}