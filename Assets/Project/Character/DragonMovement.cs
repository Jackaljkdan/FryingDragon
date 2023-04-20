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

        private Vector3 moveDirection;

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }
        #endregion 

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            moveDirection = new Vector3(horizontal, 0, vertical);

            moveDirection = moveDirection.normalized * speed;

        }

        private void FixedUpdate()
        {

            rb.velocity = Vector3.Lerp(rb.velocity, moveDirection, lerpSpeed);
        }
    }
}