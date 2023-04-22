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
        public Rigidbody sweepTester;

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
        public Vector2 cumulativeInput;

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
            cumulativeInput = Vector3.zero;
        }

        float horizontal;
        private void Update()
        {
            horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            cumulativeInput.x += horizontal;
            cumulativeInput.y += vertical;

            animator.SetFloat(xHash, horizontal);
            animator.SetFloat(zHash, vertical);
        }

        private void OnAnimatorMove()
        {
            deltaPosition += animator.deltaPosition;
            deltaRotation *= Quaternion.Euler(0, horizontal * TimeUtils.AdjustToFrameRate(rotationSpeed), 0);
        }

        [DebugField]
        public Collider sweepHit;

        private void FixedUpdate()
        {
            float magnitude = deltaPosition.magnitude;

            localDelta = transform.InverseTransformDirection(animator.deltaPosition).WithX(0);
            if (cumulativeInput.y == 0)
                localDelta.z = 0;

            adjustedDelta = transform.TransformDirection(localDelta).WithY(0).normalized * magnitude;

            rb.MoveRotation(rb.rotation * deltaRotation);

            if (sweepTester.SweepTest(adjustedDelta, out RaycastHit hit, adjustedDelta.magnitude, QueryTriggerInteraction.Ignore))
                sweepHit = hit.collider;
            else
            {
                sweepHit = null;
                rb.MovePosition(rb.position + adjustedDelta);
            }


            cumulativeInput = Vector2.zero;
            deltaPosition = Vector3.zero;
            deltaRotation = Quaternion.identity;
        }
    }
}