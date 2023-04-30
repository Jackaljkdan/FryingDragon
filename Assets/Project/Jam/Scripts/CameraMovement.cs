using JK.Injection;
using JK.Utils;
using Project.Dragon;
using Project.Flames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class CameraMovement : MonoBehaviour
    {
        #region Inspector

        public Transform target;
        public float smoothSpeed = 0.125f;
        public float minZPosition = -10.3f;

        [DebugField]
        public Vector3 offset;

        [DebugField]
        public float cameraY;

        [Injected]
        public Transform dragonTransform;

        [Injected]
        private SignalBus signalBus;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonTransform = context.Get<Transform>(this, "dragon");
            signalBus = context.Get<SignalBus>(this);
        }

        #endregion

        private void Awake()
        {
            Inject();
        }

        private void Start()
        {
            cameraY = transform.position.y;

            signalBus.AddListener<FirefighterSpawnSignal>(OnFirefighterSpawned);
            signalBus.AddListener<FirefighterExitSignal>(OnFirefighterExited);

            if (target)
                return;

            SetTarget(dragonTransform);
            SetOffset();
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<FirefighterSpawnSignal>(OnFirefighterSpawned);
            signalBus.RemoveListener<FirefighterExitSignal>(OnFirefighterExited);
        }

        private void SetOffset()
        {
            offset = transform.position - target.position;
        }

        private void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void OnFirefighterSpawned(FirefighterSpawnSignal signal)
        {
            SetTarget(signal.firefighter.transform);
        }

        private void OnFirefighterExited(FirefighterExitSignal signal)
        {
            SetTarget(dragonTransform);
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            Vector3 desiredPosition = target.position + offset;
            desiredPosition.z = Mathf.Max(desiredPosition.z, minZPosition);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition.WithY(cameraY), smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}