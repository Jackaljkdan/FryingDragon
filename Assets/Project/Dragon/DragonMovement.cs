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

        [RuntimeField]
        public Vector3 deltaPosition;

        [RuntimeField]
        public Quaternion deltaRotation;

        [RuntimeField]
        public Vector3 velocity;

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

            velocity = Vector3.zero;
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            animator.SetFloat(xHash, horizontal);
            animator.SetFloat(zHash, vertical);
        }

        private void OnAnimatorMove()
        {
            deltaPosition += animator.deltaPosition;
            deltaRotation *= animator.deltaRotation;
        }

        private void FixedUpdate()
        {
            rb.MovePosition(rb.position + deltaPosition);
            //rb.MoveRotation(rb.rotation * deltaRotation);

            deltaPosition = Vector3.zero;
            deltaRotation = Quaternion.identity;
        }
    }
}