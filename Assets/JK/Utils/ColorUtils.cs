using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class ColorUtils
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            Color copy = color;
            copy.a = alpha;
            return copy;
        }

        public static Color Darkened(this Color color, float intensity)
        {
            Color darkened = new Color(
                r: color.r * intensity,
                g: color.g * intensity,
                b: color.b * intensity,
                a: color.a
            );

            return darkened;
        }
    }
}