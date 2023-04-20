using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class AxisPlayerCameraRaycastingActionQueue : ActionQueue
    {
        #region Inspector

        public string axis = "Fire1";

        public float maxDistance = 100;

        public LayerMask mask = LayerMaskUtils.Everything;

        [RuntimeField]
        public bool hasInput;

        [Injected]
        public Transform playerCamera;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            playerCamera = context.Get<Transform>(this, "player.camera");
        }

        private void Awake()
        {
            Inject();
        }

        public bool TryAction()
        {
            bool hasHit = Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, maxDistance, mask);

            return TryAction(new ActionArgs(
                playerCamera.position,
                playerCamera.forward,
                hasHit,
                hit
            ));
        }

        private void Update()
        {
            if (UnityEngine.Input.GetAxisRaw(axis) > 0)
            {
                if (hasInput)
                    return;

                hasInput = true;

                TryAction();
            }
            else
            {
                hasInput = false;
            }
        }
    }
}