using JK.Actuators;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Sounds
{
    public class FootstepsOnMovementActuator : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private AbstractMovementActuatorBehaviour movementActuator;

        public float maxSecondsBetweenSteps = 0.75f;
        public float minSecondsBetweenSteps = 0.4f;

        public float maxSpeedFactor = 2f;

        public List<AudioClip> clips = null;

        public List<AudioSource> audioSources;

        [RuntimeField]
        public float initialSpeed;

        private void Reset()
        {
            movementActuator = GetComponentInParent<AbstractMovementActuatorBehaviour>();
            audioSources = new List<AudioSource>();
            GetComponentsInChildren(audioSources);
        }

        #endregion

        private int lastSourceIndex = 0;
        private bool wasMoving = false;
        private float secondsSinceFootstep = 0;

        private void Awake()
        {
            initialSpeed = movementActuator.Speed;
        }

        private void Start()
        {
            // TODO: ho refactorato ma questo l'ho lasciato indietro
            //movementActuator.onMovement.AddListener(OnMovement);

            lastSourceIndex = 0;
            wasMoving = false;
            secondsSinceFootstep = 0;
        }

        private void OnMovement(Vector3 velocity)
        {
            if (velocity.sqrMagnitude > 0)
            {
                float speedFactor = movementActuator.Speed / initialSpeed;
                float clampedSpeedFactor = Mathf.Clamp(speedFactor, 0.8f, maxSpeedFactor);
                float proportionalSecondsBetweenSteps = Mathf.Lerp(
                    maxSecondsBetweenSteps,
                    minSecondsBetweenSteps,
                    Mathf.InverseLerp(1, maxSpeedFactor, clampedSpeedFactor)
                );

                if (!wasMoving)
                {
                    float halfSecondsBetween = proportionalSecondsBetweenSteps / 2;
                    if (secondsSinceFootstep < halfSecondsBetween)
                        secondsSinceFootstep = halfSecondsBetween;

                    wasMoving = true;
                }

                secondsSinceFootstep += Time.deltaTime;

                if (secondsSinceFootstep >= proportionalSecondsBetweenSteps)
                {
                    secondsSinceFootstep = 0;
                    PlayFootstep();
                }
            }
            else if (wasMoving)
            {
                wasMoving = false;
            }
        }

        private void PlayFootstep()
        {
            lastSourceIndex = (lastSourceIndex + 1) % audioSources.Count;
            audioSources[lastSourceIndex].PlayRandomClip(clips, oneShot: true);
        }

        private void OnDestroy()
        {
            // TODO: ho refactorato ma questo l'ho lasciato indietro
            //movementActuator.onMovement.RemoveListener(OnMovement);
        }
    }
}