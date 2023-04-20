using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    public abstract class AbstractAction : MonoBehaviour
    {
        #region Inspector



        #endregion

        protected virtual void Start()
        {
            // allow disabiling in inspector
        }

        public abstract bool TryAction(ActionArgs args);
    }
}