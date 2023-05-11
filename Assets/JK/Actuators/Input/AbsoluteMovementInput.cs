using JK.Injection;
using JK.UI;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    public class AbsoluteMovementInput : AbstractMovementInput
    {
        #region Inspector

        public float inertiaLerp = 0.03f;

        public bool alwaysRun = false;

        [RuntimeField]
        public Vector3 inertia;

        [Injected]
        public new Camera camera;

        [Injected]
        public Transform cameraTransform;

        [Injected]
        public InputTypeDetector inputTypeDetector;

        #endregion

        protected Context context;

        [InjectMethod]
        public virtual void Inject()
        {
            context = Context.Find(this);
            camera = context.Get<Camera>(this);
            cameraTransform = context.Get<Transform>(this, "camera");
            inputTypeDetector = context.GetOptional<InputTypeDetector>();
        }

        protected virtual void Awake()
        {
            Inject();
        }

        protected override void OnEnable()
        {
            inertia = movement.transform.forward;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            movement.Move(Vector3.zero);
            base.OnDisable();
        }

        private Vector3 GetTargetForward()
        {
            if (inputTypeDetector == null || inputTypeDetector.current.Value == InputType.Keyboard)
            {
                Vector3 myScreenPosition = ScreenUtils.NormalizePosition(camera.WorldToScreenPoint(movement.transform.position).WithZ(0));
                Vector3 normalizedMousePosition = ScreenUtils.NormalizePosition(UnityEngine.Input.mousePosition);

                return (normalizedMousePosition - myScreenPosition).normalized.WithSwappedYZ();
            }
            else
            {
                return new Vector3(
                    UnityEngine.Input.GetAxis("RightHorizontal"),
                    0,
                    UnityEngine.Input.GetAxis("RightVertical")
                );
            }
        }

        protected void Update()
        {
            Vector3 targetForward = GetTargetForward();

            float run = !alwaysRun
                ? 1 + UnityEngine.Input.GetAxis("Run")
                : 2
            ;

            inertia = Vector3.Lerp(inertia, targetForward, TimeUtils.AdjustToFrameRate(inertiaLerp * run)).normalized;

            Vector3 cameraRelative = UnityEngine.Input.GetAxis("Vertical") * run * cameraTransform.forward.WithY(0).normalized;
            cameraRelative += UnityEngine.Input.GetAxis("Horizontal") * run * cameraTransform.right.WithY(0).normalized;

            cameraRelative = Vector3.ClampMagnitude(cameraRelative, run);

            Vector3 movementRelative = movement.transform.InverseTransformDirection(cameraRelative);

            movement.Move(movementRelative, inertia);
        }
    }
}