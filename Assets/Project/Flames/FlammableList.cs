using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FlammableList : MonoBehaviour
    {
        #region Inspector

        public List<Flammable> list;

        private void Reset()
        {
            transform.root.GetComponentsInChildren(list);
        }

        #endregion
    }
}