using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class DotBasedAudioVolumeScript : MonoBehaviour
    {
        #region Inspector

        public AudioSource audioSource;

        public float maxVolume = 1;

        public float maxDot = 0.2f;

        public float minVolume = 0;

        public float minDot = -0.1f;

        [Injected]
        public Transform player;

        private void Reset()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            player = context.Get<Transform>(this, "player");
        }

        private void Awake()
        {
            Inject();
        }

        private void Update()
        {
            Transform myTransform = transform;

            Vector3 directionToPlayer = (player.position - myTransform.position).normalized;
            float dot = Vector3.Dot(myTransform.forward, directionToPlayer);
            float clamped = Mathf.Clamp(dot, minDot, maxDot);
            float t = Mathf.InverseLerp(minDot, maxDot, clamped);
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, t);
        }
    }
}