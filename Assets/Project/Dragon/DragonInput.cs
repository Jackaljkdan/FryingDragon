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

        public AxisAsButtonClass dropButton = new AxisAsButtonClass("Drop");

        public float mouseInertiaLerp = 0.02f;

        public DragonMovement dragonMovement;
        public DragonItemHolder dragonItemHolder;

        [RuntimeField]
        public Vector2 inertia;

        [Injected]
        public new Transform camera;

        private void Reset()
        {
            dragonMovement = GetComponent<DragonMovement>();
            dragonItemHolder = GetComponent<DragonItemHolder>();
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

        private void Start()
        {
            Vector3 fwd = dragonMovement.transform.forward;
            inertia = new Vector2(fwd.x, fwd.z);
        }

        private void Update()
        {
            if (dropButton.GetAxisDown())
                dragonItemHolder.DropItem();

            inertia = (inertia + (new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseInertiaLerp)).normalized;

            float run = 1 + Input.GetAxis("Run");

            Vector3 cameraRelative = Input.GetAxis("Vertical") * run * camera.forward.WithY(0).normalized;
            cameraRelative += Input.GetAxis("Horizontal") * run * camera.right.WithY(0).normalized;

            Vector3 dragonRelative = dragonMovement.characterControllerTransform.InverseTransformDirection(cameraRelative);

            //dragonMovement.Move(dragonRelative.WithY(mouseMultiplier * run * inertia));
            dragonMovement.Move(new Vector2(dragonRelative.x, dragonRelative.z), inertia);
        }
    }
}