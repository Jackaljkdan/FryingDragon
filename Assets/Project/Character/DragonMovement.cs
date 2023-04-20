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

        public Rigidbody rb;

        public float speed = 5.0f;
        public float lerpSpeed = 0.15f;

        [RuntimeField]
        public Vector3 moveDirection;

        [RuntimeField]
        public Vector3 velocity;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        #endregion 

        private void Start()
        {
            moveDirection = Vector3.zero;
            velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection = Vector3.ClampMagnitude(moveDirection, 1) * speed;
            velocity = Vector3.Lerp(velocity, moveDirection, lerpSpeed);
            rb.velocity = velocity;
        }
    }
}