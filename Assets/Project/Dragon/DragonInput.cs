using JK.Actuators;
using JK.Injection;
using JK.UI;
using JK.Utils;
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

        public HybridAnimatedMovement dragonMovement;
        public DragonItemHolder dragonItemHolder;
        public DragonInteractore dragonInteractore;

        [RuntimeField]
        public bool shouldUseMouseInput = true;

        [RuntimeField]
        public Vector3 inertia;

        [Injected]
        public new Camera camera;

        [Injected]
        public Transform cameraTransform;

        [Injected]
        public InputTypeDetector inputTypeDetector;

        private void Reset()
        {
            dragonMovement = GetComponent<HybridAnimatedMovement>();
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
            inertia = dragonMovement.transform.forward;
            shouldUseMouseInput = true;
        }

        private void OnEnable()
        {
            dragonInteractore.enabled = true;
        }

        private void OnDisable()
        {
            dragonMovement.Move(Vector3.zero);
            dragonInteractore.enabled = false;
        }

        private Vector3 GetTargetForward()
        {
            if (inputTypeDetector == null || inputTypeDetector.current.Value == InputType.Keyboard)
            {
                Vector3 myScreenPosition = ScreenUtils.NormalizePosition(camera.WorldToScreenPoint(dragonMovement.transform.position).WithZ(0));
                Vector3 normalizedMousePosition = ScreenUtils.NormalizePosition(Input.mousePosition);

                return (normalizedMousePosition - myScreenPosition).normalized.WithSwappedYZ();
            }
            else
            {
                return new Vector3(
                    Input.GetAxis("RightHorizontal"),
                    0,
                    Input.GetAxis("RightVertical")
                );
            }
        }

        private void Update()
        {
            if (dropButton.GetAxisDown())
                dragonItemHolder.DropItem();

            Vector3 targetForward = GetTargetForward();

            float run = 1 + Input.GetAxis("Run");

            inertia = Vector3.Lerp(inertia, targetForward, TimeUtils.AdjustToFrameRate(mouseInertiaLerp * run)).normalized;

            Vector3 cameraRelative = Input.GetAxis("Vertical") * run * cameraTransform.forward.WithY(0).normalized;
            cameraRelative += Input.GetAxis("Horizontal") * run * cameraTransform.right.WithY(0).normalized;

            cameraRelative = Vector3.ClampMagnitude(cameraRelative, run);

            Vector3 dragonRelative = dragonMovement.transform.InverseTransformDirection(cameraRelative);

            dragonMovement.Move(dragonRelative, inertia);
        }
    }
}