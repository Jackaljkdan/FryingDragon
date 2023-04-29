using JK.Injection;
using JK.Utils;
using Project.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    [EarlyExecutionOrder]
    public class DragonInput : MonoBehaviour
    {
        #region Inspector

        public float mouseInertiaLerp = 0.02f;

        public float mouseMultiplier = 4f;

        public float shiftMultiplier = 2f;

        public DragonMovement dragonMovement;

        [RuntimeField]
        public float inertia;

        [Injected]
        public new Transform camera;

        private void Reset()
        {
            dragonMovement = GetComponent<DragonMovement>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            camera = context.Get<Transform>(this, "camera");
        }

        private void Awake()
        {
            Inject();
        }

        private void Update()
        {
            inertia = Mathf.Lerp(inertia, Input.GetAxis("Mouse X"), mouseInertiaLerp);

            float run = 1 + Input.GetAxis("Run");


            dragonMovement.Move(
                camera.TransformDirection(new Vector3(
                    Input.GetAxis("Horizontal") * run,
                    0,
                    Input.GetAxis("Vertical") * run
                ))
                .WithY(0)
                .WithY(mouseMultiplier * run * inertia)
            );

            if (Input.GetKeyDown(KeyCode.G))
                GetComponent<DragonItemHolder>().DropItem();
        }
    }
}