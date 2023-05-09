using JK.Actuators;
using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Project.Character
{
    [DisallowMultipleComponent]
    public class DragonMovement : AbstractMovement
    {
        #region Inspector

        public Animator animator;
        public Rigidbody rb;
        public CharacterController characterController;

        [RuntimeField]
        public Transform characterControllerTransform;

        public float speed = 5.0f;
        public float lerpSpeed = 0.15f;
        public float rotationSpeed = 10.0f;

        public float xLerp = 0.1f;

        [RuntimeField]
        public Vector3 movementInput;
        [RuntimeField]
        public Vector3 forwardInput;

        [RuntimeField]
        public float xInertia;

        [RuntimeField]
        public Vector3 deltaPosition;

        [RuntimeField]
        public Quaternion deltaRotation;

        [DebugField]
        public Vector3 localDelta;

        [DebugField]
        public Vector3 adjustedDelta;

        [Injected]
        public Transform mainCamera;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            mainCamera = context.Get<Transform>(this, "camera");
        }
        private void Awake()
        {
            Inject();
        }

        private void Reset()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
        }

        #endregion

        [HideInInspector]
        public int xHash;
        [HideInInspector]
        public int zHash;

        private void Start()
        {
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");

            xInertia = 0;
            deltaRotation = Quaternion.identity;
            forwardInput = transform.forward;

            characterControllerTransform = characterController.transform;
            characterControllerTransform.SetParent(transform.parent, worldPositionStays: true);
        }

        private void OnEnable()
        {
            characterController.transform.position = rb.position;
            characterController.enabled = true;
        }

        private void OnDisable()
        {
            characterController.enabled = false;
        }

        public override void Move(Vector3 movementInput, Vector3 forwardInput)
        {
            this.movementInput = movementInput;
            this.forwardInput = forwardInput;

            float rightDot = Vector3.Dot(transform.right, forwardInput);
            rightDot = Mathf.Min(1, Mathf.Max(-1, rightDot * 100));

            float xTarget = movementInput.x != 0 ? movementInput.x : rightDot;
            xInertia = Mathf.Lerp(xInertia, xTarget, TimeUtils.AdjustToFrameRate(xLerp));

            animator.SetFloat(xHash, xInertia);
            animator.SetFloat(zHash, movementInput.z);
        }

        private void OnAnimatorMove()
        {
            Transform myTransform = transform;

            deltaPosition = animator.deltaPosition;
            float magnitude = deltaPosition.magnitude;

            localDelta = myTransform.InverseTransformDirection(deltaPosition);
            if (movementInput.x == 0)
                localDelta.x = 0;

            adjustedDelta = myTransform.TransformDirection(localDelta).WithY(0).normalized * magnitude * speed;
            characterController.Move(adjustedDelta.WithY(-9));

            movementInput = Vector3.zero;
        }

        private void FixedUpdate()
        {
            rb.MoveRotation(Quaternion.LookRotation(forwardInput));
            rb.MovePosition(characterControllerTransform.position);
            characterControllerTransform.rotation = rb.rotation;

            deltaRotation = Quaternion.identity;
        }
    }
}