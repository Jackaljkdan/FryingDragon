using JK.Injection;
using JK.UI;
using JK.Utils;
using Project.Character;
using System;
using System.Collections;
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

        public float mouseInertiaLerp = 0.003f;
        public float rotationSmoothTime = 1;
        public float maxRotationSpeed = 1;
        public float joyRotationSpeed = 5f;
        public float joyInputThreshold = 0.1f;

        public DragonMovement dragonMovement;
        public DragonItemHolder dragonItemHolder;
        public DragonInteractore dragonInteractore;

        [RuntimeField]
        public bool shouldUseMouseInput = true;

        [RuntimeField]
        public Vector2 inertia;

        [Injected]
        public new Camera camera;

        [Injected]
        public Transform cameraTransform;

        [Injected]
        public InputTypeDetector inputTypeDetector;

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
            camera = context.Get<Camera>(this);
            cameraTransform = context.Get<Transform>(this, "camera");
            inputTypeDetector = context.GetOptional<InputTypeDetector>();
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            Vector3 fwd = dragonMovement.transform.forward;
            inertia = new Vector2(fwd.x, fwd.z);
            shouldUseMouseInput = true;
        }

        private void OnEnable()
        {
            dragonInteractore.enabled = true;
        }

        private void OnDisable()
        {
            dragonMovement.Move(Vector2.zero);
            dragonInteractore.enabled = false;
        }

        //private Vector2 dampVelocity;

        private Vector2 GetTargetForward()
        {
            if (inputTypeDetector == null || inputTypeDetector.current.Value == InputType.Keyboard)
            {
                Vector3 myScreenPosition = ScreenUtils.NormalizePosition(camera.WorldToScreenPoint(dragonMovement.transform.position).WithZ(0));
                Vector3 currentMousePos = Input.mousePosition;

                return (ScreenUtils.NormalizePosition(currentMousePos) - myScreenPosition).normalized;
            }
            else
            {
                return new Vector2(
                    Input.GetAxis("RightHorizontal"),
                    Input.GetAxis("RightVertical")
                );
            }
        }

        private void Update()
        {
            if (dropButton.GetAxisDown())
                dragonItemHolder.DropItem();

            Vector2 targetForward = GetTargetForward();

            float run = 1 + Input.GetAxis("Run");

            inertia = Vector2.Lerp(inertia, targetForward, TimeUtils.AdjustToFrameRate(mouseInertiaLerp * run)).normalized;

            //inertia = targetForward;
            //inertia = Vector2.SmoothDamp(inertia, targetForward, ref dampVelocity, rotationSmoothTime, maxRotationSpeed * run);

            Vector3 cameraRelative = Input.GetAxis("Vertical") * run * cameraTransform.forward.WithY(0).normalized;
            cameraRelative += Input.GetAxis("Horizontal") * run * cameraTransform.right.WithY(0).normalized;

            Vector3 dragonRelative = dragonMovement.characterControllerTransform.InverseTransformDirection(cameraRelative);

            //dragonMovement.Move(dragonRelative.WithY(mouseMultiplier * run * inertia));
            dragonMovement.Move(new Vector2(dragonRelative.x, dragonRelative.z), inertia);
        }
    }
}