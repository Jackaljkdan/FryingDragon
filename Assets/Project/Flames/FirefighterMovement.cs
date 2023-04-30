using DG.Tweening;
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
    public class FirefighterMovement : MonoBehaviour
    {
        #region Inspector

        public float speed = 1;
        public float rotationSpeed = 1;

        public Animator animator;
        public IkAnchors ikAnchors;
        public CharacterController characterController;

        [RuntimeField]
        public float ikWeight;

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

        private int xHash;
        private int zHash;

        [RuntimeField]
        public Vector2 movementInput;
        [RuntimeField]
        public Vector2 rotationInput;

        private void Awake()
        {
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");

            characterControllerTransform = characterController.transform;
        }

        private void Start()
        {

        }

        public void Move(Vector2 movementInput, Vector2 rotationInput)
        {
            this.movementInput = movementInput;
            this.rotationInput = rotationInput;

            animator.SetFloat(xHash, movementInput.x);
            animator.SetFloat(zHash, movementInput.y);
        }

        private void OnAnimatorMove()
        {
            deltaPosition = animator.deltaPosition;
            float magnitude = deltaPosition.magnitude;

            localDelta = characterControllerTransform.InverseTransformDirection(deltaPosition);
            if (movementInput.x == 0)
                localDelta.x = 0;

            adjustedDelta = characterControllerTransform.TransformDirection(localDelta).WithY(0).normalized * magnitude * speed;
            characterController.Move(adjustedDelta.WithY(-9));

            characterControllerTransform.rotation = Quaternion.LookRotation(new Vector3(rotationInput.x, 0, rotationInput.y));

            movementInput = Vector2.zero;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            ikAnchors.SetIKWithWeight(animator, ikWeight);
        }

        private Tween ikWeightTween;

        public Tween DOIkWeight(float endValue, float seconds)
        {
            ikWeightTween?.Kill();

            ikWeightTween = DOTween.To(
                () => ikWeight,
                val => ikWeight = val,
                endValue,
                seconds
            );

            ikWeightTween.SetTarget(this);

            return ikWeightTween;
        }
    }
}