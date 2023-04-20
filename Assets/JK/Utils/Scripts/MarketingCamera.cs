using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class MarketingCamera : MonoBehaviour
    {
        #region Inspector

        public KeyCode activationKey = KeyCode.Keypad0;

        public bool isActive = false;

        [Tooltip("Rotate right, if shift is down negatively rotate on z")]
        public KeyCode positiveRotationKey = KeyCode.E;
        [Tooltip("Rotate left, if shift is down positively rotate on z")]
        public KeyCode negativeRotationKey = KeyCode.Q;

        public KeyCode upKey = KeyCode.Z;
        public KeyCode downKey = KeyCode.X;

        public float movementSpeed = 0.01f;
        public float rotationSpeed = 1;
        public float zRotationSpeed = 1;

        public List<string> injectedObjectsToDisableOnActivate = new List<string>()
        {
            "fps",
            "version",
        };

        public UnityEvent onActivate = new UnityEvent();
        public UnityEvent onDeactivate = new UnityEvent();

        [RuntimeField]
        public Vector3 localPositionOnActivation;

        #endregion

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(activationKey))
            {
                isActive = !isActive;

                if (isActive)
                {
                    onActivate.Invoke();
                    localPositionOnActivation = transform.localPosition;

                    Context context = Context.Find(this);
                    foreach (string id in injectedObjectsToDisableOnActivate)
                        context.Get<GameObject>(this, id).SetActive(false);
                }
                else
                {
                    transform.localPosition = localPositionOnActivation;
                    onDeactivate.Invoke();
                }
            }

            if (UnityEngine.Input.GetKey(KeyCode.Keypad9))
                transform.Rotate(Vector3.right, TimeUtils.AdjustToFrameRate(rotationSpeed) * -1, Space.Self);

            if (UnityEngine.Input.GetKey(KeyCode.Keypad8))
                transform.Rotate(Vector3.right, TimeUtils.AdjustToFrameRate(rotationSpeed) * 1, Space.Self);

            if (UnityEngine.Input.GetKey(KeyCode.Keypad4))
                transform.Rotate(Vector3.up, TimeUtils.AdjustToFrameRate(rotationSpeed) * -1, Space.World);

            if (UnityEngine.Input.GetKey(KeyCode.Keypad6))
                transform.Rotate(Vector3.up, TimeUtils.AdjustToFrameRate(rotationSpeed) * 1, Space.World);

            if (UnityEngine.Input.GetKeyDown(KeyCode.N))
            {
                movementSpeed -= 0.1f;
                rotationSpeed -= 0.1f;
                zRotationSpeed -= 0.1f;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.M))
            {
                movementSpeed += 0.1f;
                rotationSpeed += 0.1f;
                zRotationSpeed += 0.1f;
            }

            // j increase rotation speed
            if (UnityEngine.Input.GetKeyDown(KeyCode.J))
                rotationSpeed += 0.1f;

            // k decrease rotation speed
            if (UnityEngine.Input.GetKeyDown(KeyCode.K))
                rotationSpeed -= 0.1f;


            if (!isActive)
                return;

            float adjustedRotationSpeed = TimeUtils.AdjustToFrameRate(rotationSpeed);

            transform.Rotate(Vector3.up, adjustedRotationSpeed * UnityEngine.Input.GetAxis("Mouse X"), Space.World);
            transform.Rotate(Vector3.right, adjustedRotationSpeed * UnityEngine.Input.GetAxis("Mouse Y"), Space.Self);

            if (UnityEngine.Input.GetKey(positiveRotationKey) && UnityEngine.Input.GetKey(negativeRotationKey) && UnityEngine.Input.GetKey(KeyCode.LeftShift))
                transform.localEulerAngles = transform.localEulerAngles.WithZ(0);

            float keyRotationInput = UnityEngine.Input.GetKey(positiveRotationKey)
                ? -1
                : UnityEngine.Input.GetKey(negativeRotationKey)
                    ? 1
                    : 0;

            if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                transform.Rotate(Vector3.forward, TimeUtils.AdjustToClampedFrameRate(zRotationSpeed) * keyRotationInput, Space.Self);
            else
                transform.Rotate(Vector3.up, adjustedRotationSpeed * -keyRotationInput);

            Vector3 movementDirection = new Vector3(
                UnityEngine.Input.GetAxis("Horizontal"),
                0,
                UnityEngine.Input.GetAxis("Vertical")
            );

            transform.Translate(TimeUtils.AdjustToFrameRate(movementSpeed) * movementDirection.normalized, Space.Self);

            if (UnityEngine.Input.GetKey(upKey))
                transform.Translate(TimeUtils.AdjustToFrameRate(movementSpeed) * Vector3.up, Space.Self);
            else if (UnityEngine.Input.GetKey(downKey))
                transform.Translate(TimeUtils.AdjustToFrameRate(movementSpeed) * Vector3.down, Space.Self);
        }
    }
}