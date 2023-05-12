using JK.Injection;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class HybridAnimatedMovement : AbstractMovement
    {
        #region Inspector

        public Animator animator;
        public Rigidbody rb;
        public CharacterController characterController;

        [RuntimeField]
        public Transform characterControllerTransform;

        public float speed = 1.0f;
        public float xLerp = 0.1f;

        public float gravity = -9;

        [RuntimeField]
        public Vector3 movementInput;
        [RuntimeField]
        public Vector3 localMovementInput;
        [RuntimeField]
        public Vector3 forwardInput;

        [RuntimeField]
        public float xInertia;

        [RuntimeField]
        public Vector3 deltaPosition;

        [DebugField]
        public float deltaMagnitude;

        [DebugField]
        public Vector3 actualMovement;

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

            Transform myTransform = transform;

            localMovementInput = myTransform.InverseTransformDirection(movementInput);

            float rightDot = Vector3.Dot(myTransform.right, forwardInput);
            rightDot = Mathf.Min(1, Mathf.Max(-1, rightDot * 100));

            float xTarget = localMovementInput.x != 0 ? localMovementInput.x : rightDot;
            xInertia = Mathf.Lerp(xInertia, xTarget, TimeUtils.AdjustToFrameRate(xLerp));

            animator.SetFloat(xHash, xInertia);
            animator.SetFloat(zHash, localMovementInput.z);
        }

        private void OnAnimatorMove()
        {
            Transform myTransform = transform;

            deltaPosition = animator.deltaPosition;
            deltaMagnitude = deltaPosition.magnitude;

            actualMovement = movementInput.normalized * deltaMagnitude * speed;
            characterController.Move(actualMovement.WithY(TimeUtils.AdjustToFrameRate(gravity)));

            movementInput = Vector3.zero;
        }

        private void FixedUpdate()
        {
            rb.MoveRotation(Quaternion.LookRotation(forwardInput));
            rb.MovePosition(characterControllerTransform.position);
            characterControllerTransform.rotation = rb.rotation;
        }
    }
}