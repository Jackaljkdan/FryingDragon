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
    public class DragonMovement : MonoBehaviour
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

        [RuntimeField]
        public Vector3 deltaPosition;

        [RuntimeField]
        public Quaternion deltaRotation;

        [DebugField]
        public Vector3 localDelta;

        [DebugField]
        public Vector3 adjustedDelta;

        [DebugField]
        public Vector2 input;

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

        private int xHash;
        private int zHash;

        private void Start()
        {
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");

            deltaPosition = Vector3.zero;
            deltaRotation = Quaternion.identity;
            input = Vector3.zero;

            characterControllerTransform = characterController.transform;
            characterControllerTransform.SetParent(transform.parent);
        }

        private void Update()
        {
            input = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );

            animator.SetFloat(xHash, input.x);
            animator.SetFloat(zHash, input.y);

        }

        private void LateUpdate()
        {
        }

        private void OnAnimatorMove()
        {
            Transform myTransform = transform;

            deltaPosition = animator.deltaPosition;
            float magnitude = deltaPosition.magnitude;

            localDelta = myTransform.InverseTransformDirection(deltaPosition).WithX(0);
            if (input.y == 0)
                localDelta.z = 0;

            adjustedDelta = myTransform.TransformDirection(localDelta).WithY(0).normalized * magnitude * speed;
            characterController.Move(adjustedDelta.WithY(-9));

            deltaRotation *= Quaternion.Euler(0, input.x * TimeUtils.AdjustToFrameRate(rotationSpeed), 0);
        }

        [DebugField]
        public Collider sweepHit;

        private void FixedUpdate()
        {
            rb.MoveRotation(rb.rotation * deltaRotation);
            rb.MovePosition(characterControllerTransform.position);
            characterControllerTransform.rotation = rb.rotation;

            deltaRotation = Quaternion.identity;
        }
    }
}