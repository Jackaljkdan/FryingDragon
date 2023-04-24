using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flammable
{
    [DisallowMultipleComponent]
    public class Flammable : MonoBehaviour
    {
        #region Inspector

        public new ParticleSystem particleSystem;

        private void Reset()
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        [ContextMenu("Start fire")]
        private void StartFireInEditMode()
        {
            if (Application.isPlaying)
                StartFire();
        }

        [ContextMenu("Stop fire")]
        private void StopFireInEditMode()
        {
            if (Application.isPlaying)
                StopFire();
        }


        #endregion

        public bool IsOnFire => particleSystem.isPlaying;

        private void Start()
        {
            particleSystem.gameObject.SetActive(false);
        }

        public void StartFire()
        {
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
        }

        public void StopFire()
        {
            particleSystem.Stop();
        }
    }
}