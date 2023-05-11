using JK.Actuators;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FirefighterMovement : AbstractMovement
    {
        #region Inspector

        public float speed = 1;
        public float gravity = -9;

        public Animator animator;
        public CharacterController characterController;

        [RuntimeField]
        public Vector3 movementInput;
        [RuntimeField]
        public Vector3 forwardInput;

        [RuntimeField]
        public Transform characterControllerTransform;

        [RuntimeField]
        public Vector3 deltaPosition;

        [DebugField]
        public Vector3 localDelta;

        [DebugField]
        public Vector3 adjustedDelta;

        private void Reset()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        #endregion

        [HideInInspector]
        public int xHash;
        [HideInInspector]
        public int zHash;

        private void Awake()
        {
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");

            characterControllerTransform = characterController.transform;

            forwardInput = transform.forward;
        }

        private void Start()
        {

        }

        public override void Move(Vector3 movementInput, Vector3 forwardInput)
        {
            this.movementInput = movementInput;
            this.forwardInput = forwardInput;

            animator.SetFloat(xHash, movementInput.x);
            animator.SetFloat(zHash, movementInput.z);
        }

        private void OnAnimatorMove()
        {
            deltaPosition = animator.deltaPosition;
            float magnitude = deltaPosition.magnitude;

            localDelta = characterControllerTransform.InverseTransformDirection(deltaPosition);
            if (movementInput.x == 0)
                localDelta.x = 0;

            adjustedDelta = characterControllerTransform.TransformDirection(localDelta).WithY(0).normalized * magnitude * speed;
            characterController.Move(adjustedDelta.WithY(gravity));

            characterControllerTransform.rotation = Quaternion.LookRotation(forwardInput);

            movementInput = Vector3.zero;
        }
    }
}