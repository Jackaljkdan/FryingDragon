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

        private Vector3 input;

        private void Start()
        {
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");

            deltaRotation = Quaternion.identity;

            characterControllerTransform = characterController.transform;
            characterControllerTransform.SetParent(transform.parent);
        }

        public void Move(Vector3 input)
        {
            this.input = input;
            animator.SetFloat(xHash, input.x != 0 ? input.x : input.y);
            animator.SetFloat(zHash, input.z);
        }

        private void OnAnimatorMove()
        {
            Transform myTransform = transform;

            deltaPosition = animator.deltaPosition;
            float magnitude = deltaPosition.magnitude;

            localDelta = myTransform.InverseTransformDirection(deltaPosition);
            if (input.x == 0)
                localDelta.x = 0;

            adjustedDelta = myTransform.TransformDirection(localDelta).WithY(0).normalized * magnitude * speed;
            characterController.Move(adjustedDelta.WithY(-9));

            deltaRotation *= Quaternion.Euler(0, input.y * TimeUtils.AdjustToFrameRate(rotationSpeed), 0);

            input = Vector2.zero;
        }

        private void FixedUpdate()
        {
            rb.MoveRotation(rb.rotation * deltaRotation);
            rb.MovePosition(characterControllerTransform.position);
            characterControllerTransform.rotation = rb.rotation;

            deltaRotation = Quaternion.identity;
        }
    }
}