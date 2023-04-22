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

        public float speed = 5.0f;
        public float lerpSpeed = 0.15f;
        public float rotationSpeed = 10.0f;

        [RuntimeField]
        public Vector3 deltaPosition;

        [RuntimeField]
        public Quaternion deltaRotation;

        [RuntimeField]
        public Vector3 velocity;

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
        private Vector3 movement;

        private void Start()
        {
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");

            deltaPosition = Vector3.zero;
            deltaRotation = Quaternion.identity;

            velocity = Vector3.zero;
        }

        float horizontal;
        private void Update()
        {
            horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            animator.SetFloat(xHash, horizontal);
            animator.SetFloat(zHash, vertical);
        }

        private void OnAnimatorMove()
        {
            deltaPosition += animator.deltaPosition;
            deltaRotation *= Quaternion.Euler(0, horizontal * rotationSpeed, 0);
        }

        private void FixedUpdate()
        {
            float magnitude = deltaPosition.magnitude;
            Vector3 localDelta = transform.InverseTransformDirection(animator.deltaPosition).WithX(0);
            deltaPosition = transform.TransformDirection(localDelta).WithY(0).normalized * magnitude;
            rb.MovePosition(rb.position + deltaPosition);
            rb.MoveRotation(rb.rotation * deltaRotation);

            deltaPosition = Vector3.zero;
            deltaRotation = Quaternion.identity;
        }
    }
}