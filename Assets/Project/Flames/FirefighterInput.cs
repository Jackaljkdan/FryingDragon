using JK.Actuators;
using JK.Actuators.Input;
using JK.Injection;
using JK.Utils;
using Project.Dragon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FirefighterInput : AbsoluteMovementInput
    {
        #region Inspector

        public float mouseInertiaLerp = 0.01f;
        public float joyRotationSpeed = 5f;
        public float joyInputThreshold = 0.1f;

        public ParticleSystem extinguisherParticleSystem;

        public AudioSource extinguisherAudioSource;

        public DragonInteractore interactore;

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            interactore.enabled = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            interactore.enabled = false;
        }
    }
}