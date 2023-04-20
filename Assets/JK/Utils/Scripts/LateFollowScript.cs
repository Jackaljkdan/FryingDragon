using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class LateFollowScript : FollowScript
    {
        #region Inspector



        #endregion

        private void LateUpdate()
        {
            transform.position = target.position + offset;
        }
    }
}