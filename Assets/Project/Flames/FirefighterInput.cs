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

        public FirefighterMovement movement;

        public ParticleSystem extinguisherParticleSystem;

        public AudioSource extinguisherAudioSource;

        public DragonInteractore interactore;

        [RuntimeField]
        public Vector2 inertia;

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
            Vector3 fwd = movement.transform.forward;
            inertia = new Vector2(fwd.x, fwd.z);
        }

        private void OnEnable()
        {
            interactore.enabled = true;
        }

        private void OnDisable()
        {
            interactore.enabled = false;
            Vector3 fwd = movement.transform.forward;
            movement.Move(new Vector2(0, 0), new Vector2(fwd.x, fwd.z));
        }

        bool shouldUseMouseInput = false;
        private Vector3 lastMousePosition = Vector3.zero;

        private Vector2 GetTargetForward()
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
                return (ScreenUtils.NormalizePosition(currentMousePos) - myScreenPosition).normalized;


            return new Vector2(horizontal, vertical).normalized;
        }

        private void Update()
        {
            Vector3 myScreenPosition = ScreenUtils.NormalizePosition(camera.WorldToScreenPoint(movement.transform.position).WithZ(0));
            Vector2 targetForward = GetTargetForward();

            float run = 2;

            inertia = Vector2.Lerp(inertia, targetForward, TimeUtils.AdjustToFrameRate(mouseInertiaLerp * run)).normalized;

            Vector3 cameraRelative = Input.GetAxis("Vertical") * run * cameraTransform.forward.WithY(0).normalized;
            cameraRelative += Input.GetAxis("Horizontal") * run * cameraTransform.right.WithY(0).normalized;

            Vector3 relative = movement.transform.InverseTransformDirection(cameraRelative);

            movement.Move(new Vector2(relative.x, relative.z), inertia);
        }
    }
}