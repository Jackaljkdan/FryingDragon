using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class CharacterControllerHitListener : MonoBehaviour
    {
        #region Inspector

        public UnityEvent onCollisionEnter = new UnityEvent();
        public UnityEvent onCollisionStay = new UnityEvent();

        [RuntimeHeader]
        public int lastHitFrame;

        [RuntimeField]
        public CharacterController lastHit;

        #endregion

        public void OnControllerColliderHit(ControllerColliderHit hit)
        {
            lastHit = hit.controller;

            if (Time.frameCount != lastHitFrame + 1)
                onCollisionEnter.Invoke();
            else
                onCollisionStay.Invoke();

            lastHitFrame = Time.frameCount;
        }
    }
}
