using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class CharacterControllerAnimatedMovement : AbstractMovement
    {
        #region Inspector

        public float speed = 1;
        public float gravity = -9;

        public Animator animator;
        public CharacterController characterController;

        [RuntimeField]
        public Vector3 movementInput;
        [RuntimeField]
        public Vector3 localMovementInput;
        [RuntimeField]
        public Vector3 forwardInput;

        [RuntimeField]
        public Transform characterControllerTransform;

        [DebugField]
        public Vector3 deltaPosition;

        [DebugField]
        public float deltaMagnitude;

        [DebugField]
        public Vector3 actualMovement;

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

            localMovementInput = transform.InverseTransformDirection(movementInput);

            animator.SetFloat(xHash, localMovementInput.x);
            animator.SetFloat(zHash, localMovementInput.z);
        }

        private void OnAnimatorMove()
        {
            deltaPosition = animator.deltaPosition;
            deltaMagnitude = deltaPosition.magnitude;

            actualMovement = movementInput.normalized * deltaMagnitude * speed;
            characterController.Move(actualMovement.WithY(TimeUtils.AdjustToFrameRate(gravity)));

            characterControllerTransform.rotation = Quaternion.LookRotation(forwardInput);

            movementInput = Vector3.zero;
        }
    }
}