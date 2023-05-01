using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class ScreenUtils
    {
        public static Vector3 NormalizePosition(Vector3 position)
        {
            return new Vector3(
                position.x / Screen.width,
                position.y / Screen.height,
                position.z
            );
        }

        public static Vector2 NormalizePosition(Vector2 position)
        {
            return new Vector2(
                position.x / Screen.width,
                position.y / Screen.height
            );
        }
    }
}