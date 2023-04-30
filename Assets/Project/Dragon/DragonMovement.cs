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

        private Vector2 movementInput;
        private Vector2 rotationInput;

        private void Start()
        {
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");

            deltaRotation = Quaternion.identity;
            Vector3 fwd = transform.forward;
            rotationInput = new Vector2(fwd.x, fwd.z);

            characterControllerTransform = characterController.transform;
            characterControllerTransform.SetParent(transform.parent, worldPositionStays: true);
        }

        public void Move(Vector2 movementInput)
        {
            Vector3 fwd = transform.forward;
            Move(movementInput, new Vector2(fwd.x, fwd.z));
        }

        public void Move(Vector2 movementInput, Vector2 rotationInput)
        {
            this.movementInput = movementInput;
            this.rotationInput = rotationInput;

            Vector3 rotationInput3 = new Vector3(rotationInput.x, 0, rotationInput.y);
            float rightDot = Vector3.Dot(transform.right, rotationInput3);
            rightDot = Mathf.Min(1, Mathf.Max(-1, rightDot * 100));

            animator.SetFloat(xHash, movementInput.x != 0 ? movementInput.x : rightDot);
            animator.SetFloat(zHash, movementInput.y);
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

            //deltaRotation *= Quaternion.Euler(0, input.y * TimeUtils.AdjustToFrameRate(rotationSpeed), 0);
            //targetRotation = Quaternion.LookRotation(new Vector3(rotationInput.x, 0, rotationInput.y));

            movementInput = Vector2.zero;
            //rotationInput = Vector2.zero;
        }

        private void FixedUpdate()
        {
            //rb.MoveRotation(rb.rotation * deltaRotation);
            rb.MoveRotation(Quaternion.LookRotation(new Vector3(rotationInput.x, 0, rotationInput.y)));
            rb.MovePosition(characterControllerTransform.position);
            characterControllerTransform.rotation = rb.rotation;

            deltaRotation = Quaternion.identity;
        }
    }
}