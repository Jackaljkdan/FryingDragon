using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        private Vector2 input;

        private void Awake()
        {
            xHash = Animator.StringToHash("X");
            zHash = Animator.StringToHash("Z");

            characterControllerTransform = characterController.transform;
        }

        private void Start()
        {

        }

        public void Move(Vector2 input)
        {
            this.input = input;
            animator.SetFloat(xHash, input.x);
            animator.SetFloat(zHash, input.y);
        }

        private void OnAnimatorMove()
        {
            deltaPosition = animator.deltaPosition;
            float magnitude = deltaPosition.magnitude;

            localDelta = characterControllerTransform.InverseTransformDirection(deltaPosition).WithX(0);
            if (input.y == 0)
                localDelta.z = 0;

            characterControllerTransform.Rotate(0, input.x * Mathf.Sign(input.y) * TimeUtils.AdjustToFrameRate(rotationSpeed), 0);

            adjustedDelta = characterControllerTransform.TransformDirection(localDelta).WithY(0).normalized * magnitude * speed;
            characterController.Move(adjustedDelta.WithY(-9));

            input = Vector2.zero;
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