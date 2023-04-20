using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public class InjectedGameObjectedToggleKey : MonoBehaviour
    {
        #region Inspector

        public KeyCode key;

        public bool needsShift = false;
        public bool needsCtrl = false;

        [Conditional(nameof(onlyToggleOn), inverse: true)]
        public bool onlyToggleOff = false;
        [Conditional(nameof(onlyToggleOff), inverse: true)]
        public bool onlyToggleOn = false;

        public string injectionId = string.Empty;

        [Injected]
        public GameObject target;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            target = context.Get<GameObject>(this, injectionId);
        }

        private void Awake()
        {
            Inject();
        }

        private void Update()
        {
            if (target == null)
                return;

            if (!UnityEngine.Input.GetKeyDown(key))
                return;

            if (needsShift && !InputUtils.GetAnyShift())
                return;

            if (needsCtrl && !InputUtils.GetAnyControl())
                return;

            if (onlyToggleOff && !target.activeSelf)
                return;

            if (onlyToggleOn && target.activeSelf)
                return;

            target.SetActive(!target.activeSelf);
        }
    }
}