using JK.Actuators;
using JK.Injection;
using JK.Utils;
using Project.Dragon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FirefighterInput : MonoBehaviour
    {
        #region Inspector

        public float mouseInertiaLerp = 0.01f;
        public float joyRotationSpeed = 5f;
        public float joyInputThreshold = 0.1f;

        public CharacterControllerAnimatedMovement movement;

        public ParticleSystem extinguisherParticleSystem;

        public AudioSource extinguisherAudioSource;

        public DragonInteractore interactore;

        [RuntimeField]
        public Vector3 inertia;

        [Injected]
        public new Camera camera;

        [Injected]
        public Transform cameraTransform;

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            camera = context.Get<Camera>(this);
            cameraTransform = context.Get<Transform>(this, "camera");
        }

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            inertia = movement.transform.forward;
        }

        private void OnEnable()
        {
            interactore.enabled = true;
        }

        private void OnDisable()
        {
            interactore.enabled = false;
            movement.Move(Vector3.zero);
        }

        bool shouldUseMouseInput = false;
        private Vector3 lastMousePosition = Vector3.zero;

        private Vector3 GetTargetForward()
        {
            Vector3 myScreenPosition = ScreenUtils.NormalizePosition(camera.WorldToScreenPoint(movement.transform.position).WithZ(0));
            Vector3 currentMousePos = Input.mousePosition;
            float horizontal = Input.GetAxis("RightHorizontal");
            float vertical = Input.GetAxis("RightVertical");

            if (!shouldUseMouseInput && lastMousePosition != currentMousePos)
                shouldUseMouseInput = true;

            if (Mathf.Abs(horizontal) > joyInputThreshold || Mathf.Abs(vertical) > joyInputThreshold)
                shouldUseMouseInput = false;

            lastMousePosition = currentMousePos;

            if (shouldUseMouseInput)
                return (ScreenUtils.NormalizePosition(currentMousePos) - myScreenPosition).WithSwappedYZ().normalized;


            return new Vector3(horizontal, 0, vertical).normalized;
        }

        private void Update()
        {
            Vector3 targetForward = GetTargetForward();

            float run = 2;

            inertia = Vector3.Lerp(inertia, targetForward, TimeUtils.AdjustToFrameRate(mouseInertiaLerp * run)).normalized;

            Vector3 cameraRelative = Input.GetAxis("Vertical") * run * cameraTransform.forward.WithY(0).normalized;
            cameraRelative += Input.GetAxis("Horizontal") * run * cameraTransform.right.WithY(0).normalized;

            Vector3 relative = movement.transform.InverseTransformDirection(cameraRelative);

            movement.Move(relative, inertia);
        }
    }
}