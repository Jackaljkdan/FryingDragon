using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class InjectedLookAt : MonoBehaviour
    {
        #region Inspector

        public string injectionId = string.Empty;

        [Injected]
        public Transform target;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            target = context.Get<Transform>(this, injectionId);
        }

        private void Awake()
        {
            Inject();
        }

        private void LateUpdate()
        {
            Transform myTransform = transform;
            myTransform.rotation = Quaternion.LookRotation(target.position - myTransform.position, Vector3.up);
        }
    }
}