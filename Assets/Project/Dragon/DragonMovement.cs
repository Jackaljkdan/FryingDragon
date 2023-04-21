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

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 cameraForward = mainCamera.forward.WithY(0).normalized;
            Vector3 cameraRight = mainCamera.right.WithY(0).normalized;

            movement = cameraForward * vertical + cameraRight * horizontal;
            Vector3 localMovement = transform.InverseTransformDirection(movement);

            animator.SetFloat(xHash, localMovement.x);
            animator.SetFloat(zHash, localMovement.z);
        }

        private void OnAnimatorMove()
        {
            deltaPosition += animator.deltaPosition;
            deltaRotation *= animator.deltaRotation;
        }

        private void FixedUpdate()
        {
            deltaPosition.y = 0;

            rb.MovePosition(rb.position + deltaPosition);

            Vector3 movementDirection = new Vector3(movement.x, 0, movement.z);

            if (movementDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed));
            }

            deltaPosition = Vector3.zero;
            deltaRotation = Quaternion.identity;
        }
    }
}