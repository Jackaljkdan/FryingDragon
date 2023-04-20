using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class GizmosUtils
    {
        public static void WithRestoredColor(UnityAction action)
        {
            Color prev = Gizmos.color;

            action();

            Gizmos.color = prev;
        }
    }
}