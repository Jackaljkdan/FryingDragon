using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    public class ActiveRandomizer : AbstractRandomizer
    {
        #region Inspector

        public GameObject target;

        private void Reset()
        {
            target = transform.GetChild(0).GetGameObjectSafely();
        }

        #endregion

        public override void Randomize()
        {
            target.SetActive(UnityEngine.Random.Range(0, 2) == 0);
        }
    }
}