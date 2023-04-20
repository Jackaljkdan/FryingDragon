using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public static class ZoneUtils
    {

        public static bool AreVerticalLevelsEqual(Vector3 pos1, Vector3 pos2)
        {
            return Mathf.Abs(pos1.y - pos2.y) < 6f;

        }
    }
}