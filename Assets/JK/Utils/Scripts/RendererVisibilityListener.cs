using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public class RendererVisibilityListener : MonoBehaviour
    {
        #region Inspector

        public UnityEvent onBecameVisible = new UnityEvent();
        public UnityEvent onBecameInvisible = new UnityEvent();

        #endregion

        private void OnBecameVisible()
        {
            onBecameVisible.Invoke();
        }

        private void OnBecameInvisible()
        {
            onBecameInvisible.Invoke();
        }
    }
}